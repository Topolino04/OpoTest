using DevExpress.Xpo;

namespace OpoTest
{
    public class PlantillaRespuesta : XPObject
    {
        public PlantillaRespuesta(Session session) : base(session) { }

        private PlantillaPregunta pregunta;
        [Association]
        [Aggregated]
        public PlantillaPregunta Pregunta
        {
            get => pregunta;
            set => SetPropertyValue(nameof(Pregunta), ref pregunta, value);
        }

        private string texto;
        public string Texto
        {
            get => texto;
            set => SetPropertyValue(nameof(Texto), ref texto, value);
        }

        private bool correcta;
        public bool Correcta
        {
            get => correcta;
            set => SetPropertyValue(nameof(Correcta), ref correcta, value);
        }

        private string explicacion;
        [Size(200)]
        public string Explicacion
        {
            get => explicacion;
            set => SetPropertyValue(nameof(Explicacion), ref explicacion, value);
        }

        public void SetAgregation(object aggregatedObject)
        {
            if (aggregatedObject is PlantillaPregunta pregunta)
                Pregunta = Session.GetObjectByKey<PlantillaPregunta>(pregunta.Oid);
        }
    }
}
