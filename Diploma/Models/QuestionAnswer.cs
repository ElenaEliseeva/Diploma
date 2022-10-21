using System;
using System.Collections.Generic;

namespace Diploma.Models
{
    public partial class QuestionAnswer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }

        public virtual Answer Answer { get; set; } = null!;
        public virtual Question Question { get; set; } = null!;
    }
}
