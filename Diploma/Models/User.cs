using System;
using System.Collections.Generic;

namespace Diploma.Models
{
    public partial class User
    {
        public User()
        {
            ModalTimes = new HashSet<ModalTime>();
            SecondTests = new HashSet<SecondTest>();
        }

        public int UserId { get; set; }
        public int Age { get; set; }
        public int PersonalityId { get; set; }
        public int ModalTypeId { get; set; }
        public int TestId { get; set; }
        public DateTime UserCreateDate { get; set; }

        public virtual ModalType ModalType { get; set; } = null!;
        public virtual Personality Personality { get; set; } = null!;
        public virtual Test Test { get; set; } = null!;
        public virtual ICollection<ModalTime> ModalTimes { get; set; }
        public virtual ICollection<SecondTest> SecondTests { get; set; }
    }
}
