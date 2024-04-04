using System.Collections;
using TMPro;
using UnityEngine;

namespace com.github.zehsteam.Hitmarker.MonoBehaviours;

public class MessageItemBehaviour : MonoBehaviour
{
    public TextMeshProUGUI textUGUI;

    private void Start()
    {
        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(HitmarkerBase.Instance.configManager.MessageDuration);
        yield return StartCoroutine(FadeOut(1f));
        yield return null;

        Destroy(gameObject);
    }

    public void SetText(string text)
    {
        SetText(text, Color.white);
    }

    public void SetText(string text, Color color)
    {
        textUGUI.text = text;
        textUGUI.color = color;
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
        Color32 color = textUGUI.color;
        color.a = (byte)a;
        textUGUI.color = color;
    }
}
