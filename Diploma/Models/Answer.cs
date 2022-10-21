using System;
using System.Collections.Generic;

namespace Diploma.Models
{
    public partial class Answer
    {
        public Answer()
        {
            QuestionAnswers = new HashSet<QuestionAnswer>();
        }

        public int AnswerId { get; set; }
        public string AnswerText { get; set; } = null!;
        public bool AnswerResult { get; set; }
        public string? AnswerTextResult { get; set; }

        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }
    }
}
