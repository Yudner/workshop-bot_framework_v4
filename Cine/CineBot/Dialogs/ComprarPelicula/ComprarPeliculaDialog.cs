using CineBot.Batabase;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CineBot.Dialogs.ComprarPelicula
{
    public class ComprarPeliculaDialog: ComponentDialog
    {
        public ComprarPeliculaDialog()
        {
            var waterfallStep = new WaterfallStep[]
            {
                MostrarPeliculas,
                PedirNombre,
                PedirDNI,
                ConfirmeCompra,
                Fin
            };
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallStep));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
        }

        private async Task<DialogTurnResult> MostrarPeliculas(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Esta es nuestra cartelera", cancellationToken: cancellationToken);
            
            await Task.Delay(1500);
            return await stepContext.PromptAsync(
             nameof(TextPrompt),
             new PromptOptions { Prompt = Opciones()}
            );
        }

        private Activity Opciones()
        {
            var ListaPeliculas = DatabaseServices.GetPiliculas();

            var listAttachments = new List<Attachment>();

            foreach (var item in ListaPeliculas)
            {
                var card = new HeroCard()
                {
                    Title = item.nombre,
                    Subtitle = $"Precio: {item.precio}",
                    Images = new List<CardImage>() { new CardImage(item.imagen)},
                    Buttons =  new List<CardAction>()
                    {
                        new CardAction(){Title = "Comprar", Value =item.nombre, Type = ActionTypes.ImBack},
                        new CardAction(){Title = "Ver información", Value =item.informacion, Type = ActionTypes.OpenUrl}
                    }
                };
                listAttachments.Add(card.ToAttachment());
            }

            var reply = MessageFactory.Attachment(listAttachments);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            return reply as Activity;
        }

        private async Task<DialogTurnResult> PedirNombre(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(
                 nameof(TextPrompt),
                 new PromptOptions { Prompt = MessageFactory.Text("Por favor ingresa tu nombre completo")}
            );
        }

        private async Task<DialogTurnResult> PedirDNI(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userName = stepContext.Context.Activity.Text;

            return await stepContext.PromptAsync(
                 nameof(TextPrompt),
                 new PromptOptions { Prompt = MessageFactory.Text($"Genial {userName}, ahora ingresa tu DNI") },
                 cancellationToken
            );
        }

        private async Task<DialogTurnResult> ConfirmeCompra(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(
                nameof(TextPrompt),
                new PromptOptions { Prompt = ButtonConfirmation()},
                cancellationToken
            );
        }

        private Activity ButtonConfirmation()
        {
            var reply = MessageFactory.Text("¿Confirmas esta compra?");

            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction(){Title = "Si", Value = "Si", Type = ActionTypes.ImBack},
                    new CardAction(){Title = "No", Value = "No", Type = ActionTypes.ImBack}

                }
            };

            return reply;
        }

        private async Task<DialogTurnResult> Fin(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userOption = stepContext.Context.Activity.Text.ToLower();

            switch (userOption)
            {
                case "si":
                    await stepContext.Context.SendActivityAsync("Felicidades, tu compra se realizó con éxito.", cancellationToken: cancellationToken);
                    break;
                case "no":
                    await stepContext.Context.SendActivityAsync("No hay problema, será para la próxima", cancellationToken: cancellationToken);
                    break;
                default:
                    await stepContext.BeginDialogAsync(nameof(RootDialog), cancellationToken: cancellationToken);
                    break;
            }

            return await stepContext.ContinueDialogAsync(cancellationToken: cancellationToken);
        }
    }
}
