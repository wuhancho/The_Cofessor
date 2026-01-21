using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
    public static FadeController Instance { get; private set; }
    [SerializeField] private Image fadeImage;
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (fadeImage == null)
        {
            // Crear Canvas + Image dinámicamente si no se asignó en el inspector
            var canvasGO = new GameObject("FadeCanvas");
            var canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 1000;
            DontDestroyOnLoad(canvasGO);

            var imgGO = new GameObject("FadeImage");
            imgGO.transform.SetParent(canvasGO.transform, false);
            fadeImage = imgGO.AddComponent<Image>();
            fadeImage.color = new Color(0f, 0f, 0f, 0f);
            var rect = fadeImage.rectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
            DontDestroyOnLoad(imgGO);
        }
    }

    public void FadeAndLoadScene(string sceneName, float duration)
    {
        StartCoroutine(FadeOutAndLoad(sceneName, duration));
    }

    public void FadeOut(float duration, Action onComplete = null)
    {
        StartCoroutine(FadeRoutine(0f, 1f, duration, onComplete));
    }

    public void FadeIn(float duration, Action onComplete = null)
    {
        Debug.Log("FadeIn started");
        StartCoroutine(FadeRoutine(1f, 0f, duration, onComplete));
    }

    private IEnumerator FadeOutAndLoad(string sceneName, float duration)
    {
        yield return FadeRoutineCoroutine(0f, 1f, duration);
        SceneManager.LoadScene(sceneName);
        // opcional: esperar un frame y hacer fade-in
        yield return null;
        yield return FadeRoutineCoroutine(1f, 0f, duration);
    }

    private IEnumerator FadeRoutine(float from, float to, float duration, Action onComplete)
    {
        
        yield return FadeRoutineCoroutine(from, to, duration);
        onComplete?.Invoke();
    }

    private IEnumerator FadeRoutineCoroutine(float from, float to, float duration)
    {
        if (fadeImage == null) yield break;
        float t = 0f;
        Color c = fadeImage.color;
        while (t < duration)
        {
            fadeImage.raycastTarget = true;
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(from, to, Mathf.Clamp01(t / duration));
            fadeImage.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }
        fadeImage.raycastTarget = to > 0f;
        fadeImage.color = new Color(c.r, c.g, c.b, to);
    }

}