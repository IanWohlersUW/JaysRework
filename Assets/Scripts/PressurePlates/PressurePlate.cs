﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class PressurePlate : MonoBehaviour
{
    public virtual void Create(Vector2Int coords)
    {
        if (GameBoard.instance.pressurePlates.Contains(coords))
        {
            Debug.LogError("Attempting to spawn plate in filled space");
            return;
        }
        GameBoard.instance.pressurePlates.Add(coords, this); // place this piece on the board
        GetComponent<Rigidbody2D>().position = GameBoard.instance.GridToWorld(coords);
        GameBoard.ScaleToGameboard(GetComponent<SpriteRenderer>());
    }
    public Vector2Int GetPosition() => GameBoard.instance.pressurePlates.Reverse[this];

    public virtual void OnStep(GamePiece piece) { }
    public virtual void OffStep(GamePiece piece) { }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var piece = collision.gameObject.GetComponent<GamePiece>();
        if (piece == null)
            return;
        OnStep(piece);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var piece = collision.gameObject.GetComponent<GamePiece>();
        if (piece == null)
            return;
        OffStep(piece);
    }

    public virtual void DestroyPiece()
    {
        GameBoard.instance.pressurePlates.Remove(this);
        Destroy(gameObject);
    }

}