namespace UttuHub.API.DTOs.Project
{
    // UC 233 - Used when updating an existing Project (PUT /projects/{id})
    // UserId is NOT updatable - ownership cannot be transferred
    // CHANGED: TechStack is now List<string> - joined with comma on save
    public class ProjectUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<string> TechStack { get; set; } = new(); // CHANGED: List instead of comma string
        public string? GithubUrl { get; set; }
        public string? LiveUrl { get; set; }
    }
}