using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace ConsoleEchoBot
{
    public class EchoBot : IBot
    {
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            if (turnContext.Activity.Type == ActivityTypes.Message && !string.IsNullOrEmpty(turnContext.Activity.Text))
            {
                if (turnContext.Activity.Text.Equals("quit", StringComparison.OrdinalIgnoreCase))
                {
                    await turnContext.SendActivityAsync("Bye!", cancellationToken: cancellationToken);
                }
                else
                {
                    await turnContext.SendActivityAsync($"You sent '{turnContext.Activity.Text}'", cancellationToken: cancellationToken);
                }
            }
            else
            {
                await turnContext.SendActivityAsync($"{turnContext.Activity.Type} event detected", cancellationToken: cancellationToken);
            }
        }
    }
}
