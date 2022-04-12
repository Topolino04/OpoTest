using DevExpress.Xpo;
using System.Collections.Generic;
using System.Linq;

namespace OpoTest
{
    public class Tema : XPObject
    {
        public Tema(Session session) : base(session) { }

        private string nombre;
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        public string Arbol => Padre != null ? $"{Padre.Arbol}.{Numero}" : Numero.ToString();

        public string DisplayFormatCompleto => $"{Arbol} {Nombre}";
        public string DisplayFormat => $"{Numero} {Nombre}";



        int numero;
        public int Numero
        {
            get => numero;
            set => SetPropertyValue(nameof(Numero), ref numero, value);
        }

        private string explicacion;
        [Size(200)]
        public string Explicacion
        {
            get => explicacion;
            set => SetPropertyValue(nameof(Explicacion), ref explicacion, value);
        }

        [Association]
        public XPCollection<PlantillaPregunta> Preguntas => GetCollection<PlantillaPregunta>(nameof(Preguntas));

        [Association]
        public XPCollection<Examen> Examenes => GetCollection<Examen>(nameof(Examenes));

        [Association]
        public XPCollection<Convocatoria> Convocatorias => GetCollection<Convocatoria>(nameof(Convocatorias));

        Tema padre;
        [Association("TemasGerarquia")]
        public Tema Padre
        {
            get => padre;
            set => SetPropertyValue(nameof(Padre), ref padre, value);
        }

        [Association("TemasGerarquia")]
        public XPCollection<Tema> Hijos => GetCollection<Tema>(nameof(Hijos));

        public IEnumerable<Tema> AllTemas => Session.GetObjectsByKeyFromQuery(ClassInfo, false, $"WITH q AS ( SELECT OID FROM Tema WHERE OID = {Oid} UNION ALL SELECT H.OID FROM Tema H, q P WHERE H.Padre = P.OID) SELECT * FROM q where OID != {Oid}").OfType<Tema>();

    }
}
