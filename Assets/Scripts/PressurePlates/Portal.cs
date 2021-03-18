using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : PressurePlate
{
    [HideInInspector]
    public Vector2Int dest;

    public override void Create(Vector2Int coords) =>
        Debug.LogError("Need to specify a destination for our portal, use other Create method");

    public void Create(Vector2Int coords, Vector2Int dest)
    {
        base.Create(coords);
        this.dest = dest;
    }

    public override void OnStep(GamePiece piece)
    {
        if (piece == null)
            return;
        StartCoroutine(MovePiece(piece));
    }

    IEnumerator MovePiece(GamePiece piece)
    {
        // We need to wait for the player to fully land on the manhole
        yield return new WaitUntil(() => !piece.isMoving);
        piece.WarpPiece(dest);
    }
}
