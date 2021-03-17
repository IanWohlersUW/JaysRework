using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [NotNull]
    public Player jay;
    [NotNull]
    public GamePiece follower;
    [NotNull]
    public CarTrigger carTrigger;
    [NotNull]
    public Portal portalPrefab;
    [NotNull]
    public Spawner copSpawner;

    private bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        var player = Instantiate(jay);
        player.Create(Vector2Int.zero); // can change this spawn position

        var fanHole1 = Instantiate(copSpawner);
        fanHole1.Create(Vector2Int.right);

        var fanHole2 = Instantiate(copSpawner);
        fanHole2.Create(Vector2Int.right * 2);

        var carColumn = Instantiate(carTrigger);
        carColumn.Create(2, 5);

        var carColumn2 = Instantiate(carTrigger);
        carColumn2.Create(3, 5);

        var portal = Instantiate(portalPrefab);
        portal.Create(Vector2Int.up, new Vector2Int(2, -3));

        GameBoard.instance.player = player;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove || !GameBoard.instance.player.MovePlayer())
            return;
        canMove = false;
        StartCoroutine(ExecuteTurn());
    }

    IEnumerator ExecuteTurn()
    {
        yield return new WaitUntil(MovingFinished);

        // Then launch our cars
        var cars = GameBoard.instance.GetCars();
        foreach (CarTrigger carDispenser in cars)
            carDispenser.Tic();
        yield return new WaitUntil(() => cars.All(dispenser => !dispenser.isMoving));

        // And finally clean up our turn
        yield return new WaitUntil(MovingFinished);
        canMove = true;
    }

    private bool MovingFinished()
    {
        var player = GameBoard.instance.player;
        if (player.isMoving)
            return false;
        return player.followers.All(follower => !follower.isMoving);
    }
}
