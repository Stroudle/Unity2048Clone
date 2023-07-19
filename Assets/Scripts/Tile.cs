using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Cell cell { get; private set; }
    public int number { get; private set; }

    private Image background;
    private TextMeshProUGUI text;

    public void MoveTo(Cell cell)
    {
        Spawn(cell);
    }

    public void Spawn(Cell cell)
    {
        if(this.cell != null)
        {
            this.cell.tile = null;
        }

        cell.tile = this;
        transform.position = cell.transform.position; 
        this.cell = cell;
    }

    private void Awake()
    {
        background= GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
}
