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
    private List<Piece> _piecesToEliminate = new List<Piece>(2);
    [SerializeField] private Side side;

    private Tile _tile;

    public void SetTile(Tile tile)
    {
        _tile = tile;
    }
    
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

    private void OnMovePiece()
    {
        if (!gameHandler.HasToTake && gameHandler.CurrentPiece == this)
        {
            EliminatePieces();
        }
        PrepareForNextTurn();
    }

    private void PrepareForNextTurn()
    {
        _pieceHasToTake = false;
        if (!gameHandler.HasToTake)
        {
            CheckNormalPlayablity();
        }

        HandleClickEnable();
    }

    private void OnMouseDown()
    {
        Debug.Log(gameHandler.TurnHandler.Turn);
        gameHandler.ClearTilePlayabilityMatrix();
        CheckHasToTake();
        ChangeTilePlayability();
        gameHandler.SetClickedAPiece(true);
        gameHandler.SetCurrentPiece(this);
    }

    private void ChangeTilePlayability()
    {
        if (!gameHandler.HasToTake)
        {
            if (side == Side.White && gameHandler.TurnHandler.Turn == Turn.White)
            {
                if (_board.Matrix[_coordinates.x + 1, _coordinates.y + 1].Piece == null &&
                    !_board.Matrix[_coordinates.x + 1, _coordinates.y + 1].IsWood)
                {
                    gameHandler.ChangeTilePlayability(1, _coordinates.x + 1, _coordinates.y + 1);
                }

                if (_board.Matrix[_coordinates.x - 1, _coordinates.y + 1].Piece == null &&
                    !_board.Matrix[_coordinates.x - 1, _coordinates.y + 1].IsWood)
                {
                    gameHandler.ChangeTilePlayability(1, _coordinates.x - 1, _coordinates.y + 1);
                }
            }
            else if (side == Side.Red && gameHandler.TurnHandler.Turn == Turn.Red)
            {
                if (_board.Matrix[_coordinates.x + 1, _coordinates.y - 1].Piece == null &&
                    !_board.Matrix[_coordinates.x + 1, _coordinates.y - 1].IsWood)
                {
                    gameHandler.ChangeTilePlayability(1, _coordinates.x + 1, _coordinates.y - 1);
                }

                if (_board.Matrix[_coordinates.x - 1, _coordinates.y - 1].Piece == null &&
                    !_board.Matrix[_coordinates.x - 1, _coordinates.y - 1].IsWood)
                {
                    gameHandler.ChangeTilePlayability(1, _coordinates.x - 1, _coordinates.y - 1);
                }
            }
        }
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
        gameHandler.PlaceToBoard(this, _coordinates.x, _coordinates.y);
    }
    
    private void RemoveFromGrid()
    {
        gameHandler.RemoveFromBoard(_coordinates.x, _coordinates.y);
    }

    private void CheckNormalPlayablity()
    {
        if (side == Side.White && gameHandler.TurnHandler.Turn == Turn.White)
        {
            _normalPlayable = (_board.Matrix[_coordinates.x + 1, _coordinates.y + 1].Piece == null &&
                               !_board.Matrix[_coordinates.x + 1, _coordinates.y + 1].IsWood) ||
                               (_board.Matrix[_coordinates.x - 1, _coordinates.y + 1].Piece == null && 
                               !_board.Matrix[_coordinates.x - 1, _coordinates.y + 1].IsWood);
        }
        else if (side == Side.Red && gameHandler.TurnHandler.Turn == Turn.Red)
        {
            _normalPlayable = (_board.Matrix[_coordinates.x + 1, _coordinates.y - 1].Piece == null &&
                               !_board.Matrix[_coordinates.x + 1, _coordinates.y - 1].IsWood) ||
                              (_board.Matrix[_coordinates.x - 1, _coordinates.y - 1].Piece == null && 
                               !_board.Matrix[_coordinates.x - 1, _coordinates.y - 1].IsWood);
        }
    }

    private void CheckHasToTake()
    {
        _piecesToEliminate.Clear();
        Vector2Int placeToMoveOnTake;
        if (side == Side.White)
        {
            if (_board.Matrix[_coordinates.x + 1, _coordinates.y + 1].Piece?.side == Side.Red &&
                !_board.Matrix[_coordinates.x + 1, _coordinates.y + 1].IsWood)
            {
                if (_board.Matrix[_coordinates.x + 2, _coordinates.y + 2].Piece == null &&
                    !_board.Matrix[_coordinates.x + 2, _coordinates.y + 2].IsWood)
                {
                    placeToMoveOnTake = HasToTake(_coordinates.x + 2, _coordinates.y + 2);
                    gameHandler.ChangeTilePlayability(1,placeToMoveOnTake.x,placeToMoveOnTake.y);
                    _piecesToEliminate.Add(_board.Matrix[_coordinates.x + 1, _coordinates.y + 1].Piece);
                }
            }
            if (_board.Matrix[_coordinates.x - 1, _coordinates.y + 1].Piece?.side == Side.Red &&
                !_board.Matrix[_coordinates.x - 1, _coordinates.y + 1].IsWood)
            {
                if (_board.Matrix[_coordinates.x - 2, _coordinates.y + 2].Piece == null &&
                    !_board.Matrix[_coordinates.x - 2, _coordinates.y + 2].IsWood)
                {
                    placeToMoveOnTake = HasToTake(_coordinates.x - 2, _coordinates.y + 2);
                    gameHandler.ChangeTilePlayability(1,placeToMoveOnTake.x,placeToMoveOnTake.y);
                    _piecesToEliminate.Add(_board.Matrix[_coordinates.x - 1, _coordinates.y + 1].Piece);
                }
            }
        }
        if (side == Side.Red)
        {
            if (_board.Matrix[_coordinates.x + 1, _coordinates.y - 1].Piece?.side == Side.White &&
                !_board.Matrix[_coordinates.x + 1, _coordinates.y - 1].IsWood)
            {
                if (_board.Matrix[_coordinates.x + 2, _coordinates.y - 2].Piece == null &&
                    !_board.Matrix[_coordinates.x + 2, _coordinates.y - 2].IsWood)
                {
                    placeToMoveOnTake = HasToTake(_coordinates.x + 2, _coordinates.y - 2);
                    gameHandler.ChangeTilePlayability(1,placeToMoveOnTake.x,placeToMoveOnTake.y);
                    _piecesToEliminate.Add(_board.Matrix[_coordinates.x + 1, _coordinates.y - 1].Piece);
                }
            }
            if (_board.Matrix[_coordinates.x - 1, _coordinates.y - 1].Piece?.side == Side.White &&
                !_board.Matrix[_coordinates.x - 1, _coordinates.y - 1].IsWood)
            {
                if (_board.Matrix[_coordinates.x - 2, _coordinates.y - 2].Piece == null &&
                    !_board.Matrix[_coordinates.x - 2, _coordinates.y - 2].IsWood)
                {
                    placeToMoveOnTake = HasToTake(_coordinates.x - 2, _coordinates.y - 2);
                    gameHandler.ChangeTilePlayability(1,placeToMoveOnTake.x,placeToMoveOnTake.y);
                    _piecesToEliminate.Add(_board.Matrix[_coordinates.x - 1, _coordinates.y - 1].Piece);
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
    
    public void EliminatePieces()
    {
        for (var i = 0; i < _piecesToEliminate.Count; i++)
        {
            Destroy(_piecesToEliminate[i].gameObject);
        }
    }
    
    private void OnDestroy()
    {
        gameHandler.OnMovePiece -= OnMovePiece;
        _tile.Piece = null;
    }
}
