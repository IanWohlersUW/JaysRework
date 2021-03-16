using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : PressurePlate
{
    public GamePiece toSpawn;

    public override void OffStep(GamePiece piece)
    {
        var spawnPos = GetPosition();
        if (GameBoard.instance.pieces.Contains(spawnPos))
            return; // this manhole is still occupied
        var spawned = Instantiate(toSpawn, GameBoard.instance.GridToWorld(spawnPos),
            Quaternion.identity);
        spawned.Create(spawnPos);
        GameBoard.instance.player.followers.Add(spawned);
        DestroyPiece();
    }
}
