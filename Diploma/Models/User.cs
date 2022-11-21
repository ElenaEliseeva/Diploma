using System;
using System.Collections.Generic;

namespace Diploma.Models
{
    public partial class User
    {
        public User()
        {
            TestResults = new HashSet<TestResult>();
        }

        public int UserId { get; set; }
        public int Age { get; set; }
        public int PersonalityId { get; set; }
        public int ModalTypeId { get; set; }
        public int TestId { get; set; }
        public DateTime UserCreateDate { get; set; }
        public TimeSpan TestTimeResult { get; set; }
        public string ClarifyingQuestionOne { get; set; } = null!;
        public string ClarifyingQuestionTwo { get; set; } = null!;
        public string? ClarifyingQuestionThree { get; set; }

        public virtual ModalType ModalType { get; set; } = null!;
        public virtual Personality Personality { get; set; } = null!;
        public virtual Test Test { get; set; } = null!;
        public virtual ICollection<TestResult> TestResults { get; set; }
    }
}
