namespace AdessoTurkey.Application.DTOs
{
    public class DrawRequestDto
    {
        public string DrawerFirstName { get; set; } = string.Empty;
        public string DrawerLastName { get; set; } = string.Empty;
        public int NumberOfGroups { get; set; }
    }
}
