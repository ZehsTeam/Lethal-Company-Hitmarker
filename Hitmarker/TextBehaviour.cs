using System.Collections;
using TMPro;
using UnityEngine;

namespace com.github.zehsteam.Hitmarker;

internal class TextBehaviour : MonoBehaviour
{
    private Animation fadeOutAnimation;

    private void Start()
    {
        fadeOutAnimation = gameObject.GetComponent<Animation>();

        float duration = HitmarkerBase.Instance.configManager.MessageDuration;

        StartCoroutine(FadeOutAndDestroy(duration));
    }

    private IEnumerator FadeOutAndDestroy(float time)
    {
        yield return new WaitForSeconds(time);

        fadeOutAnimation.Play();

        float length = fadeOutAnimation.clip.length + 0.1f;
        yield return new WaitForSeconds(length);

        Destroy(gameObject);
    }

    public void SetText(string message, Color color)
    {
        TextMeshProUGUI text = gameObject.GetComponent<TextMeshProUGUI>();

        text.text = message;
        text.color = color;
    }
}
