////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////// - CameraCar Script - Version 1.0.190920 - Created by Maloke Games 2019 - Visit us here: https://maloke.itch.io/ 
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////
////// This script is a very basic camera movement that emulates a Car movement to demonstrate how to work with the AutomotiveGUI.
////// It does not contains proper collision nor physics but feel free to modify and use it as you wish!
//////
////// Controls: W-S (Accelerate/Brake or Reverse), A-D (Turn), Space (Reset Attitude) and LeftShift (Faster Turn/Acceleration).
//////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

public class CameraCar : MonoBehaviour
{
    //References
    public Collider carCollider;
    public Rigidbody rigidBody;

    //Behavior Configuration
    [Space]
    public float acceleration = 0.005f;
    public float maxSpeed = 10, turningSpeed = 10, reverseFactor = 0.5f;
    //

    //Current Values
    [Space]
    public bool isGrounded, haveFuel = true;
    public bool reverse;
    public float engineInput, turnInput, speed;
    //


    ////// Initialization
    void Awake()
    {
        if (carCollider == null) carCollider = GetComponent<Collider>();
        if (  rigidBody == null)   rigidBody = GetComponent<Rigidbody>();
    }
    //////


    ////////////// Read Input and Calculations
    void Update()
    {
        // Verify if Car is Grounded
        RaycastHit hit;
        Physics.Raycast(transform.position, -transform.up, out hit, carCollider.bounds.extents.y + 0.1f /* 0.5f*/);
        if (hit.collider != null && hit.collider.gameObject != this) isGrounded = true; else isGrounded = false;
        //if (hit.collider != null) print(hit.collider.name);
        //

        //Reset Position
        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButtonDown(1)) transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        //

        if (AutomotiveGUI.current == null) haveFuel = true;
        else if (AutomotiveGUI.current != null)
        {
            if (AutomotiveGUI.current.fuelTarget == 0) haveFuel = false; else haveFuel = true;
        } 

        /////// Read Keyboard Input
        if (isGrounded)
        {
            //Basic Mode Input
            //engineInput = (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S) ? -3 : 0);
            //turnInput = (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0);
            //
            

            //Set Reverse Mode or Normal Drive
            if (!reverse && Input.GetKeyDown(KeyCode.S) && speed < 0.005f && haveFuel) reverse = true;
            else if (reverse && Input.GetKeyUp(KeyCode.S) ) reverse = false;
            //

            //Set Input Value
            turnInput =( (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0) )* ((Input.GetKey(KeyCode.LeftShift)) ? 3 : 1);
            if (!reverse)
            {
                if (speed >= 0.005f) engineInput = (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S) ? -3 : 0);
                else engineInput = (Input.GetKey(KeyCode.W) ? 3 : 0);
            }
            else engineInput = (Input.GetKey(KeyCode.W) ? 3 : 0) + (Input.GetKey(KeyCode.S) ? -Mathf.Abs(reverseFactor) : 0);
            //

            //No fuel
            if (!haveFuel) engineInput = Mathf.Clamp(engineInput, -3, 0);
        }
        else
        {
            engineInput = 0;
            turnInput = 0;
        }
        ///////


        //Speed Acceleration
        speed = Mathf.Lerp(speed, engineInput, acceleration * ((Input.GetKey(KeyCode.LeftShift))? 3 : 1));
        //

    }
    //////////////


    //////////////// Makes Camera Movement
    void FixedUpdate()
    {
        transform.Translate(new Vector3(0, 0, Mathf.Clamp(speed * maxSpeed * Time.fixedDeltaTime, -maxSpeed, maxSpeed)));
        transform.Rotate(new Vector3(0, (speed > 0.005f || speed < -0.005f) ? turnInput * turningSpeed * Time.fixedDeltaTime : 0, 0));
    }
    ////////////////
    


}
