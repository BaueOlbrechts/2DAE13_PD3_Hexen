using BoardSystem;
using GameSystem.CardCommandProviders;
using GameSystem.Models;
using GameSystem.Views;
using MoveSystem;
using System;
using System.Collections;
using UnityEngine;
using Utils;

public class GameLoop : SingletonMonoBehaviour<GameLoop>
{
    [SerializeField]
    private PositionHelper _positionHelper = null;

    [SerializeField]
    private static int _boardRings = 3;

    private BoardPiece _playerPiece = null;
    private HexTile _selectedTile = null;
    private ICardCommand<BoardPiece> _currentCardCommand = null;

    public Board<BoardPiece> Board { get; } = new Board<BoardPiece>(_boardRings);
    public BoardPiece PlayerPiece => _playerPiece;
    public HexTile PlayerTile => Board.TileOf(PlayerPiece);
    public HexTile SelectedTile => _selectedTile;
    public CardManager<BoardPiece> CardManager { get; internal set; }


    public event EventHandler<EventArgs> CardUsed;


    private void Awake()
    {
        CardManager = new CardManager<BoardPiece>(Instance.Board);

        CardManager.Register(KnockbackCardCommandProvider.Name, new KnockbackCardCommandProvider());
        CardManager.Register(LineAttackCardCommandProvider.Name, new LineAttackCardCommandProvider());
        CardManager.Register(MoveCardCommandProvider.Name, new MoveCardCommandProvider());
        CardManager.Register(SlashCardCommandProvider.Name, new SlashCardCommandProvider());

        ConnectTileViews();
        ConnectBoardPieceViews();
    }

    private void ConnectTileViews()
    {
        var tileViews = FindObjectsOfType<HexTileView>();
        foreach (var tileView in tileViews)
        {
            var hexPosition = _positionHelper.ToHexPosition(Board, tileView.transform.position);
            var tile = Board.TileAt(hexPosition);
            tileView.Model = tile;
        }
    }

    private void ConnectBoardPieceViews()
    {
        var pieceViews = FindObjectsOfType<BoardPieceView>();
        foreach (var pieceView in pieceViews)
        {
            var worldPosition = pieceView.transform.position;
            var boardPosition = _positionHelper.ToHexPosition(Board, worldPosition);
            var tile = Board.TileAt(boardPosition);

            var piece = new BoardPiece(pieceView.IsPlayer);

            Board.Place(tile, piece);

            pieceView.Model = piece;

            if (pieceView.IsPlayer)
                _playerPiece = pieceView.Model;
        }
    }


    public void HoverOver(HexTile hexTile)
    {
        if (_playerPiece != null && _currentCardCommand != null)
        {
            Board.UnHighlight(_currentCardCommand.HexTiles(Board, PlayerTile, _selectedTile));

            _selectedTile = hexTile;

            Board.Highlight(_currentCardCommand.HexTiles(Board, PlayerTile, _selectedTile));
        }
    }

    public void SelectTile(HexTile hexTile)
    {
        if (_playerPiece != null && _currentCardCommand != null)
        {
            if (hexTile == _selectedTile)
            {
                Board.UnHighlight(_currentCardCommand.HexTiles(Board, PlayerTile, _selectedTile));

                _currentCardCommand.Execute(Board, _playerPiece, _selectedTile);

                OnCardUsed(new EventArgs());

                _selectedTile = null;
                _currentCardCommand = null;
            }
        }
    }

    public void SelectCard(ICardCommand<BoardPiece> cardCommand)
    {
        if (_currentCardCommand != null)
            Board.UnHighlight(_currentCardCommand.HexTiles(Board, PlayerTile, _selectedTile));

        _currentCardCommand = cardCommand;

        if (_currentCardCommand != null)
            Board.Highlight(_currentCardCommand.HexTiles(Board, PlayerTile, _selectedTile));
    }

    public void OnCardUsed(EventArgs arg)
    {
        EventHandler<EventArgs> handler = CardUsed;
        handler?.Invoke(this, arg);
    }
}