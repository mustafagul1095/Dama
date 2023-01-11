using UnityEngine;

public class Board : MonoBehaviour
{
    private readonly Tile[,] _matrix = new Tile[10, 10];

    public Tile[,] Matrix => _matrix;

    private void Awake()
    {
        var tiles = GetComponentsInChildren<Tile>();
        foreach (var tile in tiles) 
        {
            tile.FindCoordinates();
            _matrix[tile.Coordinate.x, tile.Coordinate.y] = tile;
        }
    }
    
    // private void FillGridEdges()
    // {
    //     for (int i = 0; i < 10; i++)
    //     {
    //         _pieceArray[0, i] = 3;
    //         _pieceArray[9, i] = 3;
    //         _pieceArray[i, 0] = 3;
    //         _pieceArray[i, 9] = 3;
    //     }
    // }
}