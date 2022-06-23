using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CustomizerManager : MonoBehaviour
{
    public int charLimit = 12;
    private string charName;
    public int raceInt = 2;
    private int skinInt = 0;
    private int faceInt = 0;
    public int headAccessory = 0;
    private int chestAccessory = 0;
    private int handAccessory = 0;

    public CharacterProfile playerProfile;
    public MaterialLists faces;
    public MaterialLists bearSkins;
    public MaterialLists bunnySkins;
    public MaterialLists catSkins;
    public Renderer[] baseAnimalSkins;
    public Renderer[] baseAnimalFaces;
    public GameObject[] baseAnimals;
    public GameObject[] baseAnimalHeadAccessories;
    public GameObject[] baseAnimalChestAccessories;
    public GameObject[] baseAnimalHandAccessories;

    [SerializeField]
    private TMP_Text[] selectorTexts;
    [SerializeField]
    private TMP_InputField nameInputField;

    void Start()
    {
        nameInputField.characterLimit = charLimit;
    }

    private void OnDisable()
    {
        //Reset options here or load save
        ResetAll();
        LoadCustomization();
    }

    private void OnEnable()
    {
        ResetAll();
    }

    public void SaveCustomization()
    {
        playerProfile.SaveNameString(name);
        playerProfile.SaveRaceInt(raceInt);
        playerProfile.SaveSkinInt(skinInt);
        playerProfile.SaveFaceInt(faceInt);
        playerProfile.SaveHeadAccessoryInt(headAccessory);
        playerProfile.SaveChestAccessoryInt(chestAccessory);
        playerProfile.SaveHandAccessoryInt(handAccessory);
    }

    public void LoadCustomization()
    {
        SetName(playerProfile.LoadNameString());
        SetRaceInt(playerProfile.LoadRaceInt());
        UpdateSkinInt(playerProfile.LoadSkinInt());
        UpdateFaceInt(playerProfile.LoadFaceInt());
        UpdateHeadAccessoryInt(playerProfile.LoadHeadAccessoryInt());
        UpdateChestAccessoryInt(playerProfile.LoadChestAccessoryInt());
        UpdateHandAccessoryInt(playerProfile.LoadHandAccessoryInt());
    }

    public void ResetAll()
    {
        SetName("");
        SetRaceInt(2);
        UpdateSkinInt(-skinInt);
        UpdateFaceInt(-faceInt);
        UpdateHeadAccessoryInt(-headAccessory);
        UpdateChestAccessoryInt(-chestAccessory);
        UpdateHandAccessoryInt(-handAccessory);
    }

    public void SetName(string input)
    {
        charName = input;
        nameInputField.text = input;
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
                if(skinInt + updateAmount > bearSkins.materialList.Length - 1)
                {
                    skinInt = 0;
                }
                else if(skinInt + updateAmount < 0)
                {
                    skinInt = bearSkins.materialList.Length - 1;
                }
                else
                {
                    skinInt += updateAmount;
                }
                baseAnimalSkins[raceInt].sharedMaterial = bearSkins.materialList[skinInt];
                selectorTexts[0].text = "Bear " + (skinInt + 1);
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
                selectorTexts[0].text = "Bunny " + (skinInt + 1);
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
                selectorTexts[0].text = "Cat " + (skinInt + 1);
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
        selectorTexts[1].text = "Face " + (faceInt + 1);
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

        if(headAccessory > 0)
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
        if(headAccessory > 0)
        {
            for (int i = 1; i < accessories.Count + 1; i++)
            {
                if (i == headAccessory)
                {
                    accessories[i - 1].gameObject.SetActive(true);
                    break;
                }
            }
            selectorTexts[2].text = "Accessory " + (headAccessory);
        }
        else
        {
            selectorTexts[2].text = "No Accessory";
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
            selectorTexts[3].text = "Accessory " + (chestAccessory);
        }
        else
        {
            selectorTexts[3].text = "No Accessory";
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
            selectorTexts[4].text = "Accessory " + (handAccessory);
        }
        else
        {
            selectorTexts[4].text = "No Accessory";
        }
    }
}