using MeowPlanet.Models;


namespace MeowPlanet.ViewModels
{
    public class MessageBoxModel
    {
        public string UserName { get; set; }
        public string UserPhoto { get; set; }
        public bool HasUnread { get; set; }
        public ICollection<ContactMembers> ContactMembers { get; set; }

    }
}
