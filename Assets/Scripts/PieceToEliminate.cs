using UnityEngine;

public class PieceToEliminate
{
    private Piece _pieceToEliminate;
    private Vector2Int _pieceToEliminateCoords = new Vector2Int(500, 500);
    public Piece Piece => _pieceToEliminate;
    public Vector2Int PieceToEliminateCoords => _pieceToEliminateCoords;
    
    public void SetPieceToEliminate(Piece piece)
    {
        _pieceToEliminate = piece;
    }
    public void SetPieceToEliminateCoords(int x, int y)
    {
        _pieceToEliminateCoords.x = x;
        _pieceToEliminateCoords.y = y;
    }
    
    public void DestroyPieceToEliminate()
    {
        //RemoveFromGridArray(PieceToEliminateCoords.x, PieceToEliminateCoords.y);
        GameObject.Destroy(_pieceToEliminate.gameObject);
        SetPieceToEliminateCoords(500,500);
    }
}