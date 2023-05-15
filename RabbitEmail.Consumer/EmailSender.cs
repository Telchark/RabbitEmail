using MailKit.Net.Smtp;
using MimeKit;
using RabbitEmail.Shared;

namespace RabbitEmail.Consumer
{
    public class EmailSender
    {
        private readonly ISmtpClient _smtpClient = new SmtpClient();
        public void Send(MimeMessage message, SendingMethod sendingMethod)
        {
            switch (sendingMethod)
            {
                case SendingMethod.Console:
                    Console.WriteLine(message);
                    break;
                case SendingMethod.SMTP:
                    _smtpClient.Connect("smtp.freesmtpservers.com", 25, false);
                    _smtpClient.Send(message);
                    _smtpClient.Disconnect(true);
                    Console.WriteLine($"Smtp message has been sended.");
                    break;
            }
        }
    }
}
