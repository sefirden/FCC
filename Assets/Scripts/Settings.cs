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
    public bool invertInputSlider; //когда тру инвертируем слайдер для ввода цифр
    public int themeColor;

    public UI ui; //скрипт уи

    public string[] myLangs; //список языков   

    private void Awake() //запускается до всех стартов
    {
        ui = FindObjectOfType<UI>();
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

#if UNITY_ANDROID || UNITY_EDITOR
    void OnApplicationFocus(bool focusStatus) //при сворачивании игры ставит ее на паузу, даже если этого нделал игрок и как раз сейвит игру, если так работает, то можно убрать сейв при выходе из игры
    {
        if (focusStatus == false)
        {
            SaveSystem.Instance.SettingsSave(); //сохраняем настройки с новым языком
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
