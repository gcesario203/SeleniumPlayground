namespace Lib.Model
{
    public class Municipio
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public List<Zona> Zonas {get;set;} = new List<Zona>();
    }
}