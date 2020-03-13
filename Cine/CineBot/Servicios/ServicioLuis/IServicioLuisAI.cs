using Microsoft.Bot.Builder.AI.Luis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineBot.Servicios.ServicioLuis
{
    public interface IServicioLuisAI
    {
        public LuisRecognizer luisRecognizer { get; set; }
    }
}
