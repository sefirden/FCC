using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{

    public static Settings Instance { get; private set; } //определяем

    public KeyCode screenShotButton;
    public string language; //язык приложения
    public string convertFrom; //конвертируем из
    public string convertTo; //конвертируем в
    public string inputValue; //последние введенные данные
    public int decimalPlaces; //количество знаков после запятой
    public bool inputLayer; //выбор режима ввода данных, если тру то поле для ввода, если фелс то слайдер

    

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

    void Update()
    {
        if (Input.GetKeyDown(screenShotButton))
        {
            ScreenCapture.CaptureScreenshot("screenshot " + System.DateTime.Now.ToString("MM - dd - yy(HH - mm - ss)") + ".png");
        }
    }
}
