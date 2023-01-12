using System;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private Board _board;
    private TurnHandler _turnHandler = new TurnHandler();
    public TurnHandler TurnHandler => _turnHandler;

    public Board Board => _board;

    private Piece _currentPiece;

    public Piece CurrentPiece => _currentPiece;

    private bool _redPlayersTurn = false;
    private bool _clickedAPiece = false;

    private List<Piece> whitePieces = new List<Piece>();
    private List<Piece> redPieces = new List<Piece>();

    private bool _redHasToTake = false;
    public bool RedHasToTake => _redHasToTake;
    
    private bool _whiteHasToTake = false;
    public bool WhiteHasToTake => _whiteHasToTake;
    
    private bool _pieceMoved = false;

    public Action OnClickedAPiece;
    public Action OnMovePiece;

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
    
    public void SetClickedAPiece(bool state)
    {
        _clickedAPiece = state;
        OnClickedAPiece?.Invoke();
    }

    public void SetCurrentPiece(Piece piece)
    {
        _currentPiece = piece;
    }

    public void SetPieceMoved(bool state)
    {
        _pieceMoved = state;
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
        SetRedHasToTake(false) ;
        SetWhiteHasToTake(false);
        _currentPiece.ChangeCoordinate(x,y);
        TurnHandler.PassTurn();
        OnMovePiece?.Invoke();
        
    }
    
}
