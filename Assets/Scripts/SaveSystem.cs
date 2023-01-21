using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using System.Text;

public class SaveSystem : MonoBehaviour
{

    public SettingsSaves settings = new SettingsSaves();
    private string path;

    public static Dictionary<string, string> Language;
    public static SaveSystem Instance { get; private set; } //определяем

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
        SettingsLoad(); //загружаем настройки, если их тут не подгрузить, то в методе ниже с языком не подтянет выбранный язык и кнопки будут без надписей, скрипт с настройками должен загружаться раньше этого скрипта, выставить в настройках юнити
    }

    public void LoadLanguage(string lang) //загрузка языковых фраз из файла и создание словаря с парой ключ - фраза, язык передаем аргументом при загрузке настроек
    {

        if (Language == null)
        {
            Language = new Dictionary<string, string>(); //создаем словарь
        }

        Language.Clear(); //очищаем его, не помню зачем

        string allTexts = (Resources.Load(@"Languages/" + lang) as TextAsset).text; //читаем весь текст из нужного языкового файла

        string[] lines = allTexts.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None); //режем текст по строкам
        string key, value;

        //тут весь текст делим на ключ и фразу, способ из гайдов, лучше не трогать пока работает)
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].IndexOf("-") >= 0 && !lines[i].StartsWith("#"))
            {
                key = lines[i].Substring(0, lines[i].IndexOf("-"));
                value = lines[i].Substring(lines[i].IndexOf("-") + 1,
                    lines[i].Length - lines[i].IndexOf("-") - 1).Replace("\\n", Environment.NewLine);
                Language.Add(key, value);
            }
        }
    }

    public static string GetText(string key) //метод для чтения фразы по ключу из словаря
    {
        if (!Language.ContainsKey(key))
        {
            Debug.LogError("There is no key with name: [" + key + "] in your text files");
            return null;
        }

        return Language[key];
    }

 

    public void SettingsLoad() //сохранение настроек
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "Settings.json");
#else
        path = Path.Combine(Application.dataPath, "Settings.json"); //путь к файлу с сейвами
#endif
        if (File.Exists(path)) //если файл сейва есть
        {
            settings = JsonUtility.FromJson<SettingsSaves>(File.ReadAllText(path)); //грузим жсон из файла и применяем настройки
            Settings.Instance.language = settings.language;
            Settings.Instance.convertFrom = settings.convertFrom;
            Settings.Instance.convertTo = settings.convertTo;
            Settings.Instance.inputValue = settings.inputValue;
            Settings.Instance.inputSliderValue = settings.inputSliderValue;
            Settings.Instance.decimalPlaces = settings.decimalPlaces;
            Settings.Instance.decimalSlider = settings.decimalSlider;
            Settings.Instance.sliderNumbersQ = settings.sliderNumbersQ;
            Settings.Instance.inputLayer = settings.inputLayer;
            Settings.Instance.invertInputSlider = settings.invertInputSlider;
            Settings.Instance.themeColor = settings.themeColor;


            LoadLanguage(settings.language); //загружаем язык
        }
        else //если файла сейва нет, то загружаем настройки по умолчанию
        {
            Settings.Instance.language = "en";
            Settings.Instance.convertFrom = "mpg (US)";
            Settings.Instance.convertTo = "l/100km";
            Settings.Instance.inputValue = "25";
            Settings.Instance.inputSliderValue = "25";
            Settings.Instance.decimalPlaces = 2;
            Settings.Instance.decimalSlider = 1;
            Settings.Instance.sliderNumbersQ = 2;
            Settings.Instance.inputLayer = false;
            Settings.Instance.invertInputSlider = false;
            Settings.Instance.themeColor = 0;

            LoadLanguage("en"); //по умолчанию английский язык
            Debug.Log("SettingsDefault");
        }
    }

    public void SettingsSave() //сохраняем настройки
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "Settings.json");
#else
        path = Path.Combine(Application.dataPath, "Settings.json");
#endif
        settings.language = Settings.Instance.language;
        settings.convertFrom = Settings.Instance.convertFrom;
        settings.convertTo = Settings.Instance.convertTo;
        settings.inputValue = Settings.Instance.inputValue;
        settings.inputSliderValue = Settings.Instance.inputSliderValue;
        settings.decimalPlaces = Settings.Instance.decimalPlaces;
        settings.decimalSlider = Settings.Instance.decimalSlider;
        settings.sliderNumbersQ = Settings.Instance.sliderNumbersQ;
        settings.inputLayer = Settings.Instance.inputLayer;
        settings.invertInputSlider = Settings.Instance.invertInputSlider;
        settings.themeColor = Settings.Instance.themeColor;

        File.WriteAllText(path, JsonUtility.ToJson(settings)); //берем все и записываем в жсон
    }


    private void OnApplicationQuit() //при выходе из приложения
    {

        //PlayServicesGoogle.Instance.CollectData(); //собираем данные
       // PlayServicesGoogle.Instance.SaveToJson(); //пишем в JSON
       // PlayServicesGoogle.Instance.SaveToCloud(); //пишем в облако true
        SettingsSave(); //сохраняем настройки
    }
}

[Serializable]
public class SettingsSaves //класс с настройками, нужен для сохранения
{
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
}