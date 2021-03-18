using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 * This is like, ok. It's hacky but it does the job
 */
[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class Level : ScriptableObject
{
    [System.Serializable]
    public class PlayerPos
    {
        public Player player;
        public Vector2Int pos;
    }

    // All gamepieces ("living" tiles) we can place in a level
    [System.Serializable]
    public class PiecePos
    {
        public GamePiece piece;
        public Vector2Int pos;
    }

    // All tiles that trigger when stepped on
    [System.Serializable]
    public class PlatePos
    {
        public PressurePlate plate;
        public Vector2Int pos;
    }

    // Portals ARE pressure plates, but they need an additional "dest" field
    [System.Serializable]
    public class PortalPos
    {
        public Portal portal;
        public Vector2Int pos;
        public Vector2Int dest;
    }

    [System.Serializable]
    public class CarPos
    {
        public CarTrigger carTrigger;
        public int x;
        public int countdown;
    }

    [System.Serializable]
    public class TilePos
    {
        public Vector2Int pos;
        public TileReader.TileType type;
    }

    public List<PiecePos> pieces;
    public List<PlatePos> pressurePlates;
    public List<PortalPos> portals;
    public List<TilePos> tiles;
    public List<CarPos> cars;
    public PlayerPos jay;

    public Tilemap levelPlaymat;

    /*
    public void LoadLevel()
    {
        // There should be a way to configure the camera as well?
        OverwriteTilemap(levelPlaymat, GameBoard.instance.playmat);

        // This seems good
        var jayInstance = Instantiate(jay.player);
        jayInstance.Create(jay.pos);

        if (pieces != null)
            foreach (PiecePos piece in pieces)
            {
                var pieceInstance = Instantiate(piece.piece);
                pieceInstance.Create(piece.pos);
            }

        if (pressurePlates != null)
            foreach (PlatePos plate in pressurePlates)
            {
                var plateInstance = Instantiate(plate.plate);
                plateInstance.Create(plate.pos);
            }

        if (portals != null)
            foreach (PortalPos portal in portals)
            {
                var portalInstance = Instantiate(portal.portal);
                portalInstance.Create(portal.pos);
            }

        if (cars != null)
            foreach (CarPos car in cars)
            {
                var carInstance = Instantiate(car.carTrigger);
                carInstance.Create(car.x, car.countdown);
            }
    }

    // Reads tiles from the given "read" tilemap and clears/writes over the write Tilemap
    private void OverwriteTilemap(Tilemap read, Tilemap write)
    {
        write.ClearAllTiles();
        for (int x = -100; x < 100; x++)
        {
            for (int y = -100; y < 100; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                write.SetTile(position, read.GetTile(position));
            }
        }
    }
    */
}