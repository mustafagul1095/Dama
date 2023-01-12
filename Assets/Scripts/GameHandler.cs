using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private Board _board;
    [SerializeField] private Pieces _pieces;

    public Board Board => _board;
    public Pieces Pieces => _pieces;
    
    
    private TurnHandler _turnHandler = new TurnHandler();
    public TurnHandler TurnHandler => _turnHandler;

    private bool _clickedAPiece = false;

    private bool _redHasToTake = false;
    public bool RedHasToTake => _redHasToTake;
    
    private bool _whiteHasToTake = false;
    public bool WhiteHasToTake => _whiteHasToTake;
    
    
    private Piece _currentPiece;
    
    public Piece CurrentPiece => _currentPiece;
    
    public Action OnRefreshTiles;
    public Action OnMovePiece;

    private void Awake()
    {
        Board.Init();
        Pieces.Init();
        Pieces.PrepareForNextTurn();
    }

    public void PlaceToBoard(Piece piece, int x, int y)
    {
        _board.Matrix[x, y].Piece = piece;
        piece.SetTile(_board.Matrix[x, y]);
    }

    public void RemoveFromBoard(int x, int y)
    {
        _board.Matrix[x, y].Piece = null;
    }

    public void ClearTilePlayabilityMatrix()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                DisableTilePlayability(i, j);
            }
        }
    }

    public void ChangeTilePlayability(int value, int x, int y)
    {
        _board.Matrix[x, y].Playable = value == 1;
    }

    public void DisableTilePlayability(int x, int y)
    {
        _board.Matrix[x, y].Playable = false;
    }
    
    public void RefreshTiles()
    {
        OnRefreshTiles?.Invoke();
    }

    public void SetCurrentPiece(Piece piece)
    {
        _currentPiece = piece;
    }

    public void SetRedHasToTake(bool state)
    {
        _redHasToTake = state;
    }
    
    public void SetWhiteHasToTake(bool state)
    {
        _whiteHasToTake = state;
    }

    public void MovePiece(int x, int y)
    {
        _currentPiece.ChangeCoordinate(x,y);
        TurnHandler.PassTurn();
        OnMovePiece?.Invoke();
        _pieces.PrepareForNextTurn();
    }

}
