using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[RequireComponent(typeof(Tilemap))]
public class GameBoard : MonoBehaviour
{
    [SerializeField]
    private List<TileReader.NamedTile> paletteValues; // These values are loaded into our tilePalette field

    public static GameBoard instance;
    public BiMap<Vector2Int, GamePiece> pieces = new BiMap<Vector2Int, GamePiece>();
    public BiMap<Vector2Int, PressurePlate> pressurePlates = new BiMap<Vector2Int, PressurePlate>();
    public BiMap<int, CarTrigger> cars = new BiMap<int, CarTrigger>();

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

    // This is convoluted, it might just be better to store cars in a list
    public List<CarTrigger> GetCars() => GameBoard.instance.cars.GetKeys()
        .Select(x => GameBoard.instance.cars.Forward[x])
        .ToList();

    public Vector3 GridToWorld(Vector2Int coords) => playmat.GetCellCenterWorld(VectorUtils.Vec2To3(coords));

    public static void ScaleToGameboard(SpriteRenderer sprite)
    {
        var currSize = sprite.bounds.size;
        var boardCellSize = instance.playmat.cellSize;

        Vector3 scale = new Vector3(boardCellSize.x / currSize.x, boardCellSize.y / currSize.y, 1);
        sprite.gameObject.transform.localScale = scale;
    }
}
