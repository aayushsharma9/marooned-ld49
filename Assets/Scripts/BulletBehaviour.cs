using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject bulletParticleSystem;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Instantiate(bulletParticleSystem, other.contacts[0].point, Quaternion.identity);
        Destroy(gameObject);
    }
}