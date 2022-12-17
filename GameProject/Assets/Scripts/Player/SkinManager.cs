using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SkinManager : MonoBehaviour
{
    // All skin packs that the game knows about
    [SerializeField] private SkinPack[] skinPacks;
    [SerializeField] private bool loadAllOnStart;
    public static SkinManager Instance { get; private set; }
    private SkinAssetReference chosenSkin;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        string chosen = PlayerPrefs.GetString("PlayerSkin");
        if (chosen.Equals(""))
        {
            PlayerPrefs.SetString("PlayerSkin", "0/0");
            chosenSkin = skinPacks[0].Skins[0];
        }
        else
        {
            chosenSkin = SkinFromPrefs();
        }
    }

    private IEnumerator Start()
    {
        if (loadAllOnStart) yield return LoadAvailablePacks();
    }

    private SkinAssetReference SkinFromPrefs()
    {
        string[] splits = PlayerPrefs.GetString("PlayerSkin").Split("/");
        return skinPacks[Int16.Parse(splits[0])].Skins[Int16.Parse(splits[1])];
    }

    public IEnumerator LoadAvailablePacks()
    {
        foreach (SkinPack pack in skinPacks)
        {
            foreach (SkinAssetReference skin in pack.Skins)
            {
                AsyncOperationHandle<Skin> handle = skin.LoadAssetAsync<Skin>();
                yield return handle;
            }
        }
    }

    public IEnumerator LoadOnlyChosenSkin()
    {
        AsyncOperationHandle<Skin> handle = chosenSkin.LoadAssetAsync<Skin>();
        yield return handle;
    }

    public SkinPack[] SkinPacks()
    {
        SkinPack[] skinPacks = new SkinPack[this.skinPacks.Length];
        this.skinPacks.CopyTo(skinPacks, 0);
        return skinPacks;
    }

    public void SetChosenSkinReference(SkinAssetReference skin)
    {
        for (int i = 0; i < skinPacks.Length; i++)
        {
            List<SkinAssetReference> skins = skinPacks[i].Skins;
            for (int j = 0; j < skins.Count; j++)
            {
                if (skins[j].Equals(skin))
                {
                    PlayerPrefs.SetString("PlayerSkin", i + "/" + j);
                    chosenSkin = skin;
                    return;
                }
            }
        }
        Debug.Log("Trying to set chosen skin to a non-existent skin");
    }

    public SkinAssetReference GetChosenSkinReference()
    {
        return chosenSkin;
    }

    public void OnDestroy()
    {
        foreach (SkinPack pack in skinPacks)
        {
            foreach (SkinAssetReference skin in pack.Skins)
            {
                if (skin.Asset != null) skin.ReleaseAsset();
            }
        }
    }
}
