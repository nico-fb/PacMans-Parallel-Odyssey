using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -70f, 70f);

        if (transform.parent.position.y > 0){
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            transform.parent.rotation = Quaternion.Euler(0, yRotation, 0);
        } else {
            transform.rotation = Quaternion.Euler(-xRotation, -yRotation, 0);
            transform.parent.rotation = Quaternion.Euler(0, -yRotation, 0);
        }
    }
}
