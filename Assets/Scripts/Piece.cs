using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] private GameHandler gameHandler;

    [SerializeField] private Side _side;

    [SerializeField] private GameObject _promotionObject;
    
    private Vector2Int _coord = new Vector2Int();

    public Vector2Int Coord => _coord;
    
    private bool _normalPlayable = false;
    
    private CapsuleCollider _collider;
    private bool _pieceHasToTake = false;
    
    public bool PieceHasToTake => _pieceHasToTake;

    private List<Piece> _piecesToEliminate = new List<Piece>(2);

    public List<Piece> PiecesToEliminate => _piecesToEliminate;

    public Side Side => _side;

    private Tile _tile;
    
    private PiecePlayability _playability;

    public void SetTile(Tile tile)
    {
        _tile = tile;
    }
    
    public void Init()
    {
        _collider = GetComponent<CapsuleCollider>();
        FindCoordinates();
        PlaceToGrid();
        gameHandler.OnMovePiece += OnMovePiece;
        InitPlayability();
    }

    private void InitPlayability()
    {
        if (_side == Side.White)
        {
            _playability = new WhitePiecePlayability(this, gameHandler);
        }
        else
        {
            _playability = new RedPiecePlayability(this, gameHandler);
        }
    }
    
    private void OnMovePiece()
    {
        if (gameHandler.CurrentPiece == this)
        {
            EliminatePieces();
            _playability.TryPromote();
        }
    }

    public void PrepareForNextTurn()
    {
        if (!MySideHasToTake())
        {
            _normalPlayable = _playability.GetCalculatedNormalPlayability();
        }
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
            _playability.ChangeHasToTakePlayability();
        }
        else
        {
            _playability.ChangeNormalPlayability();
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



    public void CalculateHasToTakeForNextTurn()
    {
        _piecesToEliminate.Clear();
        _playability.FindPiecesToEliminate();
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

    public void HandleClickEnable()
    {
        if (_side == gameHandler.TurnHandler.Turn)
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
        else
        {
            _collider.enabled = false;
        }
    }

    private void EliminatePieces()
    {
        CalculateHasTaken();

        foreach (var piece in _piecesToEliminate)
        {
            piece.RemoveFromGrid();
            gameHandler.Pieces.RemovePiece(piece);
            Destroy(piece.gameObject);
        }
    }

    private void CalculateHasTaken()
    {
        if (_piecesToEliminate.Count > 0)
        {
            if (_side == Side.Red)
            {
                gameHandler.RedHasTaken = true;
            }
            else
            {
                gameHandler.WhiteHasTaken = true;
            }
        }
    }

    private void OnDestroy()
    {
        gameHandler.OnMovePiece -= OnMovePiece;
        _tile.Piece = null;
    }

    public void Promote()
    {
        _playability = new PromotedPiecePlayability(this, gameHandler);
        _promotionObject.SetActive(true);
    }
}
