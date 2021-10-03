using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBehaviour : MonoBehaviour
{
    private const float INITIAL_FUEL = 50;
    private const int INITIAL_HEALTH = 50;
    private const int INITIAL_BULLETS = 2;

    public static ShipBehaviour instance;

    [SerializeField] private int maxBullets;
    [SerializeField] private GameObject shipCollisionParticles, shipExplosionParticles;

    private int currentBullets;
    private float fuel;
    private int health;

    private void Awake()
    {
        instance = this;
        health = INITIAL_HEALTH;
        fuel = INITIAL_FUEL;
        currentBullets = INITIAL_BULLETS;
    }

    void Update()
    {

    }

    public void DamageShip()
    {
        int damageType = Random.Range(0, 3);

        switch (damageType)
        {
            case 0:
                //Thrusters
                Thrusters.instance.SetControllable(false);
                GUIManager.instance.SetNotification("thrusters unstable");
                break;
            case 1:
                //Steers
                Thrusters.instance.SetSteerable(false);
                GUIManager.instance.SetNotification("rotors jammed");
                break;
            case 2:
                //Cannons
                gameObject.GetComponent<ShipCannon>().enabled = false;
                GUIManager.instance.SetNotification("cannons disabled");
                break;
        }
    }

    public bool RepairShip()
    {
        //TODO: Option to repair parts
        if (PlayerBehaviour.instance.GetScrapCollected() >= 10)
        {
            Thrusters.instance.SetControllable(true);
            Thrusters.instance.SetSteerable(true);
            gameObject.GetComponent<ShipCannon>().enabled = true;
            GUIManager.instance.DisableNotifications();
            Debug.Log("Ship repaired");
            AudioManager.instance.Play("repaired");
            return true;
        }
        Debug.Log("Not enough scrap");

        return false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Hazard":
                AudioManager.instance.Play("ship_bump");
                Camera.main.GetComponent<CameraShake>().enabled = true;
                Instantiate(shipCollisionParticles, other.contacts[0].point, Quaternion.identity);
                OffsetHealth(-10);
                if (health < 50)
                {
                    DamageShip();
                }
                break;
        }
    }

    public float GetFuel()
    {
        return fuel;
    }

    public int GetMaxBullets()
    {
        return maxBullets;
    }

    public int GetCurrentBullets()
    {
        return currentBullets;
    }

    public int GetHealth()
    {
        return health;
    }

    public void OffsetFuel(float amount)
    {
        fuel += amount;
        if (fuel >= 100) fuel = 100;
        if (fuel <= 0) fuel = 0;
    }

    public void OffsetBullets(int num)
    {
        currentBullets += num;
        if (currentBullets >= maxBullets) currentBullets = maxBullets;
        if (currentBullets <= 0) currentBullets = 0;
    }

    public void OffsetHealth(int num)
    {
        health += num;
        if (health >= 100) health = 100;
        if (health <= 0)
        {
            health = 0;
            GUIManager.instance.GameOver("your ship was destroyed");
        }
    }
}