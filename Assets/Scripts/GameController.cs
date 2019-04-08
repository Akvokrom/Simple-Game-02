using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    public GameObject[] enemyNames;
    public bool[] enemies;
    public int[] additionalEnemies;

    CharactersSpawner CharactersSpawner;

    public int startMapSize;
    internal int mapSize;
    internal int mapWidth;
    internal int mapHeight;
    internal int gameLevel;
    public float setTimer;
    internal float Timer;
    float FastEnemiesSpawmTimer = 2f;

    void Awake()
    {
        //Generate map
        newlevel();
    }

    void Start()
    {
        CharactersSpawner = GameObject.Find("GameController").gameObject.GetComponent<CharactersSpawner>();
    }

    private void Update()
    {
        if(Timer > 0)
        {
            Timer -= Time.deltaTime;
        }
        else
        {
            Timer = 0;

            //if time is over, summon fast enemies
            if(FastEnemiesSpawmTimer > 0)
            {
                FastEnemiesSpawmTimer -= Time.deltaTime;
            }
            else
            {
                FastEnemiesSpawmTimer = 2f;
                CharactersSpawner.setEnemyPosition(enemyNames.Length - 1);
            }

        }
    }
    public void newlevel()
    {
        if (!PlayerPrefs.HasKey("Level"))
        {
            PlayerPrefs.SetInt("Level", 1);
        }

        gameLevel = PlayerPrefs.GetInt("Level");

        //Set field size
        mapSize = startMapSize + (gameLevel - 1) * 20;
        Timer = setTimer;
        mapWidth = (int)Mathf.Round(Screen.width * Mathf.Sqrt((float)mapSize / (Screen.width * Screen.height)));
        mapHeight = (int)Mathf.Round(Screen.height * Mathf.Sqrt((float)mapSize / (Screen.width * Screen.height)));
    }
}
