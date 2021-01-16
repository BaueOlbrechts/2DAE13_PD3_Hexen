using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSystem.Utils
{
    public class CardCommandProviderAttribute : Attribute
    {
        public string Name;

        public CardCommandProviderAttribute(string name)
        {
            Name = name;
        }
    }
}
