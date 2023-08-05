using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public delegate void GameOver();
    public event GameOver OnGameOver;

    public delegate void IncreaseScore(int value);
    public event IncreaseScore OnIncreaseScore;

    [SerializeField]
    private Tile _tilePrefab;
    [SerializeField]
    private Transform _tileParentObject;

    private CellGrid _grid;
    private List<Tile> _tileList;
    private WeightedRandomGenerator _rng;

    private void Awake()
    {
        _grid = GetComponentInChildren<CellGrid>();
        _tileList = new List<Tile>(16);

        _rng = new WeightedRandomGenerator();
        _rng.AddNumberWithWeight(2, 95.0f);
        _rng.AddNumberWithWeight(4, 5.0f);
    }

    private void OnEnable()
    {
        KeyboardInput.OnKeyboardInput += OnInputRecievedHandler;
        MobileInput.OnMobileInput += OnInputRecievedHandler;
    }

    private void OnDisable() 
    {
        KeyboardInput.OnKeyboardInput -= OnInputRecievedHandler;
        MobileInput.OnMobileInput += OnInputRecievedHandler;
    }

    public void ClearBoard()
    {
        foreach (var cell in _grid.Cells) 
        {
            if(cell.Tile != null)
            {
                Destroy(cell.Tile.gameObject);
            }

            cell.Tile = null;
        }

        _tileList.Clear();
    }

    public void SpawnTile()
    {
        if(_tileList.Count != _grid.Size)
        {
            Tile tile = Instantiate(_tilePrefab, _tileParentObject.transform);
            tile.Spawn(_grid.GetRandomEmptyCell(), _rng.GetRandomNumber());
            _tileList.Add(tile);
        }
    }

    private void OnInputRecievedHandler(Vector2Int input)
    {
        MoveTiles(input);
    }

    private void MoveTiles(Vector2Int direction)
    {
        int xOrigin, yOrigin, xStep, yStep;
        CalculateDirectionValues(direction, out xOrigin, out yOrigin, out xStep, out yStep);

        bool boardChanged = false;

        for(int x = xOrigin; x >= 0 && x < _grid.Width; x += xStep)
        {
            for(int y = yOrigin; y >= 0 && y < _grid.Height; y += yStep)
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

    private void CalculateDirectionValues(Vector2Int direction, out int xOrigin, out int yOrigin, out int xStep, out int yStep)
    {
        xOrigin = direction.x == 1 ? _grid.Width - 1 : 0;
        xStep = direction.x == 1 ? -1 : 1;

        yOrigin = direction.y == -1 ? _grid.Height - 1 : 0;
        yStep = direction.y == -1 ? -1 : 1;
    }

    private IEnumerator WaitForChanges()
    {
        InputBlocker.BlockInputs();
        yield return new WaitForSeconds(Tile.TweeningDuration);
        InputBlocker.UnblockInputs();
        FinalizeRound();
    }

    private void FinalizeRound()
    {
        ResetTiles();
        SpawnTile();
        CheckForGameOver();
    }

    private void ResetTiles()
    {
        foreach(var tile in _tileList)
        {
            tile.CanMerge = true;
        }
    }

    private void CheckForGameOver()
    {
        if(_tileList.Count != _grid.Size)
        {
            return;
        }

        foreach(var tile in _tileList)
        {
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

            foreach(var direction in directions)
            {
                Cell adjacentCell = _grid.GetAdjacentCell(tile.Cell, direction);
                if(adjacentCell != null && CanMerge(tile, adjacentCell.Tile))
                {
                    return;
                }
            }
        }

        OnGameOver?.Invoke();
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
        _tileList.Remove(a);
        a.Merge(b.Cell);

        int value = b.TileValue * 2;
        b.SetMergedTileValue(value);
        OnIncreaseScore?.Invoke(value);
    }
}