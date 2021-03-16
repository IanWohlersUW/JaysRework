using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Direction { Right, Down, Left, Up }

public class VectorUtils
{
    public static Vector2Int Vec3To2(Vector3Int orig) => new Vector2Int(orig.x, orig.y);
    public static Vector3Int Vec2To3(Vector2Int orig, int z = 0) => new Vector3Int(orig.x, orig.y, z);
    public static List<Vector2Int> GetAdjacentTiles(Vector2Int pos)
    {
        int x = pos.x, y = pos.y;
        return new List<Vector2Int> {
            new Vector2Int(x, y + 1),
            new Vector2Int(x, y - 1),
            new Vector2Int(x + 1, y),
            new Vector2Int(x - 1, y)
        };
    }

    // Returns a weighted average between vectors (first, second)
    // Sinusoidally smooths this interpolation
    public static Vector3 SmoothInterp(Vector3 a, Vector3 b, float interp)
    {
        if (interp < 0 || interp > 1)
            throw new ArgumentException();
        return Vector3.Lerp(a, b, Mathf.Cos((1 + interp) * Mathf.PI));
    }

    public static Vector2Int DirToVec(Direction dir)
    {
        switch (dir)
        {
            case Direction.Right:
                return Vector2Int.right;
            case Direction.Down:
                return Vector2Int.down;
            case Direction.Left:
                return Vector2Int.left;
            case Direction.Up:
                return Vector2Int.up;
            default:
                return Vector2Int.zero;
        }
    }
}