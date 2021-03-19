using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GamePiece
{
    public List<Vector2Int> locationHist = new List<Vector2Int>(); // should cap this array (this is not great
    public List<GamePiece> followers = new List<GamePiece>();

    protected override void Start()
    {
        base.Start();
        if (GameBoard.instance.player != null)
            Debug.LogError("Created a singleton player when one already exists");
        GameBoard.instance.player = this;
    }

    /*
    public override void Create(Vector2Int coords)
    {
        base.Create(coords);
        if (GameBoard.instance.player != null)
            Debug.LogError("Created a singleton player when one already exists");
        GameBoard.instance.player = this;
    }
    */

    public override bool MovePiece(Vector2Int dest)
    {
        Vector2Int oldPos = GetPosition();
        bool oldJustWarped = justWarped;

        if (!base.MovePiece(dest))
            return false;
        if (!oldJustWarped)
            locationHist.Insert(0, oldPos);
        MoveFollowers();
        return true;
    }

    public override bool WarpPiece(Vector2Int dest)
    {
        Vector2Int oldPos = GetPosition();
        if (!base.WarpPiece(dest))
            return false;
        locationHist.Insert(0, oldPos);
        return true;
    }

    // moves the player for this turn, returns false if the move wouldn't happen
    public bool TurnMove()
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

    public void MoveFollowers(int startIndex = 0)
    {
        for (int i = startIndex; i < followers.Count; i++)
            followers[i].MovePiece(locationHist[i]);
    }

    public void RemoveFollower(GamePiece follower)
    {
        int pos = followers.IndexOf(follower);
        if (pos < 0)
            return; // GamePiece not following player!
        var gap = follower.GetPosition();
        GameBoard.instance.pieces.Remove(follower);
        MoveFollowers(pos + 1);
        followers.RemoveAt(pos);
    }

    public bool OnZebra()
    {
        var currTile = GameBoard.instance.playmat.GetTile(VectorUtils.Vec2To3(GetPosition()));
        var tileType = GameBoard.instance.tilePalette.GetType(currTile);
        return tileType.HasValue && tileType.Value == TileReader.TileType.Zebra;
    }
}
