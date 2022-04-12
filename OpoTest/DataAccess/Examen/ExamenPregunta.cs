using DevExpress.Xpo;
using System.Linq;

namespace OpoTest
{
    public class ExamenPregunta : XPObject
    {
        public ExamenPregunta(Session session) : base(session) { }


        private Examen examen;
        [Association]
        [Aggregated]
        public Examen Examen
        {
            get => examen;
            set => SetPropertyValue(nameof(Examen), ref examen, value);
        }

        private PlantillaPregunta plantilla;
        public PlantillaPregunta Plantilla
        {
            get => plantilla;
            set => SetPropertyValue(nameof(Plantilla), ref plantilla, value);
        }

        private string enunciado;
        public string Enunciado
        {
            get => enunciado;
            set => SetPropertyValue(nameof(Enunciado), ref enunciado, value);
        }

        private string explicacion;
        public string Explicacion
        {
            get => explicacion;
            set => SetPropertyValue(nameof(Explicacion), ref explicacion, value);
        }

        private bool? estado;
        public bool? Estado
        {
            get => estado;
            set => SetPropertyValue(nameof(Estado), ref estado, value);
        }

        public void Resolver()
        {
            Estado = Respuestas.All(x => x.Seleccionada == x.Correcta);
        }

        public void RaiseEstadoChanged() => OnChanged(nameof(Estado));
        [Association]
        public XPCollection<ExamenRespuesta> Respuestas => GetCollection<ExamenRespuesta>(nameof(Respuestas));

    }
}
