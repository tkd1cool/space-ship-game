using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int maxPlayerHealth = 7;
    public Vector2 secondsBetweenPowerupsMinMax;
    float secondsSinceLastPowerup = 0f;
    public float secondsToMaxSpawnRate;
    Vector2 botLeft;
    Vector2 topRight;

    public GameObject powerUp;

    private void Start()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerController>().setMaxHealth(maxPlayerHealth);
        }
        Camera cam = Camera.main;
        EdgeCollider2D poop = cam.transform.GetChild(0).gameObject.GetComponent<EdgeCollider2D>();
        botLeft = poop.points[1];
        topRight = poop.points[3];
    }
    private void Update()
    {
        secondsSinceLastPowerup += Time.deltaTime;
        if (Mathf.Lerp(secondsBetweenPowerupsMinMax.x, secondsBetweenPowerupsMinMax.y, Time.timeSinceLevelLoad / secondsToMaxSpawnRate) < secondsSinceLastPowerup)
        {
            Vector2 spawn;
            int counter = 0;
            do
            {
                spawn = MyUtils.Random(botLeft, topRight);
                counter++;
            }
            while (Physics.CheckBox(spawn, new Vector3(1, 1, 1)) && counter < 1000);
            GameObject PowerUp = Instantiate(powerUp, spawn, Quaternion.identity);
            secondsSinceLastPowerup = 0;
            PowerUp.GetComponent<PowerUp>().randomize();
        }
    }
}
