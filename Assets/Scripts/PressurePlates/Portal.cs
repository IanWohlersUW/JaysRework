using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : PressurePlate
{
    [SerializeField]
    private GameObject worldDest;
    private Vector2Int dest;

    protected override void Start()
    {
        dest = GameBoard.instance.WorldToGrid(worldDest.transform.position);
        base.Start();
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
