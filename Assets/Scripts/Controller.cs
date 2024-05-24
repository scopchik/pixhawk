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
                GameObject clickedObject = hit.collider.gameObject;
                if (clickedObject.CompareTag("Wire"))
                {
                    selectedObject = clickedObject;
                    Debug.Log("Selected wire: " + selectedObject.name);
                    HandleObjectClick(selectedObject);
                }
                else if (clickedObject.CompareTag("Port"))
                {
                    selectedPort = clickedObject;
                    Debug.Log("Selected port: " + selectedPort.name);
                    HandleObjectClick(selectedPort);
                }   
            }
            else
            {
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
                    if ((selectedPort.name + ".001") == selectedObject.name)
                    {
                        Debug.Log("Calling MoveWireToPort");
                        MoveWireToPort(selectedObject, selectedPort);
                    }
                    else
                    {
                        Debug.Log("Îøèáêà: èìåíà ïîðòà è ïðîâîäà íå ñîâïàäàþò, îöåíêà ñíèæåíà");
                        wrongText.text = "Îøèáêà: èìåíà ïîðòà è ïðîâîäà íå ñîâïàäàþò, îöåíêà ñíèæåíà";
                        count++;
                        wrongNumberText.text = "Îøèáêè: " + count.ToString();
                    }
                    selectedPort = null;
                    selectedObject = null;
                }
            }
            else
            {
                if (selectedPort != null && selectedObject != null && selectedObject.CompareTag("Wire"))
                {
                    if ((selectedPort.name + ".001") == selectedObject.name)
                    {
                        Debug.Log("Calling MoveWireToPort");
                        MoveWireToPort(selectedObject, selectedPort);
                    }
                    else
                    {
                        Debug.Log("Îøèáêà: èìåíà ïîðòà è ïðîâîäà íå ñîâïàäàþò");
                        wrongText.text = "Îøèáêà: èìåíà ïîðòà è ïðîâîäà íå ñîâïàäàþò";
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
        Transform wireTransform = wire.transform.GetChild(0);
        Transform portTransform = port.transform.GetChild(0);
        float yOffset = 0f;
        float xOffset = 0f;
        float zOffset = 0f; 
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
        float duration = 1f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            parentTransform.localPosition = Vector3.Lerp(startParentLocalPosition, targetLocalPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
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
        Debug.Log("Ïðîâîä âñòàâëåí â ïîðò");
    }

    private void ConnectWire(GameObject port)
    {
        Debug.Log("Ïîäêëþ÷åí ïðîâîä ê ïîðòó: " + port.name);
    }

    private void DisplayPortInfo(GameObject port)
    {
        string portInfo = "Íàçâàíèå ïîðòà: " + port.name + "\nÎïèñàíèå ïîðòà: " + GetPortDescription(port);
        Debug.Log(portInfo);
        infoPortText.text = portInfo;
    }

    private void DisplayWireInfo(GameObject wire)
    {
        string wireInfo = "Íàçâàíèå ïîðòà: " + wire.name + "\nÎïèñàíèå ïîðòà: " + GetPortDescription(wire);
        Debug.Log(wireInfo);
        infoWireText.text = wireInfo;
    }

    private string GetPortDescription(GameObject port)
    {
        return "Îïèñàíèå ïîðòà: ...";
    }

    private void ActivateTutorialMode()
    {
        currentMode = Mode.Tutorial;
        Debug.Log("Ïåðåêëþ÷åíî â ðåæèì îáó÷åíèÿ");
    }

    private void ActivateTestMode()
    {
        currentMode = Mode.Test;
        Debug.Log("Ïåðåêëþ÷åíî â òåñòîâûé ðåæèì");
    }
}
