namespace Blog.Services.Core.Email.Invitation
{
    public class Invitation
    {
        public string InviterEmail { get; set; }
        public string InviterName { get; set; }
        public string SecurityStamp { get; set; }
        public int WorkspaceId { get; set; }
        public string WorkspaceName { get; set; }
        public string ProjectName { get; set; }
        public int? ProjectId { get; set; }
        public string Email { get; set; }
        public int WorkspaceUserId { get; set; }
        public bool isExternalUser { get; set; }
    }
}
