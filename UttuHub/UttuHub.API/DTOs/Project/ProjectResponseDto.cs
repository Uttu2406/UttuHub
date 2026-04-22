namespace UttuHub.API.DTOs.Project
{
    // UC 231.1, 232 - Returned when fetching Project(s)
    // CHANGED: TechStack is now List<string> - split from comma string on read
    public class ProjectResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<string> TechStack { get; set; } = new(); // CHANGED: List instead of comma string
        public string? GithubUrl { get; set; }
        public string? LiveUrl { get; set; }
        public int UserId { get; set; }
    }
}