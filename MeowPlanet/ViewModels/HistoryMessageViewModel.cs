using MeowPlanet.Models;

namespace MeowPlanet.ViewModels
{
    public class HistoryMessageViewModel
    {
        public string SelfName { get; set; }
        public string SelfPhoto { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public List<Message> Messages { get; set; }
    }
}
