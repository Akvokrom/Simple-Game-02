using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
    public int bombs;
    public int firePower;
    public int speed;
    public float armor;

    public GameObject powerUpText;
    GameController gameController;

    void Start()
    {
        gameController = GameObject.Find("GameController").gameObject.GetComponent<GameController>();
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            BombSpawner bombSpawner = collision.gameObject.GetComponent<BombSpawner>();

            //Add power up to player
            playerController.speed += speed;
            playerController.armor += armor;
            bombSpawner.numberOfBombs += bombs;
            bombSpawner.firePower += firePower;

            GameObject powerUpTextNew = Instantiate(powerUpText, transform.position, Quaternion.identity);

            //Set power up text
            string powerUpString = "";

            if (speed != 0)
            {
                powerUpString = "+" + speed.ToString() + " speed";
            }
            else if (armor != 0)
            {
                powerUpString = "+" + armor.ToString() + " armor";
            }
            else if (bombs != 0)
            {
                powerUpString = "+" + bombs.ToString() + " bomb";
            }
            else if (firePower != 0)
            {
                powerUpString = "+" + firePower.ToString() + " fire power";
            }

            powerUpTextNew.GetComponentInChildren<TextMesh>().text = powerUpString;

            Destroy(powerUpTextNew, 1);

            Destroy(gameObject);
        }
    }
}
