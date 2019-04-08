using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    MapGenerator MapGenerator;
    UIController UIController;
    public GameObject[] powerUps;

    void Start()
    {
        MapGenerator = GameObject.Find("GameController").gameObject.GetComponent<MapGenerator>();
        UIController = GameObject.Find("GameController").gameObject.GetComponent<UIController>();
    }

    public void SpawnPowerUp()
    {
        //Spawn power up chance
        int randomBonus = Random.Range(0, powerUps.Length * 2 + PlayerPrefs.GetInt("Level"));

        if (randomBonus < powerUps.Length)
        {
            Instantiate(powerUps[randomBonus], transform.position, Quaternion.identity);
        }
    }

    private void OnDestroy()
    {
        //Update paths after destroy box
        if(UIController.GamePhase == "Playng")
        {
            MapGenerator.surfaces[1].UpdateNavMesh(MapGenerator.surfaces[1].navMeshData);
        } 
    }
}
