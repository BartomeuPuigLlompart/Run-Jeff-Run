using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotLevel : MonoBehaviour
{
    [SerializeField] int objectID;
    [SerializeField] ObjectType objectType;
    [SerializeField] int objectPrice;
    [SerializeField] bool unlocked;

    
    [SerializeField]
    Color unlockedColor, lockedColor,
        imageUnlockedColor, imageLockedColor;

    [SerializeField] GameObject areYouSureObject;

    bool oldUnlocked;
    Image objectImage;
    AreYouSureShopPanel areYouSure;

    //Variables for its child
    GameObject slotChild, textChild;
    RectTransform childRectTransform;

    float lockedLeft = 160f, lockedRight = 160f,
        lockedTop = 100f, lockedBottom = 200f;

    float unlockedLeft = 125f, unlockedRight = 125f,
        unlockedTop = 100f, unlockedBottom = 130f;

    Image childImage;

    string idBuff, typeBuff;

    // Start is called before the first frame update
    void Start()
    {
        //Set the buffs for the PlayerPrefs
        idBuff = objectID.ToString();
        typeBuff = "Level";

        //Get the "Are you sure?" Panel
        if (areYouSureObject) areYouSure = areYouSureObject.GetComponent<AreYouSureShopPanel>();

        //Get the info unlocked/active from PlayerPrefs
        unlocked = (PlayerPrefs.GetInt(typeBuff + idBuff, 1) == 1); //Check if it's already unlocked and if it's not the default skin

        oldUnlocked = unlocked;

        //Get the image for change the color later
        objectImage = gameObject.GetComponent<Image>();

        //Get the info of its child
        slotChild = gameObject.transform.GetChild(0).gameObject;
        textChild = gameObject.transform.GetChild(1).gameObject;

        if (slotChild)
        {
            childRectTransform = slotChild.GetComponent<RectTransform>();
            childImage = slotChild.GetComponent<Image>();

            //Set already the info from unlocked/active  values
            if (unlocked)
            {
                SetLeft(childRectTransform, unlockedLeft);
                SetRight(childRectTransform, unlockedRight);
                SetTop(childRectTransform, unlockedTop);
                SetBottom(childRectTransform, unlockedBottom);

                //Set the color of unlocked once
                objectImage.color = unlockedColor;
                childImage.color = imageUnlockedColor;
            }
            else
            {
                SetLeft(childRectTransform, lockedLeft);
                SetRight(childRectTransform, lockedRight);
                SetTop(childRectTransform, lockedTop);
                SetBottom(childRectTransform, lockedBottom);

                //Set the color of locked once
                objectImage.color = lockedColor;
                childImage.color = imageLockedColor;
            }

            //Make sure textChild exists 
            if (textChild)
            {
                //Set the price, then show it if not bought
                textChild.GetComponent<Text>().text = objectPrice.ToString();
                textChild.SetActive(!unlocked);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Setting the image size for this GameObject's child
        if (unlocked && !oldUnlocked)
        {
            SetLeft(childRectTransform, unlockedLeft);
            SetRight(childRectTransform, unlockedRight);
            SetTop(childRectTransform, unlockedTop);
            SetBottom(childRectTransform, unlockedBottom);
            textChild.SetActive(false);

            //Set the color of unlocked once
            objectImage.color = unlockedColor;
            childImage.color = imageUnlockedColor;
        }
        else if (!unlocked && oldUnlocked)
        {
            SetLeft(childRectTransform, lockedLeft);
            SetRight(childRectTransform, lockedRight);
            SetTop(childRectTransform, lockedTop);
            SetBottom(childRectTransform, lockedBottom);
            textChild.SetActive(true);

            //Set the color of locked once
            objectImage.color = lockedColor;
            childImage.color = imageLockedColor;
        }

        oldUnlocked = unlocked;

        //Check at any moment if the item is still active
        unlocked = (PlayerPrefs.GetInt(typeBuff + idBuff, 0) == 1);
    }

    public void Pressed()
    {
        if (!areYouSureObject.active)
        {
            //Check if the player didn't buy it yet
            if (!unlocked)
            {
                int currCoins = PlayerPrefs.GetInt("CurrCoins", 0);

                //Check if the player has enough coins to get it
                if (currCoins >= objectPrice)
                {
                    //Print the "are you sure?" panel
                    areYouSureObject.SetActive(true);
                    areYouSure.PrintAndGetInfo(objectID, objectType, objectPrice);
                }
            }
            else
            {
                //Load Level
                PlayerPrefs.SetInt("CurrLevel", objectID);
            }
        }
    }

    public static void SetLeft(RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }
}
