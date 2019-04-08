using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
    
public class BombSpawner : MonoBehaviour
{
    public GameObject bomb;
    UIController UIController;

    public int startFirePower = 1;
    public int startNumberOfBombs = 1;
    public int startMaximumBombs = 2;
    public float startFuse = 2;

    internal int firePower;
    internal int numberOfBombs;
    internal int maximumBombs;
    internal float fuse;

    void Start()
    {
        UIController = GameObject.Find("GameController").GetComponent<UIController>();
        setValues();
    }

    public void setValues()
    {
        firePower = startFirePower;
        numberOfBombs = startNumberOfBombs;
        maximumBombs = startMaximumBombs;
        fuse = startFuse;
    }

    void Update()
    {
        if (numberOfBombs > maximumBombs)
        {
            numberOfBombs = maximumBombs;
        }

        if (Input.GetButtonDown("Jump") && numberOfBombs >= 1 && UIController.GamePhase != "MainMenu")
        {
            Vector3 spawnPos = new Vector3(Mathf.Round(transform.position.x), 0, Mathf.Round(transform.position.z));
            var newBomb = Instantiate(bomb, spawnPos, Quaternion.identity) as GameObject;
            newBomb.GetComponent<BombScript>().firePower = firePower;
            newBomb.GetComponent<BombScript>().fuse = fuse;
            numberOfBombs--;
            Invoke("AddBomb", fuse);
        }
    }

    public void AddBomb()
    {
        //Prevent crating extrabomb after bonus
        CancelInvoke("AddBomb");

        if (numberOfBombs == 0)
        {
            numberOfBombs++;
        }
    }
}
