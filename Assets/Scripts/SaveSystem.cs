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
    public static SaveSystem Instance { get; private set; } //����������

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
        SettingsLoad(); //��������� ���������, ���� �� ��� �� ����������, �� � ������ ���� � ������ �� �������� ��������� ���� � ������ ����� ��� ��������, ������ � ����������� ������ ����������� ������ ����� �������, ��������� � ���������� �����
    }

    public void LoadLanguage(string lang) //�������� �������� ���� �� ����� � �������� ������� � ����� ���� - �����, ���� �������� ���������� ��� �������� ��������
    {

        if (Language == null)
        {
            Language = new Dictionary<string, string>(); //������� �������
        }

        Language.Clear(); //������� ���, �� ����� �����

        string allTexts = (Resources.Load(@"Languages/" + lang) as TextAsset).text; //������ ���� ����� �� ������� ��������� �����

        string[] lines = allTexts.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None); //����� ����� �� �������
        string key, value;

        //��� ���� ����� ����� �� ���� � �����, ������ �� ������, ����� �� ������� ���� ��������)
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].IndexOf(":") >= 0 && !lines[i].StartsWith("#"))
            {
                key = lines[i].Substring(0, lines[i].IndexOf(":"));
                value = lines[i].Substring(lines[i].IndexOf(":") + 1,
                    lines[i].Length - lines[i].IndexOf(":") - 1).Replace("\\n", Environment.NewLine);
                Language.Add(key, value);
            }
        }
    }

    public static string GetText(string key) //����� ��� ������ ����� �� ����� �� �������
    {
        if (!Language.ContainsKey(key))
        {
            Debug.LogError("There is no key with name: [" + key + "] in your text files");
            return null;
        }

        return Language[key];
    }

 

    public void SettingsLoad() //���������� ��������
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "Settings.json");
#else
        path = Path.Combine(Application.dataPath, "Settings.json"); //���� � ����� � �������
#endif
        if (File.Exists(path)) //���� ���� ����� ����
        {
            settings = JsonUtility.FromJson<SettingsSaves>(File.ReadAllText(path)); //������ ���� �� ����� � ��������� ���������
            Settings.Instance.language = settings.language;
            Settings.Instance.convertFrom = settings.convertFrom;
            Settings.Instance.convertTo = settings.convertTo;
            Settings.Instance.inputValue = settings.inputValue;
            Settings.Instance.decimalPlaces = settings.decimalPlaces;
            Settings.Instance.decimalSlider = settings.decimalPlaces;
            Settings.Instance.sliderNumbersQ = settings.sliderNumbersQ;
            Settings.Instance.inputLayer = settings.inputLayer;

            LoadLanguage(settings.language); //��������� ����
        }
        else //���� ����� ����� ���, �� ��������� ��������� �� ���������
        {
            Settings.Instance.language = "en";
            Settings.Instance.convertFrom = "mpg (US)";
            Settings.Instance.convertTo = "l/100km";
            Settings.Instance.inputValue = "0";
            Settings.Instance.decimalPlaces = 2;
            Settings.Instance.decimalSlider = 1;
            Settings.Instance.sliderNumbersQ = 2;
            Settings.Instance.inputLayer = false;

            LoadLanguage("en"); //�� ��������� ���������� ����
            Debug.Log("SettingsDefault");
        }
    }

    public void SettingsSave() //��������� ���������
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
        settings.decimalPlaces = Settings.Instance.decimalPlaces;
        settings.decimalPlaces = Settings.Instance.decimalSlider;
        settings.sliderNumbersQ = Settings.Instance.sliderNumbersQ;
        settings.inputLayer = Settings.Instance.inputLayer;

        File.WriteAllText(path, JsonUtility.ToJson(settings)); //����� ��� � ���������� � ����
    }


    private void OnApplicationQuit() //��� ������ �� ����������
    {

        //PlayServicesGoogle.Instance.CollectData(); //�������� ������
       // PlayServicesGoogle.Instance.SaveToJson(); //����� � JSON
       // PlayServicesGoogle.Instance.SaveToCloud(); //����� � ������ true
        SettingsSave(); //��������� ���������
    }
}

[Serializable]
public class SettingsSaves //����� � �����������, ����� ��� ����������
{
    public string language; //���� ����������
    public string convertFrom; //������������ ��
    public string convertTo; //������������ �
    public string inputValue; //��������� ��������� ������
    public int decimalPlaces; //���������� ������ ����� �������
    public int decimalSlider; //���������� ������ ����� ������� � ��������
    public int sliderNumbersQ; //���������� ����� ��������� ��� ������ ����
    public bool inputLayer; //����� ������ ����� ������, ���� ��� �� ���� ��� �����, ���� ���� �� �������
}