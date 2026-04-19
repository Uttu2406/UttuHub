namespace UttuHub.API.DTOs.Project
{
    // UC 233 - Used when updating an existing Project (PUT /projects/{id})
    // UserId is NOT updatable - ownership cannot be transferred
    public class ProjectUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? TechStack { get; set; }
        public string? GithubUrl { get; set; }
        public string? LiveUrl { get; set; }
    }
}
