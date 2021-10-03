using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkBuilder : MonoBehaviour
{
    public static LinkBuilder instance;

    [SerializeField] private int initLinkCount;
    [SerializeField] private GameObject linkElement;
    [SerializeField] private GameObject linkElementToShip, player, ship;

    private int linkElementCount;
    private GameObject lastLink;
    private float adjustedLength;

    void Awake()
    {
        if (LinkBuilder.instance == null)
            LinkBuilder.instance = this;
        else
            Destroy(gameObject);
        lastLink = linkElementToShip;

        AddLinks(initLinkCount);
    }

    private void Update()
    {
        if (PlayerBehaviour.instance.GetIsDead()) return;

        if (Vector2.Distance(player.transform.position, lastLink.transform.position) > 0.4f)
            player.transform.position = lastLink.transform.position + lastLink.transform.right * 0.4f;

        if (Vector2.Distance(ship.transform.position, linkElementToShip.transform.position) > 0.3f)
            ship.transform.position = linkElementToShip.transform.position + linkElementToShip.transform.right * -0.3f;
    }

    public void AddLink()
    {
        Vector3 spawnPos = lastLink.transform.position + lastLink.transform.right * 0.4f;
        GameObject newLink = Instantiate(linkElement, spawnPos, Quaternion.identity);
        lastLink.GetComponent<Joint2D>().connectedBody = newLink.GetComponent<Rigidbody2D>();
        newLink.GetComponent<Joint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
        player.transform.position = newLink.transform.position + newLink.transform.right * 0.4f;
        lastLink = newLink;
    }

    public void AddLinks(int count)
    {
        for (int i = 0; i < count; i++) AddLink();
    }
}