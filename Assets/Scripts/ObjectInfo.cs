using UnityEngine;
using UnityEngine.UI;

public class ObjectInfo : MonoBehaviour
{
    public Text infoText;
    private void Start()
    {
        if (infoText == null)
        {
            Debug.LogError("Не задан объект Text для отображения информации!");
        }
    }

    private void OnMouseDown()
    {
        DisplayObjectInfo();
    }

    private void DisplayObjectInfo()
    {
        string objectName = gameObject.name;
        string objectType = gameObject.GetType().ToString();
        string objectInfo = "Имя объекта: " + objectName + "\nТип объекта: " + objectType;
        infoText.text = objectInfo;
    }
}
