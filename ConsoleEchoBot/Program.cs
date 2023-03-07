using ConsoleEchoBot;

Console.WriteLine("Console EchoBot is online. I will repeat any message you send me!");
Console.WriteLine("Say \"quit\" to end.");

var adapter = new ConsoleAdapter();
var echoBot = new EchoBot();

adapter.ProcessActivityAsync(async (turnContext, cancellationToken) => await echoBot.OnTurnAsync(turnContext)).Wait();