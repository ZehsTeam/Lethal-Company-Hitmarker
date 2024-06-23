using System.Collections.Generic;
using UnityEngine;

namespace com.github.zehsteam.Hitmarker.MonoBehaviours;

public class HitmarkerCanvasBehaviour : MonoBehaviour
{
    public static HitmarkerCanvasBehaviour Instance;

    public HitmarkerImageBehaviour HitmarkerImageBehaviour;
    public AudioClip HitSFX;
    public RectTransform MessageListTransform;
    public RectTransform MessageItemPrefab;

    private Queue<RectTransform> _messageItemPool = [];

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        InitializeMessageItemPool();
    }

    private void InitializeMessageItemPool()
    {
        _messageItemPool = [];

        for (int i = 0; i < 20; i++)
        {
            RectTransform rectTransform = Instantiate(MessageItemPrefab, MessageListTransform);
            rectTransform.gameObject.SetActive(false);
            _messageItemPool.Enqueue(rectTransform);
        }
    }

    public void ShowHitmarker(bool killed = false)
    {
        ShowHitmarkerImage(killed);
        PlayHitSFX();
    }

    private void ShowHitmarkerImage(bool killed = false)
    {
        if (!Plugin.ConfigManager.ShowHitmarkerImage.Value) return;

        HitmarkerImageBehaviour.ShowImage(killed);
    }

    private void PlayHitSFX()
    {
        if (!Plugin.ConfigManager.PlayHitmarkerSound.Value) return;

        HUDManager.Instance.UIAudio.PlayOneShot(HitSFX);
    }

    public void ShowDamageMessage(string enemyName, int damage = 1)
    {
        if (!Plugin.ConfigManager.ShowDamageMessage.Value) return;

        ShowMessage($"{enemyName} -{damage} HP");
    }

    public void ShowKillMessage(string enemyName, bool fromLocalPlayer = true, string fromPlayerName = "")
    {
        if (!Plugin.ConfigManager.ShowKillMessage.Value) return;
        if (!fromLocalPlayer && Plugin.ConfigManager.OnlyShowLocalKillMessage.Value) return;

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
        SpawnMessageItemFromPool(text, color);
    }

    private RectTransform SpawnMessageItemFromPool(string text, Color color)
    {
        if (_messageItemPool == null || _messageItemPool.Count == 0)
        {
            Plugin.logger.LogError("Error: Failed to spawn message item from pool. Message item pool is either null or empty.");
            return null;
        }

        RectTransform rectTransform = _messageItemPool.Dequeue();
        rectTransform.gameObject.SetActive(true);
        rectTransform.SetAsLastSibling();

        if (rectTransform.TryGetComponent(out MessageItemBehaviour behaviour))
        {
            behaviour.SetText(text, color);
        }

        _messageItemPool.Enqueue(rectTransform);

        return rectTransform;
    }
}
