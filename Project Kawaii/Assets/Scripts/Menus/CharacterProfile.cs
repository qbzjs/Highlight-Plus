using UnityEngine;

/// <summary>
/// Profile to contain a characters customization settings, can be used for NPCs
/// </summary>
[CreateAssetMenu(menuName = "MikelW/Create Character Profile")]
public class CharacterProfile : ScriptableObject
{
    [Header("Base Save Settings")]
    [SerializeField]
    private bool saveInPlayerPrefs = true;
    [SerializeField]
    private string prefPrefix = "Character_";

    [Header("Character Settings")]
    [SerializeField]
    private string charName = "Mikel";
    [SerializeField]
    private int raceInt = 2;
    [SerializeField]
    private int skinInt = 0;
    [SerializeField]
    private int faceInt = 0;
    [SerializeField]
    private int headAccessory = 0;
    [SerializeField]
    private int chestAccessory = 0;
    [SerializeField]
    private int handAccessory = 0;

    public void SaveNameString(string savedName)
    {
        if (saveInPlayerPrefs)
            PlayerPrefs.SetString(prefPrefix + "Name", savedName);

        charName = savedName;
    }

    public string LoadNameString()
    {
        if (saveInPlayerPrefs && PlayerPrefs.HasKey(prefPrefix + "Name"))
        {
            string loadedName = PlayerPrefs.GetString(prefPrefix + "Name");
            return loadedName;
        }
        else
        {
            return charName;
        }
    }

    public void SaveRaceInt(int savedRaceInt)
    {
        if (saveInPlayerPrefs)
            PlayerPrefs.SetInt(prefPrefix + "Race", savedRaceInt);

        raceInt = savedRaceInt;
    }


    public int LoadRaceInt()
    {
        if (saveInPlayerPrefs && PlayerPrefs.HasKey(prefPrefix + "Race"))
        {
            int loadedIndex = PlayerPrefs.GetInt(prefPrefix + "Race");
            return loadedIndex;
        }
        else
        {
            return raceInt;
        }
    }

    public void SaveSkinInt(int savedSkinInt)
    {
        if (saveInPlayerPrefs)
            PlayerPrefs.SetInt(prefPrefix + "Skin", savedSkinInt);

        skinInt = savedSkinInt;
    }


    public int LoadSkinInt()
    {
        if (saveInPlayerPrefs && PlayerPrefs.HasKey(prefPrefix + "Skin"))
        {
            int loadedIndex = PlayerPrefs.GetInt(prefPrefix + "Skin");
            return loadedIndex;
        }
        else
        {
            return skinInt;
        }
    }

    public void SaveFaceInt(int savedFaceInt)
    {
        if (saveInPlayerPrefs)
            PlayerPrefs.SetInt(prefPrefix + "Face", savedFaceInt);

        faceInt = savedFaceInt;
    }


    public int LoadFaceInt()
    {
        if (saveInPlayerPrefs && PlayerPrefs.HasKey(prefPrefix + "Face"))
        {
            int loadedIndex = PlayerPrefs.GetInt(prefPrefix + "Face");
            return loadedIndex;
        }
        else
        {
            return faceInt;
        }
    }

    public void SaveHeadAccessoryInt(int savedHeadAccessoryInt)
    {
        if (saveInPlayerPrefs)
            PlayerPrefs.SetInt(prefPrefix + "Head_Accessory", savedHeadAccessoryInt);

        headAccessory = savedHeadAccessoryInt;
    }


    public int LoadHeadAccessoryInt()
    {
        if (saveInPlayerPrefs && PlayerPrefs.HasKey(prefPrefix + "Head_Accessory"))
        {
            int loadedIndex = PlayerPrefs.GetInt(prefPrefix + "Head_Accessory");
            return loadedIndex;
        }
        else
        {
            return headAccessory;
        }
    }

    public void SaveChestAccessoryInt(int savedChestAccessoryInt)
    {
        if (saveInPlayerPrefs)
            PlayerPrefs.SetInt(prefPrefix + "Chest_Accessory", savedChestAccessoryInt);

        chestAccessory = savedChestAccessoryInt;
    }


    public int LoadChestAccessoryInt()
    {
        if (saveInPlayerPrefs && PlayerPrefs.HasKey(prefPrefix + "Chest_Accessory"))
        {
            int loadedIndex = PlayerPrefs.GetInt(prefPrefix + "Chest_Accessory");
            return loadedIndex;
        }
        else
        {
            return chestAccessory;
        }
    }

    public void SaveHandAccessoryInt(int savedHandAccessoryInt)
    {
        if (saveInPlayerPrefs)
            PlayerPrefs.SetInt(prefPrefix + "Hand_Accessory", savedHandAccessoryInt);

        handAccessory = savedHandAccessoryInt;
    }


    public int LoadHandAccessoryInt()
    {
        if (saveInPlayerPrefs && PlayerPrefs.HasKey(prefPrefix + "Hand_Accessory"))
        {
            int loadedIndex = PlayerPrefs.GetInt(prefPrefix + "Hand_Accessory");
            return loadedIndex;
        }
        else
        {
            return handAccessory;
        }
    }
}