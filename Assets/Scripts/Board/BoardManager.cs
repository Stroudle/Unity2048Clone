using System.Collections;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public delegate void GameOver();
    public event GameOver OnGameOver;


    #region Fields
    private WeightedRandomGenerator _weightedRng;
    private float _tweenDuration;
    private Board _board;
    private CellGrid _grid;
    #endregion

    public void ClearBoard()
    {
        _board.ClearBoard(_grid);
    }

    public void SpawnTile()
    {
        _board.SpawnTile(_grid, _weightedRng, _tweenDuration);
    }

    public void SetupBoard(WeightedRandomGenerator rng, float tweenDuration)
    {
        _weightedRng = rng;
        _tweenDuration = tweenDuration;
    }

    #region Unity Methods
    private void Awake()
    {
        _board = GetComponent<Board>();
        _grid = GetComponentInChildren<CellGrid>();
    }
    private void OnEnable()
    {
        KeyboardInput.OnKeyboardInput += OnInputReceivedHandler;
        MobileInput.OnMobileInput += OnInputReceivedHandler;
    }

    private void OnDisable()
    {
        KeyboardInput.OnKeyboardInput -= OnInputReceivedHandler;
        MobileInput.OnMobileInput -= OnInputReceivedHandler;
    }
    #endregion

    private void OnInputReceivedHandler(Vector2Int input)
    {
        StartCoroutine(MoveTiles(input));
    }

    private IEnumerator MoveTiles(Vector2Int input)
    {
        InputBlocker.BlockInputs();
        yield return _board.MoveTiles(_grid, input, _tweenDuration);
        InputBlocker.UnblockInputs();
        RoundEnd();
    }

    private void RoundEnd()
    {
        _board.ResetTiles();
        _board.SpawnTile(_grid, _weightedRng, _tweenDuration);
        if(_board.IsGameOver(_grid))
        {
            OnGameOver?.Invoke();
        }
    }
}