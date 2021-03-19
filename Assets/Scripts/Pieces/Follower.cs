using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : GamePiece
{
    public Animator anim;

    public override bool MovePiece(Vector2Int dest)
    {
        if (!base.MovePiece(dest))
            return false;
        int nextMoveI = GameBoard.instance.player.followers.IndexOf(this) - 1;
        Vector2Int nextDest = nextMoveI < 0 ? GameBoard.instance.player.GetPosition()
            : GameBoard.instance.player.locationHist[nextMoveI];
        string trigger = DirectionToTrigger(nextDest - GetPosition());
        if (trigger != null)
            anim.SetTrigger(trigger);
        return true;
    }

    private string DirectionToTrigger(Vector2Int dir)
    {
        if (dir == Vector2Int.up)
            return "FaceUp";
        if (dir == Vector2Int.down)
            return "FaceDown";
        if (dir == Vector2Int.left)
            return "FaceLeft";
        if (dir == Vector2Int.right)
            return "FaceRight";
        return null;
    }
}
