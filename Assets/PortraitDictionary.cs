using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitDictionary : MonoBehaviour
{
    [SerializeField] private Sprite[] behindHair;
    [SerializeField] private Sprite[] ears;
    [SerializeField] private Sprite[] face;
    [SerializeField] private Sprite[] eyes;
    [SerializeField] private Sprite[] iris;
    [SerializeField] private Sprite[] nose;
    [SerializeField] private Sprite[] mouth0;
    [SerializeField] private Sprite[] mouth1;
    [SerializeField] private Sprite[] mouth2;
    [SerializeField] private Sprite[] brows0;
    [SerializeField] private Sprite[] brows1;
    [SerializeField] private Sprite[] brows2;
    [SerializeField] private Sprite[] frontHair;
    [SerializeField] private Sprite[] stache;
    [SerializeField] private Sprite[] beard;

    public static Sprite[] behindHairList;
    public static Sprite[] earsList;
    public static Sprite[] faceList;
    public static Sprite[] eyesList;
    public static Sprite[] irisList;
    public static Sprite[] noseList;
    public static Sprite[][] mouthList;
    public static Sprite[][] browsList;
    public static Sprite[] frontHairList;
    public static Sprite[] stacheList;
    public static Sprite[] beardList;

    // Start is called before the first frame update
    void Start()
    {
        behindHairList = behindHair;
        earsList = ears;
        faceList = face;
        eyesList = eyes;
        irisList = iris;
        noseList = nose;
        mouthList = new Sprite[][] { mouth0, mouth1, mouth2 };
        browsList = new Sprite[][] { brows0, brows1, brows2 };
        frontHairList = frontHair;
        stacheList = stache;
        beardList = beard;
    }
}
