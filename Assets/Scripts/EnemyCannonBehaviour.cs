using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannonBehaviour : MonoBehaviour
{
    public float bulletVelocityMagnitude, fireInterval;
    public GameObject bulletPrefab, explosionParticles;
    public AnimationClip cannonFireClip, cannonSpawnClip, cannonDestroyClip;
    public float rotationSpeed;
    private GameObject player;
    private float t;
    private GameObject exitPoint;
    private Animator animator;
    private bool isDead;

    void Awake()
    {
        t = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        exitPoint = gameObject.transform.Find("ExitPoint").gameObject;
        // animator = gameObject.GetComponent<Animator>();
        StartCoroutine(FireCannon());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator FireCannon()
    {
        animator.Play("cannon_spawn");
        yield return new WaitForSeconds(cannonSpawnClip.length);
        while (true)
        {
            yield return new WaitForSeconds(fireInterval);
            animator.Play("cannon_fire");
            while (true)
            {
                t += Time.deltaTime;

                float x = player.transform.position.x - gameObject.transform.position.x;
                float y = player.transform.position.y - gameObject.transform.position.y;
                float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.deltaTime);

                if (t >= cannonFireClip.length)
                {
                    // AudioManager.instance.Play("EnemyCannonFire");
                    GameObject bullet = (GameObject) Instantiate(bulletPrefab, exitPoint.transform.position, Quaternion.Euler(0, 0, angle));
                    Vector3 bulletVelocity = new Vector3(x, y, 0);
                    bulletVelocity.Normalize();
                    bullet.GetComponent<Rigidbody2D>().velocity = bulletVelocity * bulletVelocityMagnitude;
                    t = 0;
                    break;
                }

                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void Die()
    {
        StartCoroutine(CannonDestroyCoroutine());
    }

    IEnumerator CannonDestroyCoroutine()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 position = new Vector3(transform.position.x + Random.Range(-5, 5), transform.position.y + Random.Range(-5, 5), 0);
            Instantiate(explosionParticles, position, Quaternion.identity);
        }
        // AudioManager.instance.Play("Explosion");
        animator.Play("cannon_destroy");
        yield return new WaitForSeconds(cannonDestroyClip.length);
        Destroy(gameObject);
    }
}