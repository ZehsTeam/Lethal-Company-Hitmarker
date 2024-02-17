using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace com.github.zehsteam.Hitmarker;

internal class CanvasBehaviour : MonoBehaviour
{
    public static CanvasBehaviour Instance;

    private Image hitmarkerImage;
    private Transform infoList;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        hitmarkerImage = transform.GetChild(0).gameObject.GetComponent<Image>();
        hitmarkerImage.gameObject.SetActive(false);

        infoList = transform.GetChild(1);

        SetHitmarkerImageSize(HitmarkerBase.Instance.configManager.HitmarkerImageSize);
    }

    public void SetHitmarkerImageSize(int size)
    {
        hitmarkerImage.rectTransform.sizeDelta = new Vector2(size, size);
    }

    public void ShowHitmarker(bool killed)
    {
        if (HitmarkerBase.Instance.configManager.ShowHitmarkerImage)
        {
            StopCoroutine("ShowImage");
            StartCoroutine(ShowHitmarkerImage(0.3f, killed));
        }

        if (HitmarkerBase.Instance.configManager.PlayHitmarkerSound)
        {
            HUDManager.Instance.UIAudio.PlayOneShot(HitmarkerBase.Instance.hitSFX);
        }
    }

    public void ShowDamageText(string message)
    {
        if (!HitmarkerBase.Instance.configManager.ShowDamageMessage) return;

        ShowInfoText(message);
    }

    public void ShowKillText(string message, bool fromLocalPlayer)
    {
        if (!HitmarkerBase.Instance.configManager.ShowKillMessage) return;

        bool onlyShowLocalKillText = HitmarkerBase.Instance.configManager.OnlyShowLocalKillMessage;
        if (onlyShowLocalKillText && !fromLocalPlayer) return;

        ShowInfoText(message, Color.red);
    }

    public void ShowInfoText(string message)
    {
        ShowInfoText(message, Color.white);
    }

    public void ShowInfoText(string message, Color color)
    {
        GameObject infoText = Instantiate(HitmarkerBase.Instance.infoTextPrefab, Vector3.zero, Quaternion.identity, infoList);
        TextBehaviour textBehaviour = infoText.GetComponent<TextBehaviour>();

        textBehaviour.SetText(message, color);
    }

    private IEnumerator ShowHitmarkerImage(float time, bool killed)
    {
        if (killed)
        {
            hitmarkerImage.color = Color.red;
        }

        hitmarkerImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(time);

        hitmarkerImage.gameObject.SetActive(false);
        hitmarkerImage.color = Color.white;
    }
}
