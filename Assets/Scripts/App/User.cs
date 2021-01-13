using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class User
{ 
    public class Player
    {
        public int coins;
        public bool map1Unlocked;
        public bool map2Unlocked;
        public bool map3Unlocked;
        public bool map4Unlocked;
        public int currentPlayingTime;
        public bool online;
        public bool tasksDone;

        public int availablePlayingTime;
        public int averageDailyPlayingTime;
        public int averageRunPlayingTime;
        public int averagePauseOrMenuPlayingTime;
        public int maxRunPlayingTime;
        public int maxCoinsInSingleRun;

        public Player()
        { }

        public void createNewDefaultPlayer()
        {
            coins = 0;
            map1Unlocked = true;
            map2Unlocked = false;
            map3Unlocked = false;
            map4Unlocked = false;
            currentPlayingTime = 0;
            online = true;
            tasksDone = false;

            availablePlayingTime = 0;
            averageDailyPlayingTime = 0;
            averageRunPlayingTime = 0;
            averagePauseOrMenuPlayingTime = 0;
            maxRunPlayingTime = 0;
            maxCoinsInSingleRun = 0;
        }
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
