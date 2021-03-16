using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class GamePiece : MonoBehaviour
{
    public bool isMoving = false;
    public SpriteRenderer sr;
    Rigidbody2D rb;

    public virtual void Create(Vector2Int coords)
    {
        if (GameBoard.instance.pieces.Contains(coords))
        {
            Debug.LogError("Attempting to spawn piece in filled space");
            return;
        }
        GameBoard.instance.pieces.Add(coords, this); // place this piece on the board
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.position = GameBoard.instance.GridToWorld(coords); // and sync its piece position
        GameBoard.ScaleToGameboard(sr);
    }

    public Vector2Int GetPosition() => GameBoard.instance.pieces.Reverse[this];

    // Moves the piece to the given tile, returns false if unsuccessful
    public virtual bool MovePiece(Vector2Int dest)
    {
        if (!CanMove(dest))
            return false;
        // This should be a coroutine... that'll be a lil trickier ;)
        GameBoard.instance.pieces.Remove(this);
        GameBoard.instance.pieces.Add(dest, this);
        // rb.position = GameBoard.instance.GridToWorld(dest);
        StartCoroutine(MovePieceAnimation(dest));
        return true;
    }

    // Technically this cleans up the GameBoard
    public virtual void DestroyPiece()
    {
        GameBoard.instance.player.RemoveFollower(this);
        GameBoard.instance.pieces.Remove(this);
        Destroy(gameObject);
    }

    public virtual bool CanMove(Vector2Int dest) => !GameBoard.instance.pieces.Contains(dest);

    // This animation is really choppy and I'm not sure why :(
    IEnumerator MovePieceAnimation(Vector2Int dest, float duration = 0.5f)
    {
        isMoving = true;
        (Vector3 start, Vector3 end) = (
            gameObject.transform.position,
            GameBoard.instance.GridToWorld(dest)
        );
        float elapsed = 0f;
        while (elapsed <= duration)
        {
            float progress = elapsed / duration; // progress 0-1f
            rb.position = VectorUtils.SmoothInterp(start, end, progress);

            yield return new WaitForFixedUpdate();
            elapsed += Time.deltaTime;
            elapsed *= 1.02f;
        }
        rb.position = end;
        isMoving = false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Car")
            return;
        CameraShake.Shake(0.1f, 0.2f);
        DestroyPiece();
    }
}
