using System.Collections;
using TMPro;
using UnityEngine;

namespace com.github.zehsteam.Hitmarker.MonoBehaviours;

public class MessageItemBehaviour : MonoBehaviour
{
    public TextMeshProUGUI TextUGUI;

    private Coroutine _animationCoroutine;

    private void Start()
    {
        TextUGUI.fontSize = Plugin.ConfigManager.MessageFontSize.Value;
    }

    private void OnEnable()
    {
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }

        _animationCoroutine = StartCoroutine(PlayAnimation());
    }

    private void OnDisable()
    {
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }
    }

    private IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(Plugin.ConfigManager.MessageDuration.Value);

        float fadeOutDuration = 1f;

        SetAlpha(255f);

        float timer = 0f;
        while (timer < fadeOutDuration)
        {
            float percent = 1f / fadeOutDuration * timer;
            float alpha = 255f + (0f - 255f) * percent;

            SetAlpha(alpha);

            yield return null;
            timer += Time.deltaTime;
        }

        SetAlpha(0f);

        yield return null;

        gameObject.SetActive(false);
    }

    public void SetText(string text)
    {
        SetText(text, Color.white);
    }

    public void SetText(string text, Color color)
    {
        TextUGUI.text = text;
        TextUGUI.color = color;
    }

    private void SetAlpha(float a)
    {
        a = Mathf.Clamp(a, 0f, 255f);
        Color32 color = TextUGUI.color;
        color.a = (byte)a;
        TextUGUI.color = color;
    }
}
