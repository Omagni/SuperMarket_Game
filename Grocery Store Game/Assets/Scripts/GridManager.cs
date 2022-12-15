using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Notes:
 * Dictionary: Keys access values. Coordinates (x,y) access the tile. When trying to access a specific tile
 * use the GetTileAtPosition function. Best to use "TryGetValue" function of dictionary in case the key 
 * may or may not exist.
 * 
 * Possible to access key with square brackers (but only when you know for sure the key exists in dicitonary)
 * 
 * Someones observation:
 * When instantiating your dictionary it may be good practice to pass in the capacity (width * height in this case)
 * of it as to not consume more memory than necessary whenever the dictionary needs to resize its capacity as you 
 * add more tiles. It may be also be worth to consider using a 2d array if you are nit picky about performance and 
 * memory as you don't have to save a key for each tile. I would only suggest this if you plan to make a giant 
 * array however, may not be necessary or even optimal for small projects.
 * 
 */

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Transform cam;

    // Dictionary holds all tiles.
    private Dictionary<Vector2, Tile> tiles; // key, value - Keys (coordiantes) specify which item you want.

    // Create grid at the start of the game
    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 != y % 2); // Creates checkboard look. 
                spawnedTile.Init(isOffset);

                //get tile position
                tiles[new Vector2(x,y)] = spawnedTile;
            }
        }
        cam.transform.position = new Vector3( width / 2f - 0.5f, height / 2f - 0.5f, -10 );
    }

    // get tiles at a specified position (in dictionary)
    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (tiles.TryGetValue(pos, out var tile)) // if the tile is available
            return tile;
        return null;
    }

}
