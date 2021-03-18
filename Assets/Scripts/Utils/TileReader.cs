using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

// Helper class used to translate between [tileName <-> tileBase <-> tileEnum]
// This is the one part of the new codebase that I think is pretty bad lol
public class TileReader
{
    // Update this enum each time we add a new playmat tile (IE hell tiles, and maybe lava?)
    public enum TileType { Zebra, Sidewalk, Street }

    [System.Serializable]
    public class NamedTile
    {
        public TileType type;
        public TileBase tile;
    }

    private BiMap<TileBase, string> tileNames = new BiMap<TileBase, string>();
    private BiMap<TileBase, TileType> tileTypes = new BiMap<TileBase, TileType>();

    public TileReader(List<NamedTile> tiles)
    {
        foreach (NamedTile value in tiles)
        {
            if (value.tile == null || tileNames.Contains(value.tile.name) || tileTypes.Contains(value.type))
                continue; // Could log this instead?
            tileNames.Add(value.tile, value.tile.name);
            tileTypes.Add(value.tile, value.type);
        }
    }

    public TileBase GetTile(TileType type) => tileTypes.GetOrDefault(type, null);
    public TileBase GetTile(string name) => tileNames.GetOrDefault(name, null);
    public TileType? GetType(TileBase tile)
    {
        if (!tileTypes.Contains(tile))
            return null;
        return tileTypes.Forward[tile];
    }
    public TileType? GetType(string name) => GetType(GetTile(name));
}
