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
        var player = piece.gameObject.GetComponent<Player>();
        if (player == null)
            return;
        StartCoroutine(MovePlayer(player));
    }

    IEnumerator MovePlayer(Player player)
    {
        // We need to wait for the player to fully land on the manhole
        yield return new WaitUntil(() => !player.isMoving);
        player.MovePiece(dest);
    }
}
