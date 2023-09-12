using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private BoardManager _boardManager;
    [SerializeField]
    private GameUIManager _uiManager;

    private int _score;
    private int _highScore;

    private const int TileCount = 2;
    #endregion

    public void NewGame()
    {
        _uiManager.HideGameOverUI();
        SetScore(0);
        _boardManager.ClearBoard();
        SpawnTiles();
    }

    #region Unity Messages
    private void Awake()
    {
        SetHighScore(LoadHighScore());
    }

    private void OnEnable()
    {
        _boardManager.OnGameOver += GameOver;
        Tile.OnIncreaseScore += IncreaseScore;
    }

    private void OnDisable()
    {
        _boardManager.OnGameOver -= GameOver;
        Tile.OnIncreaseScore -= IncreaseScore;
    }

    private void Start()
    {
        NewGame();
    }
    #endregion

    #region Methods
    private void GameOver()
    {
        _uiManager.ShowGameOverUI();
    }

    private void SpawnTiles()
    {
        for(int i = 0; i < TileCount; i++)
        {
            _boardManager.SpawnTile();
        }
    }

    private void IncreaseScore(int value)
    {
        SetScore(_score + value);

        if(_score > _highScore)
        {
            SaveHighScore();
        }
    }

    private void SaveHighScore()
    {
        SetHighScore(_score);
        PlayerPrefs.SetInt("highscore", _highScore);
    }

    private int LoadHighScore()
    {
        return PlayerPrefs.GetInt("highscore", 0);
    }

    private void SetScore(int value)
    {
        _score = value;
        _uiManager.UpdateScoreUI(_score);
    }

    private void SetHighScore(int value)
    {
        _highScore = value;
        _uiManager.UpdateHighScoreUI(_highScore);
    }
    #endregion
}