using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private Pieces pieces;

    public Board Board => board;
    public Pieces Pieces => pieces;
    
    
    private readonly TurnHandler _turnHandler = new TurnHandler();
    public TurnHandler TurnHandler => _turnHandler;
    

    private bool _redHasToTake = false;
    public bool RedHasToTake => _redHasToTake;
    
    private bool _blackHasToTake = false;
    public bool BlackHasToTake => _blackHasToTake;
    
    
    private bool _redHasTaken = false;
    public bool RedHasTaken
    {
        get => _redHasTaken;
        set => _redHasTaken = value;
    }

    public bool BlackHasTaken { get; set; } = false;


    private Piece _currentPiece;
    
    public Piece CurrentPiece => _currentPiece;
    
    public Action OnRefreshTiles;
    public Action OnMovePiece;

    private void Awake()
    {
        Board.Init();
        Pieces.Init();
        Pieces.PrepareForNextTurn();
        Pieces.HandleClickEnable();

    }

    public void PlaceToBoard(Piece piece, int x, int y)
    {
        board.Matrix[x, y].Piece = piece;
        piece.SetTile(board.Matrix[x, y]);
    }

    public void RemoveFromBoard(int x, int y)
    {
        board.Matrix[x, y].Piece = null;
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
        board.Matrix[x, y].Playable = value == 1;
    }

    public void DisableTilePlayability(int x, int y)
    {
        board.Matrix[x, y].Playable = false;
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
    
    public void SetBlackHasToTake(bool state)
    {
        _blackHasToTake = state;
    }

    public void MovePiece(int x, int y)
    {
        ResetHasTaken();
        _currentPiece.ChangeCoordinate(x,y);
        OnMovePiece?.Invoke();
        Pieces.PrepareForNextTurn();
        CalculateSequentialCombo();
        Pieces.HandleClickEnable();
    }

    private void ResetHasTaken()
    {
        BlackHasTaken = false;
        _redHasTaken = false;
    }
    
    private void CalculateSequentialCombo()
    {
        if (TurnHandler.Turn == Side.Black)
        {
            if (!(_blackHasToTake && BlackHasTaken))
            {
                TurnHandler.PassTurn();
            }
        }
        else if (TurnHandler.Turn == Side.Red)
        {
            if (!(_redHasToTake && _redHasTaken))
            {
                TurnHandler.PassTurn();
            }
        }
    }
}
