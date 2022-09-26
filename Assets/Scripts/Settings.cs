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

    public static Settings Instance { get; private set; } //определяем

    public KeyCode screenShotButton;
    public string language; //язык приложения
    public string convertFrom; //конвертируем из
    public string convertTo; //конвертируем в
    public string inputValue; //последние введенные данные
    public string inputSliderValue; //последние введенные данные
    public int decimalPlaces; //количество знаков после запятой
    public int decimalSlider; //количество знаков после запятой у слайдера
    public int sliderNumbersQ; //количество всего слайдеров для выбора цифр
    public bool inputLayer; //выбор режима ввода данных, если тру то поле для ввода, если фелс то слайдер

    public TMP_Dropdown language_drop;
    public Slider decimalPlaces_slider;
    public Toggle Toggle_inputLayer;

    public GameObject SettingsLayer;
    public GameObject ValueLayer;

    public GameObject ClassicInput;
    public GameObject SliderInput;

    [SerializeField]
    string[] myLangs; //список языков   

    private void Awake() //запускается до всех стартов
    {
        if (Instance == null) //если объекта ещё нет
        {
            Instance = this; //говорим что вот кагбе он
            DontDestroyOnLoad(gameObject); //и говорим что его нельзя ломать между уровнями, иначе он нахер не нужен
        }
        else //но, если вдруг на уровне такой уже есть
        {
            Destroy(gameObject); //то ломаем его к херам
        }
        Input.multiTouchEnabled = false;
    }

    void Start()
    {
        language_drop.value = Array.IndexOf(myLangs, language);
        decimalPlaces_slider.value = decimalPlaces;
        Toggle_inputLayer.isOn = inputLayer;

        language_drop.onValueChanged.AddListener(delegate { //ставим его в дропдовн меню
        language = myLangs[language_drop.value];
        ApplyLanguageChanges(); //применяем настройки смены языка при выборе другого
        });        
    }

    void ApplyLanguageChanges() //применяем настройки смены языка при выборе другого
    {
        SaveSystem.Instance.SettingsSave(); //сохраняем настройки с новым языком
        string lvl = SceneManager.GetActiveScene().name; //получаем имя активной сцены
        SceneManager.LoadScene(lvl); //и загружаем ее заново
    }

    public void ChangeDecimalPlaces()
    {
        decimalPlaces = Convert.ToInt32(decimalPlaces_slider.value);
    }

    public void ChangeInputLayer()
    {
        inputLayer = Toggle_inputLayer.isOn;

        SaveSystem.Instance.SettingsSave(); //сохраняем настройки с новым языком
    }

    void Update()
    {
        if (Input.GetKeyDown(screenShotButton))
        {
            ScreenCapture.CaptureScreenshot("screenshot " + System.DateTime.Now.ToString("MM - dd - yy(HH - mm - ss)") + ".png");
        }
    }
}
