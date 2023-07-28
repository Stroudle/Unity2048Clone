using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.UIElements;

public class Tile : MonoBehaviour
{
    public Cell Cell { get; private set; }
    public int TileValue { get; private set; }
    public bool CanMerge { get; set; }
    public static float MovementDuration { get => _movementDuration; }

    private UnityEngine.UI.Image _background;
    private TextMeshProUGUI _text;
    private Tween _moveTween;
    private TileColorManager _colorManager = TileColorManager.Instance;

    private const float _movementDuration = .1f;
    private const float _scaleMultipier = 1.1f;

    public void MoveTo(Cell cell)
    {

        if(this.Cell != null)
        {
            this.Cell.Tile = null;
        }

        cell.Tile = this;
        _moveTween = transform.DOMove(cell.transform.position, _movementDuration);
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
        TileValue = tileNumber;
        _text.SetText(TileValue.ToString());
        _text.color = _colorManager.GetTextColor(TileValue);
        _background.color = _colorManager.GetBackgroundColor(TileValue);

        transform.DOScale(transform.localScale * _scaleMultipier, _movementDuration).SetLoops(2, LoopType.Yoyo);
    }

    public void Merge(Cell cell)
    {
        if(this.Cell != null)
        {
            this.Cell.Tile = null;
        }

        transform.SetAsFirstSibling();
        transform.DOMove(cell.transform.position, _movementDuration).OnComplete(() => {
            Destroy(gameObject);
        });

        Cell = null;
        CanMerge = false;
    }

    private void Awake()
    {
        _background= GetComponent<UnityEngine.UI.Image>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }
}
