namespace MeowPlanet.Models
{
    public class ContactMembers
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public DateTime LastTime { get; set; }
        public int UnreadCount { get; set; }
    }
}
