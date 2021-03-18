using System.Collections;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        // level.LoadLevel();
    }

    // Update is called once per frame
    void Update()
    {
        // Honestly the way this works is pretty janky
        if (!canMove || !GameBoard.instance.player.TurnMove())
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

        if (GameBoard.instance.OnGoal() && GameBoard.instance.GetCopCount() == 0)
        {
            Debug.Log("Won game!");
        }
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
