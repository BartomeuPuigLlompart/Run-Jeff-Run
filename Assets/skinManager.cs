using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skinManager : MonoBehaviour
{

    public GameObject[] players;
    
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < players.Length; i++)
        {
            players[i].SetActive(false);
        }
        int pf = PlayerPrefs.GetInt("CurrChar", 1);
        players[pf - 1].SetActive(true);
    }

   
}
