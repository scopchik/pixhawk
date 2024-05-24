using UnityEngine;

public class WireMovement : MonoBehaviour
{
    private bool isDragging = false;
    [SerializeField] private Transform attachedPort;
    
    private void OnMouseDown()
    {
        isDragging = true;
    }
    
    private void OnMouseUp()
    {
        isDragging = false;
        if (attachedPort != null)
        {
            transform.position = attachedPort.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Port"))
        {
            attachedPort = other.transform;
        }
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            transform.position = mousePosition;
        }
    }
}
