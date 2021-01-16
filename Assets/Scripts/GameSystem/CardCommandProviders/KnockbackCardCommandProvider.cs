using GameSystem.CardCommands;
using GameSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSystem.CardCommandProviders
{
    [CardCommandProvider(KnockbackCardCommandProvider.Name)]
    public class KnockbackCardCommandProvider : AbstractCardCommandProvider
    {
        public const string Name = "Knockback";
        public KnockbackCardCommandProvider() : base(new KnockbackCardCommand()) { }
    }
}
