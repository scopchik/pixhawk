using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Controller : MonoBehaviour
{
    public Text infoWireText;
    public Text infoPortText;
    public Text wrongText;
    public Text wrongNumberText;
    public Button tutorialButton;
    public Button testButton;
    public float rotationSpeed = 5f;
    public float wireSpeed = 10f;
    private int count = 0;
    public enum Mode
    {
        Tutorial,
        Test
    }
    public Mode currentMode = Mode.Tutorial;

    private bool isRotating = false;
    private Vector3 initialMousePosition;
    [SerializeField] private GameObject selectedObject;
    [SerializeField] private GameObject selectedPort;
    private float currentRotationX;

    private void Start()
    {
        tutorialButton.onClick.AddListener(ActivateTutorialMode);
        testButton.onClick.AddListener(ActivateTestMode);
    }

    void Update()
    {
        HandleMouseInput();
        HandleRotationByMouse();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // Проверяем, на какой объект мы нажали
                GameObject clickedObject = hit.collider.gameObject;

                // Если это провод, сохраняем его как selectedObject
                if (clickedObject.CompareTag("Wire"))
                {
                    selectedObject = clickedObject;
                    Debug.Log("Selected wire: " + selectedObject.name);
                    HandleObjectClick(selectedObject);
                }
                // Если это порт, сохраняем его как selectedPort
                else if (clickedObject.CompareTag("Port"))
                {
                    selectedPort = clickedObject;
                    Debug.Log("Selected port: " + selectedPort.name);
                    HandleObjectClick(selectedPort);
                }
                
            }
            else
            {
                // Если нажали вне объектов, начинаем вращение
                isRotating = true;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isRotating = false;
            
            if(Mode.Test == currentMode)
            {
                if (selectedPort != null && selectedObject != null && selectedObject.CompareTag("Wire"))
                {
                    // Перед вызовом функции проверяем, что имена порта и провода совпадают
                    if ((selectedPort.name + ".001") == selectedObject.name)
                    {
                        Debug.Log("Calling MoveWireToPort");
                        MoveWireToPort(selectedObject, selectedPort);
                    }
                    else
                    {
                        Debug.Log("Ошибка: имена порта и провода не совпадают, оценка снижена");
                        wrongText.text = "Ошибка: имена порта и провода не совпадают, оценка снижена";
                        count++;
                        wrongNumberText.text = "Ошибки: " + count.ToString();
                    }
                    selectedPort = null;
                    selectedObject = null;
                }
            }
            else
            {
                if (selectedPort != null && selectedObject != null && selectedObject.CompareTag("Wire"))
                {
                    // Перед вызовом функции проверяем, что имена порта и провода совпадают
                    if ((selectedPort.name + ".001") == selectedObject.name)
                    {
                        Debug.Log("Calling MoveWireToPort");
                        MoveWireToPort(selectedObject, selectedPort);
                    }
                    else
                    {
                        Debug.Log("Ошибка: имена порта и провода не совпадают");
                        wrongText.text = "Ошибка: имена порта и провода не совпадают";
                        wrongNumberText.text = "";
                    }
                    selectedPort = null;
                    selectedObject = null;
                }
            }

        }
    }


    private void MoveWireToPort(GameObject wire, GameObject port)
    {
        // Получаем компонент Transform провода
        Transform wireTransform = wire.transform.GetChild(0);

        // Получаем дочерний объект порта по имени
        Transform portTransform = port.transform.GetChild(0);
        float yOffset = 0f;
        float xOffset = 0f;
        float zOffset = 0f; 
        // Если порт не найден, выходим из функции
        if (portTransform == null)
        {
            Debug.LogError("Child port object not found!");
            return;
        }
        if (portTransform.localPosition.x == 0)
        {
            yOffset = Math.Abs(portTransform.localPosition.y) + wireTransform.localPosition.y;
            Debug.Log(yOffset);
        }
        else
        {
            xOffset = portTransform.localPosition.x - wireTransform.localPosition.x;
            Debug.Log(xOffset);
        }
        if (portTransform.localPosition.z == 0)
        {
            zOffset = portTransform.localPosition.z - wireTransform.localPosition.z;
        }
        StartCoroutine(MoveWireAndParent(wireTransform.parent, xOffset, yOffset, zOffset));
    }

    private IEnumerator MoveWireAndParent(Transform parentTransform, float xOffset, float yOffset, float zOffset)
    {

        Vector3 startParentLocalPosition = parentTransform.localPosition;
        Vector3 targetLocalPosition = new Vector3();
        if ((xOffset == 0)|| (yOffset == 0))
        {
            targetLocalPosition = new Vector3(xOffset, zOffset, yOffset);
        }

        // Задаем время, за которое провод должен переместиться к порту
        float duration = 1f; 

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            // Перемещаем родительский объект провода вместе с проводом
            parentTransform.localPosition = Vector3.Lerp(startParentLocalPosition, targetLocalPosition, elapsedTime / duration);

            // Обновляем время
            elapsedTime += Time.deltaTime;

            // Ждем следующего кадра
            yield return null;
        }

        // Устанавливаем конечную локальную позицию родительского объекта в точности равной целевой позиции
        parentTransform.localPosition = targetLocalPosition;
    }

    private void HandleRotationByMouse()
    {
        if (isRotating)
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            float newRotationX = Mathf.Clamp(currentRotationX + mouseY, -90f, 90f);

            transform.Rotate(Vector3.up, -mouseX, Space.World);
            transform.localRotation = Quaternion.Euler(newRotationX, transform.localEulerAngles.y, 0f);

            currentRotationX = newRotationX;
        }
    }

    private void HandleObjectClick(GameObject clickedObject)
    {
        if (currentMode == Mode.Tutorial)
        {
            if (clickedObject == clickedObject.CompareTag("Port")){
                DisplayPortInfo(clickedObject);
            }
            if (clickedObject == clickedObject.CompareTag("Wire")){
                DisplayWireInfo(clickedObject);
            }
        }
        else if (currentMode == Mode.Test)
        {
            if (clickedObject.CompareTag("Port") && selectedObject.CompareTag("Wire"))
            {
                ConnectWireToPort(selectedObject, clickedObject);
            }
            if (clickedObject == clickedObject.CompareTag("Port")){
                DisplayPortInfo(clickedObject);
            }
            if (clickedObject == clickedObject.CompareTag("Wire")){
                DisplayWireInfo(clickedObject);
            }
        }
    }

    private void ConnectWireToPort(GameObject wire, GameObject port)
    {
        Debug.Log("Провод вставлен в порт");
    }

    private void ConnectWire(GameObject port)
    {
        Debug.Log("Подключен провод к порту: " + port.name);
    }

    private void DisplayPortInfo(GameObject port)
    {
        string portInfo = "Название порта: " + port.name + "\nОписание порта: " + GetPortDescription(port);
        Debug.Log(portInfo);
        infoPortText.text = portInfo;
    }

    private void DisplayWireInfo(GameObject wire)
    {
        string wireInfo = "Название порта: " + wire.name + "\nОписание порта: " + GetPortDescription(wire);
        Debug.Log(wireInfo);
        infoWireText.text = wireInfo;
    }

    private string GetPortDescription(GameObject port)
    {
        return "Описание порта: ...";
    }

    private void ActivateTutorialMode()
    {
        currentMode = Mode.Tutorial;
        Debug.Log("Переключено в режим обучения");
    }

    private void ActivateTestMode()
    {
        currentMode = Mode.Test;
        Debug.Log("Переключено в тестовый режим");
    }
}
