using System.Linq;
using UnityEngine;

public class Grid2048 : MonoBehaviour
{
    public Row[] rows { get; private set; }
    public Cell[] cells { get; private set; }

    public int size => cells.Length;
    public int height => rows.Length;
    public int width => size / height;

    public Cell GetAdjacentCell(Cell cell, Vector2Int direction)
    {
        Vector2Int coordinates = cell.coordinates;
        coordinates.x += direction.x;
        coordinates.y -= direction.y;

        return GetCell(coordinates);
    }

    public Cell GetCell(Vector2Int coordinates)
    {
        return GetCell(coordinates.x, coordinates.y);
    }

    public Cell GetCell(int x, int y)
    {
        if(x >= 0 && x < width && y >= 0 && y < height)
        {
            return rows[y].cells[x];
        }

        return null;
    }

    public Cell GetRandomEmptyCell()
    {
        System.Random random = new();
        var emptyCells = cells.Where(cell => !cell.Occupied);
        return emptyCells.OrderBy(_ => random.Next()).FirstOrDefault();
    }

    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();
        cells = GetComponentsInChildren<Cell>();
    }

    private void Start() 
    {
        AssignCellCoordinates();
    }

    private void AssignCellCoordinates()
    {
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < rows[y].cells.Length; x++)
            {
                rows[y].cells[x].coordinates = new Vector2Int(x, y);
            }
        }
    }
}
