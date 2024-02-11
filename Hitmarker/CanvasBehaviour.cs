using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace com.github.zehsteam.Hitmarker;

internal class CanvasBehaviour : MonoBehaviour
{
    public static CanvasBehaviour Instance;

    private Image image;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        image = GetComponentInChildren<Image>();
        image.gameObject.SetActive(false);

        SetHitmarkerImageSize(HitmarkerBase.Instance.configManager.HitmarkerImageSize);
    }

    public void SetHitmarkerImageSize(int size)
    {
        image.rectTransform.sizeDelta = new Vector2(size, size);
    }

    public void ShowHitmarker()
    {
        if (HitmarkerBase.Instance.configManager.ShowHitmarkerImage)
        {
            StopCoroutine("ShowImage");
            StartCoroutine(ShowImage(0.3f));
        }

        if (HitmarkerBase.Instance.configManager.PlayHitmarkerSound)
        {
            HUDManager.Instance.UIAudio.PlayOneShot(HitmarkerBase.Instance.hitSFX);
        }
    }

    private IEnumerator ShowImage(float time)
    {
        image.gameObject.SetActive(true);

        yield return new WaitForSeconds(time);

        image.gameObject.SetActive(false);
    }
}
