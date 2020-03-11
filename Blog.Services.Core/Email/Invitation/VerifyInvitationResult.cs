namespace Blog.Services.Core.Email.Invitation
{
    public class VerifyInvitationResult
    {
        public string InviterEmail { get; set; }
        public string SecurityStamp { get; set; }
        public bool IsVerified { get; set; }
    }
}
