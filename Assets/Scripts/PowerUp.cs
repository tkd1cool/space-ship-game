using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public SpriteRenderer sr;
    public int powerUp;
    int numberOfPowerUps = 5;
    public void randomize()
    {
        powerUp = Random.Range(1, numberOfPowerUps + 1);
        //homingBullets = 1
        //tripleShot = 2
        //laser = 3
        if (powerUp == 1)
        {
            sr.sprite = Resources.Load<Sprite>("PowerUps/HomingBullet");
        }
        else
        if (powerUp == 2)
        {
            sr.sprite = Resources.Load<Sprite>("PowerUps/TripleShot");
        }
        else
        if (powerUp == 3)
        {
            sr.sprite = Resources.Load<Sprite>("PowerUps/Laser");
        }
        else
        if (powerUp == 4)
        {
            sr.sprite = Resources.Load<Sprite>("PowerUps/BigBullet");
        }
        else
            if(powerUp == 5)
        {
            sr.sprite = Resources.Load<Sprite>("PowerUps/MiniBullet");
        }


    }
}
