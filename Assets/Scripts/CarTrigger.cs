using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarTrigger : MonoBehaviour
{
    private static readonly Vector3 CAR_SCALE = new Vector3(0.15f, 0.17f, 1f);

    public bool isMoving = false;
    [NotNull]
    public SpriteRenderer carPrefab;
    [NotNull]
    public TMPro.TextMeshProUGUI countdownText;
    [NotNull]
    public Image warningSign;

    private int countdownValue; // access this via the countdown property
    public int countdown
    {
        get { return countdownValue; }
        set {
            countdownValue = value;
            countdownText.text = $"{value}";
        }
    }

    public virtual void Create(int x, int countdown)
    {
        if (GameBoard.instance.cars.Contains(x))
        {
            Debug.LogError("Attempting to spawn car in filled space");
            return;
        }
        GameBoard.instance.cars.Add(x, this);
        transform.position = new Vector3(GetXPos(), 0, 0);
        this.countdown = countdown;
    }

    public virtual void Tic()
    {
        countdown--;
        if (countdown == 0)
        {
            StartCoroutine(SendCarAnimation());
            GetComponentInChildren<Canvas>().enabled = false; // delete our UI element on destroy
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SendCarAnimation());
            countdown--;
            Debug.Log(countdown);
        }
    }

    public int GetPosition() => GameBoard.instance.cars.Reverse[this];
    private float GetWorldX(int gridX) => GameBoard.instance.GridToWorld(new Vector2Int(gridX, 0)).x;
    private float GetXPos() => GetWorldX(GetPosition());

    IEnumerator SendCarAnimation(float duration = 0.5f)
    {
        isMoving = true;
        float xPos = GetXPos();

        float halfScreen = Screen.height / 2;
        float yTop = Camera.main.ScreenToWorldPoint(new Vector3(0, halfScreen + halfScreen * 1.2f, 0)).y;
        float yBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, halfScreen - halfScreen * 1.2f, 0)).y;
        (Vector3 start, Vector3 end) = (new Vector3(xPos, yTop, 0), new Vector3(xPos, yBottom, 0));

        var car = Instantiate(carPrefab, start, Quaternion.identity);
        car.transform.localScale = CAR_SCALE;
        car.transform.position = start;

        float elapsed = 0f;
        while (elapsed <= duration)
        {
            float progress = elapsed / duration; // progress 0-1f
            car.transform.position = VectorUtils.SmoothInterp(start, end, progress);

            yield return new WaitForFixedUpdate();
            elapsed += Time.deltaTime;
        }
        Destroy(car.gameObject);
        isMoving = false;
    }
}
