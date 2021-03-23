using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWall : GamePiece
{
    protected override void Start()
    {
        base.Start();
        sr.enabled = false; // don't show invisible walls in game
    }

    // Invisible walls can't move
    public override bool CanMove(Vector2Int dest) => false;
    protected override void OnTriggerEnter2D(Collider2D collision) { }
}
