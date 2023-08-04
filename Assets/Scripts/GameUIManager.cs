using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreText, _highScoreText;

    private CanvasGroup _gameOverUI;
    private const float _fadeDuration = 0.5f;

    private void Awake()
    {
        _gameOverUI = GetComponentInChildren<CanvasGroup>();
        _gameOverUI.interactable = false;
        _gameOverUI.alpha = 0f;
    }

    public void ShowGameOverUI()
    {
        _gameOverUI.DOFade(1.0f, _fadeDuration).OnComplete(() =>
        {
            _gameOverUI.interactable = true;
        });
    }

    public void UpdateScoreUI(int value)
    {
        _scoreText.SetText(value.ToString());
    }

    public void UpdateHighScoreUI(int value)
    {
        _highScoreText.SetText(value.ToString());
    }
}