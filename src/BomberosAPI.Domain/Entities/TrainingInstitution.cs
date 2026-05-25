namespace BomberosAPI.Domain.Entities;

public class TrainingInstitution
{
    public Guid InstitutionId {get; set;}
    public string Name {get; set;} = null!;
    public string? Acronym {get; set;}
    public string? Country {get; set;}
    public string? City {get; set;}
    public bool IsActive {get; set;}=true;

}