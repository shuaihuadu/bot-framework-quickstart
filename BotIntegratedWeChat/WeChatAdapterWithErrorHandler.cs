﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder;
using WeChatAdapter;

namespace BotIntegratedWeChat
{
    public class WeChatAdapterWithErrorHandler : WeChatHttpAdapter
    {
        public WeChatAdapterWithErrorHandler(WeChatSettings settings, IStorage storage, IBackgroundTaskQueue taskQueue, ILogger logger = null, ConversationState conversationState = null, UserState userState = null)
            : base(settings, storage, taskQueue, logger)
        {
            OnTurnError = async (turnContext, exception) =>
            {
                // Log any leaked exception from the application.
                logger.LogError($"Exception caught : {exception.Message}");

                // Send a catch-all apology to the user.
                await turnContext.SendActivityAsync("Sorry, it looks like something went wrong.");

                if (conversationState != null)
                {
                    try
                    {
                        // Delete the conversationState for the current conversation to prevent the
                        // bot from getting stuck in a error-loop caused by being in a bad state.
                        // ConversationState should be thought of as similar to "cookie-state" in a Web pages.
                        await conversationState.DeleteAsync(turnContext);
                    }
                    catch (Exception e)
                    {
                        logger.LogError($"Exception caught on attempting to Delete ConversationState : {e.Message}");
                    }
                }
            };

            this.Use(new AutoSaveStateMiddleware(conversationState, userState));
        }
    }
}
