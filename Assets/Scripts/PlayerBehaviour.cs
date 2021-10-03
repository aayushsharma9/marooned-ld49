using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public static PlayerBehaviour instance;

    [SerializeField] private int linkIncrementStep;

    private int scrapCollected;
    private bool canRepair, isDead;

    private void Awake()
    {
        instance = this;

        canRepair = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "LinkPickup":
                AudioManager.instance.Play("pickup");
                LinkBuilder.instance.AddLinks(linkIncrementStep);
                Destroy(other.gameObject);
                break;

            case "FuelPickup":
                AudioManager.instance.Play("pickup");
                ShipBehaviour.instance.OffsetFuel(5);
                Destroy(other.gameObject);
                break;

            case "ScrapPickup":
                AudioManager.instance.Play("pickup");
                OffsetScrapCollected(5); //TODO: Determine scrap value
                Destroy(other.gameObject);
                break;

            case "AmmoPickup":
                AudioManager.instance.Play("pickup");
                ShipBehaviour.instance.OffsetBullets(1);
                Destroy(other.gameObject);
                break;

            case "Ship":
                if (PlayerBehaviour.instance.GetScrapCollected() >= 10)
                    GUIManager.instance.EnableRepairPrompt(true);
                canRepair = true;
                break;

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Bullet":
                GUIManager.instance.GameOver("you killed your dumb self");
                StartCoroutine(Die());
                break;

            case "Hazard":
                GUIManager.instance.GameOver("you died");
                StartCoroutine(Die());
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Ship":
                GUIManager.instance.EnableRepairPrompt(false);
                break;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && canRepair)
        {
            if (ShipBehaviour.instance.RepairShip() == false) return;
            ShipBehaviour.instance.OffsetHealth(scrapCollected / 5);
            OffsetScrapCollected(-5 * (100 - ShipBehaviour.instance.GetHealth()));

            //TODO: Add health restored animation
        }
    }

    IEnumerator Die()
    {
        //TODO: Player Death Animation
        isDead = true;
        yield return new WaitForSeconds(0);
        Destroy(gameObject);
    }

    public int GetScrapCollected()
    {
        return scrapCollected;
    }

    public void OffsetScrapCollected(int num)
    {
        scrapCollected += num;
        if (scrapCollected <= 0) scrapCollected = 0;
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public void SetIsDead(bool isDead)
    {
        this.isDead = isDead;
    }
}