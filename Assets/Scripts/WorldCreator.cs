using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorldCreator : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private Image[] palette;
    [SerializeField] private TMP_Dropdown[] interests;
    [SerializeField] private TMP_Dropdown[] disinterests;
    [SerializeField] private TMP_Dropdown supportTrait;
    [SerializeField] private TMP_Dropdown demeanor;
    [SerializeField] private TMP_Dropdown hpBoon;
    [SerializeField] private TMP_Dropdown hpBane;
    [SerializeField] private TMP_Dropdown attributeBoon;
    [SerializeField] private TMP_Dropdown attributeBane;
    private TMP_Dropdown[] allInterests;

    private int pFace, pNose, pLips, pEar, pEye, pIris, pBrow, pHair, pBeard, pStache,
        pHairColor, pEyeColor;
    private float skinR, skinG, skinB;
    // Start is called before the first frame update
    void Start()
    {
        GeneralGameplayManager.indexesInitialization();

        TMP_Dropdown nationChoices = StaticData.findDeepChild(menu.transform, "NationTitle")
            .GetComponent<TMP_Dropdown>();
        nationChoices.options.Clear();
        for (int q = 0; q < Nation.NATION_TYPES.Length; q++)
        {
            TMP_Dropdown.OptionData opt = new TMP_Dropdown.OptionData();
            opt.text = Nation.NATION_TYPES[q];
            nationChoices.options.Add(opt);
        }

        allInterests = new TMP_Dropdown[interests.Length + disinterests.Length];
        for (int q = 0; q < interests.Length; q++)
        {
            allInterests[q] = interests[q];
        }
        for (int q = interests.Length; q < allInterests.Length; q++)
        {
            allInterests[q] = disinterests[q - interests.Length];
        }
        foreach (TMP_Dropdown drop in allInterests)
        {
            drop.options.Clear();
            TMP_Dropdown.OptionData random = new TMP_Dropdown.OptionData();
            random.text = "--Random--";
            drop.options.Add(random);
            Human.Interest[] possible = Human.Interest.values();
            foreach (Human.Interest intr in possible)
            {
                TMP_Dropdown.OptionData opt = new TMP_Dropdown.OptionData();
                opt.text = intr.getDisplayName();
                drop.options.Add(opt);
            }
            drop.RefreshShownValue();
        }
        TMP_Dropdown[] allOtherDrops = new TMP_Dropdown[] { supportTrait, demeanor, hpBoon,
            hpBane, attributeBoon, attributeBane };
        foreach (TMP_Dropdown drop in allOtherDrops)
        {
            drop.options.Clear();
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
            data.text = "--Random--";
            drop.options.Add(data);
            drop.RefreshShownValue();
        }
        Human.CombatTrait[] traits = Human.CombatTrait.values();
        foreach (Human.CombatTrait trait in traits)
        {
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
            data.text = trait.getDisplayName();
            supportTrait.options.Add(data);
        }
        Demeanor[] demeanors = Demeanor.values();
        foreach (Demeanor dem in demeanors)
        {
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
            data.text = dem.getDisplayName();
            demeanor.options.Add(data);
        }
        string[] hps = new string[] { "Head", "Torso", "Arms", "Legs" };
        foreach (string hp in hps)
        {
            TMP_Dropdown.OptionData data1 = new TMP_Dropdown.OptionData();
            data1.text = hp;
            hpBoon.options.Add(data1);
            TMP_Dropdown.OptionData data2 = new TMP_Dropdown.OptionData();
            data2.text = hp;
            hpBane.options.Add(data2);
        }
        string[] attr = new string[] { "Magic", "Skill", "Reflex", "Awareness", "Resistance" };
        foreach (string bute in attr)
        {
            TMP_Dropdown.OptionData data1 = new TMP_Dropdown.OptionData();
            data1.text = bute;
            attributeBoon.options.Add(data1);
            TMP_Dropdown.OptionData data2 = new TMP_Dropdown.OptionData();
            data2.text = bute;
            attributeBane.options.Add(data2);
        }
    }

    public void checkHairColors(int colorIdx)
    {
        ColorSet set = RNGStuff.HAIR_COLORS[colorIdx];
        for (int q = 0; q < set.getColors().Count; q++)
        {
            palette[q].color = set.getColors()[q];
        }
    }
    public void checkSkinColors(int colorIdx)
    {
        ColorSet set = RNGStuff.SKIN_COLORS[colorIdx];
        for (int q = 0; q < set.getColors().Count; q++)
        {
            palette[q].color = set.getColors()[q];
        }
    }
    public void checkEyeColors(int colorIdx)
    {
        ColorSet set = RNGStuff.EYE_COLORS[colorIdx];
        for (int q = 0; q < set.getColors().Count; q++)
        {
            palette[q].color = set.getColors()[q];
        }
    }
    public void confirmHumanOptions()
    {
        Toggle age = StaticData.findDeepChild(menu.transform, "Aging").GetComponent<Toggle>();
        GeneralGameplayManager.setAging(age.isOn);
        Transform hairOpts = StaticData.findDeepChild(menu.transform, "HairColorBack");
        int idx = 0;
        List<int> selectedHairs = new List<int>();
        for (int q = 0; q < hairOpts.childCount; q++)
        {
            Toggle toggle = hairOpts.GetChild(q).GetComponent<Toggle>();
            if (toggle != null)
            {
                if (toggle.isOn)
                {
                    selectedHairs.Add(idx);
                }
                idx++;
            }
        }
        if (selectedHairs.Count == 0)
        {
            selectedHairs.Add(0);
        }
        Transform skinOpts = StaticData.findDeepChild(menu.transform, "SkinColorBack");
        idx = 0;
        List<int> selectedSkins = new List<int>();
        for (int q = 0; q < skinOpts.childCount; q++)
        {
            Toggle toggle = skinOpts.GetChild(q).GetComponent<Toggle>();
            if (toggle != null)
            {
                if (toggle.isOn)
                {
                    selectedSkins.Add(idx);
                }
                idx++;
            }
        }
        if (selectedSkins.Count == 0)
        {
            selectedSkins.Add(0);
        }
        Transform eyeOpts = StaticData.findDeepChild(menu.transform, "EyeColorBack");
        idx = 0;
        List<int> selectedEyes = new List<int>();
        for (int q = 0; q < eyeOpts.childCount; q++)
        {
            Toggle toggle = eyeOpts.GetChild(q).GetComponent<Toggle>();
            if (toggle != null)
            {
                if (toggle.isOn)
                {
                    selectedEyes.Add(idx);
                }
                idx++;
            }
        }
        if (selectedEyes.Count == 0)
        {
            selectedEyes.Add(0);
        }

        RNGStuff.useColors(selectedHairs, selectedSkins, selectedEyes);

        switchToPane("BackgroundNationSettings");
    }

    public void confirmNationOptions()
    {
        Transform title = StaticData.findDeepChild(menu.transform, "NationTitle");
        Transform nationName = StaticData.findDeepChild(menu.transform, "NationName");
        Transform capital = StaticData.findDeepChild(menu.transform, "CapitalName");
        if (!nationName.GetComponent<NameRestricter>().isValid()
            || !capital.GetComponent<NameRestricter>().isValid())
        {
            return;
        }
        GeneralGameplayManager.initializePlayerNation(nationName.GetComponent<TMP_InputField>().text,
            capital.GetComponent<TMP_InputField>().text,
            title.GetComponent<TMP_Dropdown>().value, 0);

        switchToPane("BackgroundPlayerSettings");
    }

    public void changeRed(float r)
    {
        skinR = r;
        updateSkinColor();
    }
    public void changeGreen(float g)
    {
        skinG = g;
        updateSkinColor();
    }
    public void changeBlue(float b)
    {
        skinB = b;
        updateSkinColor();
    }
    private void updateSkinColor()
    {
        Color color = new Color(skinR, skinG, skinB);
        StaticData.findDeepChild(menu.transform, "Portrait").GetComponent<Image>().color = color;
    }

    public void confirmPlayerOptions()
    {
        string pName = StaticData.findDeepChild(menu.transform, "PlayerName").GetComponent<TMP_InputField>().text;
        if (string.IsNullOrEmpty(pName))
        {
            pName = FantasyNames.getName();
        }
        int pGenderChoice = StaticData.findDeepChild(menu.transform, "GenderOptions").GetComponent<TMP_Dropdown>().value;
        bool pGender = pGenderChoice == 1 || (pGenderChoice == 0 && RNGStuff.nextBoolean());
        int[] interestChoices = new int[allInterests.Length];
        for (int q = 0; q < interestChoices.Length; q++)
        {
            int choice = allInterests[q].value;
            if (choice == 0)
            {
                Human.Interest[] interests = Human.Interest.values();
                bool keepGoing = true;
                int rand = 0;
                while (keepGoing)
                {
                    rand = RNGStuff.nextInt(interests.Length);
                    keepGoing = false;
                    for (int w = 0; w < allInterests.Length; w++)
                    {
                        if (q != w && allInterests[w].value == choice - 1)
                        {
                            rand = (rand + 1) % interests.Length;
                            keepGoing = true;
                            break;
                        }
                    }
                }
                choice = rand;
            }
            else
            {
                choice--;
            }
            interestChoices[q] = choice;
        }
        int pTrait = supportTrait.value == 0 ? RNGStuff.nextInt(Human.CombatTrait.values().Length)
            : supportTrait.value - 1;
        int pDemeanor = demeanor.value == 0 ? RNGStuff.nextInt(Demeanor.values().Length)
            : demeanor.value - 1;
        int pHpBoon = hpBoon.value == 0 ? RNGStuff.nextInt(hpBoon.options.Count - 1)
            : hpBoon.value - 1;
        int pHpBane = hpBane.value == 0 ? RNGStuff.nextInt(hpBane.options.Count - 1)
            : hpBane.value - 1;
        int pAttributeBoon = attributeBoon.value == 0 ? RNGStuff.nextInt(attributeBoon.options.Count - 1)
            : attributeBoon.value - 1;
        int pAttributeBane = attributeBane.value == 0 ? RNGStuff.nextInt(attributeBane.options.Count - 1)
            : attributeBane.value - 1;

        int skinRed = Mathf.RoundToInt(skinR * 255);
        int skinGreen = Mathf.RoundToInt(skinG * 255);
        int skinBlue = Mathf.RoundToInt(skinB * 255);

        GeneralGameplayManager.initializePlayer(pName, pGender, pFace, pNose, pLips, pEar, pEye,
            pIris, pBrow, pHair, pStache, pBeard, interestChoices[0], interestChoices[1], interestChoices[2],
            interestChoices[3], interestChoices[4], interestChoices[5], pTrait, pDemeanor, pHpBoon, pHpBane,
            pAttributeBoon, pAttributeBane, pHairColor, skinRed, skinGreen, skinBlue, pEyeColor);

        GeneralGameplayManager.getWorldMap().generateTerrain();

        //TODO switch scene
    }

    public void switchToPane(string pane)
    {
        for (int q = 0; q < menu.transform.childCount; q++)
        {
            menu.transform.GetChild(q).gameObject.SetActive(false);
        }
        StaticData.findDeepChild(menu.transform, pane).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
