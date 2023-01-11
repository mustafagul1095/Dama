using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private Grid
    
    private Piece _currentPiece;

    private int[,] _gridArray = new int[10, 10];
    public int[,] GridArray => _gridArray;
    
    private int[,] _cubePlayabilityGrid = new int[10, 10];
    public int[,] CubePlayabilityGrid => _cubePlayabilityGrid;

    private int[,] _piecePlayabilityGrid = new int[10, 10];
    public int[,] PiecePlayabilityGrid => _piecePlayabilityGrid;
    
    private bool _redPlayersTurn = false;
    private bool _clickedAPiece = false;

    private bool _hasToTake = false;
    public bool HasToTake => _hasToTake;
    
    private bool _pieceMoved = false;

    public Action OnClickedAPiece;
    public Action OnMovePiece;
    
    

    private void Awake()
    {
        FillGridEdges();
    }
    
    private void FillGridEdges()
    {
        for (int i = 0; i < 10; i++)
        {
            _gridArray[0, i] = 3;
            _gridArray[9, i] = 3;
            _gridArray[i, 0] = 3;
            _gridArray[i, 9] = 3;
        }
    }

    public void PlaceToGridArray(int value, int x, int y)
    {
        _gridArray[x, y] = value;
    }

    public void RemoveFromGridArray(int x, int y)
    {
        _gridArray[x, y] = 0;
    }

    public void ClearCubePlayabilityGrid()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                _cubePlayabilityGrid[i, j] = 0;
            }
        }
    }
    
    public void ClearPiecePlayabilityGrid()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                _piecePlayabilityGrid[i, j] = 0;
            }
        }
    }
    
    public void PlaceToCubePlayabilityGridArray(int value, int x, int y)
    {
        _cubePlayabilityGrid[x, y] = value;
    }

    public void RemoveFromCubePlayabilityGridArray(int x, int y)
    {
        _cubePlayabilityGrid[x, y] = 0;
    }
    
    public void PlaceToPiecePlayabilityGridArray(int value, int x, int y)
    {
        _piecePlayabilityGrid[x, y] = value;
    }

    public void RemoveFromPiecePlayabilityGridArray(int x, int y)
    {
        _piecePlayabilityGrid[x, y] = 0;
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
    
    public void SetHasToTake(bool state)
    {
        _hasToTake = state;
    }
    
    public void MovePiece(int x, int y)
    {
        _hasToTake = false;
        _currentPiece.ChangeCoordinate(x,y);
        OnMovePiece?.Invoke();
    }


}
