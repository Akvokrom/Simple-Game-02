using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public Text BonusesText;
    public Text MapInfo;
    public Text MatchResult;

    internal PlayerController playerController;
    internal BombSpawner bombSpawner;
    GameController gameController;
    CharactersSpawner CharactersSpawner;
    MapGenerator MapGenerator;
    internal string GamePhase;
    Transform enemies;
    GameObject player;

    void Start()
    {
        GamePhase = "MainMenu";
        MatchResult.text = "Click to Start";

        MatchResult.gameObject.SetActive(true);
        BonusesText.gameObject.SetActive(false);
        MapInfo.gameObject.SetActive(false);

        gameController = GameObject.Find("GameController").gameObject.GetComponent<GameController>();
        CharactersSpawner = GameObject.Find("GameController").gameObject.GetComponent<CharactersSpawner>();
        MapGenerator = GameObject.Find("GameController").gameObject.GetComponent<MapGenerator>();

        enemies = GameObject.Find("Enemies").gameObject.transform;
    }

    void Update()
    {
        //Starting game
        if (GamePhase == "MainMenu" && Input.GetMouseButtonDown(0))
        {
            MatchResult.gameObject.SetActive(false);
            BonusesText.gameObject.SetActive(true);
            MapInfo.gameObject.SetActive(true);
            GamePhase = "Playing";

            CharactersSpawner.spawnPlayer();
            CharactersSpawner.spawnEnemies();
            MapGenerator.updatesurfeces();
            player = GameObject.FindGameObjectWithTag("Player");

            bombSpawner = player.gameObject.GetComponent<BombSpawner>();
            playerController = player.gameObject.GetComponent<PlayerController>();

        }
        //Game over
        else if (GamePhase == "Lose" && Input.GetMouseButtonDown(0))
        {
            newLevel();
        }

        //Game playing
        if (GamePhase == "Playing")
        {
            //Player die
            if (player == null)
            {
                MatchResult.text = "You lose :(\nClick to restart";
                MatchResult.gameObject.SetActive(true);
                GamePhase = "Lose";
            }
            //All enemies die, player win
            else if (enemies.childCount <= 0)
            {
                MatchResult.text = "You WIN!!!\nPortal spawned!";
                MatchResult.gameObject.SetActive(true);
                GamePhase = "Win";
                CharactersSpawner.portalSpawner();
                Invoke("MatchResultTextClose", 2f);
            }

            //Text for bonuses
            BonusesText.text =
                "Speed: " + playerController.speed.ToString() +
                "\nArmor: " + Mathf.Round(playerController.armor).ToString() +
                "\nBombs: " + bombSpawner.numberOfBombs.ToString() +
                "\nFire Power: " + bombSpawner.firePower.ToString();

            //Text for map info
            MapInfo.text =
                "Level: " + PlayerPrefs.GetInt("Level").ToString() +
                "\nMap size: " + gameController.mapSize.ToString() +
                "\nTime left: " + Mathf.Round(gameController.Timer).ToString() +
                "\nEnemies: " + enemies.childCount.ToString();
        }

    }

    public void newLevel()
    {
        //Update map info
        gameController.newlevel();

        //Generate new map
        MapGenerator.newlevel();

        GamePhase = "MainMenu";
        MatchResult.text = "Click to Start";

        //Destroy all enemies
        GameObject enemies = GameObject.Find("Enemies");
        for (int i = 0; i < enemies.transform.childCount; i++)
        {
            Destroy(enemies.transform.GetChild(i).gameObject);
        }

        //Destroy all power ups
        GameObject[] powerUp = GameObject.FindGameObjectsWithTag("PowerUp");
        foreach(GameObject pu in powerUp)
        {
            Destroy(pu);
        }

        //Destroy bombs
        GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");
        foreach (GameObject bomb in bombs)
        {
            Destroy(bomb);
        }

        MatchResult.gameObject.SetActive(true);
        BonusesText.gameObject.SetActive(false);
        MapInfo.gameObject.SetActive(false);
    }

    void MatchResultTextClose()
    {
        MatchResult.gameObject.SetActive(false);
    }
}
