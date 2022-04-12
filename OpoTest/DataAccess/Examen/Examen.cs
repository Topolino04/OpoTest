using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpoTest
{
    public class Examen : XPObject
    {
        public Examen(Session session) : base(session) { }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Fecha = DateTime.Now;

        }

        private DateTime fecha;
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        [PersistentAlias("Preguntas[Estado].Count")]
        public int PreguntasCorrectas => (int)EvaluateAlias(nameof(PreguntasCorrectas));

        [PersistentAlias("Preguntas[Estado == false].Count")]
        public int PreguntasIncorrectas => (int)EvaluateAlias(nameof(PreguntasIncorrectas));

        [PersistentAlias("Preguntas[Estado is not null].Count")]
        public int PregntasRespondidas => (int)EvaluateAlias(nameof(PregntasRespondidas));

        [PersistentAlias("Preguntas[Estado is null].Count")]
        public int PreguntasPendientes => (int)EvaluateAlias(nameof(PreguntasPendientes));

        [PersistentAlias("Preguntas[].Count")]
        public int TotalPreguntas => (int)EvaluateAlias(nameof(TotalPreguntas));

        [Association]
        public XPCollection<Tema> Temas => GetCollection<Tema>(nameof(Temas));

        [Association]
        public XPCollection<ExamenPregunta> Preguntas => GetCollection<ExamenPregunta>(nameof(Preguntas));

    }
}
