using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersSpawner : MonoBehaviour
{
    GameController gameController;

    public GameObject playerPrefab;
    internal GameObject player;
    public GameObject portal;

    internal GameObject[] enemyNames;
    internal int[] additionalEnemies;
    bool[] enemies;

    void Start()
    {
        gameController = GameObject.Find("GameController").gameObject.GetComponent<GameController>();

        enemyNames = gameController.enemyNames;
        additionalEnemies = gameController.additionalEnemies;
        enemies = gameController.enemies;
    }

    internal void spawnPlayer()
    {
        bool placeSelected = false;

        int mapHeight = gameController.mapHeight;
        int mapWidth = gameController.mapWidth;

        Vector3 position = new Vector3(Random.Range(-mapWidth, mapWidth + 1), 0, Random.Range(-mapHeight, mapHeight + 1));

        while (!placeSelected)
        {
            position = new Vector3(Random.Range(-mapWidth, mapWidth + 1), 0, Random.Range(-mapHeight, mapHeight + 1));

            Ray ray = new Ray(new Vector3(position.x, 2, position.z), Vector3.down);
            Physics.Raycast(ray, out RaycastHit hit);

            //Check if current cell is empty
            if (hit.collider != null && hit.collider.gameObject.tag == "Ground")
            {
                Vector3[] directions = new Vector3[] { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
                foreach (Vector3 dir in directions)
                {
                    Ray rayPos = new Ray(position, dir);
                    Physics.Raycast(rayPos, out RaycastHit hitPos);

                    if (hitPos.collider != null)
                    {
                        //Check if player have atleast 3 cells to move
                        Vector3 distance = hitPos.collider.transform.position - position;
                        if (Mathf.Abs(distance.x) > 2 || Mathf.Abs(distance.z) > 2)
                        {
                            placeSelected = true;
                            break;
                        }
                    }
                }
            }
        }

        GameObject oldplayer = GameObject.FindGameObjectWithTag("Player");

        //Check if player don't die
        if (oldplayer == null)
        {
            //Place player
            player = Instantiate(playerPrefab, position, Quaternion.identity);
        }
        else
        {
            oldplayer.transform.position = position;
            player = oldplayer;
        }

    }

    public void spawnEnemies()
    {
        //Create enemies
        for (int i = 0; i < enemyNames.Length; i++)
        {
            if (enemies[i])
            {       
                //Add 1 enemy per level
                int addEnemies = gameController.gameLevel;

                for (int e = 1; e <= additionalEnemies[i] + addEnemies; e++)
                {
                    setEnemyPosition(i);
                }
            }

        }
    }

    public void setEnemyPosition(int indexEnemy)
    {
        if (player != null)
        {
            bool placeSelectedEnemy = false;
            int mapWidth = gameController.mapWidth;
            int mapHeight = gameController.mapHeight;

            Vector3 position = new Vector3(Random.Range(-mapWidth, mapWidth + 1), 0, Random.Range(-mapHeight, mapHeight + 1));

            //Check if cell empty or near player
            while (!placeSelectedEnemy)
            {
                position = new Vector3(Random.Range(-mapWidth, mapWidth + 1), 0, Random.Range(-mapHeight, mapHeight + 1));

                Ray ray = new Ray(new Vector3(position.x, 2, position.z), Vector3.down);
                Physics.Raycast(ray, out RaycastHit hit);

                //Check if current cell is empty
                if (hit.collider != null && hit.collider.gameObject.tag == "Ground" && hit.collider.gameObject.tag != "Enemy")
                {
                    //Check distence with player
                    Vector3 distance = hit.collider.transform.position - player.transform.position;
                    if (Mathf.Abs(distance.x) >= 1 || Mathf.Abs(distance.z) > 1)
                    {
                        placeSelectedEnemy = true;
                        break;
                    }
                }

            }

            //Place enemy
            var enemy = Instantiate(enemyNames[indexEnemy], position, Quaternion.identity);
            enemy.transform.parent = GameObject.Find("Enemies").gameObject.transform;
        }
    }

    public void portalSpawner()
    {
        bool placeSelected = false;

        int mapHeight = gameController.mapHeight;
        int mapWidth = gameController.mapWidth;

        Vector3 position = new Vector3(Random.Range(-mapWidth, mapWidth + 1), 0, Random.Range(-mapHeight, mapHeight + 1));

        while (!placeSelected)
        {
            position = new Vector3(Random.Range(-mapWidth, mapWidth + 1), 0, Random.Range(-mapHeight, mapHeight + 1));

            Ray ray = new Ray(new Vector3(position.x, 2, position.z), Vector3.down);
            Physics.Raycast(ray, out RaycastHit hit);

            //Check if current cell is empty
            if (hit.collider != null && hit.collider.gameObject.tag == "Ground")
            {
                placeSelected = true;
                break;
            }
        }

        //Place portal
        Instantiate(portal, position, Quaternion.identity);
    }
}
