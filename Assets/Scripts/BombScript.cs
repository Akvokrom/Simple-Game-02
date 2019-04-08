using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    GameController gameController;
    MapGenerator MapGenerator;
    Vector3[] directions = new Vector3[] { Vector3.up, Vector3.down, Vector3.left, Vector3.right };
    public GameObject fire;
    internal int firePower;
    internal float fuse;

    void Start()
    {
        MapGenerator = GameObject.Find("GameController").gameObject.GetComponent<MapGenerator>();
        MapGenerator.updatesurfeces();

        Invoke("Explode", fuse);
        gameController = GameObject.Find("GameController").GetComponent<GameController>();

    }

    public void Explode()
    {
        //Prevent double explode
        CancelInvoke("Explode");

        //Create center fire
        Instantiate(fire, transform.position, Quaternion.identity);

        //Create upper fire
        for (int z = 1; z <= firePower; z++)
        {
            Ray ray = new Ray(new Vector3(transform.position.x, 2, transform.position.z + z), Vector3.down);
            Physics.Raycast(ray, out RaycastHit hit);

            if (hit.collider != null && hit.collider.gameObject.tag != "Wall")
            {
                Instantiate(fire, new Vector3(transform.position.x, 0, transform.position.z + z), Quaternion.identity);

                if(hit.collider.gameObject.tag != "Ground")
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        //Create lower fire
        for (int z = -1; z >= -firePower; z--)
        {
            Ray ray = new Ray(new Vector3(transform.position.x, 2, transform.position.z + z), Vector3.down);
            Physics.Raycast(ray, out RaycastHit hit);

            if (hit.collider != null && hit.collider.gameObject.tag != "Wall")
            {
                Instantiate(fire, new Vector3(transform.position.x, 0, transform.position.z + z), Quaternion.identity);

                if (hit.collider.gameObject.tag != "Ground")
                {
                    break;
                }
            }
            else 
            {
                break;
            }

        }

        //Create right fire
        for (int x = 1; x <= firePower; x++)
        {
            Ray ray = new Ray(new Vector3(transform.position.x + x, 2, transform.position.z), Vector3.down);
            Physics.Raycast(ray, out RaycastHit hit);

            if (hit.collider != null && hit.collider.gameObject.tag != "Wall")
            {
                Instantiate(fire, new Vector3(transform.position.x + x, 0, transform.position.z), Quaternion.identity);

                if (hit.collider.gameObject.tag != "Ground")
                {
                    break;
                }
            }
            else 
            {
                break;
            }
        }

        //Create left fire
        for (int x = -1; x >= -firePower; x--)
        {
            Ray ray = new Ray(new Vector3(transform.position.x + x, 2, transform.position.z), Vector3.down);
            Physics.Raycast(ray, out RaycastHit hit);

            if (hit.collider != null && hit.collider.gameObject.tag != "Wall")
            {
                Instantiate(fire, new Vector3(transform.position.x + x, 0, transform.position.z), Quaternion.identity);

                if (hit.collider.gameObject.tag != "Ground")
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        //Destroy bomb
        Destroy(gameObject);

        //Update paths
        MapGenerator.surfaces[1].UpdateNavMesh(MapGenerator.surfaces[1].navMeshData);
        
    }

    private void OnTriggerExit(Collider collision)
    {
        //Return collision if player leaves
        GetComponent<SphereCollider>().isTrigger = false;
    }
}
