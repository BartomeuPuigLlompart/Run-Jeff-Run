using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject firstUI;

    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject levels;
    [SerializeField] private GameObject shop;

    [SerializeField] GameObject areYouSureObject;

    [SerializeField] Text[] coinsTexts;

    int playerCoins;

    // Start is called before the first frame update
    void Start()
    {
        //Debug Only
        //PlayerPrefs.SetInt("CurrCoins", 650);
        playerCoins = PlayerPrefs.GetInt("CurrCoins", 0);

        //Set the values
        PlayerPrefs.SetInt("Char1", 1);
        PlayerPrefs.SetInt("House1", 1);
        PlayerPrefs.SetInt("Enemy1", 1);

        //Inactive every panel and then active the first selected one
        menu.SetActive(false);
        levels.SetActive(false);
        shop.SetActive(false);

        firstUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < coinsTexts.Length; i++)
        {
            if(coinsTexts[i]) //Check if it exists
            {
                int textValue;
                int.TryParse(coinsTexts[i].text, out textValue);
                int playerPref = PlayerPrefs.GetInt("CurrCoins", 0);

                if (textValue != playerPref)
                {
                    coinsTexts[i].text = playerPref.ToString();
                }
            }
        }
    }

    public void GoToMenu()
    {
        if(!areYouSureObject.active)
        {
            menu.SetActive(true);
            levels.SetActive(false);
            shop.SetActive(false);
        }
    }

    public void GoToLevels()
    {
        levels.SetActive(true);
        menu.SetActive(false);
        shop.SetActive(false);
    }

    public void GoToShop()
    {
        shop.SetActive(true);
        menu.SetActive(false);
        levels.SetActive(false);
    }

    public void GoToLevel1() //For Presentation only
    {
        SceneManager.LoadScene(2); //Menu scene
    }
}
