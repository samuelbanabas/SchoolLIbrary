namespace SchoolLIbrary.Models
{
    public class NotificationModel
    {
        public int Id { get; set; }
        public LibraryUser User { get; set; }
        public string NotificationType { get; set; }
        public string Message { get; set; }
        public DateTime SentTime { get; set; }
    }
}
