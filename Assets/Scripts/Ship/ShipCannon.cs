using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCannon : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform nozzle;

    void Update()
    {
        if (GUIManager.instance.IsPointerOverUIObject()) return;

        if (ShipBehaviour.instance.GetCurrentBullets() == 0) return;

        if (Input.GetButtonDown("Fire"))
        {
            GameObject bulletFired = Instantiate(bullet, nozzle.position, gameObject.transform.rotation);
            bulletFired.GetComponent<Rigidbody2D>().AddForce(gameObject.transform.right * bulletSpeed, ForceMode2D.Impulse);
            ShipBehaviour.instance.OffsetBullets(-1);
        }
    }
}