using UnityEngine;

public static class Vector2IntExtensions
{
    public static Vector2Int GetCoordinateFromPosition(Vector3 position)
    {
        return new Vector2Int(
            Mathf.RoundToInt(position.x / 10) + 1,
            Mathf.RoundToInt(position.z / 10) + 1);
    }
}