using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class GameBoard : MonoBehaviour
{
    [SerializeField]
    private List<TileReader.NamedTile> paletteValues; // These values are loaded into our tilePalette field

    public static GameBoard instance;

    public BiMap<Vector2Int, PressurePlate> pressurePlates = new BiMap<Vector2Int, PressurePlate>();
    public BiMap<int, CarTrigger> cars = new BiMap<int, CarTrigger>();
    public List<Goal> goals = new List<Goal>();
    public BiMap<Vector2Int, GamePiece> pieces = new BiMap<Vector2Int, GamePiece>();

    [HideInInspector]
    public Tilemap playmat; // the "playmat" is the base level of tiles like sidewalks
    [HideInInspector]
    public TileReader tilePalette;
    [HideInInspector]
    public Player player; // give a reference to our main player

    // Start is called before the first frame update
    void Awake()
    {
        tilePalette = new TileReader(paletteValues);
        playmat = GetComponent<Tilemap>();
        instance = this;
    }

    public List<CarTrigger> GetCars() => cars.GetKeys()
        .Select(x => cars.Forward[x])
        .ToList();

    public Vector3 GridToWorld(Vector2Int coords) => playmat.GetCellCenterWorld(VectorUtils.Vec2To3(coords));
    public Vector2Int WorldToGrid(Vector3 worldPos) => VectorUtils.Vec3To2(playmat.WorldToCell(worldPos));

    public int GetCopCount() => pieces.GetKeys()
            .Select(coord => pieces.Forward[coord])
            .Where(piece => piece.tag == "Cop")
            .Count();

    public bool OnGoal() => goals.Any(goal => goal.JayOnGoal());
}
