using System.Collections;
using UnityEngine;

public class ArrowFader : MonoBehaviour
{
    [SerializeField] private float fadeSpeed = 10f;
    [SerializeField] private float maxAlpha = 0.8f;
    private SpriteRenderer sr;
    private Coroutine fadeRoutine;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        SetAlpha(0f); // Start invisible
    }

    public void FadeIn()
    {
        StartFade(maxAlpha);
    }

    public void FadeOut()
    {
        StartFade(0f);
    }

    private void StartFade(float targetAlpha)
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeTo(targetAlpha));
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        float startAlpha = sr.color.a;

        while (!Mathf.Approximately(sr.color.a, targetAlpha))
        {
            float newAlpha = Mathf.MoveTowards(sr.color.a, targetAlpha, Time.deltaTime * fadeSpeed);
            SetAlpha(newAlpha);
            yield return null;
        }

        SetAlpha(targetAlpha);
        fadeRoutine = null;
    }

    private void SetAlpha(float alpha)
    {
        Color c = sr.color;
        c.a = alpha;
        sr.color = c;
    }
}