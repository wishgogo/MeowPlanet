using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeowPlanet.Models
{
    public partial class Member
    {
        public Member()
        {
            Adopts = new HashSet<Adopt>();
            Cats = new HashSet<Cat>();
            Clues = new HashSet<Clue>();
            Favorites = new HashSet<Favorite>();
            MessageReceiveds = new HashSet<Message>();
            MessageSends = new HashSet<Message>();
            Orderlists = new HashSet<Orderlist>();
            Sitters = new HashSet<Sitter>();
        }

        public int MemberId { get; set; }
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "請輸入有效的密碼")]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "密碼必須是6-10位")]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "請輸入有效的手機號碼")]
        [RegularExpression(@"^09[0-9]{8}$", ErrorMessage = "手機號碼格式不正確")]
        public string Phone { get; set; } = null!;
        [Required(ErrorMessage = "請輸入有效的名字")]
        public string Name { get; set; } = null!;
        public string? Photo { get; set; }

        public virtual ICollection<Adopt> Adopts { get; set; }
        public virtual ICollection<Cat> Cats { get; set; }
        public virtual ICollection<Clue> Clues { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<Message> MessageReceiveds { get; set; }
        public virtual ICollection<Message> MessageSends { get; set; }
        public virtual ICollection<Orderlist> Orderlists { get; set; }
        public virtual ICollection<Sitter> Sitters { get; set; }
    }
}
