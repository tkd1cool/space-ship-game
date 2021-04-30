using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    Color c;
    public float baseSpeed = 10;
    public float turnSpeed = 1;
    public float drag = 0.5f;
    float speed;
    Rigidbody2D rb;
    Vector2 closestPlayerPos;

    public GameObject bulletHitEffect;
    GameObject[] otherPlayers;

    public void Setup(int playerNumber, Vector2 playerVelocity, Vector2 playerForward, string playerName)
    {

        gameObject.layer = 11 + playerNumber;
        c = PlayerInfo.GetColor(playerNumber);
        GetComponent<SpriteRenderer>().color = c;
        rb = GetComponent<Rigidbody2D>();
        speed = (playerVelocity + baseSpeed * playerForward).magnitude;
        rb.velocity = playerVelocity + baseSpeed * playerForward;
        var players = GameObject.FindGameObjectsWithTag("Player");
        otherPlayers = new GameObject[players.Length - 1];
        int indexOffset = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].name.Equals(playerName))
            {
                indexOffset++;
                continue;
            }
            otherPlayers[i - indexOffset] = players[i];
        }
    }

    public void Update()
    {
        closestPlayerPos = Vector2.positiveInfinity;
        float closestPlayerDistance = Mathf.Infinity;
        for (int i = 0; i < otherPlayers.Length; i++)
        {
                float distance = Vector2.Distance(transform.position, otherPlayers[i].transform.position);
                if (distance < closestPlayerDistance)
                {
                    closestPlayerPos = otherPlayers[i].transform.position;
                    closestPlayerDistance = distance;
                }
        }
    }

    public void FixedUpdate()
    {
        Vector2 dir = closestPlayerPos - rb.position;
        dir.Normalize();
        float rotateAmount = Vector3.Cross(dir, transform.up).z;
        if (!float.IsNaN(rotateAmount))
        {
            rb.angularVelocity = -rotateAmount * turnSpeed;
        }
        rb.velocity = transform.up * speed;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);
        Quaternion rot = Quaternion.LookRotation(contact.normal, Vector3.up);
        GameObject effect = Instantiate(bulletHitEffect, contact.point, rot);
        var main = effect.GetComponent<ParticleSystem>().main;
        main.startColor = c;
        Destroy(gameObject);
    }
}
