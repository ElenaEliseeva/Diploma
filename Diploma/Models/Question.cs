using System;
using System.Collections.Generic;

namespace Diploma.Models
{
    public partial class Question
    {
        public Question()
        {
            QuestionAnswers = new HashSet<QuestionAnswer>();
            TestQuestions = new HashSet<TestQuestion>();
        }

        public int QuestionId { get; set; }
        public int QuestionNumber { get; set; }
        public string QuestionText { get; set; } = null!;

        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }
        public virtual ICollection<TestQuestion> TestQuestions { get; set; }
    }
}
