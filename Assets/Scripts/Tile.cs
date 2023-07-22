using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Cell Cell { get; private set; }
    public int TileValue { get; private set; }

    private Image _background;
    private TextMeshProUGUI _text;

    public void MoveTo(Cell cell)
    {
        Spawn(cell);
    }

    public void Spawn(Cell cell)
    {
        if(this.Cell != null)
        {
            this.Cell.Tile = null;
        }

        cell.Tile = this;
        transform.position = cell.transform.position; 
        this.Cell = cell;
    }

    public void SetTileValue(int tileNumber)
    {
        this.TileValue = tileNumber;
        this._text.SetText(TileValue.ToString());
    }

    private void Awake()
    {
        _background= GetComponent<Image>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }
}
