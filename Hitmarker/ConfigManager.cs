using BepInEx.Configuration;
using System;

namespace com.github.zehsteam.Hitmarker;

internal class ConfigManager
{
    // Hitmarker
    private ConfigEntry<bool> ShowHitmarkerImageCfg;
    private ConfigEntry<int> HitmarkerImageSizeCfg;
    private ConfigEntry<bool> PlayHitmarkerSoundCfg;

    // Hitmarker
    internal bool ShowHitmarkerImage
    {
        get
        {
            return ShowHitmarkerImageCfg.Value;
        }
        set
        {
            ShowHitmarkerImageCfg.Value = value;
        }
    }

    internal int HitmarkerImageSize
    {
        get
        {
            return HitmarkerImageSizeCfg.Value;
        }
        set
        {
            int newValue = Math.Clamp(value, 10, 500);
            HitmarkerImageSizeCfg.Value = newValue;
            CanvasBehaviour.Instance.SetHitmarkerImageSize(newValue);
        }
    }

    internal bool PlayHitmarkerSound
    {
        get
        {
            return PlayHitmarkerSoundCfg.Value;
        }
        set
        {
            PlayHitmarkerSoundCfg.Value = value;
        }
    }

    public ConfigManager()
    {
        BindConfigs();
    }

    private void BindConfigs()
    {
        ConfigFile config = HitmarkerBase.Instance.Config;

        // Hitmarker
        ShowHitmarkerImageCfg = config.Bind(
            new ConfigDefinition("Hitmarker Settings", "showHitmarkerImage"),
            true,
            new ConfigDescription("Do you want to show the hitmarker image?")
        );

        HitmarkerImageSizeCfg = config.Bind(
            new ConfigDefinition("Hitmarker Settings", "hitmarkerImageSize"),
            40,
            new ConfigDescription("The size of the hitmarker image in pixels.")
        );

        PlayHitmarkerSoundCfg = config.Bind(
            new ConfigDefinition("Hitmarker Settings", "playHitmarkerSound"),
            true,
            new ConfigDescription("Do you want to play the hitmarker sound?")
        );
    }
}
