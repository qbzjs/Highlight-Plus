using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private CharacterProfile playerProfile;

    private string charName;
    private static int raceInt = 2;
    private static int skinInt = 0;
    private static int faceInt = 0;
    private static int headAccessory = 0;
    private static int chestAccessory = 0;
    private static int handAccessory = 0;

    [SerializeField]
    private MaterialLists faces;
    [SerializeField]
    private MaterialLists bearSkins;
    [SerializeField]
    private MaterialLists bunnySkins;
    [SerializeField]
    private MaterialLists catSkins;
    [SerializeField]
    private Renderer[] baseAnimalSkins;
    [SerializeField]
    private Renderer[] baseAnimalFaces;
    [SerializeField]
    private GameObject[] baseAnimals;
    [SerializeField]
    private GameObject[] baseAnimalHeadAccessories;
    [SerializeField]
    private GameObject[] baseAnimalChestAccessories;
    [SerializeField]
    private GameObject[] baseAnimalHandAccessories;
    #endregion Variables

    #region Unity Methods
    private void Awake()
    {
        LoadPlayerCharacter();   
    }
    #endregion Unity Methods

    public static int GetRaceInt()
    {
        return raceInt;
    }

    public void SaveCustomization()
    {
        playerProfile.SaveNameString(charName);
        playerProfile.SaveRaceInt(raceInt);
        playerProfile.SaveSkinInt(skinInt);
        playerProfile.SaveFaceInt(faceInt);
        playerProfile.SaveHeadAccessoryInt(headAccessory);
        playerProfile.SaveChestAccessoryInt(chestAccessory);
        playerProfile.SaveHandAccessoryInt(handAccessory);
    }

    private void LoadPlayerCharacter()
    {
        //SetName(playerProfile.LoadNameString());
        SetRaceInt(playerProfile.LoadRaceInt());
        UpdateSkinInt(playerProfile.LoadSkinInt());
        UpdateFaceInt(playerProfile.LoadFaceInt());
        UpdateHeadAccessoryInt(playerProfile.LoadHeadAccessoryInt());
        UpdateChestAccessoryInt(playerProfile.LoadChestAccessoryInt());
        UpdateHandAccessoryInt(playerProfile.LoadHandAccessoryInt());
    }

    public void SetRaceInt(int index)
    {
        //Reset 
        UpdateSkinInt(-skinInt);
        UpdateFaceInt(-faceInt);
        UpdateHeadAccessoryInt(-headAccessory);
        UpdateChestAccessoryInt(-chestAccessory);
        UpdateHandAccessoryInt(-handAccessory);

        raceInt = index;

        for (int i = 0; i < baseAnimals.Length; i++)
        {
            if (i == index)
                baseAnimals[i].SetActive(true);
            else
            {
                baseAnimals[i].transform.rotation = Quaternion.Euler(0, 220, 0);
                baseAnimals[i].SetActive(false);
            }
        }
    }

    public void UpdateSkinInt(int updateAmount)
    {
        switch (raceInt)
        {
            case 0:
                if (skinInt + updateAmount > bearSkins.materialList.Length - 1)
                {
                    skinInt = 0;
                }
                else if (skinInt + updateAmount < 0)
                {
                    skinInt = bearSkins.materialList.Length - 1;
                }
                else
                {
                    skinInt += updateAmount;
                }
                baseAnimalSkins[raceInt].sharedMaterial = bearSkins.materialList[skinInt];
                break;
            case 1:
                if (skinInt + updateAmount > bunnySkins.materialList.Length - 1)
                {
                    skinInt = 0;
                }
                else if (skinInt + updateAmount < 0)
                {
                    skinInt = bunnySkins.materialList.Length - 1;
                }
                else
                {
                    skinInt += updateAmount;
                }
                baseAnimalSkins[raceInt].sharedMaterial = bunnySkins.materialList[skinInt];
                break;
            case 2:
                if (skinInt + updateAmount > catSkins.materialList.Length - 1)
                {
                    skinInt = 0;
                }
                else if (skinInt + updateAmount < 0)
                {
                    skinInt = catSkins.materialList.Length - 1;
                }
                else
                {
                    skinInt += updateAmount;
                }
                baseAnimalSkins[raceInt].sharedMaterial = catSkins.materialList[skinInt];
                break;
            default:
                Debug.LogError("Race int is out of bounds");
                break;
        }
    }

    public void UpdateFaceInt(int updateAmount)
    {
        if (faceInt + updateAmount > faces.materialList.Length - 1)
        {
            faceInt = 0;
        }
        else if (faceInt + updateAmount < 0)
        {
            faceInt = faces.materialList.Length - 1;
        }
        else
        {
            faceInt += updateAmount;
        }
        baseAnimalFaces[raceInt].sharedMaterial = faces.materialList[faceInt];
    }

    public void UpdateHeadAccessoryInt(int updateAmount)
    {
        Transform[] components = baseAnimalHeadAccessories[raceInt].GetComponentsInChildren<Transform>(true);
        List<Transform> accessories = new List<Transform>();

        for (int i = 0; i < components.Length; i++)
        {
            if (components[i].name.Contains("locator"))
                continue;
            else
                accessories.Add(components[i]);
        }

        if (headAccessory > 0)
            accessories[headAccessory - 1].gameObject.SetActive(false);

        if (headAccessory + updateAmount > accessories.Count)
        {
            headAccessory = 0;
        }
        else if (headAccessory + updateAmount < 0)
        {
            headAccessory = accessories.Count;
        }
        else
        {
            headAccessory += updateAmount;
        }
        if (headAccessory > 0)
        {
            for (int i = 1; i < accessories.Count + 1; i++)
            {
                if (i == headAccessory)
                {
                    accessories[i - 1].gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    public void UpdateChestAccessoryInt(int updateAmount)
    {
        Transform[] components = baseAnimalChestAccessories[raceInt].GetComponentsInChildren<Transform>(true);
        List<Transform> accessories = new List<Transform>();

        for (int i = 0; i < components.Length; i++)
        {
            if (components[i].name.Contains("locator"))
                continue;
            else
                accessories.Add(components[i]);
        }

        if (chestAccessory > 0)
            accessories[chestAccessory - 1].gameObject.SetActive(false);

        if (chestAccessory + updateAmount > accessories.Count)
        {
            chestAccessory = 0;
        }
        else if (chestAccessory + updateAmount < 0)
        {
            chestAccessory = accessories.Count;
        }
        else
        {
            chestAccessory += updateAmount;
        }
        if (chestAccessory > 0)
        {
            for (int i = 1; i < accessories.Count + 1; i++)
            {
                if (i == chestAccessory)
                {
                    accessories[i - 1].gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    public void UpdateHandAccessoryInt(int updateAmount)
    {
        Transform[] components = baseAnimalHandAccessories[raceInt].GetComponentsInChildren<Transform>(true);
        List<Transform> accessories = new List<Transform>();

        for (int i = 0; i < components.Length; i++)
        {
            if (components[i].name.Contains("locator"))
                continue;
            else
                accessories.Add(components[i]);
        }

        if (handAccessory > 0)
            accessories[handAccessory - 1].gameObject.SetActive(false);

        if (handAccessory + updateAmount > accessories.Count)
        {
            handAccessory = 0;
        }
        else if (handAccessory + updateAmount < 0)
        {
            handAccessory = accessories.Count;
        }
        else
        {
            handAccessory += updateAmount;
        }
        if (handAccessory > 0)
        {
            for (int i = 1; i < accessories.Count + 1; i++)
            {
                if (i == handAccessory)
                {
                    accessories[i - 1].gameObject.SetActive(true);
                    break;
                }
            }
        }
    }
}