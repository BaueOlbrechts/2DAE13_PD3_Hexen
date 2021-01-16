using GameSystem.Models;
using MoveSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSystem.CardCommandProviders
{
    public abstract class AbstractCardCommandProvider : ICardCommandProvider<BoardPiece>
    {
        private List<ICardCommand<BoardPiece>> _commands;

        public AbstractCardCommandProvider(params ICardCommand<BoardPiece>[] commands)
        {
            _commands = commands.ToList();
        }

        public List<ICardCommand<BoardPiece>> Commands()
        {
            return _commands;
        }
    }
}
