namespace AdessoTurkey.Application.DTOs
{
    /// <summary>
    /// Kura çekme ve listeleme için response
    /// </summary>
    public class DrawResponseDto
    {
        public int? Id { get; set; }
        public string? DrawerFirstName { get; set; }
        public string? DrawerLastName { get; set; }
        public string? DrawerFullName { get; set; }
        public int? NumberOfGroups { get; set; }
        public DateTime? DrawDate { get; set; }
        public List<DrawGroupResponseDto> Groups { get; set; } = new();
    }
}
