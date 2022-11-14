using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lib.Model
{
    public class Zona
    {
        public string Name { get; set; }

        public List<Secao> Secoes {get;set;} = new List<Secao>();
    }
}