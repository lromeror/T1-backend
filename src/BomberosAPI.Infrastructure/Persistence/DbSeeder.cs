using BomberosAPI.Application.Common.Constants;
using BomberosAPI.Application.Common.Interfaces;
using BomberosAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BomberosAPI.Infrastructure.Persistence;

/// <summary>
/// Siembra datos de prueba en Development. Idempotente: no duplica si ya existen.
/// </summary>
public static class DbSeeder
{
    private const string DefaultPassword = "Smab2026!";

    private static readonly (string Code, string Name)[] SeedRoles =
    [
        (Roles.SystemAdmin,        "System Administrator"),
        (Roles.Admin,              "Administrator"),
        (Roles.Medical,            "Medical Personnel"),
        (Roles.FirefighterTrainee, "Firefighter Trainee"),
        (Roles.Capacitator,        "Capacitator / Instructor"),
        (Roles.Researcher,         "Researcher"),
        (Roles.FireChief,          "Fire Chief"),
    ];

    private static readonly (string Email, string First, string Last, string RoleCode, string? Phone)[] SeedUsers =
    [
        ("sysadmin@smab.app",       "Admin",    "Sistema",    Roles.SystemAdmin,        null),
        ("admin@smab.app",          "Sara",     "Flores",     Roles.Admin,              null),
        ("medico@smab.app",         "Michael",  "Poveda",     Roles.Medical,            "+593 99-100-0001"),
        ("enfermera@smab.app",      "Valeria",  "Castro",     Roles.Medical,            "+593 99-100-0002"),
        ("nutricionista@smab.app",  "Andrea",   "Rivas",      Roles.Medical,            "+593 99-100-0003"),
        ("medico2@smab.app",        "Daniel",   "Ortega",     Roles.Medical,            "+593 99-100-0004"),
        ("bombero@smab.app",        "Carlos",   "Ruiz",       Roles.FirefighterTrainee, "+593 98-200-0001"),
        ("bombero2@smab.app",       "Marco",    "Torres",     Roles.FirefighterTrainee, "+593 98-200-0002"),
        ("bombero3@smab.app",       "Sara",     "Vega",       Roles.FirefighterTrainee, "+593 98-200-0003"),
        ("bombero4@smab.app",       "Luis",     "Paredes",    Roles.FirefighterTrainee, "+593 98-200-0004"),
        ("bombero5@smab.app",       "Diego",    "Carrillo",   Roles.FirefighterTrainee, "+593 98-200-0005"),
        ("capacitador@smab.app",    "Luis",     "Herrera",    Roles.Capacitator,        null),
        ("investigador@smab.app",   "Ana",      "Torres",     Roles.Researcher,         null),
        ("jefe@smab.app",           "Roberto",  "Mendoza",    Roles.FireChief,          null),
    ];

    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db     = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();

        logger.LogInformation("DbSeeder: starting...");

        var institution = await SeedInstitutionAsync(db);
        var roleMap     = await SeedRolesAsync(db);
        await SeedUsersAsync(db, hasher, institution.InstitutionId, roleMap);
        await SeedHealthPersonnelAsync(db);
        await SeedTraineesAsync(db);
        var location    = await SeedLocationAsync(db, institution.InstitutionId);
        await SeedSessionsAsync(db, institution.InstitutionId, location.TrainingLocationId);

        logger.LogInformation("DbSeeder: done. Password for all test users: {Password}", DefaultPassword);
    }

    // ── Institution ──────────────────────────────────────────────────────────

    private static async Task<TrainingInstitution> SeedInstitutionAsync(AppDbContext db)
    {
        var existing = await db.TrainingInstitutions.FirstOrDefaultAsync(i => i.Acronym == "SMAB");
        if (existing is not null) return existing;

        var institution = new TrainingInstitution
        {
            InstitutionId = Guid.NewGuid(),
            Name          = "Sistema de Monitoreo y Análisis de Bomberos",
            Acronym       = "SMAB",
            Country       = "Ecuador",
            City          = "Quito",
            IsActive      = true,
        };

        db.TrainingInstitutions.Add(institution);
        await db.SaveChangesAsync();
        return institution;
    }

    // ── Roles ─────────────────────────────────────────────────────────────────

    private static async Task<Dictionary<string, Guid>> SeedRolesAsync(AppDbContext db)
    {
        var existingCodes = await db.Roles
            .Select(r => r.Code)
            .ToHashSetAsync();

        var toInsert = SeedRoles
            .Where(r => !existingCodes.Contains(r.Code))
            .Select(r => new Role
            {
                RoleId = Guid.NewGuid(),
                Code   = r.Code,
                Name   = r.Name,
            })
            .ToList();

        if (toInsert.Count > 0)
        {
            db.Roles.AddRange(toInsert);
            await db.SaveChangesAsync();
        }

        return await db.Roles.ToDictionaryAsync(r => r.Code, r => r.RoleId);
    }

    // ── Users ─────────────────────────────────────────────────────────────────

    private static async Task SeedUsersAsync(
        AppDbContext db,
        IPasswordHasher hasher,
        Guid institutionId,
        Dictionary<string, Guid> roleMap)
    {
        var existingEmails = await db.Users
            .Select(u => u.Email)
            .ToHashSetAsync();

        foreach (var (email, first, last, roleCode, phone) in SeedUsers)
        {
            if (existingEmails.Contains(email)) continue;

            var userId = Guid.NewGuid();

            db.Users.Add(new User
            {
                UserId        = userId,
                InstitutionId = institutionId,
                Email         = email,
                FirstName     = first,
                LastName      = last,
                Phone         = phone,
                AccountStatus = "active",
                CreatedAt     = DateTime.UtcNow,
            });

            db.UserCredentials.Add(new UserCredential
            {
                UserCredentialId      = Guid.NewGuid(),
                UserId                = userId,
                PasswordHash          = hasher.Hash(DefaultPassword),
                HashAlgorithm         = "bcrypt",
                LastPasswordChangeAt  = DateTime.UtcNow,
            });

            if (roleMap.TryGetValue(roleCode, out var roleId))
            {
                db.UserRoles.Add(new UserRole
                {
                    UserRoleId = Guid.NewGuid(),
                    UserId     = userId,
                    RoleId     = roleId,
                    StartDate  = DateTime.UtcNow,
                    IsActive   = true,
                });
            }
        }

        await db.SaveChangesAsync();
    }

    // ── Health Personnel ──────────────────────────────────────────────────────

    private static async Task SeedHealthPersonnelAsync(AppDbContext db)
    {
        if (await db.HealthPersonnel.AnyAsync()) return;

        var hpData = new[]
        {
            ("medico@smab.app",        "Médico",        "Cardiología",   "MED-001", true),
            ("enfermera@smab.app",     "Enfermero",     "Urgencias",     "ENF-001", false),
            ("nutricionista@smab.app", "Nutricionista", "Deportiva",     "NUT-001", false),
            ("medico2@smab.app",       "Médico",        "Medicina General", "MED-002", false),
        };

        var hpEmails  = hpData.Select(h => h.Item1).ToList();
        var userIdMap = await db.Users
            .Where(u => hpEmails.Contains(u.Email))
            .ToDictionaryAsync(u => u.Email, u => u.UserId);

        foreach (var (email, profession, specialty, license, canApprove) in hpData)
        {
            if (!userIdMap.TryGetValue(email, out var userId)) continue;

            db.HealthPersonnel.Add(new HealthPersonnel
            {
                HealthPersonnelId  = Guid.NewGuid(),
                UserId             = userId,
                Profession         = profession,
                Specialty          = specialty,
                LicenseNumber      = license,
                CanApproveDischarges = canApprove,
            });
        }

        await db.SaveChangesAsync();
    }

    // ── Trainee Firefighters ──────────────────────────────────────────────────

    private static async Task SeedTraineesAsync(AppDbContext db)
    {
        if (await db.TraineeFirefighters.AnyAsync()) return;

        var traineeData = new[]
        {
            ("bombero@smab.app",  "BOM-2026-001", "M", "O+"),
            ("bombero2@smab.app", "BOM-2026-002", "M", "A+"),
            ("bombero3@smab.app", "BOM-2026-003", "F", "B+"),
            ("bombero4@smab.app", "BOM-2026-004", "M", "AB+"),
            ("bombero5@smab.app", "BOM-2026-005", "M", "O-"),
        };

        var traineeEmails = traineeData.Select(t => t.Item1).ToList();
        var userIdMap     = await db.Users
            .Where(u => traineeEmails.Contains(u.Email))
            .ToDictionaryAsync(u => u.Email, u => u.UserId);

        foreach (var (email, code, sex, blood) in traineeData)
        {
            if (!userIdMap.TryGetValue(email, out var userId)) continue;

            db.TraineeFirefighters.Add(new TraineeFirefighter
            {
                TraineeFirefighterId = Guid.NewGuid(),
                UserId               = userId,
                ApplicantCode        = code,
                Sex                  = sex,
                BloodType            = blood,
                TrainingStatus       = "Active",
            });
        }

        await db.SaveChangesAsync();
    }

    // ── Training Location ─────────────────────────────────────────────────────

    private static async Task<TrainingLocation> SeedLocationAsync(AppDbContext db, Guid institutionId)
    {
        var existing = await db.TrainingLocations.FirstOrDefaultAsync(l => l.InstitutionId == institutionId);
        if (existing is not null) return existing;

        var location = new TrainingLocation
        {
            TrainingLocationId = Guid.NewGuid(),
            InstitutionId      = institutionId,
            Name               = "Centro de Entrenamiento Alpha",
            LocationType       = "Outdoor",
            Address            = "Av. Bomberos 4500, Quito",
            MaxCapacity        = 30,
        };

        db.TrainingLocations.Add(location);
        await db.SaveChangesAsync();
        return location;
    }

    // ── Training Sessions ─────────────────────────────────────────────────────

    private static async Task SeedSessionsAsync(AppDbContext db, Guid institutionId, Guid locationId)
    {
        if (await db.TrainingSessions.AnyAsync()) return;

        var adminId = await db.Users
            .Where(u => u.Email == "admin@smab.app")
            .Select(u => u.UserId)
            .FirstOrDefaultAsync();

        if (adminId == Guid.Empty) return;

        var now = DateTime.UtcNow;

        var sessions = new List<TrainingSession>
        {
            Session("G1", "Capacitación G1 — Evaluación Inicial",      "Chequeo-Rutinario", "Scheduled",   now.AddDays(3),   now.AddDays(3).AddHours(4),  8),
            Session("G2", "Capacitación G2 — Evaluación Física",        "Eval-Fisica",       "InProgress",  now.AddDays(-1),  now.AddDays(-1).AddHours(4), 10),
            Session("G3", "Capacitación G3 — Simulacro Interior",       "Simulacro",         "Scheduled",   now.AddDays(7),   now.AddDays(7).AddHours(4),  6),
            Session("G4", "Capacitación G4 — Evaluación Térmica",       "Eval-Termica",      "Scheduled",   now.AddDays(10),  now.AddDays(10).AddHours(4), 14),
            Session("G5", "Capacitación G5 — Evaluación Post-Rescate",  "Post-Rescate",      "Scheduled",   now.AddDays(14),  now.AddDays(14).AddHours(4), 9),
            Session("B1", "Capacitación B1 — Evaluación Física",        "Eval-Fisica",       "Finished",    now.AddDays(-30), now.AddDays(-30).AddHours(4), 12),
            Session("B2", "Capacitación B2 — Chequeo Rutinario",        "Chequeo-Rutinario", "Finished",    now.AddDays(-20), now.AddDays(-20).AddHours(4), 8),
            Session("B3", "Capacitación B3 — Simulacro Casa Fuego",     "Simulacro",         "Finished",    now.AddDays(-10), now.AddDays(-10).AddHours(4), 15),
            Session("C1", "Capacitación C1 — Evaluación Inicial",       "Eval-Inicial",      "Cancelled",   now.AddDays(-45), now.AddDays(-45).AddHours(4), 5),
            Session("C2", "Capacitación C2 — Evaluación Térmica",       "Eval-Termica",      "Cancelled",   now.AddDays(-35), now.AddDays(-35).AddHours(4), 9),
        };

        foreach (var s in sessions)
        {
            s.InstitutionId      = institutionId;
            s.TrainingLocationId = locationId;
            s.CreatedByUserId    = adminId;
        }

        db.TrainingSessions.AddRange(sessions);
        await db.SaveChangesAsync();
    }

    private static TrainingSession Session(
        string code, string title, string type,
        string status, DateTime start, DateTime end, int capacity) => new()
    {
        TrainingSessionId = Guid.NewGuid(),
        SessionCode       = code,
        Title             = title,
        Description       = $"Sesión de entrenamiento: {type}. Presentarse hidratado y con equipo completo.",
        Status            = status,
        ScheduledStart    = start,
        ScheduledEnd      = end,
        PlannedCapacity   = capacity,
        ActualStart       = status is "InProgress" or "Finished" ? start : null,
        ActualEnd         = status == "Finished" ? end : null,
    };
}
