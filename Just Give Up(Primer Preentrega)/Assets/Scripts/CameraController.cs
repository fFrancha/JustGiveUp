
using UnityEngine;

public class CameraController : MonoBehaviour
{
     float rotX;
     float rotY;
     public float sensX;
     public float sensY;
     public Transform orientation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensY;
        rotY += mouseX;
        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, -90f, 90f);
        transform.rotation = Quaternion.Euler(rotX, rotY, 0);
        orientation.rotation = Quaternion.Euler(0, rotY, 0);
        
    }
}
