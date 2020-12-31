using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour
{
    enum ObjectType { Character, House, Enemy};

    [SerializeField] ObjectType objectType;
    [SerializeField] bool active, unlocked;

    [SerializeField] Color activeColor, inactiveColor,
        imageActiveColor, imageInactiveColor;

    bool oldActive, oldUnlocked;

    Image objectImage;

    //Variables for its child
    GameObject slotChild;
    RectTransform childRectTransform;

    float lockedLeft = 96.07938f, lockedRight = 96.07938f,
        lockedTop = 39.49072f, lockedBottom = 121.2907f;

    float unlockedLeft = 72.19879f, unlockedRight = 72.19879f,
        unlockedTop = 64.57646f, unlockedBottom = 64.57643f;

    Image childImage;

    // Start is called before the first frame update
    void Start()
    {
        objectImage = gameObject.GetComponent<Image>();

        slotChild = gameObject.transform.GetChild(0).gameObject;

        if(slotChild)
        {
            childRectTransform = slotChild.GetComponent<RectTransform>();
            childImage = slotChild.GetComponent<Image>();
        }
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
        }
        else if(!unlocked && oldUnlocked)
        {
            SetLeft(childRectTransform, lockedLeft);
            SetRight(childRectTransform, lockedRight);
            SetTop(childRectTransform, lockedTop);
            SetBottom(childRectTransform, lockedBottom);
        }

        oldActive = active;
        oldUnlocked = unlocked;
    }

    public void Pressed()
    {
        if(!unlocked)
        {
            //Print the "are you sure?" panel
        }
        else
        {
            if(!active)
            {
                //Set this subject as active
            }
            else
            {
                //Nothing, it's already activated
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
