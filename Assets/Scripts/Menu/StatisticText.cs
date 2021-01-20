using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticText : MonoBehaviour
{
    enum Type { INT, FLOAT, STRING };

    [SerializeField] Type playerPrefType;
    [SerializeField] string playerPrefKey;

    string playerPrefValue;
    int playerPrefIntValue;
    float playerPrefFloatValue;

    Text text;

    // Start is called before the first frame update
    void Start()
    {
        setValue();
        printText();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasValueChanged())
        {
            setValue();
            printText();
        }
    }

    void setValue()
    {
        switch(playerPrefType)
        {
            case Type.INT:
                playerPrefIntValue = PlayerPrefs.GetInt(playerPrefKey, 0);
                playerPrefValue = playerPrefIntValue.ToString();
                break;
            case Type.FLOAT:
                playerPrefFloatValue = PlayerPrefs.GetFloat(playerPrefKey, 0);
                playerPrefValue = playerPrefFloatValue.ToString();
                break;
            case Type.STRING:
                playerPrefValue = PlayerPrefs.GetString(playerPrefKey);
                break;
            default: break;
        }
    }

    bool hasValueChanged()
    {
        switch (playerPrefType)
        {
            case Type.INT:
                return playerPrefIntValue != PlayerPrefs.GetInt(playerPrefKey, 0);
            case Type.FLOAT:
                return playerPrefFloatValue != PlayerPrefs.GetFloat(playerPrefKey, 0);
            case Type.STRING:
                return playerPrefValue != PlayerPrefs.GetString(playerPrefKey);
            default: break;
        }
        return false;
    }

    void printText()
    {
        text.text = playerPrefValue;
    }
}
