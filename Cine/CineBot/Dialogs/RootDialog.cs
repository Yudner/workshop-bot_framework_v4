using CineBot.Dialogs.ComprarPelicula;
using CineBot.Servicios.ServicioLuis;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CineBot.Dialogs
{
    public class RootDialog: ComponentDialog
    {
        private readonly IServicioLuisAI _luisServiceAI;
        public RootDialog(IServicioLuisAI luisServiceAI)
        {
            _luisServiceAI = luisServiceAI;
            var waterfallStep = new WaterfallStep[]
            {
                Inicio,
                Fin
            };
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallStep));
            AddDialog(new ComprarPeliculaDialog());
        }

        private async Task<DialogTurnResult> Inicio(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var luisResult = await _luisServiceAI.luisRecognizer.RecognizeAsync(stepContext.Context, cancellationToken);
            return await Intenciones(stepContext, luisResult, cancellationToken);
        }

        private async Task<DialogTurnResult> Intenciones(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            var topIntent = luisResult.GetTopScoringIntent();
            switch (topIntent.intent)
            {
                case "Saludar":
                    await IntencionSaludar(stepContext, cancellationToken);
                    break;
                case "Despedir":
                    await IntencionDespedir(stepContext, cancellationToken);
                    break;
                case "Agradecer":
                    await IntencionAgradecer(stepContext, cancellationToken);
                    break;
                case "None":
                    await IntencionNone(stepContext, cancellationToken);
                    break;
                case "ComprarPelicula":
                    return await IntencionComprarPelicula(stepContext, cancellationToken);
                    
            }
            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task IntencionSaludar(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Hola, que gusto hablar contigo", cancellationToken: cancellationToken);
        }

        private async Task IntencionDespedir(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Espero verte pronto", cancellationToken: cancellationToken);
        }

        private async Task IntencionAgradecer(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Gracias a ti por escribirme", cancellationToken: cancellationToken);
        }

        private async Task IntencionNone(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Lo siento, pero no entiendo", cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> IntencionComprarPelicula(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.BeginDialogAsync(nameof(ComprarPeliculaDialog), cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> Fin(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
    }
}
