using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{

    public static Settings Instance { get; private set; } //����������

    public KeyCode screenShotButton;
    public string language; //���� ����������
    public string convertFrom; //������������ ��
    public string convertTo; //������������ �
    public string inputValue; //��������� ��������� ������
    public int decimalPlaces; //���������� ������ ����� �������
    public bool inputLayer; //����� ������ ����� ������, ���� ��� �� ���� ��� �����, ���� ���� �� �������

    

    private void Awake() //����������� �� ���� �������
    {
        if (Instance == null) //���� ������� ��� ���
        {
            Instance = this; //������� ��� ��� ����� ��
            DontDestroyOnLoad(gameObject); //� ������� ��� ��� ������ ������ ����� ��������, ����� �� ����� �� �����
        }
        else //��, ���� ����� �� ������ ����� ��� ����
        {
            Destroy(gameObject); //�� ������ ��� � �����
        }
        Input.multiTouchEnabled = false;

    }

    void Update()
    {
        if (Input.GetKeyDown(screenShotButton))
        {
            ScreenCapture.CaptureScreenshot("screenshot " + System.DateTime.Now.ToString("MM - dd - yy(HH - mm - ss)") + ".png");
        }
    }
}
