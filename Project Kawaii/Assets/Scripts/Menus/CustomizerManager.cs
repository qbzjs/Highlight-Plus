using UnityEngine;
using TMPro;

public class CustomizerManager : MonoBehaviour
{
    public string name;
    public int charLimit = 12;
    public int raceInt = 2;
    public int skinInt = 0;
    public int faceInt = 0;
    public int headAccessory = 0;
    public int chestAccessory = 0;
    public int handAccessory = 0;

    public PlayerProfile playerProfile;
    public MaterialLists faces;
    public MaterialLists bearSkins;
    public MaterialLists bunnySkins;
    public MaterialLists catSkins;
    public Renderer[] baseAnimalSkins;
    public Renderer[] baseAnimalFaces;
    public GameObject[] baseAnimals;

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
        SetName(playerProfile.LoadNameString());
        SetRaceInt(playerProfile.LoadRaceInt());
        UpdateSkinInt(playerProfile.LoadSkinInt());
        UpdateFaceInt(playerProfile.LoadFaceInt());
    }

    private void OnEnable()
    {
        ResetAll();
    }

    public void ResetAll()
    {
        SetName("");
        SetRaceInt(2);
        UpdateSkinInt(-skinInt);
        UpdateFaceInt(-faceInt);
        headAccessory = 0;
        chestAccessory = 0;
        handAccessory = 0;
    }

    public void SetName(string input)
    {
        name = input;
        nameInputField.text = input;
    }

    public void SetRaceInt(int index)
    {
        raceInt = index;
        //Reset 

        UpdateSkinInt(-skinInt);
        UpdateFaceInt(-faceInt);
        headAccessory = 0;
        chestAccessory = 0;
        handAccessory = 0;

        for(int i = 0; i < baseAnimals.Length; i++)
        {
            if (i == index)
                baseAnimals[i].SetActive(true);
            else
                baseAnimals[i].SetActive(false);
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
        headAccessory += updateAmount;
    }

    public void UpdateChestAccessoryInt(int updateAmount)
    {
        chestAccessory += updateAmount;
    }

    public void UpdateHandAccessoryInt(int updateAmount)
    {
        handAccessory += updateAmount;
    }
}