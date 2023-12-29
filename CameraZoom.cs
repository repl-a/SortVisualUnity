using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 10f; 
    public float minZoom = 5f;    
    public float maxZoom = 60f;   

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>(); 
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (cam.orthographic)
        {
            // For orthographic camera
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
        else
        {
            // For perspective camera
            cam.fieldOfView -= scroll * zoomSpeed;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minZoom, maxZoom);
        }
    }
}
