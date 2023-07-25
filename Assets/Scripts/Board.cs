using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private Tile _tilePrefab;

    private Grid2048 _grid;
    private List<Tile> _tiles;
    private System.Random _random = new System.Random();

    private const int InitialTileCount = 2;
    private readonly (int TileValue, double Percentage) [] PossibleTileValues = {
        (2, 90.0),
        (4, 10.0),
    };

    private void Awake()
    {
        _grid = GetComponentInChildren<Grid2048>();
        _tiles = new List<Tile>(16);
    }

    private void Start() 
    {
        SpawnTiles();
    }

    private void OnEnable()
    {
        InputHandler.OnKeyboardInput += OnInputRecievedHandler;
        SwipeDetection.OnSwipeInput += OnInputRecievedHandler;
    }

    private void OnDisable() 
    {
        InputHandler.OnKeyboardInput -= OnInputRecievedHandler;
        SwipeDetection.OnSwipeInput += OnInputRecievedHandler;
    }

    private void SpawnTiles()
    {
        for(int i = 0; i < InitialTileCount; i++)
        {
            SpawnTile();
        }
    }

    private void SpawnTile()
    {
        Tile tile = Instantiate(_tilePrefab, _grid.transform);
        tile.SetTileValue(GetRandomTileValue());
        tile.Spawn(_grid.GetRandomEmptyCell());
        _tiles.Add(tile);
    }

    private int GetRandomTileValue()
    {
        double randomNumber = _random.NextDouble() * 100;
        double cumulativePercentage = 0;
        foreach(var (tileValue, percentage) in PossibleTileValues)
        {
            cumulativePercentage += percentage;
            if(randomNumber < cumulativePercentage)
            {
                return tileValue;
            }
        }
        return PossibleTileValues[0].TileValue;
    }

    private void OnInputRecievedHandler(Vector2Int input)
    {
        MoveTiles(input);
    }

    private void MoveTiles(Vector2Int direction)
    {
        int xStart = direction.x == 1 ? (_grid.Width - 1) : (direction.x == -1 ? 0 : 0);
        int xIncrement = direction.x == 1 ? -1 : (direction.x == -1 ? 1 : 1);

        int yStart = direction.y == 1 ? 0 : (direction.y == -1 ? (_grid.Height - 1) : 0);
        int yIncrement = direction.y == 1 ? 1 : (direction.y == -1 ? -1 : 1);

        for(int x = xStart; x >= 0 && x < _grid.Width; x += xIncrement)
        {
            for(int y = yStart; y >= 0 && y < _grid.Height; y += yIncrement)
            {
                Cell cell = _grid.GetCell(x, y);

                if(cell.Occupied)
                {
                    MoveTile(cell.Tile, direction);
                }
            }
        }

        foreach (var tile in _tiles)
        {
            tile.CanMerge = true;
        }

        if(_tiles.Count != _grid.Size)
        {
            SpawnTile();
        }
    }

    private void MoveTile(Tile tile, Vector2Int direction)
    {
        Cell destination = null;
        Cell adjacent = _grid.GetAdjacentCell(tile.Cell, direction);

        while(adjacent != null)
        {
            if(adjacent.Occupied)
            {
                if(CanMerge(tile, adjacent.Tile))
                {
                    Merge(tile, adjacent.Tile);
                }
                break;
            }

            destination  = adjacent;
            adjacent = _grid.GetAdjacentCell(adjacent, direction);
        }

        if(destination != null)
        {
            tile.MoveTo(destination);
        }
    }

    private bool CanMerge(Tile a, Tile b)
    {
        return a.TileValue == b.TileValue && b.CanMerge;
    }

    private void Merge(Tile a, Tile b)
    {
        _tiles.Remove(a);
        a.Merge(b.Cell);
        b.SetTileValue(a.TileValue + b.TileValue);
    }
}
