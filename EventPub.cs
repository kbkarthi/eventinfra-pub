namespace eventinfra_pub
{
    public class EventPub
    {
        public Guid EventId { get; set; }

        public string EventName { get; set; } = string.Empty;

        public string EventDescription { get; set; } = string.Empty;
    }
}
