using RabbitEmail.Producer;
using RabbitEmail.Shared;
using RabbitMQ.Client;
using System.Text.Json;

var uri = new Uri("amqp://guest:guest@localhost:5672/EmailHost");
var connectionFactory = new ConnectionFactory
{
    Uri = uri,
    AutomaticRecoveryEnabled = true,
};

using var producer = new Producer(connectionFactory);
producer.ConnectToServer();

Console.WriteLine("Sending messages press ESC to stop.");

var exit = false;
var consoleTask = new Task(() =>
{
    while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;
    exit = true;
});
consoleTask.Start();

var message = new MailMessageDTO
{
    SenderName = "Andrzej",
    ReciverName = "Bober",
    From = "andrzej@email.pl",
    To = "bober@email.pl",
    Subject = "Title",
    Body = "Text",
    SendingMethod = SendingMethod.SMTP
};
var body = JsonSerializer.Serialize(message);

var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(1));
while (await periodicTimer.WaitForNextTickAsync() && !exit)
{
    producer.Publish(body, Bindings.Email.ToString());
}

Console.WriteLine("Sending has been stopped.");
Console.ReadKey();
