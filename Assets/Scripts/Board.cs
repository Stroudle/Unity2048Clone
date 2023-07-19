using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private Tile tilePrefab;

    private Grid2048 grid;
    private List<Tile> tiles;

    private const int START_TILES = 2;

    private void Awake()
    {
        grid = GetComponentInChildren<Grid2048>();
        tiles = new List<Tile>(16);
    }

    private void Start() 
    {
        SpawnTiles();
    }

    private void OnEnable()
    {
        InputHandler.OnMovementInput += OnMovementInputHandler;
    }

    private void OnDisable() 
    {
        InputHandler.OnMovementInput -= OnMovementInputHandler;
    }

    private void SpawnTiles()
    {
        for(int i = 0; i < START_TILES; i++)
        {
            SpawnTile();
        }
    }

    private void SpawnTile()
    {
        Tile tile = Instantiate(tilePrefab, grid.transform);
        tile.Spawn(grid.GetRandomEmptyCell());
        tiles.Add(tile);
    }

    //modificar para input unity
    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.W))
    //    {
    //        MoveTiles(Vector2Int.up);
    //    }
    //    else if(Input.GetKeyDown(KeyCode.A))
    //    {
    //        MoveTiles(Vector2Int.left);
    //    }
    //    else if(Input.GetKeyDown(KeyCode.S))
    //    {
    //        MoveTiles(Vector2Int.down);
    //    }
    //    else if(Input.GetKeyDown(KeyCode.D))
    //    {
    //        MoveTiles(Vector2Int.right);
    //    }
    //}

    private void OnMovementInputHandler(Vector2Int input) => MoveTiles(input);

    private void MoveTiles(Vector2Int direction)
    {
        int xStart = direction.x == 1 ? 0 : (direction.x == -1 ? grid.width - 1 : 0);
        int xIncrement = direction.x == 1 ? 1 : (direction.x == -1 ? -1 : 1);

        int yStart = direction.y == 1 ? 0 : (direction.y == -1 ? grid.height - 1 : 0);
        int yIncrement = direction.y == 1 ? 1 : (direction.y == -1 ? -1 : 1);

        for(int x = xStart; x >= 0 && x < grid.width; x += xIncrement)
        {
            for(int y = yStart; y >= 0 && y < grid.height; y += yIncrement)
            {
                Cell cell = grid.GetCell(x, y);

                if(cell.Occupied)
                {
                    MoveTile(cell.tile, direction);
                }
            }
        }
    }

    private void MoveTile(Tile tile, Vector2Int direction)
    {
        Cell destination = null;
        Cell adjacent = grid.GetAdjacentCell(tile.cell, direction);

        while(adjacent != null)
        {
            if(adjacent.Occupied)
            {
                break;
            }

            destination  = adjacent;
            adjacent = grid.GetAdjacentCell(adjacent, direction);
        }

        if(destination != null)
        {
            tile.MoveTo(destination);
        }
    }
}
