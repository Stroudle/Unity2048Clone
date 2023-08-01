using UnityEngine;

public class Row : MonoBehaviour
{
    public Cell[] Cells { get; private set; }

    private void Awake()
    {
        Cells = GetComponentsInChildren<Cell>();
    }
}