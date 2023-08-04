using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Board _board;
    public UIController _uiController;

    private int _score;
    private int _highScore;

    private const int TileCount = 2;

    private void Awake()
    {
        SetHighScore(LoadHighScore());
    }

    private void OnEnable()
    {
        _board.OnGameOver += GameOver;
        _board.OnIncreaseScore += IncreaseScore;
    }

    private void OnDisable()
    {
        _board.OnGameOver -= GameOver;
        _board.OnIncreaseScore -= IncreaseScore;
    }

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        NewGame();
    }

    private void GameOver()
    {
        _uiController.GameOverUI();
    }

    public void NewGame()
    {
        SetScore(0);
        _board.ClearBoard();
        SpawnTiles();
    }

    private void SpawnTiles()
    {
        for(int i = 0; i < TileCount; i++)
        {
            _board.SpawnTile();
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
        _uiController.SetScore(_score);
    }

    private void SetHighScore(int value)
    {
        _highScore = value;
        _uiController.SetHighScore(_highScore);
    }
}
