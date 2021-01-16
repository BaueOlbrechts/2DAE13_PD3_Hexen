using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameSystem.Utils
{
    public class CardCommandProviderTypeHelper
    {
        private static string[] _cardNames = new string[0];

        public static string[] FindCardCommandProviderTypes()
        {
            if (_cardNames.Length == 0)
                _cardNames = InternalFindCardCommandProviderTypes();

            return _cardNames;
        }

        private static string[] InternalFindCardCommandProviderTypes()
        {
            var types = new List<string>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    var attribute = type.GetCustomAttribute<CardCommandProviderAttribute>();
                    if (attribute != null)
                    {
                        types.Add(attribute.Name);
                    }
                }
            }

            return types.ToArray();
        }
    }
}
