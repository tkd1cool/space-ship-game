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
    float timeToDamage = 0.5f;

    RaycastHit2D ray;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Setup(int playerLayer, Transform player)
    {
        this.player = player;

        List<string> layersList = new List<string>(5);
        layersList.Add("Default");
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
    }

    // Update is called once per frame
    void Update()
    {

        lr.SetPosition(0, player.position);

        lr.SetPosition(1, ray.point);
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
            hitObject.GetComponent<PlayerController>().takeDamage();
            return;
        }
        Destroy(hitObject);
    }
}
