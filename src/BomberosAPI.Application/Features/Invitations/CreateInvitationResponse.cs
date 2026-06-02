namespace BomberosAPI.Application.Features.Invitations;

public record CreateInvitationResponse(
    InvitationDto Invitation,
    string PlainToken
);