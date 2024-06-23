using UnityEngine;

namespace com.github.zehsteam.Hitmarker;

internal class Content
{
    public static GameObject HitmarkerCanvasPrefab;

    public static void Load()
    {
        LoadAssetsFromAssetBundle();
    }

    private static void LoadAssetsFromAssetBundle()
    {
        try
        {
            var dllFolderPath = System.IO.Path.GetDirectoryName(Plugin.Instance.Info.Location);
            var assetBundleFilePath = System.IO.Path.Combine(dllFolderPath, "hitmarker_assets");
            AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundleFilePath);

            HitmarkerCanvasPrefab = assetBundle.LoadAsset<GameObject>("HitmarkerCanvas");

            Plugin.logger.LogInfo("Successfully loaded assets from AssetBundle!");
        }
        catch (System.Exception e)
        {
            Plugin.logger.LogError($"Error: failed to load assets from AssetBundle.\n\n{e}");
        }
    }
}
