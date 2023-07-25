using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Tile : MonoBehaviour
{
    public Cell Cell { get; private set; }
    public int TileValue { get; private set; }
    public bool CanMerge { get; set; }

    private Image _background;
    private TextMeshProUGUI _text;

    private const float _tweenDuration = .1f;
    private const float _scaleMultipier = 1.1f;

    public void MoveTo(Cell cell)
    {

        if(this.Cell != null)
        {
            this.Cell.Tile = null;
        }

        cell.Tile = this;
        transform.DOMove(cell.transform.position, _tweenDuration);
        this.Cell = cell;
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
        CanMerge = true;
    }

    public void SetTileValue(int tileNumber)
    {
        this.TileValue = tileNumber;
        this._text.SetText(TileValue.ToString());
        transform.DOScale(transform.localScale * _scaleMultipier, _tweenDuration).SetLoops(2, LoopType.Yoyo);
    }

    public void Merge(Cell cell)
    {
        if(this.Cell != null)
        {
            this.Cell.Tile = null;
        }

        transform.DOMove(cell.transform.position, _tweenDuration).OnComplete(() => {
            Destroy(gameObject);
        });

        Cell = null;
        CanMerge = false;
    }

    private void Awake()
    {
        _background= GetComponent<Image>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }
}
