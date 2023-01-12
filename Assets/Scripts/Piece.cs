using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] private GameHandler gameHandler;
    private Board _board;
    private Vector2Int _coord = new Vector2Int();
    private bool _normalPlayable = false;
    private CapsuleCollider _collider;
    private bool _pieceHasToTake = false;

    public bool PieceHasToTake => _pieceHasToTake;

    private List<Piece> _piecesToEliminate = new List<Piece>(2);
    [SerializeField] private Side _side;
    
    public Side Side => _side;

    private Tile _tile;

    public void SetTile(Tile tile)
    {
        _tile = tile;
    }
    
    public void Init()
    {
        _board = gameHandler.Board;
        _collider = GetComponent<CapsuleCollider>();
        FindCoordinates();
        PlaceToGrid();
        gameHandler.OnMovePiece += OnMovePiece;
    }
    
    private void OnMovePiece()
    {
        if (gameHandler.CurrentPiece == this)
        {
            EliminatePieces();
        }
    }

    public void PrepareForNextTurn()
    {
        if (!MySideHasToTake())
        {
            CheckNormalPlayablity();
        }
        HandleClickEnable();
    }

    private void OnMouseDown()
    {
        gameHandler.ClearTilePlayabilityMatrix();
        ChangeTilePlayability();
        gameHandler.RefreshTiles();
        gameHandler.SetCurrentPiece(this);
    }

    private void ChangeTilePlayability()
    {
        if(MySideHasToTake())
        {
            ChangeHasToTakePlayability();
        }
        else
        {
            ChangeNormalPlayability();
        }
    }

    private void ChangeHasToTakePlayability()
    {
        Vector2Int placeToMove;
        if (_side == Side.White && gameHandler.TurnHandler.Turn == Turn.White)
        {
            if (_board.Matrix[_coord.x + 1, _coord.y + 1].Piece?._side == Side.Red &&
                !_board.Matrix[_coord.x + 1, _coord.y + 1].IsWood)
            {
                if (_board.Matrix[_coord.x + 2, _coord.y + 2].Piece == null &&
                    !_board.Matrix[_coord.x + 2, _coord.y + 2].IsWood)
                {
                    placeToMove = new Vector2Int(_coord.x + 2, _coord.y + 2);
                    gameHandler.ChangeTilePlayability(1,placeToMove.x,placeToMove.y);
                }
            }
            if (_board.Matrix[_coord.x - 1, _coord.y + 1].Piece?._side == Side.Red &&
                !_board.Matrix[_coord.x - 1, _coord.y + 1].IsWood)
            {
                if (_board.Matrix[_coord.x - 2, _coord.y + 2].Piece == null &&
                    !_board.Matrix[_coord.x - 2, _coord.y + 2].IsWood)
                {
                    placeToMove = new Vector2Int(_coord.x - 2, _coord.y + 2); 
                    gameHandler.ChangeTilePlayability(1,placeToMove.x,placeToMove.y);
                }
            }
        }
        if (_side == Side.Red && gameHandler.TurnHandler.Turn == Turn.Red)
        {
            if (_board.Matrix[_coord.x + 1, _coord.y - 1].Piece?._side == Side.White &&
                !_board.Matrix[_coord.x + 1, _coord.y - 1].IsWood)
            {
                if (_board.Matrix[_coord.x + 2, _coord.y - 2].Piece == null &&
                    !_board.Matrix[_coord.x + 2, _coord.y - 2].IsWood)
                {
                    placeToMove = new Vector2Int(_coord.x + 2, _coord.y - 2); 
                    gameHandler.ChangeTilePlayability(1,placeToMove.x,placeToMove.y);
                }
            }
            if (_board.Matrix[_coord.x - 1, _coord.y - 1].Piece?._side == Side.White &&
                !_board.Matrix[_coord.x - 1, _coord.y - 1].IsWood)
            {
                if (_board.Matrix[_coord.x - 2, _coord.y - 2].Piece == null &&
                    !_board.Matrix[_coord.x - 2, _coord.y - 2].IsWood)
                {
                    placeToMove = new Vector2Int(_coord.x - 2, _coord.y - 2);
                    gameHandler.ChangeTilePlayability(1,placeToMove.x,placeToMove.y);
                }
            }
        }
    }

    private void ChangeNormalPlayability()
    {
        if (_side == Side.White && gameHandler.TurnHandler.Turn == Turn.White)
        {
            if (_board.Matrix[_coord.x + 1, _coord.y + 1].Piece == null &&
                !_board.Matrix[_coord.x + 1, _coord.y + 1].IsWood)
            {
                gameHandler.ChangeTilePlayability(1, _coord.x + 1, _coord.y + 1);
            }

            if (_board.Matrix[_coord.x - 1, _coord.y + 1].Piece == null &&
                !_board.Matrix[_coord.x - 1, _coord.y + 1].IsWood)
            {
                gameHandler.ChangeTilePlayability(1, _coord.x - 1, _coord.y + 1);
            }
        }
        else if (_side == Side.Red && gameHandler.TurnHandler.Turn == Turn.Red)
        {
            if (_board.Matrix[_coord.x + 1, _coord.y - 1].Piece == null &&
                !_board.Matrix[_coord.x + 1, _coord.y - 1].IsWood)
            {
                gameHandler.ChangeTilePlayability(1, _coord.x + 1, _coord.y - 1);
            }

            if (_board.Matrix[_coord.x - 1, _coord.y - 1].Piece == null &&
                !_board.Matrix[_coord.x - 1, _coord.y - 1].IsWood)
            {
                gameHandler.ChangeTilePlayability(1, _coord.x - 1, _coord.y - 1);
            }
        }
    }

    private void FindCoordinates()
    {
        _coord = Vector2IntExtensions.GetCoordinateFromPosition(transform.position);
    }

    private Vector3 CoordinatesToPosition(int coordinateX, int coordinateY)
    {
        return new Vector3((coordinateX - 1) * 10, transform.position.y, (coordinateY - 1) * 10);
    }

    public void ChangeCoordinate(int x, int y)
    {
        RemoveFromGrid();
        _coord.x = x;
        _coord.y = y;
        transform.position = CoordinatesToPosition(x, y);
        PlaceToGrid();
    }
    
    private void PlaceToGrid()
    {
        gameHandler.PlaceToBoard(this, _coord.x, _coord.y);
    }
    
    private void RemoveFromGrid()
    {
        gameHandler.RemoveFromBoard(_coord.x, _coord.y);
    }

    private void CheckNormalPlayablity()
    {
        if (_side == Side.White && gameHandler.TurnHandler.Turn == Turn.White)
        {
            _normalPlayable = (_board.Matrix[_coord.x + 1, _coord.y + 1].Piece == null &&
                               !_board.Matrix[_coord.x + 1, _coord.y + 1].IsWood) ||
                               (_board.Matrix[_coord.x - 1, _coord.y + 1].Piece == null && 
                               !_board.Matrix[_coord.x - 1, _coord.y + 1].IsWood);
        }
        else if (_side == Side.Red && gameHandler.TurnHandler.Turn == Turn.Red)
        {
            _normalPlayable = (_board.Matrix[_coord.x + 1, _coord.y - 1].Piece == null &&
                               !_board.Matrix[_coord.x + 1, _coord.y - 1].IsWood) ||
                              (_board.Matrix[_coord.x - 1, _coord.y - 1].Piece == null && 
                               !_board.Matrix[_coord.x - 1, _coord.y - 1].IsWood);
        }
    }

    public void CalculateHasToTake()
    {
        _piecesToEliminate.Clear();
        if (_side == Side.White && gameHandler.TurnHandler.Turn == Turn.White)
        {
            if (_board.Matrix[_coord.x + 1, _coord.y + 1].Piece?._side == Side.Red &&
                !_board.Matrix[_coord.x + 1, _coord.y + 1].IsWood)
            {
                if (_board.Matrix[_coord.x + 2, _coord.y + 2].Piece == null &&
                    !_board.Matrix[_coord.x + 2, _coord.y + 2].IsWood)
                {
                    _piecesToEliminate.Add(_board.Matrix[_coord.x + 1, _coord.y + 1].Piece);
                }
            }
            if (_board.Matrix[_coord.x - 1, _coord.y + 1].Piece?._side == Side.Red &&
                !_board.Matrix[_coord.x - 1, _coord.y + 1].IsWood)
            {
                if (_board.Matrix[_coord.x - 2, _coord.y + 2].Piece == null &&
                    !_board.Matrix[_coord.x - 2, _coord.y + 2].IsWood)
                {
                    _piecesToEliminate.Add(_board.Matrix[_coord.x - 1, _coord.y + 1].Piece);
                }
            }
        }
        if (_side == Side.Red && gameHandler.TurnHandler.Turn == Turn.Red)
        {
            if (_board.Matrix[_coord.x + 1, _coord.y - 1].Piece?._side == Side.White &&
                !_board.Matrix[_coord.x + 1, _coord.y - 1].IsWood)
            {
                if (_board.Matrix[_coord.x + 2, _coord.y - 2].Piece == null &&
                    !_board.Matrix[_coord.x + 2, _coord.y - 2].IsWood)
                {
                    _piecesToEliminate.Add(_board.Matrix[_coord.x + 1, _coord.y - 1].Piece);
                }
            }
            if (_board.Matrix[_coord.x - 1, _coord.y - 1].Piece?._side == Side.White &&
                !_board.Matrix[_coord.x - 1, _coord.y - 1].IsWood)
            {
                if (_board.Matrix[_coord.x - 2, _coord.y - 2].Piece == null &&
                    !_board.Matrix[_coord.x - 2, _coord.y - 2].IsWood)
                {
                    _piecesToEliminate.Add(_board.Matrix[_coord.x - 1, _coord.y - 1].Piece);
                }
            }
        }
        DecideHasToTake();
    }

    private bool MySideHasToTake()
    {
        return (_side == Side.White && gameHandler.WhiteHasToTake) || (_side == Side.Red && gameHandler.RedHasToTake);
    }
    
    private void DecideHasToTake()
    {
        _pieceHasToTake = _piecesToEliminate.Count > 0;
    }

    private void HandleClickEnable()
    {
        if (MySideHasToTake())
        {
            _collider.enabled = _pieceHasToTake;
        }
        else
        {
            _collider.enabled = _normalPlayable;
        }
    }

    private void EliminatePieces()
    {
        foreach (var piece in _piecesToEliminate)
        {
            piece.RemoveFromGrid();
            gameHandler.Pieces.RemovePiece(piece);
            Destroy(piece.gameObject);
        }
    }
    
    private void OnDestroy()
    {
        gameHandler.OnMovePiece -= OnMovePiece;
        _tile.Piece = null;
    }
}
