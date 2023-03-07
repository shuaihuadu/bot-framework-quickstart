using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace ConsoleEchoBot
{
    public class ConsoleAdapter : BotAdapter
    {
        public ConsoleAdapter() : base() { }

        public new ConsoleAdapter Use(IMiddleware middleware)
        {
            base.Use(middleware);
            return this;
        }

        public async Task ProcessActivityAsync(BotCallbackHandler callback = null)
        {
            while (true)
            {
                var msg = Console.ReadLine();
                if (msg == null)
                {
                    break;
                }

                var activity = new Activity
                {
                    Text = msg,
                    ChannelId = "console",
                    From = new ChannelAccount(id: "user", name: "User1"),
                    Recipient = new ChannelAccount(id: "bot", name: "Bot"),
                    Conversation = new ConversationAccount(id: "Convo1"),
                    Timestamp = DateTime.UtcNow,
                    Id = Guid.NewGuid().ToString(),
                    Type = ActivityTypes.Message
                };

                using (var context = new TurnContext(this, activity))
                {
                    await RunPipelineAsync(context, callback, default).ConfigureAwait(false);
                }
            }
        }

        public override async Task<ResourceResponse[]> SendActivitiesAsync(ITurnContext turnContext, Activity[] activities, CancellationToken cancellationToken)
        {
            if (turnContext == null)
            {
                throw new ArgumentNullException(nameof(turnContext));
            }
            if (activities == null)
            {
                throw new ArgumentException(nameof(activities));
            }
            if (activities.Length == 0)
            {
                throw new ArgumentException("Excepting one or more activities, but the array was empty.", nameof(activities));
            }

            var responses = new ResourceResponse[activities.Length];

            for (int index = 0; index < activities.Length; index++)
            {
                var activity = activities[index];

                switch (activity.Type)
                {
                    case ActivityTypes.Message:
                        {
                            IMessageActivity message = activity.AsMessageActivity();

                            if (message.Attachments != null && message.Attachments.Any())
                            {
                                var attachment = message.Attachments.Count == 1 ? "1 attachment" : $"{message.Attachments.Count} attachments";
                                Console.WriteLine($"{message.Text} with {attachment}");
                            }
                            else
                            {
                                Console.WriteLine($"{message.Text}");
                            }
                        }
                        break;
                    case ActivityTypesEx.Delay:
                        {
                            int delayMS = (int)activity.Value;
                            await Task.Delay(delayMS).ConfigureAwait(false);
                        }
                        break;
                    case ActivityTypes.Trace:
                        break;
                    default:
                        Console.WriteLine("Bot: activity type: {0}", activity.Type);
                        break;
                }

                responses[index] = new ResourceResponse(activity.Id);
            }

            return responses;
        }

        public override Task DeleteActivityAsync(ITurnContext turnContext, ConversationReference reference, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<ResourceResponse> UpdateActivityAsync(ITurnContext turnContext, Activity activity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
