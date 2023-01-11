using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] private GameHandler gameHandler;
    private Board _board;
    private Vector2Int _coordinates = new Vector2Int();
    private bool _normalPlayable = false;
    private CapsuleCollider _collider;
    private bool _pieceHasToTake = false;
    private List<PieceToEliminate> _piecesToEliminate;
    
    [SerializeField] private Side side;

    private void Awake()
    {
        _board = gameHandler.Board;
        _collider = GetComponent<CapsuleCollider>();
        FindCoordinates();
        PlaceToGrid();
        gameHandler.OnMovePiece += OnMovePiece;
    }
    
    private void Start()
    {
        CheckNormalPlayablity();
        HandleClickEnable();
    }
    
    private void FindCoordinates()
    {
        _coordinates = Vector2IntExtensions.GetCoordinateFromPosition(transform.position);
    }

    private Vector3 CoordinatesToPosition(int coordinateX, int coordinateY)
    {
        return new Vector3((coordinateX - 1) * 10, transform.position.y, (coordinateY - 1) * 10);
    }

    public void ChangeCoordinate(int x, int y)
    {
        RemoveFromGrid();
        _coordinates.x = x;
        _coordinates.y = y;
        transform.position = CoordinatesToPosition(x, y);
        PlaceToGrid();
    }
    
    private void PlaceToGrid()
    {
        gameHandler.PlaceToGridArray(this, _coordinates.x, _coordinates.y);
    }
    
    private void RemoveFromGrid()
    {
        gameHandler.RemoveFromGridArray(_coordinates.x, _coordinates.y);
    }

    private void CheckNormalPlayablity()
    {
        if (side == Side.White)
        {
            _normalPlayable = (_board.Matrix[_coordinates.x + 1, _coordinates.y + 1].Piece == null ||
                               _board.Matrix[_coordinates.x - 1, _coordinates.y + 1].Piece == null);
        }
        else if (side == Side.Red)
        {
            _normalPlayable = (_board.Matrix[_coordinates.x + 1, _coordinates.y - 1].Piece == null ||
                               _board.Matrix[_coordinates.x - 1, _coordinates.y - 1].Piece == null);
        }
    }

    private void CheckHasToTake()
    {
        Vector2Int placeToMoveOnTake;
        if (side == Side.White)
        {
            if (_board.Matrix[_coordinates.x + 1, _coordinates.y + 1].Piece.side == Side.Red)
            {
                if (_board.Matrix[_coordinates.x + 2, _coordinates.y + 2].Piece == null)
                {
                    placeToMoveOnTake = HasToTake(_coordinates.x + 2, _coordinates.y + 2);
                    gameHandler.PlaceToCubePlayabilityGridArray(1,placeToMoveOnTake.x,placeToMoveOnTake.y);
                    var pieceToEliminate = new PieceToEliminate();
                    pieceToEliminate.SetPieceToEliminateCoords(_coordinates.x + 1, _coordinates.y + 1);
                    _piecesToEliminate.Add(pieceToEliminate);
                }
            }
            if (_board.Matrix[_coordinates.x - 1, _coordinates.y + 1].Piece.side == Side.Red)
            {
                if (_board.Matrix[_coordinates.x - 2, _coordinates.y + 2].Piece == null)
                {
                    placeToMoveOnTake = HasToTake(_coordinates.x - 2, _coordinates.y + 2);
                    gameHandler.PlaceToCubePlayabilityGridArray(1,placeToMoveOnTake.x,placeToMoveOnTake.y);
                    var pieceToEliminate = new PieceToEliminate();
                    pieceToEliminate.SetPieceToEliminateCoords(_coordinates.x - 1, _coordinates.y + 1);
                    _piecesToEliminate.Add(pieceToEliminate);
                }
            }
        }
        if (side == Side.Red)
        {
            if (_board.Matrix[_coordinates.x + 1, _coordinates.y - 1].Piece.side == Side.White)
            {
                if (_board.Matrix[_coordinates.x + 2, _coordinates.y - 2].Piece == null)
                {
                    placeToMoveOnTake = HasToTake(_coordinates.x + 2, _coordinates.y - 2);
                    gameHandler.PlaceToCubePlayabilityGridArray(1,placeToMoveOnTake.x,placeToMoveOnTake.y);
                    var pieceToEliminate = new PieceToEliminate();
                    pieceToEliminate.SetPieceToEliminateCoords(_coordinates.x + 1, _coordinates.y - 1);
                    _piecesToEliminate.Add(pieceToEliminate);
                }
            }
            if (_board.Matrix[_coordinates.x - 1, _coordinates.y - 1].Piece.side == Side.White)
            {
                if (_board.Matrix[_coordinates.x - 2, _coordinates.y - 2].Piece == null)
                {
                    placeToMoveOnTake = HasToTake(_coordinates.x - 2, _coordinates.y - 2);
                    gameHandler.PlaceToCubePlayabilityGridArray(1,placeToMoveOnTake.x,placeToMoveOnTake.y);
                    var pieceToEliminate = new PieceToEliminate();
                    pieceToEliminate.SetPieceToEliminateCoords(_coordinates.x - 1, _coordinates.y - 1);
                    _piecesToEliminate.Add(pieceToEliminate);
                }
            }
        }
    }

    private Vector2Int HasToTake(int x, int y)
    {
        gameHandler.SetHasToTake(true);
        _pieceHasToTake = true;
        return new Vector2Int(x, y);
    }

    private void HandleClickEnable()
    {
        if (gameHandler.HasToTake)
        {
            _collider.enabled = _pieceHasToTake;
        }
        else
        {
            _collider.enabled = _normalPlayable;
        }
    }
    
    private void OnMouseDown()
    {
        gameHandler.ClearCubePlayabilityGrid();
        CheckHasToTake();
        if (!gameHandler.HasToTake)
        {
            if (side == Side.White)
            {
                if (_board.Matrix[_coordinates.x + 1, _coordinates.y + 1].Piece == null)
                {
                    gameHandler.PlaceToCubePlayabilityGridArray(1,_coordinates.x + 1, _coordinates.y + 1);
                }
                if (_board.Matrix[_coordinates.x - 1, _coordinates.y + 1].Piece == null)
                {
                    gameHandler.PlaceToCubePlayabilityGridArray(1,_coordinates.x - 1, _coordinates.y + 1);
                }
            
            }
            else if (side == Side.Red)
            {
                if (_board.Matrix[_coordinates.x + 1, _coordinates.y - 1].Piece == null)
                {
                    gameHandler.PlaceToCubePlayabilityGridArray(1,_coordinates.x + 1, _coordinates.y - 1);
                }
                if (_board.Matrix[_coordinates.x - 1, _coordinates.y - 1].Piece == null)
                {
                    gameHandler.PlaceToCubePlayabilityGridArray(1,_coordinates.x - 1, _coordinates.y - 1);
                }
            }
        }
        gameHandler.SetClickedAPiece(true);
        gameHandler.SetCurrentPiece(this);
    }

    private void OnMovePiece()
    {
        _pieceHasToTake = false;
        CheckHasToTake();
        if (!gameHandler.HasToTake)
        {
            CheckNormalPlayablity();
        }
        HandleClickEnable();
    }

    private void OnDestroy()
    {
        gameHandler.OnMovePiece -= OnMovePiece; 
    }
}
