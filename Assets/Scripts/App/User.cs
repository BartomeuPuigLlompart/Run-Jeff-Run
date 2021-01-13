using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class User
{ 
    public class Player
    {
        int coins;
        bool map1Unlocked;
        bool map2Unlocked;
        bool map3Unlocked;
        bool map4Unlocked;
        int currentPlayingTime;
        bool online;
        bool tasksDone;
        
        int availablePlayingTime;
        int averageDailyPlayingTime;
        int averageRunPlayingTime;
        int averagePauseOrMenuPlayingTime;
        int maxRunPlayingTime;
        int maxCoinsInSingleRun;

        Player()
        { }
    }

    public string rol;
    public string correo;
    public string id;
    public string idOpuesto;
    public Player player;

    public User()
    {
    }
    public User(string _rol, string _correo, string _id, string _idOpuesto)
    {
        rol = _rol;
        correo = _correo;
        id = _id;
        idOpuesto = _idOpuesto;
    }
}
