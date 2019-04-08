using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    void Start()
    {
        //Camera shake
        Camera.main.gameObject.GetComponent<Animator>().SetTrigger("Shake");

        //Self destroy
        Destroy(gameObject, 0.3f);
    }


    public void OnTriggerEnter(Collider collision)
    {
        //if box, then spawn power up
        if (collision.gameObject.GetComponent<Box>() != null)
        {
            //Fire dosn't kill power up
            GetComponent<SphereCollider>().enabled = false;    
            collision.gameObject.GetComponent<Box>().SpawnPowerUp();
        }
        //Don't destroy other fire
        else if (collision.gameObject.GetComponent<Fire>() != null)
        {
            return;
        }
        //Trigger other bomb
        else if (collision.gameObject.GetComponent<BombScript>() != null)
        {
            collision.gameObject.GetComponent<BombScript>().Explode();
        }
        else if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<PlayerController>().armor > 0)
        {
            return;
        }

        Destroy(collision.gameObject);
    }
}
