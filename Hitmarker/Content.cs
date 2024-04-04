using UnityEngine;

namespace com.github.zehsteam.Hitmarker;

internal class Content
{
    public static GameObject hitmarkerCanvasPrefab;

    public static void Load()
    {
        LoadAssetsFromAssetBundle();
    }

    private static void LoadAssetsFromAssetBundle()
    {
        try
        {
            var dllFolderPath = System.IO.Path.GetDirectoryName(HitmarkerBase.Instance.Info.Location);
            var assetBundleFilePath = System.IO.Path.Combine(dllFolderPath, "hitmarker_assets");
            AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundleFilePath);

            // NetworkHandler
            hitmarkerCanvasPrefab = assetBundle.LoadAsset<GameObject>("HitmarkerCanvas");

            HitmarkerBase.mls.LogInfo("Successfully loaded assets from AssetBundle!");
        }
        catch (System.Exception e)
        {
            HitmarkerBase.mls.LogError($"Error: failed to load assets from AssetBundle.\n\n{e}");
        }
    }
}
