namespace Lib.Model
{
    public class Secao
    {
        public string Name { get; set; }
        public int EleitoresAptos { get; set; }
        public int Comparecimento { get; set; }
        public int EleitoresFaltosos { get; set; }

        public Dictionary<string, int> Votos {get;set;} = new Dictionary<string, int>();
    }
}