using GameSystem.CardCommands;
using GameSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSystem.CardCommandProviders
{
    [CardCommandProvider(SlashCardCommandProvider.Name)]
    public class SlashCardCommandProvider : AbstractCardCommandProvider
    {
        public const string Name = "Slash";
        public SlashCardCommandProvider() : base(new SlashCardCommand()) { }
    }
}