using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.github.zehsteam.Hitmarker;

internal class CanvasBehaviour : MonoBehaviour
{
    public static CanvasBehaviour Instance;

    private Image image;
    private Transform damageList;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        image = gameObject.GetComponentInChildren<Image>();
        image.gameObject.SetActive(false);

        damageList = transform.GetChild(1);

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

    public void ShowDamageText(string name, int damage)
    {
        if (!HitmarkerBase.Instance.configManager.ShowDamageText) return;

        GameObject damageText = Instantiate(HitmarkerBase.Instance.damageTextPrefab, Vector3.zero, Quaternion.identity, damageList);
        DamageTextBehaviour damageTextBehaviour = damageText.GetComponent<DamageTextBehaviour>();
        damageTextBehaviour.SetText($"{name} -{damage} HP");
    }

    private IEnumerator ShowImage(float time)
    {
        image.gameObject.SetActive(true);

        yield return new WaitForSeconds(time);

        image.gameObject.SetActive(false);
    }
}
