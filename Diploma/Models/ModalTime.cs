namespace Diploma.Models
{
    public partial class ModalTime
    {
        public int ModalTimeId { get; set; }
        public int UserId { get; set; }
        public int ModalNumber { get; set; }
        public TimeSpan ModalTime1 { get; set; }
        public bool ModalResult { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
