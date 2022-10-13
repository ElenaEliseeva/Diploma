namespace Diploma.Models
{
    public partial class Test
    {
        public Test()
        {
            TestQuestions = new HashSet<TestQuestion>();
            Users = new HashSet<User>();
        }

        public int TestId { get; set; }
        public bool TestType { get; set; }

        public virtual ICollection<TestQuestion> TestQuestions { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
