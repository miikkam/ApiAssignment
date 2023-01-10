namespace FeedbackApi.Models
{
    public class FeedbackItem
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Message { get; set; }
        public string? Secret { get; set; }
    }
}
