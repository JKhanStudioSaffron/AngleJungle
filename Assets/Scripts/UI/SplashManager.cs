using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Purpose: Handles the transition effect and navigation to the start screen.
/// Responsibilities:
/// - Fades in and out UI elements using DOTween.
/// - Scales the logo before transitioning to the start scene.
/// - Loads the start scene after animation completion.
/// Usage:
/// - Attach this script to a GameObject in the splash screen scene.
/// - Assign the logo image and text in the inspector.
/// </summary>

public class SplashManager : MonoBehaviour
{
    public Image LogoImage;
    public TMP_Text Text;

    public float FadeInValue, FadeOutValue;
    public float ScaleValue;

    float fadeDuration = 0.6f;
    float delay = 1f;
    float scaleDuration = 1.5f;

    private void Start()
    {
        GoToStartScreen();
    }

    void GoToStartScreen()
    {
        Text.DOFade(FadeInValue, fadeDuration).SetDelay(delay);
        LogoImage.DOFade(FadeInValue, fadeDuration).SetDelay(delay).OnComplete(() =>
        {
            LogoImage.transform.parent.DOScale(ScaleValue, scaleDuration).SetDelay(delay);
            Text.DOFade(FadeOutValue, fadeDuration).SetDelay(delay - 0.5f);
            LogoImage.DOFade(FadeOutValue, fadeDuration).SetDelay(delay + 0.5f).OnComplete(() =>
            {
                SceneManager.LoadScene(Global.SCENE_START);
            });
        });
        
    }
}
