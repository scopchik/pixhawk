using UnityEngine;

public class WireMovement : MonoBehaviour
{
    private bool isDragging = false;
    [SerializeField] private Transform attachedPort;

    // Вызывается при нажатии на левую кнопку мыши над объектом провода
    private void OnMouseDown()
    {
        isDragging = true;
    }

    // Вызывается при отпускании левой кнопки мыши
    private void OnMouseUp()
    {
        isDragging = false;

        // При отпускании кнопки мыши проверяем, находится ли провод над портом,
        // и если да, то перемещаем его к порту
        if (attachedPort != null)
        {
            transform.position = attachedPort.position;
        }
    }

    // Вызывается при столкновении с другим коллайдером
    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, столкнулся ли провод с портом
        if (other.CompareTag("Port"))
        {
            // Присваиваем порт, с которым столкнулся провод
            attachedPort = other.transform;
        }
    }

    // Вызывается каждый кадр
    private void Update()
    {
        // Если провод перетаскивают, то обновляем его позицию в каждом кадре
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f; // Устанавливаем z-координату равной 0, чтобы объект не двигался вдоль оси z
            transform.position = mousePosition;
        }
    }
}
