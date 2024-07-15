using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithArrows : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the object movement

    void Update()
    {
        Vector3 movement = Vector3.zero;

        // Check for arrow key inputs and set the movement vector accordingly
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movement.x -= moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            movement.x += moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            movement.z += moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            movement.z -= moveSpeed * Time.deltaTime;
        }

        // Apply the movement to the object
        transform.Translate(movement, Space.World);
    }
}
