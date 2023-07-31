using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreText, _highScoreText;

    private CanvasGroup _gameOverUI;
    private const float _fadeDuration = 0.75f;

    private void Awake()
    {
        _gameOverUI = GetComponentInChildren<CanvasGroup>();
        _gameOverUI.interactable = false;
        _gameOverUI.alpha = 0f;
    }

    public void GameOverUI()
    {
        _gameOverUI.DOFade(1.0f, _fadeDuration).OnComplete(() =>
        {
            _gameOverUI.interactable = true;
        });
    }

    public void SetScore(int score)
    {
        _scoreText.SetText(score.ToString());
    }

    public void SetHighScore(int highScore)
    {
        _highScoreText.SetText(highScore.ToString());
    }
}
