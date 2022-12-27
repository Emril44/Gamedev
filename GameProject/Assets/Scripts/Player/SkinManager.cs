using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement;

public class SkinManager : MonoBehaviour
{
    // All skin packs that the game knows about
    [SerializeField] private SkinPack[] skinPacks;
    public static SkinManager Instance { get; private set; }
    private SkinAssetReference chosenSkin;
    public bool Loaded { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        ResourceManager.ExceptionHandler = (AsyncOperationHandle handle, Exception exception) =>
        {
            if (exception.GetType() != typeof(InvalidKeyException))
                Addressables.LogException(handle, exception);
        };
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
        yield return LoadAvailablePacks();
    }

    private SkinAssetReference SkinFromPrefs()
    {
        string[] splits = PlayerPrefs.GetString("PlayerSkin").Split("/");
        return skinPacks[Int16.Parse(splits[0])].Skins[Int16.Parse(splits[1])];
    }

    private IEnumerator LoadAvailablePacks()
    {
        foreach (SkinPack pack in skinPacks)
        {
            foreach (SkinAssetReference skin in pack.Skins)
            {
                AsyncOperationHandle<Skin> handle = skin.LoadAssetAsync<Skin>();   
                yield return handle;
            }
        }
        Loaded = true;
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

    public bool IsLoaded(SkinPack skinpack)
    {
        return skinpack.Skins[0].Asset != null;
    }

    public SkinAssetReference GetChosenSkinReference()
    {
        if (chosenSkin.Asset == null)
        {
            Debug.LogWarning("Pack with skin " + PlayerPrefs.GetString("PlayerSkin") + " is unloaded, resetting chosen skin to default.");
            SetChosenSkinReference(SkinPacks()[0].Skins[0]);
        }
        return chosenSkin;
    }
}
