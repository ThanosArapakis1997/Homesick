namespace Homesick.UI.Models
{
    public class AgentResponseDto
    {
        public string? Token { get; set; }
        public bool Done { get; set; }

        public string? Error { get; set; }
        //public List<object>? Sources { get; set; }
    }
}
