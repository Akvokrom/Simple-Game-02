using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    UIController UIController;
    Rigidbody rd;
    public float startSpeed;
    public float startArmor;

    internal float speed;
    internal float armor;

    bool isFalling;

    void Start()
    {
        rd = GetComponent<Rigidbody>();
        UIController = GameObject.Find("GameController").gameObject.GetComponent<UIController>();

        armor = startArmor;
        speed = startSpeed;
    }

    void Update()
    {
        //Check for game phase
        if (UIController.GamePhase == "Playing" || UIController.GamePhase == "Win")
        {
            if (!isFalling)
            {
                //Get input from player
                float x = Input.GetAxisRaw("Horizontal");
                float z = Input.GetAxisRaw("Vertical");

                //Prevent diagonal movement
                if (Math.Abs(x) >= Math.Abs(z))
                    z = 0;
                else if (Math.Abs(z) >= Math.Abs(x))
                    x = 0;

                //Calculate and set movement
                Vector3 movement = new Vector3(x, 0, z) * speed * Time.deltaTime;
                rd.velocity = movement;

                //Check for armor
                if (armor > 0)
                {
                    armor -= Time.deltaTime;
                }
                else
                {
                    armor = 0;
                }

            }
            else
            {
                //Player falls
                rd.velocity = Vector3.down * 10;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Player leave ground
        if(collision.gameObject.tag == "Ground")
        {
            isFalling = true;
            Destroy(gameObject, 1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Player lose if collision with enemy and don't have armor bonus
        if (collision.gameObject.tag == "Enemy" && armor <= 0)
        {
            Destroy(gameObject);
        }
        //Player collide with portal
        else if (collision.gameObject.tag == "Finish")
        {
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
            Destroy(collision.gameObject);
            UIController.newLevel();
            armor = startArmor;
            speed = startSpeed;
            GetComponent<BombSpawner>().setValues();
        }
    }

}
