using BoardSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoveSystem
{
    public class CardManager<TPiece> where TPiece : class, IPiece<TPiece>
    {
        private Dictionary<string, ICardCommandProvider<TPiece>> _providers = new Dictionary<string, ICardCommandProvider<TPiece>>();
        private ICardCommandProvider<TPiece> _activeProvider;
        private List<HexTile> _validTiles = new List<HexTile>();
        private Board<TPiece> _board;

        public CardManager(Board<TPiece> board)
        {
            _board = board;
        }

        public void Register(string name, ICardCommandProvider<TPiece> provider)
        {
            if (_providers.ContainsKey(name))
                return;

            _providers.Add(name, provider);
        }

        public ICardCommand<TPiece> GetCardCommand(string name)
        {
            _providers.TryGetValue(name, out var cardCommandProvider);
            return cardCommandProvider.Commands()[0];
        }
        public List<HexTile> SetTiles(List<HexTile> hexTiles)
        {
            _validTiles = hexTiles;
            return _validTiles;
        }
        public List<HexTile> Tiles()
        {
            return _validTiles;
        }
    }
}