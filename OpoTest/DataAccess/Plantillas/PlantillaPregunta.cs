using System;
using System.Linq;
using DevExpress.Xpo;

namespace OpoTest

{
    public class PlantillaPregunta : XPObject
    {
        public PlantillaPregunta(Session session) : base(session) { }

        private Tema tema;
        [Association]
        public Tema Tema
        {
            get => tema;
            set => SetPropertyValue(nameof(Tema), ref tema, value);
        }

        private string enunciado;
        [Size(1000)]
        public string Enunciado
        {
            get { return enunciado; }
            set { SetPropertyValue(nameof(Enunciado), ref enunciado, value); }
        }

        private string explicacion;
        [Size(4000)]
        public string Explicacion
        {
            get { return explicacion; }
            set { SetPropertyValue(nameof(Explicacion), ref explicacion, value); }
        }

        [Association()]
        public XPCollection<PlantillaRespuesta> Respuestas => GetCollection<PlantillaRespuesta>(nameof(Respuestas));

        public ExamenPregunta GenerarExamenPregunta()
        {
            var result = new ExamenPregunta(Session)
            {
                Plantilla = this,
                Enunciado = Enunciado,
                Explicacion = Explicacion
            };
            result.Respuestas.AddRange(Respuestas
                .Select(x => new ExamenRespuesta(Session)
                {
                    Texto = x.Texto,
                    Explicacion = x.Explicacion,
                    Correcta = x.Correcta,
                }));
            return result;
        }
    }
}
