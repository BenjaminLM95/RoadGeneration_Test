using UnityEngine;

public enum TileType 
{
    Road,
    Building,
    Park,
    Sidewalk
}

public class Tile
{
    public TileType tileType;

    public TileType upTile;
    public TileType downTile;
    public TileType leftTile;
    public TileType rightTile;

    public Tile(TileType _tileType) 
    {
        tileType = _tileType;
    }

}
