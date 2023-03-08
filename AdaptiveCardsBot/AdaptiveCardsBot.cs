// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AdaptiveCardsBot
{
    public class AdaptiveCardsBot : ActivityHandler
    {

        private const string WelcomeText = @"This bot will introduce you to AdaptiveCards.
                                            Type anything to see an AdaptiveCard.";

        private readonly string[] _cards =
        {
            Path.Combine(".", "Resources", "FlightItineraryCard.json"),
            Path.Combine(".", "Resources", "ImageGalleryCard.json"),
            Path.Combine(".", "Resources", "LargeWeatherCard.json"),
            Path.Combine(".", "Resources", "RestaurantCard.json"),
            Path.Combine(".", "Resources", "SolitaireCard.json"),
        };
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            await SendWelcomeMessageAsync(turnContext, cancellationToken);
        }


        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            Random r = new Random();

            var cardAttachment = CreateAdaptiveCardAttachment(_cards[r.Next(_cards.Length)]);

            await turnContext.SendActivityAsync(MessageFactory.Attachment(cardAttachment), cancellationToken);
            await turnContext.SendActivityAsync(MessageFactory.Text("Please enter any text to see another card."), cancellationToken);
        }

        private static async Task SendWelcomeMessageAsync(ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in turnContext.Activity.MembersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync($"Welcome to Adaptive Cards Bot {member.Name}. {WelcomeText}", cancellationToken: cancellationToken);
                }
            }
        }

        private static Attachment CreateAdaptiveCardAttachment(string filePath)
        {
            var adaptiveCardJson = File.ReadAllText(filePath);
            var adaptiveCardAttechment = new Attachment
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCardJson)
            };

            return adaptiveCardAttechment;
        }
    }
}
