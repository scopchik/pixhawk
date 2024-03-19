using UnityEngine;
using UnityEngine.UI;

public class ObjectInfo : MonoBehaviour
{
    public Text infoText; // Ссылка на текстовый объект в UI для отображения информации

    private void Start()
    {
        // Убедимся, что у нас есть ссылка на Text в UI
        if (infoText == null)
        {
            Debug.LogError("Не задан объект Text для отображения информации!");
        }
    }

    private void OnMouseDown()
    {
        // Получаем информацию об объекте и отображаем ее в UI
        DisplayObjectInfo();
    }

    private void DisplayObjectInfo()
    {
        // Получаем имя объекта
        string objectName = gameObject.name;

        // Получаем тип объекта
        string objectType = gameObject.GetType().ToString();

        // Формируем строку с информацией
        string objectInfo = "Имя объекта: " + objectName + "\nТип объекта: " + objectType;

        // Отображаем информацию в UI
        infoText.text = objectInfo;

        // Дополнительно можно активировать или деактивировать окно информации
        // infoPanel.SetActive(true);
    }
}
