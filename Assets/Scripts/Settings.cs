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
    public bool invertInputSlider; //����� ��� ����������� ������� ��� ����� ����

    public TMP_Dropdown language_drop;
    public Slider decimalPlaces_slider;
    public Slider decimalSlider_slider;
    public Slider sliderNumbersQ_slider;
    public Toggle Toggle_inputLayer;
    public Toggle Toggle_invertInputSlider;

    public GameObject SettingsLayer;
    public GameObject ValueLayer;
    public GameObject SliderSettingsHide;

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
        decimalSlider_slider.value = decimalSlider;
        sliderNumbersQ_slider.value = sliderNumbersQ;
        Toggle_inputLayer.isOn = inputLayer;
        Toggle_invertInputSlider.isOn = invertInputSlider;

        if (inputLayer)
        {
            SliderSettingsHide.SetActive(false);
        }
        else
        {
            SliderSettingsHide.SetActive(true);
        }

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
        SaveSystem.Instance.SettingsSave(); //��������� ��������� � ����� ������
    }

    public void ChangeDecimalSlider()
    {
        decimalSlider = Convert.ToInt32(decimalSlider_slider.value);

        if((decimalSlider + sliderNumbersQ) > 5)
        sliderNumbersQ_slider.value = 5 - decimalSlider;

        SaveSystem.Instance.SettingsSave(); //��������� ��������� � ����� ������
    }

    public void ChangeNumberQSlider()
    {
        sliderNumbersQ = Convert.ToInt32(sliderNumbersQ_slider.value);

        if ((decimalSlider + sliderNumbersQ) > 5)
        decimalSlider_slider.value = 5 - sliderNumbersQ;

        SaveSystem.Instance.SettingsSave(); //��������� ��������� � ����� ������
    }

    public void ChangeInvertInputSlider()
    {
        invertInputSlider = Toggle_invertInputSlider.isOn;

        SaveSystem.Instance.SettingsSave();
    }

    public void ChangeInputLayer()
    {
        inputLayer = Toggle_inputLayer.isOn;

        if(inputLayer)
        {
            SliderSettingsHide.SetActive(false);
        }
        else
        {
            SliderSettingsHide.SetActive(true);
        }

        SaveSystem.Instance.SettingsSave();
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
