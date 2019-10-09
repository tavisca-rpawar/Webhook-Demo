namespace WebApplication3.Models
{
    public class GitHubPayLoad
    {
        public string Action { get; set; }
        public PullRequest Pull_request { get; set; }
    }
}
