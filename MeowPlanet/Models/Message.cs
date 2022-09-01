using System;
using System.Collections.Generic;

namespace MeowPlanet.Models
{
    public partial class Message
    {
        public int MessageId { get; set; }
        public int SendId { get; set; }
        public int ReceivedId { get; set; }
        public string MessageContent { get; set; } = null!;
        public DateTime Time { get; set; }
        public bool IsRead { get; set; }

        public virtual Member Received { get; set; } = null!;
        public virtual Member Send { get; set; } = null!;
    }
}
