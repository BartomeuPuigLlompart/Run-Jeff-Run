using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ObjectType { Character, House, Enemy, Level };

public class SlotItem : MonoBehaviour
{
    [SerializeField] int objectID;
    [SerializeField] ObjectType objectType;
    [SerializeField] int objectPrice;
    [SerializeField] bool active, unlocked;

    public int playerNumber;
    [SerializeField] Color activeColor, inactiveColor,
        imageActiveColor, imageInactiveColor;

    [SerializeField] GameObject areYouSureObject;

    bool oldActive, oldUnlocked;

    Image objectImage;

    
    AreYouSureShopPanel areYouSure;

    //Variables for its child
    GameObject slotChild, textChild;
    RectTransform childRectTransform;

    float lockedLeft = 96.07938f, lockedRight = 96.07938f,
        lockedTop = 39.49072f, lockedBottom = 121.2907f;

    float unlockedLeft = 72.19879f, unlockedRight = 72.19879f,
        unlockedTop = 64.57646f, unlockedBottom = 64.57643f;

    Image childImage;

    string idBuff, typeBuff;

    GameObject tapAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        //Set the buffs for the PlayerPrefs
        idBuff = objectID.ToString();
        switch (objectType)
        {
            case ObjectType.Character:
                typeBuff = "Char";
                break;
            case ObjectType.House:
                typeBuff = "House";
                break;
            case ObjectType.Enemy:
                typeBuff = "Enemy";
                break;
            default: break;
        }

        ////Debug Only: It sets the PlayerPrefs
        //if(objectID != 1) PlayerPrefs.SetInt("Char" + objectID.ToString(), 0);
        //else PlayerPrefs.SetInt("Char1", 1);
        //PlayerPrefs.SetInt("CurrChar", 1);

        //Get the "Are you sure?" Panel
        if (areYouSureObject) areYouSure = areYouSureObject.GetComponent<AreYouSureShopPanel>();



        //Get the info unlocked/active from PlayerPrefs
        active = (PlayerPrefs.GetInt("Curr" + typeBuff, 1) == objectID); //Active if it's already activated
        unlocked = (PlayerPrefs.GetInt(typeBuff + idBuff, 1) == 1); //Check if it's already unlocked and if it's not the default skin
        if (objectID == 1) unlocked = true;

        switch (objectType)
        {
            case ObjectType.Character:
                //Variables for the character image
                lockedLeft = 96.07938f;
                lockedRight = 96.07938f;
                lockedTop = 39.49072f;
                lockedBottom = 121.2907f;

                unlockedLeft = 72.19879f;
                unlockedRight = 72.19879f;
                unlockedTop = 64.57646f;
                unlockedBottom = 64.57643f;
                break;
            case ObjectType.House:
                break;
            case ObjectType.Enemy:
                break;
        }

        oldActive = active;
        oldUnlocked = unlocked;

        //Get the image for change the color later
        objectImage = gameObject.GetComponent<Image>();
        
        //Get the info of its child
        slotChild = gameObject.transform.GetChild(0).gameObject;
        textChild = gameObject.transform.GetChild(1).gameObject;

        //Make sure slotChild exists
        if(slotChild)
        {
            childRectTransform = slotChild.GetComponent<RectTransform>();
            childImage = slotChild.GetComponent<Image>();

            //Set already the info from unlocked/active  values
            if (active)
            {
                //Set the color of activated once
                objectImage.color = activeColor;
                childImage.color = imageActiveColor;
            }
            else
            {
                //Set the color of inactivated once
                objectImage.color = inactiveColor;
                childImage.color = imageInactiveColor;
            }

            if(unlocked)
            {
                SetLeft(childRectTransform, unlockedLeft);
                SetRight(childRectTransform, unlockedRight);
                SetTop(childRectTransform, unlockedTop);
                SetBottom(childRectTransform, unlockedBottom);
            }
            else
            {
                SetLeft(childRectTransform, lockedLeft);
                SetRight(childRectTransform, lockedRight);
                SetTop(childRectTransform, lockedTop);
                SetBottom(childRectTransform, lockedBottom);
            }

            //Make sure textChild exists 
            if(textChild)
            {
                //Set the price, then show it if not bought
                textChild.GetComponent<Text>().text = objectPrice.ToString();
                textChild.SetActive(!unlocked);
            }            
        }

        tapAudioSource = GameObject.FindGameObjectWithTag("SoundManager");
    }

    // Update is called once per frame
    void Update()
    {
        //A locked slot can never be activated
        if (active && !unlocked) active = false;


        //Setting the item color
        if (active && !oldActive)
        {
            //Set the color of activated once
            objectImage.color = activeColor;
            childImage.color = imageActiveColor;
        }
        else if(!active && oldActive)
        {
            //Set the color of inactivated once
            objectImage.color = inactiveColor;
            childImage.color = imageInactiveColor;
        }

        //Setting the image size for this GameObject's child
        if(unlocked && !oldUnlocked)
        {
            SetLeft(childRectTransform, unlockedLeft);
            SetRight(childRectTransform, unlockedRight);
            SetTop(childRectTransform, unlockedTop);
            SetBottom(childRectTransform, unlockedBottom);
            textChild.SetActive(false);
        }
        else if(!unlocked && oldUnlocked)
        {
            SetLeft(childRectTransform, lockedLeft);
            SetRight(childRectTransform, lockedRight);
            SetTop(childRectTransform, lockedTop);
            SetBottom(childRectTransform, lockedBottom);
            textChild.SetActive(true);
        }

        oldActive = active;
        oldUnlocked = unlocked;

        //Check at any moment if the item is still active
        active = (PlayerPrefs.GetInt("Curr" + typeBuff, 1) == objectID);
        unlocked = (PlayerPrefs.GetInt(typeBuff + idBuff, 0) == 1);
    }

    public void Pressed()
    {
        if(!areYouSureObject.active)
        {
            if (tapAudioSource) tapAudioSource.GetComponent<SoundManager>().PlayTapSound();

            //Check if the player didn't buy it yet
            if (!unlocked)
            {
                int currCoins = PlayerPrefs.GetInt("CurrCoins", 0);

                //Check if the player has enough coins to get it
                if(currCoins >= objectPrice)
                {
                    //Print the "are you sure?" panel
                    areYouSureObject.SetActive(true);
                    areYouSure.PrintAndGetInfo(objectID, objectType, objectPrice);
                }
            }
            else
            {
                if (!active)
                {
                    //Set this subject as active and save it in its PlayerPref
                    //active = true;
                    PlayerPrefs.SetInt("Curr" + typeBuff, objectID);
                    if (objectID == 2)
                    {
                        PlayerPrefs.SetFloat("bluecharacter", 2);
                    }

                }
                else
                {
                    //Nothing, it's already activated
                }
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
