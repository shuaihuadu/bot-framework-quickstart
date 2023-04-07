using BotIntegratedWeChat;
using Microsoft.Bot.Builder;
using WeChatAdapter;

var builder = WebApplication.CreateBuilder(args);


builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});


builder.Services.AddControllers().AddXmlSerializerFormatters();
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Create the storage we'll be using for User and Conversation state. (Memory is great for testing purposes.)
builder.Services.AddSingleton<IStorage, MemoryStorage>();
// Create the User state. (Used in this bot's Dialog implementation.)
builder.Services.AddSingleton<UserState>();
// Create the Conversation state. (Used by the Dialog system itself.)
builder.Services.AddSingleton<ConversationState>();
// Load WeChat settings.
var wechatSettings = new WeChatSettings();
builder.Configuration.Bind("WeChatSettings", wechatSettings);
builder.Services.AddSingleton(wechatSettings);
// Configure hosted serivce.
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
builder.Services.AddHostedService<QueuedHostedService>();
builder.Services.AddSingleton<WeChatAdapterWithErrorHandler>();
// The Dialog that will be run by the bot.
builder.Services.AddSingleton<MainDialog>();
// Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
builder.Services.AddTransient<IBot, RichCardsBot>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();