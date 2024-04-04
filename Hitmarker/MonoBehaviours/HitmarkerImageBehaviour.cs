using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace com.github.zehsteam.Hitmarker.MonoBehaviours;

public class HitmarkerImageBehaviour : MonoBehaviour
{
    public Image image;
    public Color32 defaultColor;
    public Color32 killedColor;

    private Coroutine fadeOutCoroutine;

    private void Start()
    {
        int size = HitmarkerBase.Instance.configManager.HitmarkerImageSize;
        image.rectTransform.sizeDelta = new Vector2(size, size);

        SetAlpha(0f);
    }

    public void ShowImage(bool killed = false)
    {
        image.color = killed ? killedColor : defaultColor;

        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        }

        fadeOutCoroutine = StartCoroutine(FadeOut(0.25f));
    }

    private IEnumerator FadeOut(float duration)
    {
        SetAlpha(255f);

        float timer = 0f;
        while (timer < duration)
        {
            float percent = (1f / duration) * timer;
            float alpha = 255f + (0f - 255f) * percent;

            SetAlpha(alpha);

            yield return null;
            timer += Time.deltaTime;
        }

        SetAlpha(0f);
    }

    private void SetAlpha(float a)
    {
        Color32 color = image.color;
        color.a = (byte)a;
        image.color = color;
    }
}
