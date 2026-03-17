using UnityEngine;
using UnityEngine.InputSystem;

public class Airplane : MonoBehaviour
{
    [Header("Engine")]
    public float thrust = 40f; // ????????????????????

    [Header("Aerodynamics")]
    public float liftCoefficient = 0.02f; // ????????????? Lift
    public float dragCoefficient = 0.02f; // ????????????
    public float sideDrag = 2f;           // ????????????????

    [Header("BANK TURN")]
    public float turnStrength = 0.5f; // ?????????????????????????????

    [Header("STALL")]
    public float stallAngle = 35f;        // ??????????? stall
    public float stallLiftMultiplier = 0.3f; // ?? lift ????? stall

    [Header("Control")]
    public float pitchPower = 15f; // ???????
    public float rollPower = 20f;  // ?????
    public float yawPower = 15f;   // ?????? turn

    Rigidbody rb;
    bool engineOn = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // ?????????????????????
        // rb.centerOfMass = new Vector3(0, -0.5f, 0);
        rb.centerOfMass = new Vector3(0, -0.6f, -0.2f);
    }

    void FixedUpdate()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        // -------- THRUST --------
        if (kb.spaceKey.isPressed)
        {
            engineOn = true;

            // ????????????????
            rb.AddRelativeForce(Vector3.forward * thrust, ForceMode.Acceleration);
        }

        // --- SPEED - ???????????????????? -------
        float forwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);

        // --- LIFT - Lift depends on forward speed -
        // The faster the plane moves ? the stronger the lift.
        if (engineOn && forwardSpeed > 5f)
        {
            // Lift ? velocity²
            float lift = forwardSpeed * forwardSpeed * liftCoefficient;

            // ??????????????????????????? stall
            float pitchAngle = Vector3.Angle(transform.forward,
                                             Vector3.ProjectOnPlane(transform.forward,
                                             Vector3.up));

            if (pitchAngle > stallAngle)
            {
                // ?? lift ????? stall
                lift *= stallLiftMultiplier;
            }

            // ???????? Upward force lifts the airplane.
            rb.AddForce(transform.up * lift, ForceMode.Acceleration);

            // ???? vector ??? Lift
            Debug.DrawRay(transform.position, transform.up * 5f, Color.green);
        }

        // --- DRAG (Air Resistance) -------
        Vector3 drag = -rb.linearVelocity * dragCoefficient;
        rb.AddForce(drag);

        // -------- SIDE DRAG (?? drift ???????) --------
        Vector3 sideVel = Vector3.Dot(rb.linearVelocity, transform.right) * transform.right;
        rb.AddForce(-sideVel * sideDrag);

        // --- CONTROL Input --------
        float pitch = 0;
        float roll = 0;
        float yaw = 0;

        if (kb.sKey.isPressed) pitch = 1;
        if (kb.wKey.isPressed) pitch = -1;

        if (kb.aKey.isPressed) roll = 1;
        if (kb.dKey.isPressed) roll = -1;

        if (kb.qKey.isPressed) yaw = -1;
        if (kb.eKey.isPressed) yaw = 1;

        // --- TORQUE CONTROL --------
        rb.AddRelativeTorque(new Vector3(pitch * pitchPower, yaw * yawPower, -roll * rollPower));

        // -------- BANKED TURN --------
        // ??????????????????????? Lift ??????? ?????????????????????
        float bankAmount = Vector3.Dot(transform.right, Vector3.up);

        rb.AddForce(transform.right * bankAmount * forwardSpeed * turnStrength);

        // ??????? forward
        Debug.DrawRay(transform.position, transform.forward * 5f, Color.blue);
    }
}
