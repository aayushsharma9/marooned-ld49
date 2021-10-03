using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private float levelIncrementInterval;
    private int levelCount;

    void Awake()
    {
        instance = this;
        Time.timeScale = 1;
        StartCoroutine(IncrementLevel());
    }

    IEnumerator IncrementLevel()
    {
        yield return new WaitForSeconds(levelIncrementInterval);
        levelCount++;
    }

    public int GetLevelCount()
    {
        return levelCount;
    }

    private void Update()
    {
        if (ShipBehaviour.instance.GetHealth() == 100 && ShipBehaviour.instance.GetFuel() == 100)
        {
            GUIManager.instance.Success();
        }
    }

    public void GameOver()
    {

    }

    public void Pause(bool pause)
    {
        if (pause) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    //TODO: Low oxygen emergency?
}