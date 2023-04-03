using System;
using System.Collections.Generic;

namespace Diploma.Models
{
    public partial class TestResult
    {
        public int TestResultId { get; set; }
        public int? TestNumber { get; set; }
        public TimeSpan? ModalTimeResult { get; set; }
        public bool? ModalResult { get; set; }
        public bool? TestResultt { get; set; }
        public TimeSpan? TestTimeResult { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
