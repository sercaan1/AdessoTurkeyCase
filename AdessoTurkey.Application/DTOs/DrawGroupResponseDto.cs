namespace AdessoTurkey.Application.DTOs
{
    public class DrawGroupResponseDto
    {
        public string GroupName { get; set; } = string.Empty;
        public List<DrawTeamResponseDto> Teams { get; set; } = new();
    }
}
