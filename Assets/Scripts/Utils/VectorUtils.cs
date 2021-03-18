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
    public static HashSet<Vector2Int> GetAdjacentTiles(Vector2Int pos) => 
        new HashSet<Vector2Int> {
            pos + Vector2Int.right,
            pos + Vector2Int.down,
            pos + Vector2Int.left,
            pos + Vector2Int.up
        };

    // Returns a weighted average between vectors (first, second)
    // Sinusoidally smooths this interpolation
    public static Vector3 SmoothInterp(Vector3 a, Vector3 b, float interp)
    {
        if (interp < 0 || interp > 1)
            throw new ArgumentException();
        float interpVal = (Mathf.Cos((1 + interp) * Mathf.PI) + 1) / 2;
        return Vector3.Lerp(a, b, interpVal);
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