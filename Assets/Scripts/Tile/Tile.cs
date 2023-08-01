using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Tile : MonoBehaviour
{
    public Cell Cell { get; private set; }
    public int TileValue { get; private set; }
    public bool CanMerge { get; set; }
    public static float MovementDuration { get => _movementDuration; }

    private Image _background;
    private TextMeshProUGUI _text;
    private ColorScheme _colorScheme;

    private const float _movementDuration = .1f;
    private const float _scaleMultipier = 1.1f;

    public void MoveTo(Cell cell)
    {
        if(Cell != null) Cell.Tile = null;

        cell.Tile = this;
        transform.DOMove(cell.transform.position, _movementDuration);
        Cell = cell;
    }

    public void Spawn(Cell cell, int value)
    {
        if(Cell != null) Cell.Tile = null;
     
        cell.Tile = this;
        transform.position = cell.transform.position;
        Cell = cell;
        CanMerge = true;

        TileValue = value;
        _colorScheme = ColorManager.Instance.GetColor(value);
        UpdateTileVisual();
    }

    public void SetTileValue(int value)
    {
        TileValue = value;
        _colorScheme = ColorManager.Instance.GetNextColor(_colorScheme);
        UpdateTileVisual();

        transform.DOScale(transform.localScale * _scaleMultipier, _movementDuration).SetLoops(2, LoopType.Yoyo);
    }

    private void UpdateTileVisual()
    {
        _text.SetText(TileValue.ToString());
        _background.color = _colorScheme.backgroundColor;
        _text.color = _colorScheme.textColor;
    }

    public void Merge(Cell cell)
    {
        if(Cell != null) Cell.Tile = null;

        transform.SetAsFirstSibling();
        transform.DOMove(cell.transform.position, _movementDuration).OnComplete(() => {
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
