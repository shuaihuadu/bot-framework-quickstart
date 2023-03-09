// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EmptyBot v4.18.1

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot
{
    public class FlightBookingRecognizer : IRecognizer
    {
        private readonly LuisRecognizer _recognizer;

        public FlightBookingRecognizer(IConfiguration configuration)
        {
            var luisAppId = configuration["LuisAppId"];
            var luisAPIKey = configuration["LuisAPIKey"];
            var luisAPIHostName = configuration["LuisAPIHostName"];

            var luisConfigured = !string.IsNullOrEmpty(luisAppId) && !string.IsNullOrEmpty(luisAPIKey) && !string.IsNullOrEmpty(luisAPIHostName);

            if (luisConfigured)
            {
                var luisApplication = new LuisApplication(luisAppId, luisAPIKey, $"https://{luisAPIHostName}");

                var recognizerOptions = new LuisRecognizerOptionsV3(luisApplication)
                {
                    PredictionOptions = new Microsoft.Bot.Builder.AI.LuisV3.LuisPredictionOptions
                    {
                        IncludeInstanceData = true
                    }
                };

                _recognizer = new LuisRecognizer(recognizerOptions);
            }
        }

        public virtual bool IsConfigured => _recognizer != null;

        public virtual async Task<RecognizerResult> RecognizeAsync(ITurnContext turnContext, CancellationToken cancellationToken) => await _recognizer.RecognizeAsync(turnContext, cancellationToken);

        public virtual async Task<T> RecognizeAsync<T>(ITurnContext turnContext, CancellationToken cancellationToken) where T : IRecognizerConvert, new() => await _recognizer.RecognizeAsync<T>(turnContext, cancellationToken);
    }
}