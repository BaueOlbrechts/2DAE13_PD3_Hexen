using GameSystem.CardCommands;
using GameSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSystem.CardCommandProviders
{
    [CardCommandProvider(LineAttackCardCommandProvider.Name)]
    public class LineAttackCardCommandProvider : AbstractCardCommandProvider
    {
        public const string Name = "LineAttack";
        public LineAttackCardCommandProvider() : base(new LineAttackCardCommand()) { }
    }
}