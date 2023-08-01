using UnityEngine;

public class CellRow : MonoBehaviour
{
    public Cell[] Cells { get; private set; }

    private void Awake()
    {
        Cells = GetComponentsInChildren<Cell>();
    }
}