using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Board _board;
    [SerializeField]
    private UIController _uiController;

    private int Score
    {
        get => _score;
        set
        {
            _score = value;
            _uiController.SetScore(value);
        }
    }

    private int HighScore
    {
        get => _highScore;
        set
        {
            _highScore = value;
            _uiController.SetHighScore(value);
        }
    }

    private int _score;
    private int _highScore;

    private const int TileCount = 2;

    private void Awake()
    {
        HighScore = LoadHighScore();
    }

    private void Start()
    {
        NewGame();
    }

    public void GameOver()
    {
        _uiController.GameOverUI();
    }

    public void NewGame()
    {
        Score = 0;
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

    public void IncreaseScore(int points)
    {
        Score += points;
        if(Score > HighScore)
        {
            SaveHighScore();
        }
    }
    private void SaveHighScore()
    {        
        HighScore = Score;
        PlayerPrefs.SetInt("highscore", HighScore);
    }

    private int LoadHighScore()
    {
        return PlayerPrefs.GetInt("highscore", 0);
    }
}
