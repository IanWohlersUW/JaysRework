using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cone : GamePiece
{
    public override bool CanMove(Vector2Int dest) => false; // Cones can't move!
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Car")
            return;
        // Play a cone spinning animation here!
        Debug.Log("spin!");
    }
}
