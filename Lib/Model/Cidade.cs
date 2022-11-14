namespace Lib.Model
{
    public class Cidade
    {
        public string Name {get;set;}

        public string Uf { get; set; }

        public List<Municipio> Municipios { get; set; } = new List<Municipio>();
    }
}