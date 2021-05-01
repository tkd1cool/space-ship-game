using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float thrustForce = 100f;
    public float turnSpeed = 100f;
    public float timeBetweenBullets = 1f;
    public float timeBetweenBigBullets = 2f;
    public float timeBetweenMiniBullets = 0.75f;
    public float recoilForce = 10f;
    public float bulletSpread = 30f;
    public int maxHealth;
    public int health;
    public bool dead = false;

    public Rigidbody2D rb;
    public GameObject bullet;
    public GameObject bigBullet;
    public GameObject miniBullet;
    public GameObject homingBullet;
    public GameObject LaserPrefab;
    public GameObject playerDeathEffect;
    public ParticleSystem rocketThrustEffect;

    int mode = 0;
    //ParticleSystem.EmissionModule emissionModule;
    GameObject healthUI;
    Text healthText;
    Transform healthBar;
    GameObject deathText;
    GameObject laser;

    string playerName;
    public int playerNumber;

    // Start is called before the first frame update
    private void Start()
    {
        playerName = gameObject.name;
        string temp = playerName.Replace("Player", "");
        playerNumber = Convert.ToInt32(temp);
        gameObject.layer = playerNumber + 7;
        InvokeRepeating(nameof(CreateBullet), 1, timeBetweenBullets);
        healthUI = FindObjectOfType<Canvas>().transform.GetChild(0).GetChild(playerNumber).gameObject;
        healthUI.SetActive(true);
        healthText = healthUI.transform.GetChild(2).gameObject.GetComponent<Text>();
        healthBar = healthUI.transform.GetChild(0).GetChild(1).transform;
        deathText = healthUI.transform.GetChild(3).gameObject;
        var playerSprite = Resources.Load<Sprite>("Players/" + playerName);
        GetComponent<SpriteRenderer>().sprite = playerSprite;
    }

    private void FixedUpdate()
    {
        float turnInput = Input.GetAxis(playerName + "Turn");
        rb.MoveRotation(rb.rotation + turnInput * turnSpeed * Time.fixedDeltaTime);
        float thrustInput = Input.GetAxisRaw(playerName + "Thrust");
        rb.AddForce(transform.up * thrustInput * thrustForce * Time.fixedDeltaTime);
        if (thrustInput < 0.5)
        {
            rocketThrustEffect.Play();
        }

    }
    private void CreateBigBullet()
    {
        //create and setup bullet
        GameObject instantiatedBullet;
        instantiatedBullet = Instantiate(bigBullet, transform.position, transform.rotation);
        instantiatedBullet.GetComponent<BigBullet>().Setup(playerNumber, rb.velocity, transform.up);
        //recoil
        rb.AddForce(-transform.up * recoilForce * 2);
    }
    private void CreateMiniBullet()
    {
        //create and setup bullet
        GameObject instantiatedBullet;
        instantiatedBullet = Instantiate(miniBullet, transform.position, transform.rotation);
        instantiatedBullet.GetComponent<MiniBullet>().Setup(playerNumber, rb.velocity, transform.up);
        //recoil
        rb.AddForce(-transform.up * recoilForce*0.5f);
    }
    private void CreateLaser()
    {
        laser = Instantiate(LaserPrefab);
        laser.GetComponent<Laser>().Setup(playerNumber + 7, transform, playerNumber);
        laser.tag = playerName + "Laser";
    }
    private void CreateBullet()
    {
        //create and setup bullet
        GameObject instantiatedBullet;
        instantiatedBullet = Instantiate(bullet, transform.position, transform.rotation);
        instantiatedBullet.GetComponent<Bullet>().Setup(playerNumber, rb.velocity, transform.up);
        //recoil
        rb.AddForce(-transform.up * recoilForce);
    }

    private void CreateTripleShot()
    {
        GameObject instantiatedBullet1;

        instantiatedBullet1 = Instantiate(bullet, transform.position, transform.rotation);
        instantiatedBullet1.GetComponent<Bullet>().Setup(playerNumber, rb.velocity, transform.up);


        Quaternion a = Quaternion.AngleAxis(bulletSpread, Vector3.forward) * transform.rotation;

        GameObject instantiatedBullet2;
        instantiatedBullet2 = Instantiate(bullet, transform.position, a);
        instantiatedBullet2.GetComponent<Bullet>().Setup(playerNumber, rb.velocity, a * Vector3.up);

        a = Quaternion.AngleAxis(-bulletSpread, Vector3.forward) * transform.rotation;

        GameObject instantiatedBullet3;
        instantiatedBullet3 = Instantiate(bullet, transform.position, a);
        instantiatedBullet3.GetComponent<Bullet>().Setup(playerNumber, rb.velocity, a * Vector3.up);
        //recoil
        rb.AddForce(-transform.up * recoilForce * 2.5f);
    }

    private void CreateHomingBullet()
    {
        //create and setup bullet
        GameObject instantiatedHomingBullet;
        instantiatedHomingBullet = Instantiate(homingBullet, transform.position, transform.rotation);
        instantiatedHomingBullet.GetComponent<HomingBullet>().Setup(playerNumber, rb.velocity, transform.up, playerName);
        //recoil
        rb.AddForce(-transform.up * recoilForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("BigBullet"))
        {
            Take2Damage();
        }

        if (!collision.collider.CompareTag("Bullet"))
        {
            return;
        }

        TakeDamage();

    }
    public void TakeDamage()
    {
        health--;
        healthText.text = health.ToString() + "/" + maxHealth.ToString();
        healthBar.localScale = new Vector3((float)health / maxHealth, 1f, 1f);
        if (health > 0 || dead)
        {
            return;
        }
        dead = true;
        OnDeath();
        return;
    }
    public void Take2Damage()
    {
        health -= 2;
        healthText.text = health.ToString() + "/" + maxHealth.ToString();
        healthBar.localScale = new Vector3((float)health / maxHealth, 1f, 1f);
        if (health > 0 || dead)
        {
            return;
        }
        dead = true;
        OnDeath();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        mode = collision.gameObject.GetComponent<PowerUp>().powerUp;
        OnPowerUp();
        Destroy(collision.gameObject);
    }

    public void SetMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
    }

    void OnPowerUp()
    {
        CancelInvoke();

        if (mode == 1)
        {
            Destroy(GameObject.FindWithTag(playerName + "Laser"));
            InvokeRepeating(nameof(CreateHomingBullet), 1, timeBetweenBullets);
            Invoke(nameof(EndPowerUp), 15);
        }
        if (mode == 2)
        {
            Destroy(GameObject.FindWithTag(playerName + "Laser"));
            InvokeRepeating(nameof(CreateTripleShot), 1, timeBetweenBullets);
            Invoke(nameof(EndPowerUp), 15);
        }
        if (mode == 3)
        {
            if (GameObject.FindWithTag(playerName + "Laser") == null)
            {
                CreateLaser();
            }
            Invoke(nameof(EndPowerUp), 15);
        }
        if (mode == 4)
        {
            Destroy(GameObject.FindWithTag(playerName + "Laser"));
            InvokeRepeating(nameof(CreateBigBullet), 1, timeBetweenBigBullets);
            Invoke(nameof(EndPowerUp), 15);
        }
        if (mode == 5)
        {
            Destroy(GameObject.FindWithTag(playerName + "Laser"));
            InvokeRepeating(nameof(CreateMiniBullet), 1, timeBetweenMiniBullets);
            Invoke(nameof(EndPowerUp), 15);
        }
    }

    void EndPowerUp()
    {
        CancelInvoke();
        Destroy(GameObject.FindWithTag(playerName + "Laser"));
        InvokeRepeating(nameof(CreateBullet), 1, timeBetweenBullets);
    }

    void OnDeath()
    {
        Destroy(GameObject.FindWithTag(playerName + "Laser"));
        GameObject effect = Instantiate(playerDeathEffect, transform.position, Quaternion.identity);
        var main = effect.GetComponent<ParticleSystem>().main;
        main.startColor = PlayerInfo.GetColor(playerNumber);
        deathText.SetActive(true);
        healthBar.gameObject.SetActive(false);
        healthText.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
