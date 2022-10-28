﻿namespace Diploma.Models
{
    public partial class User
    {
        public User()
        {
            ModalTimes = new HashSet<ModalTime>();
        }

        public int UserId { get; set; }
        public int Age { get; set; }
        public int PersonalityId { get; set; }
        public int ModalTypeId { get; set; }
        public int TestId { get; set; }

        public virtual ModalType ModalType { get; set; } = null!;
        public virtual Personality Personality { get; set; } = null!;
        public virtual Test Test { get; set; } = null!;
        public virtual ICollection<ModalTime> ModalTimes { get; set; }
    }
}
