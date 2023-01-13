using System.Collections.Generic;
using UnityEngine;

public class Pieces : MonoBehaviour
{
    [SerializeField] private GameHandler _gameHandler;
    
    private List<Piece> _whitePieces = new List<Piece>();
    
    private List<Piece> _redPieces = new List<Piece>();

    private List<Piece> _allPieces = new List<Piece>();

    public void Init()
    {
        var pieces = GetComponentsInChildren<Piece>();
        _allPieces = new List<Piece>(pieces);

        AddPiecesToSides();
        InitPieces();
    }
    
    private void AddPiecesToSides()
    {
        foreach (var piece in _allPieces)
        {
            if (piece.Side == Side.Red)
            {
                _redPieces.Add(piece);
            }
            else
            {
                _whitePieces.Add(piece);
            }
        }
    }

    private void InitPieces()
    {
        foreach (var piece in _allPieces)
        {
            piece.Init();
        }
    }
    
    public void PrepareForNextTurn()
    {
        CalculateHasToTakeForNextTurn();

        foreach (var piece in _allPieces)
        {
            piece.PrepareForNextTurn();
        }
    }

    public void HandleClickEnable()
    {
        foreach (var piece in _allPieces)
        {
            piece.HandleClickEnable();
        }
    }

    public void CalculateHasToTakeForNextTurn()
    {
        foreach (var piece in _allPieces)
        {
            piece.CalculateHasToTakeForNextTurn();
        }
        
        CalculateRedHasToTake();
        CalculateWhiteHasToTake();
    }
    
    public void CalculateWhiteHasToTake()
    {
        _gameHandler.SetWhiteHasToTake(false);

        foreach (var whitePiece in _whitePieces)
        {
            if (whitePiece.PieceHasToTake)
            {
                _gameHandler.SetWhiteHasToTake(true);
            }
        }
    }
    
    public void CalculateRedHasToTake()
    {
        _gameHandler.SetRedHasToTake(false);

        foreach (var redPiece in _redPieces)
        {
            if (redPiece.PieceHasToTake)
            {
                _gameHandler.SetRedHasToTake(true);
            }
        }
    }

    public void RemovePiece(Piece piece)
    {
        _allPieces.Remove(piece);
        if (_redPieces.Contains(piece))
        {
            _redPieces.Remove(piece);
        }
        if (_whitePieces.Contains(piece))
        {
            _whitePieces.Remove(piece);
        }
    }
}
