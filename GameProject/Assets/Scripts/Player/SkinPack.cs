using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skin Pack", menuName = "Skin Pack")]
public class SkinPack : ScriptableObject
{
    [Header("Basic separator is '+'")]
    public char separator = '+';
    [SerializeField] private string packname; // localized names for the player
    [field: SerializeField] public List<SkinAssetReference> Skins { get; private set; }

    private void OnDestroy()
    {
        foreach (SkinAssetReference reference in Skins) reference.ReleaseAsset();
    }

    public string LocalizedName()
    {
        return packname.Split(separator)[PlayerPrefs.GetInt("Language")];
    }

    public bool IsReady()
    {
        for (int i = 0; i < Skins.Count; i++) if (Skins[i].Asset == null) return false;
        return true;
    }
}
