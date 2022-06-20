using UnityEngine;

[CreateAssetMenu(menuName = "MikelW/Create Player Profile")]
public class PlayerProfile : ScriptableObject
{
    [Header("Base Save Settings")]
    [SerializeField]
    private bool saveInPlayerPrefs = true;
    [SerializeField]
    private string prefPrefix = "Player_";

    [Header("Player Settings")]
    [SerializeField]
    private string name = "Mikel";
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

        name = savedName;
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
            return name;
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
}