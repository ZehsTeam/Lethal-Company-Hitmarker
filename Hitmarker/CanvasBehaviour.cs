using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace com.github.zehsteam.Hitmarker;

internal class CanvasBehaviour : MonoBehaviour
{
    public static CanvasBehaviour Instance;

    private Image hitmarkerImage;
    private Transform messageList;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        hitmarkerImage = transform.GetChild(0).gameObject.GetComponent<Image>();
        hitmarkerImage.gameObject.SetActive(false);

        messageList = transform.GetChild(1);

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
            StopCoroutine("ShowHitmarkerImage");
            StartCoroutine(ShowHitmarkerImage(0.3f, killed));
        }

        if (HitmarkerBase.Instance.configManager.PlayHitmarkerSound)
        {
            HUDManager.Instance.UIAudio.PlayOneShot(HitmarkerBase.Instance.hitSFX);
        }
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

    public void ShowDamageMessage(string message)
    {
        if (!HitmarkerBase.Instance.configManager.ShowDamageMessage) return;

        ShowMessage(message);
    }

    public void ShowKillMessage(string message, bool fromLocalPlayer)
    {
        if (!HitmarkerBase.Instance.configManager.ShowKillMessage) return;

        bool onlyShowLocalKillText = HitmarkerBase.Instance.configManager.OnlyShowLocalKillMessage;
        if (onlyShowLocalKillText && !fromLocalPlayer) return;

        ShowMessage(message, Color.red);
    }

    public void ShowMessage(string message)
    {
        ShowMessage(message, Color.white);
    }

    public void ShowMessage(string message, Color color)
    {
        GameObject messageText = Instantiate(HitmarkerBase.Instance.messageTextPrefab, Vector3.zero, Quaternion.identity, messageList);
        TextBehaviour textBehaviour = messageText.GetComponent<TextBehaviour>();

        textBehaviour.SetText(message, color);
    }
}
