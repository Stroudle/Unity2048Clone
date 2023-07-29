using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private Tile _tilePrefab;
    [SerializeField]
    private GameObject _tilesParent;

    private Grid2048 _grid;
    private List<Tile> _tiles;
    private TileValueGenerator _rng;
    private bool _lockInput;

    private const int InitialTileCount = 2;

    private void Awake()
    {
        _grid = GetComponentInChildren<Grid2048>();
        _tiles = new List<Tile>(16);
    }

    private void Start() 
    {
        _rng = new TileValueGenerator();
        _rng.AddValueWithPercentage(2, 95.0f);
        _rng.AddValueWithPercentage(4, 5.0f);

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
        Tile tile = Instantiate(_tilePrefab, _tilesParent.transform);
        tile.Spawn(_grid.GetRandomEmptyCell(), _rng.GetRandomValue());
        _tiles.Add(tile);
    }

    private void OnInputRecievedHandler(Vector2Int input)
    {
        if(!_lockInput) MoveTiles(input);      
    }

    private void MoveTiles(Vector2Int direction)
    {
        int xStart = direction.x == 1 ? (_grid.Width - 1) : (direction.x == -1 ? 0 : 0);
        int xIncrement = direction.x == 1 ? -1 : (direction.x == -1 ? 1 : 1);

        int yStart = direction.y == 1 ? 0 : (direction.y == -1 ? (_grid.Height - 1) : 0);
        int yIncrement = direction.y == 1 ? 1 : (direction.y == -1 ? -1 : 1);

        bool boardChanged = false;
        for(int x = xStart; x >= 0 && x < _grid.Width; x += xIncrement)
        {
            for(int y = yStart; y >= 0 && y < _grid.Height; y += yIncrement)
            {
                Cell cell = _grid.GetCell(x, y);

                if(cell.Occupied)
                {
                    boardChanged |= MoveTile(cell.Tile, direction);
                }
            }
        }

        if(boardChanged)
        {
            StartCoroutine(WaitForChanges());
        }
    }

    private IEnumerator WaitForChanges()
    {
        _lockInput = true;

        yield return new WaitForSeconds(Tile.MovementDuration);

        _lockInput= false;

        FinalizeRound();
    }

    private void FinalizeRound()
    {
        foreach(var tile in _tiles)
        {
            tile.CanMerge = true;
        }

        if(_tiles.Count != _grid.Size)
        {
            SpawnTile();
        }
    }

    private bool MoveTile(Tile tile, Vector2Int direction)
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
                    return true;
                }
                break;
            }

            destination  = adjacent;
            adjacent = _grid.GetAdjacentCell(adjacent, direction);
        }

        if(destination != null)
        {
            tile.MoveTo(destination);
            return true;
        }
        return false;
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
