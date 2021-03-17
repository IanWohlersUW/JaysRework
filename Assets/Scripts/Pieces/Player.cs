using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GamePiece
{
    public List<GamePiece> followers;
    // We had a lot of complex logic to update the followers before
    // ... could we do better and simplify it this time?
    public override bool MovePiece(Vector2Int dest)
    {
        var oldPos = GetPosition();
        base.MovePiece(dest);
        MoveFollowers(oldPos);
        return true;
    }

    // moves the player for this turn, returns false if the input was invalid
    public bool MovePlayer()
    {
        if (isMoving)
            return false;
        var input = InputToDir();
        if (!input.HasValue)
            return false;
        return MovePiece(GetPosition() + VectorUtils.DirToVec(input.Value));
    }

    Direction? InputToDir()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            return Direction.Up;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            return Direction.Right;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            return Direction.Down;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            return Direction.Left;
        return null;
    }

    public void MoveFollowers(Vector2Int dest, int startIndex = 0)
    {
        for (int i = startIndex; i < followers.Count; i++)
        {
            var currPos = followers[i].GetPosition();
            followers[i].MovePiece(dest);
            dest = currPos;
        }
    }

    public void RemoveFollower(GamePiece follower)
    {
        int pos = followers.IndexOf(follower);
        if (pos < 0)
            return; // GamePiece not following player!
        var gap = follower.GetPosition();
        GameBoard.instance.pieces.Remove(follower);
        MoveFollowers(gap, pos + 1);
        followers.RemoveAt(pos);
    }

    public bool OnZebra()
    {
        var currTile = GameBoard.instance.playmat.GetTile(VectorUtils.Vec2To3(GetPosition()));
        // Need to add a tag for our zebra tiles!
        return false;
    }
}
