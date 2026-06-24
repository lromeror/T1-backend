using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BomberosAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessAudit",
                columns: table => new
                {
                    access_audit_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    auth_session_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    @event = table.Column<string>(name: "event", type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ip = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    user_agent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    success = table.Column<bool>(type: "bit", nullable: false),
                    occurred_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessAudit", x => x.access_audit_id);
                });

            migrationBuilder.CreateTable(
                name: "AuthSession",
                columns: table => new
                {
                    auth_session_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    refresh_token_hash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    device = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ip = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    user_agent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    expires_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    closed_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthSession", x => x.auth_session_id);
                });

            migrationBuilder.CreateTable(
                name: "BioimpedanceMeasurement",
                columns: table => new
                {
                    bioimpedance_measurement_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    session_participant_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    registered_by_health_personnel_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    weight_kg = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    fat_percentage = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: true),
                    muscle_mass_kg = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    body_water_pct = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: true),
                    basal_metabolic_rate = table.Column<decimal>(type: "decimal(7,2)", precision: 7, scale: 2, nullable: true),
                    taken_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BioimpedanceMeasurement", x => x.bioimpedance_measurement_id);
                });

            migrationBuilder.CreateTable(
                name: "ChangeAudit",
                columns: table => new
                {
                    change_audit_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    actor_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    entity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    entity_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    operation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    previous_values_json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    new_values_json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    occurred_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeAudit", x => x.change_audit_id);
                });

            migrationBuilder.CreateTable(
                name: "ConsentDocument",
                columns: table => new
                {
                    consent_document_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    consent_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    version = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    text_content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    valid_from = table.Column<DateTime>(type: "datetime2", nullable: false),
                    valid_until = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsentDocument", x => x.consent_document_id);
                });

            migrationBuilder.CreateTable(
                name: "CriticalAlert",
                columns: table => new
                {
                    critical_alert_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    session_participant_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    vital_signs_measurement_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    symptom_report_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    environmental_data_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    attended_by_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    alert_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    severity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    generated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    attended_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CriticalAlert", x => x.critical_alert_id);
                });

            migrationBuilder.CreateTable(
                name: "DSARRequest",
                columns: table => new
                {
                    dsar_request_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    managed_by_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    right_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    requested_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    responded_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    legal_deadline_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DSARRequest", x => x.dsar_request_id);
                });

            migrationBuilder.CreateTable(
                name: "EnvironmentalData",
                columns: table => new
                {
                    environmental_data_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    training_session_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    registered_by_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    temperature_c = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: true),
                    humidity_pct = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: true),
                    co_ppm = table.Column<decimal>(type: "decimal(7,2)", precision: 7, scale: 2, nullable: true),
                    heat_stress_index = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    measured_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvironmentalData", x => x.environmental_data_id);
                });

            migrationBuilder.CreateTable(
                name: "HealthPersonnel",
                columns: table => new
                {
                    health_personnel_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    profession = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    specialty = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    license_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    can_approve_discharges = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthPersonnel", x => x.health_personnel_id);
                });

            migrationBuilder.CreateTable(
                name: "Invitation",
                columns: table => new
                {
                    invitation_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    sender_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    target_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    training_session_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    target_role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    target_email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    invitation_token_hash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    expires_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    responded_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitation", x => x.invitation_id);
                });

            migrationBuilder.CreateTable(
                name: "MedicalHistory",
                columns: table => new
                {
                    medical_history_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    trainee_firefighter_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by_health_personnel_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    allergies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    preexisting_conditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    current_medication = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    general_observations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalHistory", x => x.medical_history_id);
                });

            migrationBuilder.CreateTable(
                name: "OfflineSyncQueue",
                columns: table => new
                {
                    offline_sync_queue_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    source_table = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    record_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    payload_json = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    attempts = table.Column<int>(type: "int", nullable: false),
                    max_attempts = table.Column<int>(type: "int", nullable: false),
                    synced = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    synced_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfflineSyncQueue", x => x.offline_sync_queue_id);
                });

            migrationBuilder.CreateTable(
                name: "PasswordResetToken",
                columns: table => new
                {
                    password_reset_token_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    token_hash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    expires_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    used_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetToken", x => x.password_reset_token_id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "SessionParticipant",
                columns: table => new
                {
                    session_participant_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    training_session_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    trainee_firefighter_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    invitation_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    participation_status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    attendance_confirmed = table.Column<bool>(type: "bit", nullable: false),
                    check_in_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    observations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionParticipant", x => x.session_participant_id);
                });

            migrationBuilder.CreateTable(
                name: "SessionResult",
                columns: table => new
                {
                    session_result_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    session_participant_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    validated_by_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    performance_score = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    risk_classification = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fit_to_continue = table.Column<bool>(type: "bit", nullable: false),
                    summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    generated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionResult", x => x.session_result_id);
                });

            migrationBuilder.CreateTable(
                name: "SymptomReport",
                columns: table => new
                {
                    symptom_report_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    session_participant_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    reported_by_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    severity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    symptoms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    requires_alert = table.Column<bool>(type: "bit", nullable: false),
                    reported_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymptomReport", x => x.symptom_report_id);
                });

            migrationBuilder.CreateTable(
                name: "TraineeFirefighter",
                columns: table => new
                {
                    trainee_firefighter_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    applicant_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    birth_date = table.Column<DateOnly>(type: "date", nullable: true),
                    sex = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    blood_type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    emergency_contact_name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    emergency_contact_phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    training_status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraineeFirefighter", x => x.trainee_firefighter_id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingInstitution",
                columns: table => new
                {
                    institution_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    acronym = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    city = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingInstitution", x => x.institution_id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingLocation",
                columns: table => new
                {
                    training_location_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    institution_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    location_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    max_capacity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingLocation", x => x.training_location_id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingSession",
                columns: table => new
                {
                    training_session_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    institution_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    training_location_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    session_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    scheduled_start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    scheduled_end = table.Column<DateTime>(type: "datetime2", nullable: false),
                    actual_start = table.Column<DateTime>(type: "datetime2", nullable: true),
                    actual_end = table.Column<DateTime>(type: "datetime2", nullable: true),
                    planned_capacity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingSession", x => x.training_session_id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    institution_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    account_status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    last_access_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "UserConsent",
                columns: table => new
                {
                    user_consent_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    institution_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    consent_document_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    granted_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    revoked_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    expires_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConsent", x => x.user_consent_id);
                });

            migrationBuilder.CreateTable(
                name: "UserCredential",
                columns: table => new
                {
                    user_credential_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    hash_algorithm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    mfa_enabled = table.Column<bool>(type: "bit", nullable: false),
                    failed_attempts = table.Column<int>(type: "int", nullable: false),
                    locked_until = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_password_change_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCredential", x => x.user_credential_id);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    user_role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    assigned_by_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.user_role_id);
                });

            migrationBuilder.CreateTable(
                name: "VitalSignsMeasurement",
                columns: table => new
                {
                    vital_signs_measurement_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    session_participant_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    registered_by_health_personnel_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    heart_rate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    systolic_pressure = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    diastolic_pressure = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    temperature_c = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: true),
                    spo2 = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: true),
                    taken_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VitalSignsMeasurement", x => x.vital_signs_measurement_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Role_code",
                table: "Role",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_email",
                table: "User",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCredential_user_id",
                table: "UserCredential",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessAudit");

            migrationBuilder.DropTable(
                name: "AuthSession");

            migrationBuilder.DropTable(
                name: "BioimpedanceMeasurement");

            migrationBuilder.DropTable(
                name: "ChangeAudit");

            migrationBuilder.DropTable(
                name: "ConsentDocument");

            migrationBuilder.DropTable(
                name: "CriticalAlert");

            migrationBuilder.DropTable(
                name: "DSARRequest");

            migrationBuilder.DropTable(
                name: "EnvironmentalData");

            migrationBuilder.DropTable(
                name: "HealthPersonnel");

            migrationBuilder.DropTable(
                name: "Invitation");

            migrationBuilder.DropTable(
                name: "MedicalHistory");

            migrationBuilder.DropTable(
                name: "OfflineSyncQueue");

            migrationBuilder.DropTable(
                name: "PasswordResetToken");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "SessionParticipant");

            migrationBuilder.DropTable(
                name: "SessionResult");

            migrationBuilder.DropTable(
                name: "SymptomReport");

            migrationBuilder.DropTable(
                name: "TraineeFirefighter");

            migrationBuilder.DropTable(
                name: "TrainingInstitution");

            migrationBuilder.DropTable(
                name: "TrainingLocation");

            migrationBuilder.DropTable(
                name: "TrainingSession");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "UserConsent");

            migrationBuilder.DropTable(
                name: "UserCredential");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "VitalSignsMeasurement");
        }
    }
}
