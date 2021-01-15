using BoardSystem;
using GameSystem.Models;
using GameSystem.Views;
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

    public Board<BoardPiece> Board { get; } = new Board<BoardPiece>(_boardRings);

    private void Awake()
    {
        ConnectBoardView(Board);
        ConnectTileViews(Board);
    }

    private void ConnectBoardView(Board<BoardPiece> board)
    {
        var boardView = FindObjectOfType<BoardView>();
        boardView.Model = board;
    }

    private void ConnectTileViews(Board<BoardPiece> board)
    {
        var tileViews = FindObjectsOfType<HexTileView>();
        foreach (var tileView in tileViews)
        {
            var hexPosition = _positionHelper.ToHexPosition(board, tileView.transform.position);
            var tile = board.TileAt(hexPosition);
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
        }
    }
}