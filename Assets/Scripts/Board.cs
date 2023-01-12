using UnityEngine;

public class Board : MonoBehaviour
{
    private readonly Tile[,] _matrix = new Tile[10, 10];

    public Tile[,] Matrix => _matrix;

    public void Init()
    {
        var tiles = GetComponentsInChildren<Tile>();
        foreach (var tile in tiles) 
        {
            tile.FindCoordinates();
            _matrix[tile.Coordinate.x, tile.Coordinate.y] = tile;
        }
    }
}