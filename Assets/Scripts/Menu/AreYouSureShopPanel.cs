using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AreYouSureShopPanel : MonoBehaviour
{
    //Variables for its child
    GameObject yesChild, noChild;
    int currItemInt;

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
        EventTrigger noTrigger = yesChild.GetComponent<EventTrigger>();
        EventTrigger.Entry noEntry = new EventTrigger.Entry();
        noEntry.eventID = EventTriggerType.PointerDown;
        noEntry.callback.AddListener((data) => { PressedNo((PointerEventData)data); });
        noTrigger.triggers.Add(noEntry);

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void PrintAndGetInfo(int id)
    {
        currItemInt = id;
    }

    public void PressedYes(PointerEventData data)
    {
        Debug.Log("OnPointerDownDelegate called.");
    }

    public void PressedNo(PointerEventData data)
    {
        Debug.Log("OnPointerDownDelegate called.");
    }
}
