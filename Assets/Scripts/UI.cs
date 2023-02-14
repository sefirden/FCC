using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;


public class UI : MonoBehaviour
{
    //input layer
    public TMP_InputField inputValue;
    public TMP_Text resultText;
    public TMP_Dropdown convertFrom_drop;
    public TMP_Dropdown convertTo_drop;

    public GameObject SettingsLayer;
    public GameObject ValueLayer;
    public GameObject DataListLayer;
    public GameObject OutputLine;
    public GameObject ClassicInput;
    public GameObject SliderInput;
    public GameObject resultTextO;
    public GameObject ExitSettingsLine;
    public GameObject loadingIndicator;

    public List<string> dropStrings;
    private List<Data> dataList;

    public GameObject sampleSliderInput;
    public List<GameObject> slidersForInput;

    public GameObject dataitem;
    public GameObject dataitemparent;
    public GameObject forcerebuildlayer;

    //settings layer
    public TMP_Dropdown language_drop;
    public TMP_Dropdown color_drop;
    public LabelPosition language_drop_fieldName;
    public LabelPosition color_drop_fieldName;
    public Slider decimalPlaces_slider;
    public Slider decimalSlider_slider;
    public Slider sliderNumbersQ_slider;
    public GameObject SliderSettingsHide;
    public GameObject Spacing;

    public TMP_Text decimalPlaces_slider_value;
    public TMP_Text decimalSlider_slider_value;
    public TMP_Text sliderNumbersQ_slider_value;


    public Toggle Toggle_inputLayer;
    public Toggle Toggle_invertInputSlider;
    public Toggle Toggle_darkMode;

    public Sprite[] SpriteToggle;
    public Image ToggleInputImage;
    public Image ToggleInvertInputImage;
    public Image ToggledarkModeImage;
    public Color[] ColorSwap;
    public Color[] DarkModeColors;

    public GameObject DecimalPoint;
    public float SuperWidth;

    private Calculations calculations;

    private void Awake()
    {
        Settings.Instance.ui = FindObjectOfType<UI>();
        calculations = FindObjectOfType<Calculations>();

        SuperWidth = ValueLayer.GetComponent<RectTransform>().rect.width;
        SetWidth();

        language_drop.value = Array.IndexOf(Settings.Instance.myLangs, Settings.Instance.language);

        for(int i = 0; i< ColorSwap.Length; i++)
        {
            var texture = new Texture2D(1, 1); 
            texture.SetPixel(0, 0, ColorSwap[i]);
            texture.Apply();
            var item = new TMP_Dropdown.OptionData(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0))); // creating dropdown item and converting texture to sprite
            color_drop.options.Add(item);
        }    
        color_drop.value = Settings.Instance.themeColor;

        decimalPlaces_slider.value = Settings.Instance.decimalPlaces;
        decimalSlider_slider.value = Settings.Instance.decimalSlider;
        sliderNumbersQ_slider.value = Settings.Instance.sliderNumbersQ;
        decimalPlaces_slider_value.text = Convert.ToString(Settings.Instance.decimalPlaces);
        decimalSlider_slider_value.text = Convert.ToString(Settings.Instance.decimalSlider);
        sliderNumbersQ_slider_value.text = Convert.ToString(Settings.Instance.sliderNumbersQ);

        Toggle_darkMode.isOn = Settings.Instance.darkMode;
        Toggle_inputLayer.isOn = Settings.Instance.inputLayer;
        Toggle_invertInputSlider.isOn = Settings.Instance.invertInputSlider;

        if (Settings.Instance.invertInputSlider)
        {
            ToggleInvertInputImage.sprite = SpriteToggle[1];
            if(Settings.Instance.darkMode)
                ToggleInvertInputImage.color = DarkModeColors[1];
            else
                ToggleInvertInputImage.color = ColorSwap[Settings.Instance.themeColor]; 
        }
        else
        {
            ToggleInvertInputImage.sprite = SpriteToggle[0];
            if (Settings.Instance.darkMode)
                ToggleInvertInputImage.color = DarkModeColors[1];
            else
                ToggleInvertInputImage.color = new Color(0.5019608f, 0.5019608f, 0.5019608f);
        }

        if (Settings.Instance.inputLayer)
        {
            ToggleInputImage.sprite = SpriteToggle[1];

            if (Settings.Instance.darkMode)
                ToggleInputImage.color = DarkModeColors[1];
            else
                ToggleInputImage.color = ColorSwap[Settings.Instance.themeColor];

            SliderSettingsHide.SetActive(false);
        }
        else
        {
            ToggleInputImage.sprite = SpriteToggle[0];

            if (Settings.Instance.darkMode)
                ToggleInputImage.color = DarkModeColors[1];
            else
                ToggleInputImage.color = new Color(0.5019608f, 0.5019608f, 0.5019608f);

            SliderSettingsHide.SetActive(true);
        }
        
        if (Settings.Instance.darkMode)
        {
            ToggledarkModeImage.sprite = SpriteToggle[1];

            if (Settings.Instance.darkMode)
                ToggledarkModeImage.color = DarkModeColors[1];
            else
                ToggledarkModeImage.color = ColorSwap[Settings.Instance.themeColor];

            color_drop.gameObject.SetActive(false);
            Spacing.SetActive(false);
        }
        else
        {
            ToggledarkModeImage.sprite = SpriteToggle[0];

            if (Settings.Instance.darkMode)
                ToggledarkModeImage.color = DarkModeColors[1];
            else
                ToggledarkModeImage.color = new Color(0.5019608f, 0.5019608f, 0.5019608f);
            color_drop.gameObject.SetActive(true);
            Spacing.SetActive(true);
        }

    }

    private void SetWidth()
    {
        float width = SuperWidth;
        float height = 72f;
        convertFrom_drop.GetComponent<RectTransform>().sizeDelta = new Vector2((width - 72f - 10f - 10f) / 2f, height);
        convertTo_drop.GetComponent<RectTransform>().sizeDelta = new Vector2((width - 72f - 10f - 10f) / 2f, height);
        ExitSettingsLine.GetComponent<HorizontalLayoutGroup>().spacing = (width - 288f) / 3f;
    }

    private void Start()
    {
        language_drop.onValueChanged.AddListener(delegate { //ставим его в дропдовн меню
            Settings.Instance.language = Settings.Instance.myLangs[language_drop.value];
            Settings.Instance.toSettings = true;
            ApplyLanguageChanges(); //применяем настройки смены языка при выборе другого
        });

        color_drop.onValueChanged.AddListener(delegate { //ставим его в дропдовн меню
            Settings.Instance.themeColor = color_drop.value;
            Settings.Instance.toSettings = true;
            ApplyLanguageChanges(); //применяем настройки смены языка при выборе другого
        });

        Toggle_darkMode.onValueChanged.AddListener(delegate
        {
            Settings.Instance.darkMode = Toggle_darkMode.isOn;
            Settings.Instance.toSettings = true;
            ApplyLanguageChanges();
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
        Settings.Instance.decimalPlaces = Convert.ToInt32(decimalPlaces_slider.value);
        decimalPlaces_slider_value.text = Convert.ToString(Settings.Instance.decimalPlaces);
        SaveSystem.Instance.SettingsSave(); //сохраняем настройки с новым языком
    }

    public void ChangeInputLayer()
    {
        Settings.Instance.inputLayer = Toggle_inputLayer.isOn;

        if (Settings.Instance.inputLayer)
        {
            ToggleInputImage.sprite = SpriteToggle[1];

            if (Settings.Instance.darkMode)
                ToggleInputImage.color = DarkModeColors[1];
            else
                ToggleInputImage.color = ColorSwap[Settings.Instance.themeColor];

            SliderSettingsHide.SetActive(false);
        }
        else
        {
            ToggleInputImage.sprite = SpriteToggle[0];

            if (Settings.Instance.darkMode)
                ToggleInputImage.color = DarkModeColors[1];
            else
                ToggleInputImage.color = new Color(0.5019608f, 0.5019608f, 0.5019608f);

            SliderSettingsHide.SetActive(true);
        }

        SaveSystem.Instance.SettingsSave();
    }

    public void ChangeNumberQSlider()
    {
        Settings.Instance.sliderNumbersQ = Convert.ToInt32(sliderNumbersQ_slider.value);

        if ((Settings.Instance.decimalSlider + Settings.Instance.sliderNumbersQ) > 5)
            decimalSlider_slider.value = 5 - Settings.Instance.sliderNumbersQ;

        decimalSlider_slider_value.text = Convert.ToString(Settings.Instance.decimalSlider);
        sliderNumbersQ_slider_value.text = Convert.ToString(Settings.Instance.sliderNumbersQ);
        SaveSystem.Instance.SettingsSave(); //сохраняем настройки с новым языком
    }

    public void ChangeDecimalSlider()
    {
        Settings.Instance.decimalSlider = Convert.ToInt32(decimalSlider_slider.value);

        if ((Settings.Instance.decimalSlider + Settings.Instance.sliderNumbersQ) > 5)
            sliderNumbersQ_slider.value = 5 - Settings.Instance.decimalSlider;

        sliderNumbersQ_slider_value.text = Convert.ToString(Settings.Instance.sliderNumbersQ);
        decimalSlider_slider_value.text = Convert.ToString(Settings.Instance.decimalSlider);
        SaveSystem.Instance.SettingsSave(); //сохраняем настройки с новым языком
    }

    public void ChangeInvertInputSlider()
    {
        Settings.Instance.invertInputSlider = Toggle_invertInputSlider.isOn;

        if (Settings.Instance.invertInputSlider)
        {
            ToggleInvertInputImage.sprite = SpriteToggle[1];
            if (Settings.Instance.darkMode)
                ToggleInvertInputImage.color = DarkModeColors[1];
            else
                ToggleInvertInputImage.color = ColorSwap[Settings.Instance.themeColor];
        }
        else
        {
            ToggleInvertInputImage.sprite = SpriteToggle[0];
            if (Settings.Instance.darkMode)
                ToggleInvertInputImage.color = DarkModeColors[1];
            else
                ToggleInvertInputImage.color = new Color(0.5019608f, 0.5019608f, 0.5019608f);
        }

        SaveSystem.Instance.SettingsSave();
    }

    public async Task SetDataToList()
    {


#if UNITY_ANDROID && !UNITY_EDITOR
        string filePath = Path.Combine(Application.persistentDataPath, "Settings.json");
#else
        string filePath = Path.Combine(Application.dataPath, "data.json"); //путь к файлу с сейвами
#endif
        dataList = new List<Data>();

        if (File.Exists(filePath))
        {
            try
            {
                foreach (Transform child in dataitemparent.transform)
                {
                    Destroy(child.gameObject);
                }
            }
            catch
            {
                Debug.LogError("No child found");
            }                      
            
            string json = await ReadFileAsync(filePath);
            
            dataList = JsonConvert.DeserializeObject<List<Data>>(json);

            for (int i = 0; i < dataList.Count; i++)
            {
                Data data = dataList[i];

                GameObject data_temp = Instantiate(dataitem, dataitem.transform.position, Quaternion.identity, dataitemparent.transform);
                data_temp.name = "dataitem";

                DataItem setdata = data_temp.GetComponent<DataItem>();
                setdata.dataValue.text = dataList[i].date.ToString();

                setdata.fromLabel.text = dataList[i].convertFrom_drop;
                setdata.fromValue.text = dataList[i].inputValue;

                setdata.toLabel.text = dataList[i].convertTo_drop;
                setdata.toValue.text = dataList[i].resultText;
            }            
        }
        else
        {
            Debug.Log("нет данных");
        }

    }

    public async void DeleteData(int index)
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        string filePath = Path.Combine(Application.persistentDataPath, "Settings.json");
#else
        string filePath = Path.Combine(Application.dataPath, "data.json"); //путь к файлу с сейвами
#endif

        dataList.RemoveAt(index);
        string newJson = JsonConvert.SerializeObject(dataList);

        loadingIndicator.SetActive(true);
        await WriteTextAsync(filePath, newJson);
        loadingIndicator.SetActive(false);

        /*
        VerticalLayoutGroup verticalLayout = forcerebuildlayer.GetComponent<VerticalLayoutGroup>();
        verticalLayout.CalculateLayoutInputVertical();
        LayoutRebuilder.ForceRebuildLayoutImmediate(forcerebuildlayer.GetComponent<RectTransform>());*/
    }

    public async void ToDataList()
    {
        await SetDataToList();
        ValueLayer.SetActive(false);
        DataListLayer.SetActive(true);
    }

    public void CollectData()
    {
        string input = Convert.ToString(calculations.doubleInput);

        Data data = new Data
        {
            inputValue = input,
            resultText = resultText.text,
            convertFrom_drop = dropStrings[convertFrom_drop.value],
            convertTo_drop = dropStrings[convertTo_drop.value],
            date = DateTime.Now
        };       

        SaveData(data);
    }

    public async void SaveData(Data data)
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        string filePath = Path.Combine(Application.persistentDataPath, "Settings.json");
#else
        string filePath = Path.Combine(Application.dataPath, "data.json"); //путь к файлу с сейвами
#endif
        List<Data> dataList = new List<Data>();
        if (File.Exists(filePath))
        {
            loadingIndicator.SetActive(true);
            string json = await ReadFileAsync(filePath);
            loadingIndicator.SetActive(false);

            dataList = JsonConvert.DeserializeObject<List<Data>>(json);
        }
        dataList.Add(data);
        string newJson = JsonConvert.SerializeObject(dataList);

        loadingIndicator.SetActive(true);
        await WriteTextAsync(filePath, newJson);
        loadingIndicator.SetActive(false);
    }

    private async Task WriteTextAsync(string filePath, string text)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            await writer.WriteAsync(text);
        }
    }

    private async Task<string> ReadFileAsync(string filePath)
    {

        using (StreamReader reader = new StreamReader(filePath))
        {
            return await reader.ReadToEndAsync();
        }

    }

}
