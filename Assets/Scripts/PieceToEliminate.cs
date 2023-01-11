using UnityEngine;

public class PieceToEliminate
{
    private Piece _piece;
    public Piece Piece => _piece;

    private Vector2Int _coordinates = new Vector2Int(500, 500);
    public Vector2Int Coordinates => _coordinates;
    
    public void SetPiece(Piece piece)
    {
        _piece = piece;
    }
    
    public void SetCoordinates(int x, int y)
    {
        _coordinates.x = x;
        _coordinates.y = y;
    }
    
    public void Destroy()
    {
    }
}