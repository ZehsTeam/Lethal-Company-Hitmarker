using BepInEx.Configuration;
using System.Collections.Generic;
using System.Reflection;

namespace com.github.zehsteam.Hitmarker;

internal class ConfigManager
{
    // General Settings
    public ConfigEntry<bool> ExtendedLogging { get; private set; }

    // Hitmarker Settings
    public ConfigEntry<bool> ShowHitmarkerImage { get; private set; }
    public ConfigEntry<int> HitmarkerImageSize { get; private set; }
    public ConfigEntry<bool> PlayHitmarkerSound { get; private set; }

    // Message Settings
    public ConfigEntry<float> MessageDuration { get; private set; }
    public ConfigEntry<int> MessageFontSize { get; private set; }
    public ConfigEntry<bool> ShowDamageMessage { get; private set; }
    public ConfigEntry<bool> ShowKillMessage { get; private set; }
    public ConfigEntry<bool> OnlyShowLocalKillMessage { get; private set; }

    public ConfigManager()
    {
        BindConfigs();
        ClearUnusedEntries();
    }

    private void BindConfigs()
    {
        ConfigFile configFile = Plugin.Instance.Config;

        // General Settings
        ExtendedLogging = configFile.Bind("General Settings", "ExtendedLogging", defaultValue: false, "Enable extended logging.");

        // Hitmarker Settings
        ShowHitmarkerImage = configFile.Bind("Hitmarker Settings", "ShowHitmarkerImage", defaultValue: true, "Do you want to show the hitmarker image?");
        HitmarkerImageSize = configFile.Bind("Hitmarker Settings", "HitmarkerImageSize", defaultValue: 40, new ConfigDescription("The size of the hitmarker image in pixels.", new AcceptableValueRange<int>(10, 100)));
        PlayHitmarkerSound = configFile.Bind("Hitmarker Settings", "PlayHitmarkerSound", defaultValue: true, "Do you want to play the hitmarker sound?");

        // Message Settings
        MessageDuration = configFile.Bind("Message Settings", "MessageDuration", defaultValue: 4f, "The message duration in seconds.");
        MessageFontSize = configFile.Bind("Message Settings", "MessageFontSize", defaultValue: 35, new ConfigDescription("The message font size in pixels.", new AcceptableValueRange<int>(10, 100)));
        ShowDamageMessage = configFile.Bind("Message Settings", "ShowDamageMessage", defaultValue: true, "Shows a message of how much damage you did to an enemy.");
        ShowKillMessage = configFile.Bind("Message Settings", "ShowKillMessage", defaultValue: true, "Shows a message when an enemy is killed.");
        OnlyShowLocalKillMessage = configFile.Bind("Message Settings", "OnlyShowLocalKillMessage", defaultValue: true, "Will only show your kill messages.");
    }

    private void ClearUnusedEntries()
    {
        ConfigFile configFile = Plugin.Instance.Config;

        // Normally, old unused config entries don't get removed, so we do it with this piece of code. Credit to Kittenji.
        PropertyInfo orphanedEntriesProp = configFile.GetType().GetProperty("OrphanedEntries", BindingFlags.NonPublic | BindingFlags.Instance);
        var orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProp.GetValue(configFile, null);
        orphanedEntries.Clear(); // Clear orphaned entries (Unbinded/Abandoned entries)
        configFile.Save(); // Save the config file to save these changes
    }
}
