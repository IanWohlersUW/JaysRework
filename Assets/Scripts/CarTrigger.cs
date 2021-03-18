using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarTrigger : MonoBehaviour
{
    private static readonly Vector3 CAR_SCALE = new Vector3(0.15f, 0.17f, 1f);

    [HideInInspector]
    public bool isMoving = false;
    [NotNull]
    public SpriteRenderer carPrefab;
    [NotNull]
    public TMPro.TextMeshProUGUI countdownText;
    [NotNull]
    public Image warningSign;
    [SerializeField]
    private int startingCountdown;

    private int countdownValue; // access this via the countdown property
    public int countdown
    {
        get { return countdownValue; }
        set {
            countdownValue = value;
            countdownText.text = $"{value}";
        }
    }

    protected virtual void Start()
    {
        int x = GameBoard.instance.WorldToGrid(transform.position).x;
        GameBoard.instance.cars.Add(x, this);
        transform.position = new Vector3(GetXPos(), 0, 0);
        this.countdown = startingCountdown;
    }

    public virtual void Tic()
    {
        if (GameBoard.instance.player.OnZebra())
            return; // don't tic if player is on a zebra tile

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
        car.enabled = true;
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
