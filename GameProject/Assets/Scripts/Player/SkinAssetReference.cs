using UnityEngine.AddressableAssets;

[System.Serializable]
public class SkinAssetReference : AssetReferenceT<Skin>
{
    public SkinAssetReference(string guid): base(guid) {}
}
