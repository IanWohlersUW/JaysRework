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
        int followerI = GameBoard.instance.player.followers.IndexOf(this);
        GamePiece nextPiece = followerI == 0 ? GameBoard.instance.player :
            GameBoard.instance.player.followers[followerI - 1];
        string trigger = DirectionToTrigger(nextPiece.GetPosition() - GetPosition());
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
