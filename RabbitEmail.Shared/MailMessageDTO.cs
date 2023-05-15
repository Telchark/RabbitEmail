namespace RabbitEmail.Shared
{
    public class MailMessageDTO
    {
        public SendingMethod SendingMethod { get; set; }
        public string? SenderName { get; set; }
        public string? ReciverName { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
    }
}
