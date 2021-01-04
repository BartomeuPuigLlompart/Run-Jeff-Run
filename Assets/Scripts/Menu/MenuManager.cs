using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject firstUI;

    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject levels;
    [SerializeField] private GameObject shop;

    [SerializeField] GameObject areYouSureObject;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Char1", 1);
        PlayerPrefs.SetInt("House1", 1);
        PlayerPrefs.SetInt("Enemy1", 1);

        menu.SetActive(false);
        levels.SetActive(false);
        shop.SetActive(false);

        firstUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
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
