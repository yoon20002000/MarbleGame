// good practice
using UnityEngine;

public class CameraController2D : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float zoomSpeed = 5f;
    public float minZoom = 3f;
    public float maxZoom = 10f;

    [Header("Move Settings")]
    public float moveSpeed = 0.5f;

    [Header("Clamp Settings")]
    public Vector2 minPosition = new Vector2(-10, -10);
    public Vector2 maxPosition = new Vector2(10, 10);

    private Camera cam;
    private Vector3 lastMousePosition;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandleDragMove();
        HandleZoom();
        ClampPosition();
    }

    void HandleDragMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentMousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 delta = lastMousePosition - currentMousePosition;
            transform.position += delta;
        }
    }

    void HandleZoom()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            float size = cam.orthographicSize;
            size -= Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;
            size = Mathf.Clamp(size, minZoom, maxZoom);
            cam.orthographicSize = size;
        }
    }

    void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minPosition.x, maxPosition.x);
        pos.y = Mathf.Clamp(pos.y, minPosition.y, maxPosition.y);
        transform.position = pos;
    }
}
