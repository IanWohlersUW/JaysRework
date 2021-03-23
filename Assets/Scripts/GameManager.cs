using System.Collections;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static GameManager instance;

    [NotNull]
    public SceneAsset nextLevel;
    public int copsToKill;
    [HideInInspector]
    public int copsKilled;
    private bool canMove = true;

    private void Start()
    {
        instance = this;
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

        if (GameBoard.instance.OnGoal() && copsKilled >= copsToKill)
        {
            FinishLevel();
        }
        canMove = true;
    }

    public void FinishLevel()
    {
        Debug.Log($"Won game! Loading level {nextLevel.name}");
        SceneManager.LoadScene(nextLevel.name);
    }

    private bool MovingFinished()
    {
        var player = GameBoard.instance.player;
        if (player.isMoving)
            return false;
        return player.followers.All(follower => !follower.isMoving);
    }

    public void OnPieceDestroyed(MonoBehaviour instance)
    {
        if (instance.gameObject.tag == "Cop")
            copsKilled++;
    }
}
