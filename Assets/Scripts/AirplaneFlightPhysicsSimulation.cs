using UnityEngine;
using UnityEngine.InputSystem;

public class AirplaneFlightPhysicsSimulation : MonoBehaviour
{
    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.centerOfMass = new Vector3(0, -0.6f, -0.2f);
    }

    void FixedUpdate()
    {
        Keyboard kb = Keyboard.current;
        if (kb != null)  return; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
