using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public static GUIManager instance;

    [SerializeField] private TextMeshProUGUI healthText, scrapText, ammoText, fuelText;
    [SerializeField] private TextMeshProUGUI healthNotifText, fuelNotifText;
    [SerializeField] private TextMeshProUGUI gameOverTitle;
    [SerializeField] private TextMeshProUGUI[] notifications;
    [SerializeField] private GameObject repairPrompt;
    [SerializeField] private GameObject GameOverPanel, successPanel;
    private GameObject healthNotifObject, fuelNotifObject;

    void Awake()
    {
        instance = this;

        healthNotifObject = healthNotifText.transform.parent.gameObject;
        fuelNotifObject = fuelNotifText.transform.parent.gameObject;
        repairPrompt.SetActive(false);
        GameOverPanel.SetActive(false);
        successPanel.SetActive(false);

        for (int i = 0; i < notifications.Length; i++)
        {
            notifications[i].transform.parent.gameObject.SetActive(false);
        }
    }

    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    void Update()
    {
        healthText.text = ShipBehaviour.instance.GetHealth() + "";
        if (ShipBehaviour.instance.GetHealth() < 15)
        {
            healthNotifObject.SetActive(true);
            healthNotifText.text = "critical";
        }
        else
        {
            healthNotifObject.SetActive(false);
        }

        fuelText.text = ShipBehaviour.instance.GetFuel().ToString("F2");
        if (ShipBehaviour.instance.GetFuel() == 0)
        {
            fuelNotifObject.SetActive(true);
            fuelNotifText.text = "empty";
        }
        else if (ShipBehaviour.instance.GetFuel() <= 25)
        {
            fuelNotifObject.SetActive(true);
            fuelNotifText.text = "low";
        }
        else
        {
            fuelNotifObject.SetActive(false);
        }

        scrapText.text = PlayerBehaviour.instance.GetScrapCollected() + "";
        ammoText.text = ShipBehaviour.instance.GetCurrentBullets() + "/" + ShipBehaviour.instance.GetMaxBullets();
    }

    public void SetNotification(string text)
    {
        bool matchFound = false;
        for (int i = 0; i < notifications.Length; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                Debug.Log("Checking argument: " + text + " and notif text " + notifications[j].text);
                Debug.Log("Result: " + notifications[j].text.Equals(text));
                if (notifications[j].text.Equals(text))
                {
                    matchFound = true;
                    break;
                }
            }
            if (matchFound) break;
            if (notifications[i].transform.parent.gameObject.activeSelf) continue;

            notifications[i].transform.parent.gameObject.SetActive(true);
            notifications[i].text = text;
            break;
        }
    }

    public void DisableNotifications()
    {
        for (int i = 0; i < notifications.Length; i++)
        {
            notifications[i].transform.parent.gameObject.SetActive(false);
        }
    }

    public void EnableRepairPrompt(bool enable)
    {
        repairPrompt.SetActive(enable);
    }

    public void SceneLoad(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void GameOver(string title)
    {
        gameOverTitle.text = title;
        Time.timeScale = 0;
        GameOverPanel.SetActive(true);
    }

    public void Success()
    {
        successPanel.SetActive(true);
        Time.timeScale = 0;
    }

}