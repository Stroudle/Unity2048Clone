using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Tile : MonoBehaviour
{
    public delegate void IncreaseScore(int value);
    public static event IncreaseScore OnIncreaseScore;

    #region Properties
    public Cell Cell { get; private set; }
    public int TileValue { get; private set; }
    public bool CanMerge { get; set; }
    #endregion


    #region Fields
    [SerializeField]
    private float _targetScale;

    private float _tweenDuration;
    private Image _background;
    private TextMeshProUGUI _text;
    private ColorScheme _colorScheme;
    #endregion

    #region Public Methods
    public void MoveTo(Cell cell)
    {
        SetCell(cell);
        transform.DOMove(Cell.transform.position, _tweenDuration);
    }

    public void Spawn(Cell cell, int value, float tweeningDuration)
    {
        SetCell(cell);
        transform.position = Cell.transform.position;

        TileValue = value;
        _colorScheme = ColorManager.Instance.GetColor(value);
        UpdateTileUI();

        _tweenDuration = tweeningDuration;
        transform.DOScale(1, _tweenDuration);
    }

    public void ApplyMerge()
    {
        TileValue = TileValue * 2;
        OnIncreaseScore?.Invoke(TileValue);
        CanMerge = false;

        _colorScheme = ColorManager.Instance.NextColor(_colorScheme);
        UpdateTileUI();

        transform.DOComplete();
        transform.DOScale(_targetScale, _tweenDuration).SetLoops(2, LoopType.Yoyo);
    }


    public void MergeInto(Cell cell)
    {
        if(Cell != null)
        {
            Cell.Tile = null;
        }
        Cell = null;

        transform.SetAsFirstSibling();
        transform.DOMove(cell.transform.position, _tweenDuration).OnComplete(() => {
            Destroy(gameObject);
        });
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
}