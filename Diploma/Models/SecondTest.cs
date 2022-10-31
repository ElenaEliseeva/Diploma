using System;
using System.Collections.Generic;

namespace Diploma.Models
{
    public partial class SecondTest
    {
        public int SecondTest1 { get; set; }
        public int UserId { get; set; }
        public int SecondTestNumber { get; set; }
        public bool SecondTestResult { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
