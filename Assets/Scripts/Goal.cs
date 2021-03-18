using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Goal : MonoBehaviour
{
    Rigidbody2D rb;
    private bool reachedGoal = false;
    protected virtual void Start()
    {
        Vector2Int coords = GameBoard.instance.WorldToGrid(transform.position);
        GameBoard.instance.goals.Add(this); // place this piece on the board
        GetComponent<Rigidbody2D>().position = 
            GameBoard.instance.GridToWorld(coords); // and sync its piece position
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != GameBoard.instance.player.gameObject)
            return;
        reachedGoal = true;
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject != GameBoard.instance.player.gameObject)
            return;
        reachedGoal = false;
    }

    public bool JayOnGoal() => reachedGoal;
}
