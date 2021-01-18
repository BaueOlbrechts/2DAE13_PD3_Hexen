using BoardSystem;
using GameSystem.CardCommandProviders;
using GameSystem.CardCommands;
using GameSystem.Models;
using GameSystem.MoveCommands;
using GameSystem.States;
using GameSystem.Views;
using MoveSystem;
using StateSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class GameLoop : SingletonMonoBehaviour<GameLoop>
{
    [SerializeField]
    private PositionHelper _positionHelper = null;

    private StateMachine<GameStateBase> _stateMachine;
    public Board<BoardPiece> Board { get; } = new Board<BoardPiece>(3);


    private void Awake()
    {
        var cardManager = new CardManager<BoardPiece>(Instance.Board);
        cardManager.Register(KnockbackCardCommandProvider.Name, new KnockbackCardCommandProvider());
        cardManager.Register(LineAttackCardCommandProvider.Name, new LineAttackCardCommandProvider());
        cardManager.Register(MoveCardCommandProvider.Name, new MoveCardCommandProvider());
        cardManager.Register(SlashCardCommandProvider.Name, new SlashCardCommandProvider());

        ConnectTileViews();
        var playerPiece = ConnectBoardPieceViews();
        ConnectCardCommandProviderView(cardManager);

        var playerGameState = new PlayerGameState(Board, playerPiece, cardManager);
        var enemyGameState = new EnemyGameState(Board, playerPiece);

        _stateMachine = new StateMachine<GameStateBase>();
        _stateMachine.RegisterState(GameStates.Player, playerGameState);
        _stateMachine.RegisterState(GameStates.Enemy, enemyGameState);
        _stateMachine.MoveTo(GameStates.Enemy);
    }

    private void ConnectCardCommandProviderView(CardManager<BoardPiece> cardManager)
    {
        var cardCommandProviderView = FindObjectOfType<CardCommandProviderView>();
        cardCommandProviderView.CardManager = cardManager;
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

    private BoardPiece ConnectBoardPieceViews()
    {
        var pieceViews = FindObjectsOfType<BoardPieceView>();
        BoardPiece playerPiece = null;

        foreach (var pieceView in pieceViews)
        {
            var worldPosition = pieceView.transform.position;
            var boardPosition = _positionHelper.ToHexPosition(Board, worldPosition);
            var tile = Board.TileAt(boardPosition);

            var piece = new BoardPiece(pieceView.IsPlayer);

            Board.Place(tile, piece);

            pieceView.Model = piece;

            if (pieceView.IsPlayer)
                playerPiece = pieceView.Model;
        }
        return playerPiece;
    }


    public void HoverOver(HexTile hexTile)
    {
        _stateMachine.CurrentState.HoverOver(hexTile);

        /*
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
        */
    }

    public void SelectTile(HexTile hexTile)
    {
        _stateMachine.CurrentState.SelectTile(hexTile);

        /*
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
        */
    }

    public void SelectCard(ICardCommand<BoardPiece> cardCommand)
    {
        _stateMachine.CurrentState.SelectCard(cardCommand);

        /*
        if (_currentCardCommand != null)
            Board.UnHighlight(CardManager.Tiles());

        _currentCardCommand = cardCommand;

        if (_currentCardCommand != null)
            Board.Highlight(CardManager.SetTiles(_currentCardCommand.HexTiles(Board, PlayerTile, _hoverTile)));
        */
    }

    /*
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
    */
}