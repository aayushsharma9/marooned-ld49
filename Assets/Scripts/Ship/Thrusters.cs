using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrusters : MonoBehaviour
{
    public static Thrusters instance;
    [SerializeField] private float boundsThreshold;
    [SerializeField] private float acceleration;
    [SerializeField] private float fuelConsumptionRate;
    [SerializeField] private float autoRotSpeed;
    [SerializeField] private bool controllable, steerable;
    private float force;
    private Rigidbody2D shipRigidbody;
    private Vector3 targetPosition;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        shipRigidbody = gameObject.GetComponent<Rigidbody2D>();
        force = acceleration * shipRigidbody.mass;
        controllable = true;
    }

    void Update()
    {
        if (GUIManager.instance.IsPointerOverUIObject()) return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        targetPosition = Camera.main.ScreenToWorldPoint(mousePos);

        if (steerable && Vector3.Distance(targetPosition, transform.position) >= 4)
        {
            float x = targetPosition.x - transform.position.x;
            float y = targetPosition.y - transform.position.y;
            float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            transform.Rotate(0, 0, autoRotSpeed * Time.deltaTime);
        }

        if (!controllable)
        {
            // shipRigidbody.AddForce(new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized * force);
            shipRigidbody.AddForce(gameObject.transform.right * force);
            // AvoidEdges();
            return;
        }

        if (Input.GetButton("Thrust") && ShipBehaviour.instance.GetFuel() > 0)
        {
            shipRigidbody.AddForce(gameObject.transform.right * force);
            ShipBehaviour.instance.OffsetFuel(-fuelConsumptionRate * Time.deltaTime);
        }
    }

    private void AvoidEdges()
    {
        if (Mathf.Abs(transform.position.x - CameraBounds.instance.leftEdge) <= boundsThreshold)
        {
            shipRigidbody.AddForce(Vector2.right * force);
        }
        if (Mathf.Abs(transform.position.x - CameraBounds.instance.rightEdge) <= boundsThreshold)
        {
            shipRigidbody.AddForce(Vector2.left * force);
        }
        if (Mathf.Abs(transform.position.y - CameraBounds.instance.topEdge) <= boundsThreshold)
        {
            shipRigidbody.AddForce(Vector2.down * force);
        }
        if (Mathf.Abs(transform.position.y - CameraBounds.instance.bottomEdge) <= boundsThreshold)
        {
            shipRigidbody.AddForce(Vector2.up * force);
        }
    }

    public void SetControllable(bool controllable)
    {
        this.controllable = controllable;
    }

    public void SetSteerable(bool steerable)
    {
        this.steerable = steerable;
    }
}