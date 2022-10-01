using System;
using System.Collections.Generic;

namespace Diploma.Models
{
    public partial class ModalType
    {
        public ModalType()
        {
            Users = new HashSet<User>();
        }

        public int ModalTypeId { get; set; }
        public string ModalTypeName { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
