using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Tile : MonoBehaviour
{
    #region Properties
    public Cell Cell { get; private set; }
    public int TileValue { get; private set; }
    public bool CanMerge { get; set; }
    #endregion

    public static readonly float TweeningDuration = 0.1f;

    #region Fields
    private Image _background;
    private TextMeshProUGUI _text;
    private ColorScheme _colorScheme;

    private const float _targetScale = 1.1f;
    #endregion

    #region Public Methods
    public void MoveTo(Cell cell)
    {
        SetCell(cell);
        transform.DOMove(Cell.transform.position, TweeningDuration);
    }

    public void Spawn(Cell cell, int value)
    {
        SetCell(cell);
        transform.position = Cell.transform.position;

        TileValue = value;
        _colorScheme = ColorManager.Instance.GetColor(value);
        UpdateTileUI();

        transform.DOScale(1, TweeningDuration);
    }

    public void SetMergedTileValue(int value)
    {
        TileValue = value;
        CanMerge = false;

        _colorScheme = ColorManager.Instance.NextColor(_colorScheme);
        UpdateTileUI();

        transform.DOComplete();
        transform.DOScale(_targetScale, TweeningDuration).SetLoops(2, LoopType.Yoyo);
    }


    public void Merge(Cell cell)
    {
        if(Cell != null)
        {
            Cell.Tile = null;
        }
        Cell = null;

        transform.SetAsFirstSibling();
        transform.DOMove(cell.transform.position, TweeningDuration).OnComplete(() => {
            Destroy(gameObject);
        });
    }
    #endregion

    #region Methods
    private void UpdateTileUI()
    {
        _text.SetText(TileValue.ToString());
        _background.color = _colorScheme.backgroundColor;
        _text.color = _colorScheme.textColor;
    }

    private void SetCell(Cell cell)
    {
        if(Cell != null)
        {
            Cell.Tile = null;
        }

        Cell = cell;
        cell.Tile = this;
    }
    #endregion

    #region Unity Messages
    private void Awake()
    {
        _background = GetComponent<Image>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        CanMerge = true;
        transform.localScale = new Vector2(0.1f, 0.1f);
    }
    #endregion
}
