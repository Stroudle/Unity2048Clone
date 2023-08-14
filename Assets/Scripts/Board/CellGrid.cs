using System.Linq;
using UnityEngine;

public class CellGrid : MonoBehaviour
{
    #region Properties
    public CellRow[] Rows { get; private set; }
    public Cell[] Cells { get; private set; }

    public int Size => Cells.Length;
    public int Height => Rows.Length;
    public int Width => Size / Height;
    #endregion

    private System.Random _random = new System.Random();

    #region Public Methods
    public Cell GetAdjacentCell(Cell cell, Vector2Int direction)
    {
        Vector2Int coordinates = cell.Coordinates;
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
        if(x >= 0 && x < Width && y >= 0 && y < Height)
        {
            return Rows[y].Cells[x];
        }

        return null;
    }

    public Cell GetRandomEmptyCell()
    {
        var emptyCells = Cells.Where(cell => !cell.Occupied);
        return emptyCells.OrderBy(_ => _random.Next()).FirstOrDefault();
    }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        Rows = GetComponentsInChildren<CellRow>();
        Cells = GetComponentsInChildren<Cell>();
    }

    private void Start() 
    {
        AssignCellCoordinates();
    }
    #endregion

    private void AssignCellCoordinates()
    {
        for(int y = 0; y < Height; y++)
        {
            for(int x = 0; x < Rows[y].Cells.Length; x++)
            {
                Rows[y].Cells[x].Coordinates = new Vector2Int(x, y);
            }
        }
    }
}