using UnityEngine;

namespace com.github.zehsteam.Hitmarker.MonoBehaviours;

public class HitmarkerCanvasBehaviour : MonoBehaviour
{
    public static HitmarkerCanvasBehaviour Instance;

    public HitmarkerImageBehaviour hitmarkerImageBehaviour;
    public AudioClip hitSFX;
    public RectTransform messageListTransform;
    public GameObject messageItemPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void ShowHitmarker(bool killed = false)
    {
        ShowHitmarkerImage(killed);
        PlayHitSFX();
    }

    private void ShowHitmarkerImage(bool killed = false)
    {
        if (!HitmarkerBase.Instance.configManager.ShowHitmarkerImage) return;

        hitmarkerImageBehaviour.ShowImage(killed: killed);
    }

    private void PlayHitSFX()
    {
        if (!HitmarkerBase.Instance.configManager.PlayHitmarkerSound) return;

        HUDManager.Instance.UIAudio.PlayOneShot(hitSFX);
    }

    public void ShowDamageMessage(string enemyName, int damage = 1)
    {
        if (!HitmarkerBase.Instance.configManager.ShowDamageMessage) return;

        ShowMessage($"{enemyName} -{damage} HP");
    }

    public void ShowKillMessage(string enemyName, bool fromLocalPlayer = true, string fromPlayerName = "")
    {
        if (!HitmarkerBase.Instance.configManager.ShowKillMessage) return;
        if (!fromLocalPlayer && HitmarkerBase.Instance.configManager.OnlyShowLocalKillMessage) return;

        if (!fromLocalPlayer)
        {
            ShowMessage($"{fromPlayerName} Killed {enemyName}", Color.red);
            return;
        }

        ShowMessage($"Killed {enemyName}", Color.red);
    }

    private void ShowMessage(string text)
    {
        ShowMessage(text, Color.white);
    }

    private void ShowMessage(string text, Color color)
    {
        GameObject gameObject = Object.Instantiate(messageItemPrefab, messageListTransform);
        MessageItemBehaviour behaviour = gameObject.GetComponent<MessageItemBehaviour>();
        behaviour.SetText(text, color);
    }
}
