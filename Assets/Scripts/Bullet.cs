using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Color c;
    public float baseSpeed = 4;
    public GameObject bulletHitEffect;

    public void Setup(int playerNumber, Vector2 playerVelocity, Vector2 playerForward)
    {
        gameObject.layer = 11 + playerNumber;
        c = PlayerInfo.GetColor(playerNumber);
        GetComponent<SpriteRenderer>().color = c;
        GetComponent<Rigidbody2D>().velocity = playerVelocity + baseSpeed * playerForward;
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
