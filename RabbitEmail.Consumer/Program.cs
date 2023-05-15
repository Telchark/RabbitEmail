using MimeKit;
using RabbitEmail.Consumer;
using RabbitEmail.Shared;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

var uri = new Uri("amqp://guest:guest@localhost:5672/EmailHost");
var connectionFactory = new ConnectionFactory
{
    Uri = uri,
    AutomaticRecoveryEnabled = true,
    DispatchConsumersAsync = false
};

using var consumer = new Consumer(connectionFactory);
consumer.ConnectToServer();
QueueListener(consumer);
Console.WriteLine("Listening on the queue. Press any key to finish.");
Console.ReadKey();

static void QueueListener(Consumer consumer)
{
    var sender = new EmailSender();
    var eventConsumer = new EventingBasicConsumer(consumer.Channel);
    eventConsumer.Received += (ch, ea) =>
    {
        var body = Encoding.UTF8.GetString(ea.Body.ToArray());
        var mailMessageDTO = new MailMessageDTO();
        if (body != null)
        {
            mailMessageDTO = JsonSerializer.Deserialize<MailMessageDTO>(body);
            MimeMessage mimeMailMessage = MailMessageDtoToMimieMessage(mailMessageDTO);
            sender.Send(mimeMailMessage, mailMessageDTO.SendingMethod);
        }
        consumer.Channel.BasicAck(ea.DeliveryTag, false);
    };
    consumer.Channel.BasicConsume(consumer.QueueName, false, eventConsumer);
}

static MimeMessage MailMessageDtoToMimieMessage(MailMessageDTO mailMessage)
{
    var mimeMessage = new MimeMessage();
    mimeMessage.From.Add(new MailboxAddress(mailMessage.SenderName, mailMessage.From));
    mimeMessage.To.Add(new MailboxAddress(mailMessage.ReciverName, mailMessage.To));
    mimeMessage.Subject = mailMessage.Subject;
    var builder = new BodyBuilder();
    builder.TextBody = mailMessage.Body;
    mimeMessage.Body = builder.ToMessageBody();
    return mimeMessage;
}