using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] drops, particleSystems;
    [SerializeField] private int dropCount;
    [SerializeField] private float dropRadius;
    [SerializeField] private float minSpeed, maxSpeed;

    private Rigidbody2D debrisRigidbody;
    private Vector3 targetPos;

    void Start()
    {
        debrisRigidbody = gameObject.GetComponent<Rigidbody2D>();
        Vector3 dir = (Random.Range(0, 4) < 1 ? GameObject.FindGameObjectWithTag("Ship").transform.position : targetPos) - transform.position;
        dir.Normalize();
        debrisRigidbody.velocity = dir * Random.Range(minSpeed, maxSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioManager.instance.Play("rock_collision");
        DestroySelf(collision.contacts[0].point);
    }

    public void DestroySelf(Vector3 pos)
    {
        SpawnDrops(pos);
        foreach (GameObject particleSystem in particleSystems)
            Instantiate(particleSystem, pos, Quaternion.identity);
        Destroy(gameObject);
    }

    private void SpawnDrops(Vector3 pos)
    {
        for (int i = 0; i < dropCount; i++)
        {
            Vector3 dropPos = pos + new Vector3(Random.Range(-dropRadius, dropRadius), Random.Range(-dropRadius, dropRadius), 0);
            Instantiate(drops[Random.Range(0, drops.Length)], dropPos, Quaternion.identity);
        }
    }

    public void SetTargetPos(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }
}