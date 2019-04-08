using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
    GameController gameController;

    public GameObject wall;
    public GameObject box;
    public GameObject ground;

    public NavMeshSurface[] surfaces;

    public float wallCreateChance = .15f;
    public float boxCreateChance = .2f;

    int mapWidth;
    int mapHeight;

    internal int mapUp;
    internal int mapDown;

    internal int mapRight;
    internal int mapLeft;

    void Start()
    {
        gameController = GameObject.Find("GameController").gameObject.GetComponent<GameController>();

        mapWidth = gameController.mapWidth;
        mapHeight = gameController.mapHeight;

        //Set Z borders
        mapUp = mapHeight - (int)Mathf.Floor(mapHeight / 2);
        mapRight = mapWidth - (int)Mathf.Floor(mapWidth / 2);

        //Set X borders
        mapDown = -(int)Mathf.Floor(mapHeight / 2);
        mapLeft = -(int)Mathf.Floor(mapWidth / 2);

        createWallsAndBoxes();

        //Build enemy paths on map
        foreach (NavMeshSurface surface in surfaces)
        {
            surface.BuildNavMesh();  
        }
    }

    void createWallsAndBoxes()
    {
        //Create ground and set position
        var floor = Instantiate(ground, new Vector3(0 + (mapWidth / 2f) + mapLeft, -1, 0 + (mapHeight / 2f) + mapDown), Quaternion.identity);

        //Set size of ground
        if (mapRight == -mapLeft)
        {
            if (mapUp == -mapDown)
            {
                floor.gameObject.transform.localScale = new Vector3(mapRight * 2 + 1, 1, mapUp * 2 + 1);
            }
            else
            {
                floor.gameObject.transform.localScale = new Vector3(mapRight * 2 + 1, 1, mapUp * 2);
            }
        }
        else
        {
            if (mapUp == -mapDown)
            {
                floor.gameObject.transform.localScale = new Vector3(mapRight * 2, 1, mapUp * 2 + 1);
            }
            else
            {
                floor.gameObject.transform.localScale = new Vector3(mapRight * 2, 1, mapUp * 2);
            }
        }

        //Set camera size
        Camera.main.gameObject.GetComponent<Camera>().orthographicSize = floor.gameObject.transform.localScale.z / 2 + 0.5f;

        //Create box, wall or nothing
        for (int x = mapLeft; x <= mapRight; x++)
        {
            for (int z = mapDown; z <= mapUp; z++)
            {
                float random = Random.value;

                if (random <= wallCreateChance)
                {
                    //Сheck for near walls
                    int wallsaround = 0;
                    for (int w = -1; w < 2; w++)
                    {
                        for (int h = -1; h < 2; h++)
                        {
                            RaycastHit hit;
                            Ray ray = new Ray(new Vector3(x + w, 2, z + h), Vector3.down);
                            Physics.Raycast(ray, out hit);

                            if (h == 0 && w == 0)
                            {
                                continue;
                            }
                            else if (hit.collider != null && hit.collider.gameObject.tag == "Wall")
                            {
                                wallsaround++;
                            }
                        }
                    }

                    //If player can pass thrue shape of walls
                    if (wallsaround < 4)
                    {
                        //Place wall
                        var walls = Instantiate(wall, new Vector3(x, 0, z), Quaternion.identity);
                        walls.transform.parent = GameObject.Find("Walls").gameObject.transform;
                    }
                }
                else if (random <= wallCreateChance + boxCreateChance)
                {
                    //Place box
                    var boxes = Instantiate(box, new Vector3(x, 0, z), Quaternion.identity);
                    boxes.transform.parent = GameObject.Find("Boxes").gameObject.transform;
                }

            }
        }
    }
    
    public void updatesurfeces()
    {
        foreach (NavMeshSurface surface in surfaces)
        {
            if(surface.navMeshData != null)
            {
                surface.UpdateNavMesh(surface.navMeshData);
            }
        }
    }

    public void newlevel()
    {
        GameObject walls = GameObject.Find("Walls");
        GameObject boxes = GameObject.Find("Boxes");

        for (int i = 0; i < walls.transform.childCount; i++)
        {
            Destroy(walls.transform.GetChild(i).gameObject);
        }

        for (int j = 0; j < boxes.transform.childCount; j++)
        {
            Destroy(boxes.transform.GetChild(j).gameObject);
        }

        createWallsAndBoxes();
        updatesurfeces();
    }
}
