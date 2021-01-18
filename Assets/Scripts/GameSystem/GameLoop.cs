using BoardSystem;
using GameSystem.CardCommandProviders;
using GameSystem.CardCommands;
using GameSystem.Models;
using GameSystem.MoveCommands;
using GameSystem.Views;
using MoveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class GameLoop : SingletonMonoBehaviour<GameLoop>
{
    [SerializeField]
    private PositionHelper _positionHelper = null;

    [SerializeField]
    private static int _boardRings = 3;

    private BoardPiece _playerPiece = null;
    private HexTile _hoverTile = null;
    private ICardCommand<BoardPiece> _currentCardCommand = null;
    private int _turnsBeforeEnemyTurn = 2;
    private int _currentTurn = 0;

    public Board<BoardPiece> Board { get; } = new Board<BoardPiece>(_boardRings);
    public BoardPiece PlayerPiece => _playerPiece;
    public HexTile PlayerTile => Board.TileOf(PlayerPiece);
    public HexTile SelectedTile => _hoverTile;
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
        SetNewPieceToTiles();
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
            Board.UnHighlight(CardManager.Tiles());

            _hoverTile = hexTile;

            Board.Highlight(CardManager.SetTiles(_currentCardCommand.HexTiles(Board, PlayerTile, _hoverTile)));
        }
        else if (_playerPiece != null && _currentCardCommand == null)
        {
            if (CardManager.Tiles() != null)
                Board.UnHighlight(CardManager.Tiles());

            var piece = Board.PieceAt(hexTile);
            if (piece != null && piece != _playerPiece)
            {
                Board.Highlight(CardManager.SetTiles(new PiecePathFindingMoveCommand().Tiles(Board, piece, piece.ToTile)));
            }
        }
    }

    public void SelectTile(HexTile hexTile)
    {
        if (_playerPiece != null && _currentCardCommand != null)
        {
            if (CardManager.Tiles().Contains(hexTile))
            {
                Board.UnHighlight(CardManager.Tiles());

                _currentCardCommand.Execute(Board, _playerPiece, _hoverTile);

                OnCardUsed(new EventArgs());


                CardManager.SetTiles(null);
                _hoverTile = null;
                _currentCardCommand = null;

                _currentTurn++;
                if(_currentTurn >= _turnsBeforeEnemyTurn)
                {
                    _currentTurn = 0;
                    MovePiecesToTile();
                    SetNewPieceToTiles();
                }
            }
        }
    }

    public void SelectCard(ICardCommand<BoardPiece> cardCommand)
    {
        if (_currentCardCommand != null)
            Board.UnHighlight(CardManager.Tiles());

        _currentCardCommand = cardCommand;

        if (_currentCardCommand != null)
            Board.Highlight(CardManager.SetTiles(_currentCardCommand.HexTiles(Board, PlayerTile, _hoverTile)));
    }

    public void MovePiecesToTile()
    {
        var piecesToMove = Board.Pieces;
        piecesToMove.Remove(_playerPiece);

        foreach (var piece in piecesToMove)
        {
            Board.Move(Board.TileOf(piece), piece.ToTile);
        }
    }
    public void SetNewPieceToTiles()
    {
        List<HexTile> availableTilesAroundPlayer = new CommandHelper(Board, PlayerPiece).AllDirections(1).GenerateTiles();
        var piecesToCheck = Board.Pieces;
        piecesToCheck.Remove(_playerPiece);

        foreach (var piece in Board.Pieces)
        {
            HexTile pieceTile = Board.TileOf(piece);

            if (availableTilesAroundPlayer.Contains(pieceTile))
            {
                piece.SetNewToTile(pieceTile);
                availableTilesAroundPlayer.Remove(pieceTile);
                piecesToCheck.Remove(piece);
            }
        }


        foreach (var piece in piecesToCheck)
        {
            HexTile pieceTile = Board.TileOf(piece);
            var fromPosition = pieceTile.HexPosition;

            if (availableTilesAroundPlayer.Count > 0)
            {
                HexTile nearestTile = availableTilesAroundPlayer[0];
                float distance = Mathf.Infinity;

                foreach (var tile in availableTilesAroundPlayer)
                {
                    var toPosition = tile.HexPosition;

                    var xDistance = Mathf.Abs(fromPosition.Q - toPosition.Q);
                    var yDistance = Mathf.Abs(fromPosition.R - toPosition.R);

                    var totalDistance = xDistance + yDistance;
                    if (totalDistance < distance)
                    {
                        distance = totalDistance;
                        nearestTile = tile;
                    }
                }

                piece.SetNewToTile(nearestTile);
                availableTilesAroundPlayer.Remove(nearestTile);
            }
            else
                piece.SetNewToTile(pieceTile);
        }

    }

    public void OnCardUsed(EventArgs arg)
    {
        EventHandler<EventArgs> handler = CardUsed;
        handler?.Invoke(this, arg);
    }
}