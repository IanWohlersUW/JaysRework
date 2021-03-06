using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class GamePiece : MonoBehaviour
{
    public event OnDestroyDelegate OnDestroyEvnt;
    public delegate void OnDestroyDelegate(MonoBehaviour instance);

    [HideInInspector]
    public bool justWarped = false;
    [HideInInspector]
    public bool isMoving = false;
    [HideInInspector]
    public SpriteRenderer sr;
    private Rigidbody2D rb;

    protected virtual void Start()
    {
        Vector2Int coords = GameBoard.instance.WorldToGrid(transform.position);
        GameBoard.instance.pieces.Add(coords, this); // place this piece on the board
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.position = GameBoard.instance.GridToWorld(coords); // and sync its piece position

        if (GameManager.instance != null)
            OnDestroyEvnt += GameManager.instance.OnPieceDestroyed;
    }

    public Vector2Int GetPosition() => GameBoard.instance.pieces.Reverse[this];

    private bool MovePieceHelper(Vector2Int dest)
    {
        if (!CanMove(dest))
            return false;
        GameBoard.instance.pieces.Remove(this);
        GameBoard.instance.pieces.Add(dest, this);
        return true;
    }

    // Moves the piece to the given tile, returns false if unsuccessful
    public virtual bool MovePiece(Vector2Int dest)
    {
        if (!MovePieceHelper(dest))
        {
            // Debug.LogError("Space was occupied, move failed");
            return false;
        }
        justWarped = false;
        StartCoroutine(MovePieceAnimation(dest));
        return true;
    }

    // Moves the piece to the given tile, returns false if unsuccessful
    public virtual bool WarpPiece(Vector2Int dest)
    {
        if (justWarped || !MovePieceHelper(dest))
        {
            // Debug.LogError("Space was occupied, warp failed");
            return false;
        }
        justWarped = true;
        StartCoroutine(WarpPieceAnimation(dest));
        return true;
    }

    // Technically this cleans up the GameBoard
    public virtual void DestroyPiece()
    {
        GameBoard.instance.player.RemoveFollower(this);
        GameBoard.instance.pieces.Remove(this);
        this.OnDestroyEvnt?.Invoke(this);
        Destroy(gameObject);
    }

    public virtual bool CanMove(Vector2Int dest) => !GameBoard.instance.pieces.Contains(dest);

    IEnumerator MovePieceAnimation(Vector2Int dest, float duration = 0.2f)
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

    IEnumerator WarpPieceAnimation(Vector2Int dest, float duration = 0.2f)
    {
        isMoving = true;
        sr.enabled = false;
        rb.position = GameBoard.instance.GridToWorld(dest);
        yield return new WaitForSeconds(duration);
        sr.enabled = true;
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
