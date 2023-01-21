using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using UnityEngine.Audio;
using System.Runtime.CompilerServices;
using System.Diagnostics;

public class Settings : MonoBehaviour
{

    public static Settings Instance { get; private set; } //����������

    public KeyCode screenShotButton;
    public string language; //���� ����������
    public string convertFrom; //������������ ��
    public string convertTo; //������������ �
    public string inputValue; //��������� ��������� ������
    public string inputSliderValue; //��������� ��������� ������
    public int decimalPlaces; //���������� ������ ����� �������
    public int decimalSlider; //���������� ������ ����� ������� � ��������
    public int sliderNumbersQ; //���������� ����� ��������� ��� ������ ����
    public bool inputLayer; //����� ������ ����� ������, ���� ��� �� ���� ��� �����, ���� ���� �� �������
    public bool invertInputSlider; //����� ��� ����������� ������� ��� ����� ����
    public int themeColor;

    public UI ui; //������ ��

    public string[] myLangs; //������ ������   

    private void Awake() //����������� �� ���� �������
    {
        ui = FindObjectOfType<UI>();
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

#if UNITY_ANDROID || UNITY_EDITOR
    void OnApplicationFocus(bool focusStatus) //��� ������������ ���� ������ �� �� �����, ���� ���� ����� ������ ����� � ��� ��� ������ ����, ���� ��� ��������, �� ����� ������ ���� ��� ������ �� ����
    {
        if (focusStatus == false)
        {
            SaveSystem.Instance.SettingsSave(); //��������� ��������� � ����� ������
        }
    }
#endif

    void Update()
    {
        if (Input.GetKeyDown(screenShotButton))
        {
            ScreenCapture.CaptureScreenshot("screenshot " + System.DateTime.Now.ToString("MM - dd - yy(HH - mm - ss)") + ".png");
        }
    }
}
