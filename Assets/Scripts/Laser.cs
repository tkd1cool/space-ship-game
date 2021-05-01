using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    public Transform player;
    public LineRenderer lr;
    public int layerMask;

    GameObject hitObject;
    float timeHitObject;
    readonly float timeToDamage = 0.5f;

    RaycastHit2D ray;
    public GameObject laserHitEffect;
    GameObject hitEffect;

    public void Setup(int playerLayer, Transform player, int playerNumber)
    {
        this.player = player;

        List<string> layersList = new List<string>(5)
        {
            "Default"
        };
        for (int i = 1; i <= 4; i++)
        {
            string stringToAdd = "Player" + i.ToString();
            if (stringToAdd != LayerMask.LayerToName(playerLayer))
            {
                layersList.Add(stringToAdd);
            }
        }
        layerMask = LayerMask.GetMask(layersList.ToArray());
        //layerMask = 1;
        hitEffect = Instantiate(laserHitEffect);
        Color c = PlayerInfo.GetColor(playerNumber);
        var main = hitEffect.GetComponent<ParticleSystem>().main;
        main.startColor=c;
        var trails = hitEffect.GetComponent<ParticleSystem>().trails;
        trails.colorOverLifetime = c;
        trails.colorOverTrail = c;
        lr.startColor = c;
        lr.endColor = c;

    }

    // Update is called once per frame
    void Update()
    {

        lr.SetPosition(0, Vector2.MoveTowards(player.position,ray.point,0.22f));
        lr.SetPosition(1, ray.point);
        Quaternion rot = Quaternion.LookRotation(ray.normal, Vector3.up);
        hitEffect.transform.rotation = rot;
        hitEffect.transform.position = ray.point;
        
    }
    private void FixedUpdate()
    {
        
        ray = Physics2D.Raycast(player.position, player.up, 100f, layerMask);
        GameObject testObject = ray.collider.gameObject;
        if (testObject.CompareTag("Border"))
        {
            timeHitObject = 0;
            hitObject = null;
            return;
        }
        if (testObject == hitObject)
        {
            timeHitObject += Time.fixedDeltaTime;
            if(timeHitObject>timeToDamage)
            {
                DamageHitObject();
                timeHitObject = 0;
            }
        } else
        {
            timeHitObject = 0;
            hitObject = ray.collider.gameObject;
        }
    }

    private void DamageHitObject()
    {
        if(hitObject.CompareTag("Player"))
        {
            hitObject.GetComponent<PlayerController>().TakeDamage();
            return;
        }
        Destroy(hitObject);
    }
    private void OnDestroy()
    {
        Destroy(hitEffect);
    }
}
