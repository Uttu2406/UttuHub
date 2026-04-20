namespace UttuHub.API.DTOs.Project
{
    // UC 231.1, 232 - Returned when fetching Project(s)
    public class ProjectResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? TechStack { get; set; }
        public string? GithubUrl { get; set; }
        public string? LiveUrl { get; set; }
        public int UserId { get; set; }
    }
}


