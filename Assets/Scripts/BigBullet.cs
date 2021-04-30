using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBullet : MonoBehaviour
{
    Color c;
    public float baseSpeed = 2.5f;
    public GameObject bigBulletHitEffect;
    public int health = 5;
    public float explosionRadius = 0.001f;

    public void Setup(int playerNumber, Vector2 playerVelocity, Vector2 playerForward)
    {
        gameObject.layer = 11 + playerNumber;
        c = PlayerInfo.GetColor(playerNumber);
        GetComponent<SpriteRenderer>().color = c;
        GetComponent<Rigidbody2D>().velocity = playerVelocity + baseSpeed * playerForward;
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach(Collider2D collider in colliders)
        {
            if(collider.CompareTag("MapTile"))
            {
                Destroy(collider.gameObject);
            }
        }

            ContactPoint2D contact = collision.GetContact(0);
            Quaternion rot = Quaternion.LookRotation(contact.normal, Vector3.up);
            GameObject effect = Instantiate(bigBulletHitEffect, contact.point, rot);
            var main = effect.GetComponent<ParticleSystem>().main;
            main.startColor = c;
            Destroy(gameObject);
    }
}
