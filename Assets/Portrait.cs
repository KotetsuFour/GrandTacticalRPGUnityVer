using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portrait : MonoBehaviour
{
    public static int HUMAN_FACE_IDX = 0;
    public static int HUMAN_MOUTH_IDX = 1;
    public static int HUMAN_NOSE_IDX = 2;
    public static int HUMAN_EARS_IDX = 3;
    public static int HUMAN_EYES_IDX = 4;
    public static int HUMAN_IRIS_IDX = 5;
    public static int HUMAN_BROWS_IDX = 6;
    public static int HUMAN_HAIR_IDX = 7;
    public static int HUMAN_STACHE_IDX = 8;
    public static int HUMAN_BEARD_IDX = 9;
    public static int HUMAN_HAIRCOLOR_IDX = 10;
    public static int HUMAN_SKINR_IDX = 11;
    public static int HUMAN_SKING_IDX = 12;
    public static int HUMAN_SKINB_IDX = 13;
    public static int HUMAN_EYECOLOR_IDX = 14;
    public void draw(Unit u)
    {
        if (u is Human)
        {
            draw((Human)u);
        }
    }
    public void draw(Human human)
    {
        draw(human.getAppearance(), human.getDemeanor(), human.getGender());
    }
    public void draw(int[] appearance, Demeanor dem, bool gender)
    {
        Color hairColor = RNGStuff.HAIR_COLORS_IN_USE.colorAtIndex(appearance[HUMAN_HAIRCOLOR_IDX]);
        Color skinColor = new Color(appearance[HUMAN_SKINR_IDX] / 255f, appearance[HUMAN_SKING_IDX] / 255f, appearance[HUMAN_SKINB_IDX] / 255f);
        Color eyeColor = RNGStuff.EYE_COLORS_IN_USE.colorAtIndex(appearance[HUMAN_EYECOLOR_IDX]);

        StaticData.findDeepChild(transform, "BehindHair").GetComponent<Image>()
            .sprite = gender ? PortraitDictionary.behindHairList[1]
            : PortraitDictionary.behindHairList[0];
        StaticData.findDeepChild(transform, "FrontHair").GetComponent<Image>()
            .sprite = PortraitDictionary.frontHairList[appearance[HUMAN_HAIR_IDX]];
        StaticData.findDeepChild(transform, "Brows").GetComponent<Image>()
            .sprite = PortraitDictionary.browsList[appearance[HUMAN_BROWS_IDX]][dem.getBrowOrientation()];
        StaticData.findDeepChild(transform, "Stache").GetComponent<Image>()
            .sprite = gender ? PortraitDictionary.stacheList[0]
            : PortraitDictionary.stacheList[appearance[HUMAN_STACHE_IDX]];
        StaticData.findDeepChild(transform, "Beard").GetComponent<Image>()
            .sprite = gender ? PortraitDictionary.beardList[0]
            : PortraitDictionary.beardList[appearance[HUMAN_BEARD_IDX]];
        StaticData.findDeepChild(transform, "BehindHair").GetComponent<Image>()
            .color = hairColor;
        StaticData.findDeepChild(transform, "FrontHair").GetComponent<Image>()
            .color = hairColor;
        StaticData.findDeepChild(transform, "Brows").GetComponent<Image>()
            .color = hairColor;
        StaticData.findDeepChild(transform, "Stache").GetComponent<Image>()
            .color = hairColor;
        StaticData.findDeepChild(transform, "Beard").GetComponent<Image>()
            .color = hairColor;

        StaticData.findDeepChild(transform, "Ears").GetComponent<Image>()
            .sprite = PortraitDictionary.earsList[appearance[HUMAN_EARS_IDX]];
        StaticData.findDeepChild(transform, "Face").GetComponent<Image>()
            .sprite = PortraitDictionary.faceList[appearance[HUMAN_FACE_IDX]];
        StaticData.findDeepChild(transform, "Nose").GetComponent<Image>()
            .sprite = PortraitDictionary.noseList[appearance[HUMAN_NOSE_IDX]];
        StaticData.findDeepChild(transform, "Mouth").GetComponent<Image>()
            .sprite = PortraitDictionary.mouthList[appearance[HUMAN_MOUTH_IDX]][dem.getMouthOrientation()];
        StaticData.findDeepChild(transform, "Ears").GetComponent<Image>()
            .color = skinColor;
        StaticData.findDeepChild(transform, "Face").GetComponent<Image>()
            .color = skinColor;
        /*
        StaticData.findDeepChild(transform, "Nose").GetComponent<Image>()
            .color = skinColor;
        StaticData.findDeepChild(transform, "Mouth").GetComponent<Image>()
            .color = skinColor;
        */

        StaticData.findDeepChild(transform, "Eyes").GetComponent<Image>()
            .sprite = PortraitDictionary.eyesList[appearance[HUMAN_EYES_IDX]];
        StaticData.findDeepChild(transform, "Iris").GetComponent<Image>()
            .sprite = PortraitDictionary.irisList[appearance[HUMAN_IRIS_IDX]];
        StaticData.findDeepChild(transform, "Iris").GetComponent<Image>()
            .color = eyeColor;
    }
}
