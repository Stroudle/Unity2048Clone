using DG.Tweening;
using UnityEngine;

public class UIController : MonoBehaviour
{
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
}
