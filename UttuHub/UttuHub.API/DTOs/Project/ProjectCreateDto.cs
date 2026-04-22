namespace UttuHub.API.DTOs.Project
{
    // UC 231 - Used when creating a new Project (POST /projects)
    // CHANGED: Removed UserId - extracted from JWT claims in controller instead
    // CHANGED: TechStack is now List<string> - joined with comma on save
    public class ProjectCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<string> TechStack { get; set; } = new(); // CHANGED: List instead of comma string
        public string? GithubUrl { get; set; }
        public string? LiveUrl { get; set; }
    }
}