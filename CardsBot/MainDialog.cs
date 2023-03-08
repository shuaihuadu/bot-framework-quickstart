using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CardsBot
{
    public class MainDialog : ComponentDialog
    {
        protected readonly ILogger _logger;

        public MainDialog(ILogger<MainDialog> logger)
            : base(nameof(MainDialog))
        {
            _logger = logger;


            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                ChoiceCardStepAsync,
                ShowCardStepAsync
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> ChoiceCardStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            _logger.LogInformation("MainDialog.ChoiceCardStepAsync");

            var options = new PromptOptions
            {
                Prompt = MessageFactory.Text("What card would you like to see? You can click or type the card name"),
                RetryPrompt = MessageFactory.Text("That was not a valid choice, please select a card or number from 1 to 9."),
                Choices = GetChoices()
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), options, cancellationToken);
        }

        private async Task<DialogTurnResult> ShowCardStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            _logger.LogInformation("MainDialog.ShowCardStepAsync");

            var attachments = new List<Attachment>();
            var reply = MessageFactory.Attachment(attachments);

            switch (((FoundChoice)stepContext.Result).Value)
            {
                case "Adaptive Card":
                    reply.Attachments.Add(Cards.CreateAdaptiveCardAttachment());
                    break;
                case "Animation Card":
                    reply.Attachments.Add(Cards.GetAnimationCard().ToAttachment());
                    break;
                case "Audio Card":
                    reply.Attachments.Add(Cards.GetAudioCard().ToAttachment());
                    break;
                case "Hero Card":
                    reply.Attachments.Add(Cards.GetHeroCard().ToAttachment());
                    break;
                case "OAuth Card":
                    reply.Attachments.Add(Cards.GetOAuthCard().ToAttachment());
                    break;
                case "Receipt Card":
                    reply.Attachments.Add(Cards.GetReceiptCard().ToAttachment());
                    break;
                case "Signin Card":
                    reply.Attachments.Add(Cards.GetSigninCard().ToAttachment());
                    break;
                case "Thumbnail Card":
                    reply.Attachments.Add(Cards.GetThumbnailCard().ToAttachment());
                    break;
                case "Video Card":
                    reply.Attachments.Add(Cards.GetVideoCard().ToAttachment());
                    break;
                default:
                    reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    reply.Attachments.Add(Cards.CreateAdaptiveCardAttachment());
                    reply.Attachments.Add(Cards.GetAnimationCard().ToAttachment());
                    reply.Attachments.Add(Cards.GetAudioCard().ToAttachment());
                    reply.Attachments.Add(Cards.GetHeroCard().ToAttachment());
                    reply.Attachments.Add(Cards.GetOAuthCard().ToAttachment());
                    reply.Attachments.Add(Cards.GetReceiptCard().ToAttachment());
                    reply.Attachments.Add(Cards.GetSigninCard().ToAttachment());
                    reply.Attachments.Add(Cards.GetThumbnailCard().ToAttachment());
                    reply.Attachments.Add(Cards.GetVideoCard().ToAttachment());
                    break;
            }
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Type anything to see another card."), cancellationToken);
            return await stepContext.EndDialogAsync();
        }

        private IList<Choice> GetChoices()
        {
            var cardOptions = new List<Choice>
            {
                new Choice { Value ="Adaptive Card", Synonyms = new List<string>{ "adaptive" } },
                new Choice { Value ="Animation Card", Synonyms = new List<string>{ "animation" } },
                new Choice { Value ="Audio Card", Synonyms = new List<string>{ "audio" } },
                new Choice { Value ="Hero Card", Synonyms = new List<string>{ "hero" } },
                new Choice { Value ="OAuth Card", Synonyms = new List<string>{ "oauth" } },
                new Choice { Value ="Receipt Card", Synonyms = new List<string>{ "receipt" } },
                new Choice { Value ="Signin Card", Synonyms = new List<string>{ "signin" } },
                new Choice { Value ="Thumbnail Card", Synonyms = new List<string>{ "thumbnail", "thumb" } },
                new Choice { Value ="Video Card", Synonyms = new List<string>{ "video" } },
                new Choice { Value ="All Cards", Synonyms = new List<string>{ "all" } }
            };

            return cardOptions;
        }
    }
}
