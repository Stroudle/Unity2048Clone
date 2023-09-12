using System.Collections;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public delegate void GameOver();
    public event GameOver OnGameOver;

    #region Fields
    private Board _board;
    private CellGrid _grid;
    private WeightedRandomGenerator _rng;
    #endregion

    public void ClearBoard()
    {
        _board.ClearBoard(_grid);
    }

    public void SpawnTile()
    {
        _board.SpawnTile(_grid, _rng);
    }

    #region Unity Methods
    private void Awake()
    {
        _board = GetComponent<Board>();
        _grid = GetComponentInChildren<CellGrid>();
        _rng = new WeightedRandomGenerator();
        _rng.AddNumberWithWeight(2, 95.0f);
        _rng.AddNumberWithWeight(4, 5.0f);
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
        yield return _board.MoveTiles(_grid, _rng, input);
        InputBlocker.UnblockInputs();
        RoundEnd();
    }

    private void RoundEnd()
    {
        _board.FinalizeRound(_grid, _rng);
        if(_board.IsGameOver(_grid))
        {
            OnGameOver?.Invoke();
        }
    }
}