using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] private GameHandler gameHandler;
    private Vector2Int _coordinates = new Vector2Int();
    private bool _normalPlayable = false;
    private CapsuleCollider _collider;
    private bool _pieceHasToTake = false;
    private List<PieceToEliminate> _piecesToEliminate;

    private enum Side
    {
        White = 1,
        Red = 2
    };

    [SerializeField] private Side side;

    private void Awake()
    {
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
        _coordinates.x = Mathf.RoundToInt(transform.position.x / 10) + 1;
        _coordinates.y = Mathf.RoundToInt(transform.position.z / 10) + 1;
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
        gameHandler.PlaceToGridArray((int)side, _coordinates.x, _coordinates.y);
    }
    
    private void RemoveFromGrid()
    {
        gameHandler.RemoveFromGridArray(_coordinates.x, _coordinates.y);
    }

    private void CheckNormalPlayablity()
    {
        if (side == Side.White)
        {
            _normalPlayable = (gameHandler.GridArray[_coordinates.x + 1, _coordinates.y + 1] == 0 ||
                               gameHandler.GridArray[_coordinates.x - 1, _coordinates.y + 1] == 0);
        }
        else if (side == Side.Red)
        {

            _normalPlayable = (gameHandler.GridArray[_coordinates.x + 1, _coordinates.y - 1] == 0 ||
                               gameHandler.GridArray[_coordinates.x - 1, _coordinates.y - 1] == 0);
        }
    }

    private void CheckHasToTake()
    {
        Vector2Int placeToMoveOnTake;
        if (side == Side.White)
        {
            if (gameHandler.GridArray[_coordinates.x + 1, _coordinates.y + 1] == 2)
            {
                if (gameHandler.GridArray[_coordinates.x + 2, _coordinates.y + 2] == 0)
                {
                    placeToMoveOnTake = HasToTake(_coordinates.x + 2, _coordinates.y + 2);
                    gameHandler.PlaceToCubePlayabilityGridArray(1,placeToMoveOnTake.x,placeToMoveOnTake.y);
                    var pieceToEliminate = new PieceToEliminate();
                    pieceToEliminate.SetPieceToEliminateCoords(_coordinates.x + 1, _coordinates.y + 1);
                    _piecesToEliminate.Add(pieceToEliminate);
                }
            }
            if (gameHandler.GridArray[_coordinates.x - 1, _coordinates.y + 1] == 2)
            {
                if (gameHandler.GridArray[_coordinates.x - 2, _coordinates.y + 2] == 0)
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
            if (gameHandler.GridArray[_coordinates.x + 1, _coordinates.y - 1] == 1)
            {
                if (gameHandler.GridArray[_coordinates.x + 2, _coordinates.y - 2] == 0)
                {
                    placeToMoveOnTake = HasToTake(_coordinates.x + 2, _coordinates.y - 2);
                    gameHandler.PlaceToCubePlayabilityGridArray(1,placeToMoveOnTake.x,placeToMoveOnTake.y);
                    var pieceToEliminate = new PieceToEliminate();
                    pieceToEliminate.SetPieceToEliminateCoords(_coordinates.x + 1, _coordinates.y - 1);
                    _piecesToEliminate.Add(pieceToEliminate);
                }
            }
            if (gameHandler.GridArray[_coordinates.x - 1, _coordinates.y - 1] == 1)
            {
                if (gameHandler.GridArray[_coordinates.x - 2, _coordinates.y - 2] == 0)
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

    private void CheckPieceToEliminate()
    {
        if (Vector2Int.Distance(gameHandler.PieceToEliminate1.PieceToEliminateCoords, _coordinates) < 0.01)
        {
            gameHandler.SetPieceToEliminate(this);
        }
    }
    private Vector2Int HasToTake(int x, int y)
    {
        gameHandler.SetHasToTake(true);
        _pieceHasToTake = true;
        gameHandler.ClearPiecePlayabilityGrid();
        gameHandler.PlaceToPiecePlayabilityGridArray(1, _coordinates.x, _coordinates.y);
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
                if (gameHandler.GridArray[_coordinates.x + 1, _coordinates.y + 1] == 0 )
                {
                    gameHandler.PlaceToCubePlayabilityGridArray(1,_coordinates.x + 1, _coordinates.y + 1);
                }
                if (gameHandler.GridArray[_coordinates.x - 1, _coordinates.y + 1] == 0)
                {
                    gameHandler.PlaceToCubePlayabilityGridArray(1,_coordinates.x - 1, _coordinates.y + 1);
                }
            
            }
            else if (side == Side.Red)
            {
                if (gameHandler.GridArray[_coordinates.x + 1, _coordinates.y - 1] == 0 )
                {
                    gameHandler.PlaceToCubePlayabilityGridArray(1,_coordinates.x + 1, _coordinates.y - 1);
                }
                if (gameHandler.GridArray[_coordinates.x - 1, _coordinates.y - 1] == 0)
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
        CheckPieceToEliminate();
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
