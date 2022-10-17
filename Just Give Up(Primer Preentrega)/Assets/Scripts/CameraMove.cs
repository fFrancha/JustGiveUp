using UnityEngine;

public class CameraMove : MonoBehaviour
{

    public Transform cameraPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraPosition.position;        
    }
}
