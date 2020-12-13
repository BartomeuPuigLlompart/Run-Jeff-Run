using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject firstUI;
    [SerializeField] private GameObject firstUIPanel;

    [SerializeField] private GameObject character;
    [SerializeField] private GameObject house;
    [SerializeField] private GameObject enemy;

    [SerializeField] private GameObject characterPanel;
    [SerializeField] private GameObject housePanel;
    [SerializeField] private GameObject enemyPanel;

    [SerializeField] private GameObject characterButton;
    [SerializeField] private GameObject houseButton;
    [SerializeField] private GameObject enemyButton;

    [SerializeField] private Color activeColor;
    [SerializeField] private Color inactiveColor;

    // Start is called before the first frame update
    void Start()
    {
        character.SetActive(false);
        house.SetActive(false);
        enemy.SetActive(false);

        characterPanel.SetActive(false);
        housePanel.SetActive(false);
        enemyPanel.SetActive(false);

        firstUI.SetActive(true);
        firstUIPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoToCharacter()
    {
        //Shop Contents
        character.SetActive(true);
        house.SetActive(false);
        enemy.SetActive(false);

        //Top Panels
        characterPanel.SetActive(true);
        housePanel.SetActive(false);
        enemyPanel.SetActive(false);

        //Color Buttons
        characterButton.GetComponent<Image>().color = activeColor;
        houseButton.GetComponent<Image>().color = inactiveColor;
        enemyButton.GetComponent<Image>().color = inactiveColor;
    }

    public void GoToHouse()
    {
        //Shop Contents
        house.SetActive(true);
        character.SetActive(false);
        enemy.SetActive(false);

        //Top Panels
        characterPanel.SetActive(false);
        housePanel.SetActive(true);
        enemyPanel.SetActive(false);

        //Color Buttons
        characterButton.GetComponent<Image>().color = inactiveColor;
        houseButton.GetComponent<Image>().color = activeColor;
        enemyButton.GetComponent<Image>().color = inactiveColor;
    }

    public void GoToEnemy()
    {
        //Shop Contents
        enemy.SetActive(true);
        character.SetActive(false);
        house.SetActive(false);

        //Top Panels
        characterPanel.SetActive(false);
        housePanel.SetActive(false);
        enemyPanel.SetActive(true);

        //Color Buttons
        characterButton.GetComponent<Image>().color = inactiveColor;
        houseButton.GetComponent<Image>().color = inactiveColor;
        enemyButton.GetComponent<Image>().color = activeColor;
    }
}
