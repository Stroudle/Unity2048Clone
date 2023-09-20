using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private Tile _tilePrefab;
    [SerializeField]
    private Transform _tileParentTransform;
    
    private List<Tile> _tileList;
    #endregion

    #region Public Methods
    public void ClearBoard(CellGrid grid)
    {
        foreach (var cell in grid.Cells) 
        {
            if(cell.Tile != null)
            {
                Destroy(cell.Tile.gameObject);
            }

            cell.Tile = null;
        }

        _tileList.Clear();
    }

    public void SpawnTile(CellGrid grid, WeightedRandomGenerator rng, float tweenDuration)
    {
        if(_tileList.Count != grid.Size)
        {
            Tile tile = Instantiate(_tilePrefab, _tileParentTransform.transform);
            tile.Spawn(grid.GetRandomEmptyCell(), rng.GetRandomNumber(), tweenDuration);
            _tileList.Add(tile);
        }
    }

    public IEnumerator MoveTiles(CellGrid grid, Vector2Int direction, float tweenDuration)
    {
        int xOrigin, yOrigin, xStep, yStep;
        CalculateDirectionValues(grid, direction, out xOrigin, out yOrigin, out xStep, out yStep);

        bool boardChanged = false;

        for(int x = xOrigin; x >= 0 && x < grid.Width; x += xStep)
        {
            for(int y = yOrigin; y >= 0 && y < grid.Height; y += yStep)
            {
                Cell cell = grid.GetCell(x, y);

                if(cell.Occupied)
                {
                    boardChanged |= MoveTile(grid, cell.Tile, direction);
                }
            }
        }

        yield return boardChanged ? new WaitForSeconds(tweenDuration) : null;
    }

    public bool IsGameOver(CellGrid grid)
    {
        if(_tileList.Count != grid.Size)
        {
            return false;
        }

        foreach(var tile in _tileList)
        {
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

            foreach(var direction in directions)
            {
                Cell adjacentCell = grid.GetAdjacentCell(tile.Cell, direction);
                if(adjacentCell != null && CanMerge(tile, adjacentCell.Tile))
                {
                    return false;
                }
            }
        }

        return true;
    }
    #endregion

    private void Awake()
    {
        _tileList = new List<Tile>(16);
    }

    #region Methods
    private void CalculateDirectionValues(CellGrid grid, Vector2Int direction, out int xOrigin, out int yOrigin, out int xStep, out int yStep)
    {
        xOrigin = direction.x == 1 ? grid.Width - 1 : 0;
        xStep = direction.x == 1 ? -1 : 1;

        yOrigin = direction.y == -1 ? grid.Height - 1 : 0;
        yStep = direction.y == -1 ? -1 : 1;
    }

    public void ResetTiles()
    {
        foreach(var tile in _tileList)
        {
            tile.CanMerge = true;
        }
    }

    private bool MoveTile(CellGrid grid, Tile tile, Vector2Int direction)
    {
        Cell destination = null;
        Cell adjacent = grid.GetAdjacentCell(tile.Cell, direction);

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
            adjacent = grid.GetAdjacentCell(adjacent, direction);
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
        a.MergeInto(b.Cell);
        b.ApplyMerge();
    }
    #endregion
}