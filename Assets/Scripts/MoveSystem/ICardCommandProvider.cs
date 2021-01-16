using BoardSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoveSystem
{
    public interface ICardCommandProvider<TPiece> where TPiece : class, IPiece<TPiece>
    {
        List<ICardCommand<TPiece>> Commands();
    }
}
