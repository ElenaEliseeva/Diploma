using System;
using System.Collections.Generic;

namespace Diploma.Models
{
    public partial class Personality
    {
        public Personality()
        {
            Users = new HashSet<User>();
        }

        public int PersonalityId { get; set; }
        public string PersonalityTitle { get; set; } = null!;
        public string PersonalityDescription { get; set; } = null!;
        public string PersonalityLink { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
