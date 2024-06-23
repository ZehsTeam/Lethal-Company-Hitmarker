using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace com.github.zehsteam.Hitmarker.MonoBehaviours;

public class HitmarkerImageBehaviour : MonoBehaviour
{
    public Image Image;
    public Color32 DefaultColor;
    public Color32 KilledColor;

    private Coroutine _fadeOutCoroutine;

    private void Start()
    {
        int size = Plugin.ConfigManager.HitmarkerImageSize.Value;
        Image.rectTransform.sizeDelta = new Vector2(size, size);

        SetAlpha(0f);
    }

    public void ShowImage(bool killed = false)
    {
        Image.color = killed ? KilledColor : DefaultColor;

        if (_fadeOutCoroutine != null)
        {
            StopCoroutine(_fadeOutCoroutine);
        }

        _fadeOutCoroutine = StartCoroutine(FadeOut(0.25f));
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
        a = Mathf.Clamp(a, 0f, 255f);
        Color32 color = Image.color;
        color.a = (byte)a;
        Image.color = color;
    }
}
