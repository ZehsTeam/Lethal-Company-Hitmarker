﻿using BepInEx.Configuration;
using System;

namespace com.github.zehsteam.Hitmarker;

internal class ConfigManager
{
    // Hitmarker Settings
    private ConfigEntry<bool> ShowHitmarkerImageCfg;
    private ConfigEntry<int> HitmarkerImageSizeCfg;
    private ConfigEntry<bool> PlayHitmarkerSoundCfg;

    // Damage Settings
    private ConfigEntry<bool> ShowDamageMessageCfg;
    private ConfigEntry<bool> ShowKillMessageCfg;
    private ConfigEntry<bool> OnlyShowLocalKillMessageCfg;

    // Hitmarker Settings
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

    // Damage Settings
    internal bool ShowDamageMessage
    {
        get
        {
            return ShowDamageMessageCfg.Value;
        }
        set
        {
            ShowDamageMessageCfg.Value = value;
        }
    }

    internal bool ShowKillMessage
    {
        get
        {
            return ShowKillMessageCfg.Value;
        }
        set
        {
            ShowKillMessageCfg.Value = value;
        }
    }

    internal bool OnlyShowLocalKillMessage
    {
        get
        {
            return OnlyShowLocalKillMessageCfg.Value;
        }
        set
        {
            OnlyShowLocalKillMessageCfg.Value = value;
        }
    }

    public ConfigManager()
    {
        BindConfigs();
    }

    private void BindConfigs()
    {
        ConfigFile config = HitmarkerBase.Instance.Config;

        // Hitmarker Settings
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

        // Damage Settings
        ShowDamageMessageCfg = config.Bind(
            new ConfigDefinition("Damage Settings", "showDamageMessage"),
            true,
            new ConfigDescription("Shows a message of how much damage you did to an enemy.")
        );

        ShowKillMessageCfg = config.Bind(
            new ConfigDefinition("Damage Settings", "showKillMessage"),
            true,
            new ConfigDescription("Shows a message when an enemy is killed.")
        );

        OnlyShowLocalKillMessageCfg = config.Bind(
            new ConfigDefinition("Damage Settings", "onlyShowLocalKillMessage"),
            false,
            new ConfigDescription("Will only show your kill messages.")
        );
    }
}
