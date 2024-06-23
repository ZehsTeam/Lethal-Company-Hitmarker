using HarmonyLib;

namespace com.github.zehsteam.Hitmarker.Patches;

[HarmonyPatch(typeof(HUDManager))]
internal class HUDManagerPatch
{
    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    static void StartPatch()
    {
        Plugin.Instance.CreateHitmarkerCanvas();
    }
}
