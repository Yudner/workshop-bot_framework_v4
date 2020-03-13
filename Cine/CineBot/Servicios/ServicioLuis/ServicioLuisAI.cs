using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineBot.Servicios.ServicioLuis
{
    public class ServicioLuisAI: IServicioLuisAI
    {
        public LuisRecognizer luisRecognizer { get; set; }
        public ServicioLuisAI(IConfiguration configuration)
        {
            var luisApplication = new LuisApplication(
             configuration["Luis.AppId"],
             configuration["Luis.ApiKey"],
             configuration["Luis.HostName"]                
           );

            var recognizerOptions = new LuisRecognizerOptionsV3(luisApplication)
            {
                PredictionOptions = new Microsoft.Bot.Builder.AI.LuisV3.LuisPredictionOptions()
                {
                    IncludeInstanceData = true
                }
            };
            luisRecognizer = new LuisRecognizer(recognizerOptions);
        }
    }
}
