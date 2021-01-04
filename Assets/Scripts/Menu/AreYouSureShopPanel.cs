using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AreYouSureShopPanel : MonoBehaviour
{
    //Variables for its child
    GameObject yesChild, noChild;
    int currItemId;
    ObjectType currItemType;

    string idBuff, typeBuff;

    //Variables of the item selected
    GameObject itemObject;
    SlotItem item;

    // Start is called before the first frame update
    void Start()
    {
        yesChild = gameObject.transform.GetChild(1).gameObject;
        noChild = gameObject.transform.GetChild(2).gameObject;

        //YES BUTTON
        EventTrigger yesTrigger = yesChild.GetComponent<EventTrigger>();
        EventTrigger.Entry yesEntry = new EventTrigger.Entry();
        yesEntry.eventID = EventTriggerType.PointerDown;
        yesEntry.callback.AddListener((data) => { PressedYes((PointerEventData)data); });
        yesTrigger.triggers.Add(yesEntry);

        //NO BUTTON
        EventTrigger noTrigger = noChild.GetComponent<EventTrigger>();
        EventTrigger.Entry noEntry = new EventTrigger.Entry();
        noEntry.eventID = EventTriggerType.PointerDown;
        noEntry.callback.AddListener((data) => { PressedNo((PointerEventData)data); });
        noTrigger.triggers.Add(noEntry);

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void PrintAndGetInfo(int id, ObjectType type)
    {
        currItemId = id;
        currItemType = type;

        idBuff = currItemId.ToString();
        switch(type)
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

        itemObject = GameObject.Find(typeBuff + idBuff);

        if (itemObject) item = itemObject.GetComponent<SlotItem>();
    }

    public void PressedYes(PointerEventData data)
    {
        PlayerPrefs.SetInt(typeBuff + idBuff, 1);

        gameObject.SetActive(false);
    }

    public void PressedNo(PointerEventData data)
    {
        gameObject.SetActive(false);
    }
}
