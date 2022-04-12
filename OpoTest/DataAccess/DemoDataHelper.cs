using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpoTest;
using OpoTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpoTest
{
    public static class DemoDataHelper
    {
        private static readonly string[] firstNames = new string[] {
            "Peter", "Ryan", "Richard", "Tom", "Mark", "Steve",
            "Jimmy", "Jeffrey", "Andrew", "Dave", "Bert", "Mike",
            "Ray", "Paul", "Brad", "Carl", "Jerry" };
        private static readonly string[] lastNames = new string[] {
            "Dolan", "Fischer", "Hamlett", "Hamilton", "Lee",
            "Lewis", "McClain", "Miller", "Murrel", "Parkins",
            "Roller", "Shipman", "Bailey", "Barnes", "Lucas", "Campbell" };
        private static readonly string[] productNames = new string[] {
            "Chai", "Chang", "Aniseed Syrup", "Chef Anton's Cajun Seasoning",
            "Chef Anton's Gumbo Mix", "Grandma's Boysenberry Spread",
            "Uncle Bob's Organic Dried Pears", "Northwoods Cranberry Sauce",
            "Mishi Kobe Niku", "Ikura", "Queso Cabrales", "Queso Manchego La Pastora",
            "Konbu", "Tofu", "Genen Shouyu", "Pavlova", "Alice Mutton",
            "Carnarvon Tigers", "Teatime Chocolate Biscuits",
            "Sir Rodney's Marmalade", "Sir Rodney's Scones",
            "Gustaf's Knäckebröd", "Tunnbröd", "Guaraná Fantástica",
            "NuNuCa Nuß-Nougat-Creme", "Gumbär Gummibärchen",
            "Schoggi Schokolade", "Rössle Sauerkraut", "Thüringer Rostbratwurst",
            "Nord-Ost Matjeshering", "Gorgonzola Telino", "Mascarpone Fabioli",
            "Geitost", "Sasquatch Ale", "Steeleye Stout", "Inlagd Sill",
            "Gravad lax", "Côte de Blaye", "Chartreuse verte",
            "Boston Crab Meat", "Jack's New England Clam Chowder",
            "Singaporean Hokkien Fried Mee", "Ipoh Coffee",
            "Gula Malacca", "Rogede sild", "Spegesild", "Zaanse koeken",
            "Chocolade", "Maxilaku", "Valkoinen suklaa", "Manjimup Dried Apples",
            "Filo Mix", "Perth Pasties", "Tourtière", "Pâté chinois",
            "Gnocchi di nonna Alice", "Ravioli Angelo", "Escargots de Bourgogne",
            "Raclette Courdavault", "Camembert Pierrot", "Sirop d'érable",
            "Tarte au sucre", "Vegie-spread", "Wimmers gute Semmelknödel",
            "Louisiana Fiery Hot Pepper Sauce", "Louisiana Hot Spiced Okra",
            "Laughing Lumberjack Lager", "Scottish Longbreads",
            "Gudbrandsdalsost", "Outback Lager", "Flotemysost",
            "Mozzarella di Giovanni", "Röd Kaviar", "Longlife Tofu",
            "Rhönbräu Klosterbier", "Lakkalikööri", "Original Frankfurter grüne Soße" };
        private static readonly Random Random = new Random();

        public static void Seed(Session session)
        {
            if (session.Query<PlantillaRespuesta>().Any()) return;
            var temas = new List<Tema>();
            for (int i = 1; i <= Random.Next(3, 5) - 1; i++)
            {
                var tema = new Tema(session) { Numero = i, Nombre = $"Tema {i}" };
                for (int j = 1; j <= Random.Next(3, 5) - 1; j++)
                {
                    var subTema = new Tema(session) { Numero = j, Nombre = $"Tema {i}.{j}", Padre = tema };
                    for (int k = 1; k <= Random.Next(3, 5) - 1; k++)
                        temas.Add(new Tema(session) { Numero = k, Nombre = $"Tema {i}.{j}.{k}", Padre = subTema });
                }
            }
            var names = new KeyValuePair<string, string>[firstNames.Length * lastNames.Length];
            for (int i = 0; i < firstNames.Length * lastNames.Length; i++)
            {
                int j = Random.Next(i + 1);
                names[i] = names[j];
                names[j] = new KeyValuePair<string, string>(firstNames[i / lastNames.Length], lastNames[i % lastNames.Length]);
            }
            for (int i = 0; i < 800; i++)
            {
                CreatePregunta(session, names[i % names.Length].Key, names[i % names.Length].Value, temas[Random.Next(temas.Count)]);
            }

            for (int i = 0; i < 20; i++)
            {
                var result = new Examen(session);
                result.Temas.Add(temas[i % temas.Count]);
                result.Preguntas.AddRange(
                    session.Query<PlantillaPregunta>()
                    .InTransaction()
                    .Where(x => x.Tema == temas[i % temas.Count])
                    .Take(80)
                    .Select(x => x.GenerarExamenPregunta()));
                result.Fecha = DateTime.Now;
            }
        }

        private static void CreatePregunta(Session session, string firstName, string lastName, Tema tema)
        {
            PlantillaPregunta pregunta = new PlantillaPregunta(session);
            pregunta.Enunciado = firstName;
            pregunta.Explicacion = lastName;
            pregunta.Tema = tema;

            for (int i = 0; i < 4; i++)
            {
                PlantillaRespuesta order = new PlantillaRespuesta(session);
                order.Explicacion = productNames[Random.Next(productNames.Length)];
                order.Texto = $"Respuesta  {Random.Next(1000)}";
                order.Correcta = i == 1;
                order.Pregunta = pregunta;
            }
            pregunta.Enunciado = "La respuesta correcta es" + pregunta.Respuestas.First(x => x.Correcta).Texto;
        }
    }
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseXpoDemoData(this IApplicationBuilder app)
        {
            var config = app.ApplicationServices.GetService<IConfiguration>();
            var connectionString = config.GetConnectionString("ConnectionString");
            using (var objectSpaceProvider = new XPObjectSpaceProvider(connectionString))
            {

                SecurityProviderService.RegisterEntities(objectSpaceProvider);
                using var objectSpace = objectSpaceProvider.CreateUpdatingObjectSpace(true) as XPObjectSpace;
                objectSpace.Session.UpdateSchema();

                DemoDataHelper.Seed(objectSpace.Session);
            }
            return app;
        }
    }
}