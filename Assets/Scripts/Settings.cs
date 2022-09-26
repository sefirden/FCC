using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using UnityEngine.Audio;
using System.Runtime.CompilerServices;

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

    public TMP_Dropdown language_drop;
    public Slider decimalPlaces_slider;
    public Toggle Toggle_inputLayer;

    public GameObject SettingsLayer;
    public GameObject ValueLayer;

    public GameObject ClassicInput;
    public GameObject SliderInput;

    [SerializeField]
    string[] myLangs; //������ ������   

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

    void Start()
    {
        language_drop.value = Array.IndexOf(myLangs, language);
        decimalPlaces_slider.value = decimalPlaces;
        Toggle_inputLayer.isOn = inputLayer;

        language_drop.onValueChanged.AddListener(delegate { //������ ��� � �������� ����
        language = myLangs[language_drop.value];
        ApplyLanguageChanges(); //��������� ��������� ����� ����� ��� ������ �������
        });        
    }

    void ApplyLanguageChanges() //��������� ��������� ����� ����� ��� ������ �������
    {
        SaveSystem.Instance.SettingsSave(); //��������� ��������� � ����� ������
        string lvl = SceneManager.GetActiveScene().name; //�������� ��� �������� �����
        SceneManager.LoadScene(lvl); //� ��������� �� ������
    }

    public void ChangeDecimalPlaces()
    {
        decimalPlaces = Convert.ToInt32(decimalPlaces_slider.value);
    }

    public void ChangeInputLayer()
    {
        inputLayer = Toggle_inputLayer.isOn;

        SaveSystem.Instance.SettingsSave(); //��������� ��������� � ����� ������
    }

    void Update()
    {
        if (Input.GetKeyDown(screenShotButton))
        {
            ScreenCapture.CaptureScreenshot("screenshot " + System.DateTime.Now.ToString("MM - dd - yy(HH - mm - ss)") + ".png");
        }
    }
}
