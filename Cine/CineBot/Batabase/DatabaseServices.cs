using CineBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineBot.Batabase
{
    public class DatabaseServices
    {
        public static List<PeliculasModels> GetPiliculas()
        {
            var lista = new List<PeliculasModels>()
            {
                new PeliculasModels()
                {
                    nombre = "Flash",
                    precio = "9.40",
                    imagen = "https://ticochatbotdevstorage.blob.core.windows.net/images/movie/1_flash.jpg",
                    informacion = "http://www.sensacine.com/peliculas/pelicula-59075",
                },
                new PeliculasModels()
                {
                    nombre = "Los vengadores",
                    precio = "7.89",
                    imagen = "https://ticochatbotdevstorage.blob.core.windows.net/images/movie/2_vengadores.jpg",
                    informacion = "http://www.sensacine.com/peliculas/pelicula-130440",
                },
                new PeliculasModels()
                {
                    nombre = "Joker",
                    precio = "7.98",
                    imagen = "https://ticochatbotdevstorage.blob.core.windows.net/images/movie/3_joker.png",
                    informacion = "http://www.sensacine.com/peliculas/pelicula-258374",
                }
            };
            return lista;
        }
    }
}
