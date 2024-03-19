using UnityEngine;
using UnityEngine.UI;

public class ObjectInfo : MonoBehaviour
{
    public Text infoText; // ������ �� ��������� ������ � UI ��� ����������� ����������

    private void Start()
    {
        // ��������, ��� � ��� ���� ������ �� Text � UI
        if (infoText == null)
        {
            Debug.LogError("�� ����� ������ Text ��� ����������� ����������!");
        }
    }

    private void OnMouseDown()
    {
        // �������� ���������� �� ������� � ���������� �� � UI
        DisplayObjectInfo();
    }

    private void DisplayObjectInfo()
    {
        // �������� ��� �������
        string objectName = gameObject.name;

        // �������� ��� �������
        string objectType = gameObject.GetType().ToString();

        // ��������� ������ � �����������
        string objectInfo = "��� �������: " + objectName + "\n��� �������: " + objectType;

        // ���������� ���������� � UI
        infoText.text = objectInfo;

        // ������������� ����� ������������ ��� �������������� ���� ����������
        // infoPanel.SetActive(true);
    }
}
