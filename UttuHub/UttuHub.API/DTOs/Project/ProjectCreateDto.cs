namespace UttuHub.API.DTOs.Project
{
    // UC 231 - Used when creating a new Project (POST /projects)
    // CHANGED: Removed UserId - extracted from JWT claims in controller instead
    public class ProjectCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? TechStack { get; set; }
        public string? GithubUrl { get; set; }
        public string? LiveUrl { get; set; }
    }
}

