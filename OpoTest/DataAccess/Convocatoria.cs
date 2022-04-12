using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpoTest
{
    public class Convocatoria : XPObject
    {
        private string nombre;
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [Association]
        public XPCollection<Tema> Temas => GetCollection<Tema>(nameof(Temas));

    }
}
