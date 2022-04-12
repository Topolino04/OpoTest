using DevExpress.Xpo;

namespace OpoTest
{
    public class ExamenRespuesta : XPObject
    {
        public ExamenRespuesta(Session session) : base(session) { }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            switch (propertyName)
            {
                case nameof(seleccionada):
                    Pregunta?.RaiseEstadoChanged();
                    break;

            }
        }

        private ExamenPregunta pregunta;
        [Association]
        [Aggregated]
        public ExamenPregunta Pregunta
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

        private bool seleccionada;
        public bool Seleccionada
        {
            get => seleccionada;
            set => SetPropertyValue(nameof(Seleccionada), ref seleccionada, value);
        }

        private string explicacion;
        [Size(200)]
        public string Explicacion
        {
            get => explicacion;
            set => SetPropertyValue(nameof(Explicacion), ref explicacion, value);
        }
    }
}
