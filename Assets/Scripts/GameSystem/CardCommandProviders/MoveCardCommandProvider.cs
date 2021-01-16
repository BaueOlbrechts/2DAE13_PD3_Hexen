using GameSystem.CardCommands;
using GameSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSystem.CardCommandProviders
{
    [CardCommandProvider(MoveCardCommandProvider.Name)]
    public class MoveCardCommandProvider : AbstractCardCommandProvider
    {
        public const string Name = "Move";
        public MoveCardCommandProvider() : base(new MoveCardCommand()) { }
    }
}