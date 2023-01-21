using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    //input layer
    public TMP_InputField inputValue;
    public TMP_Text resultText;
    public TMP_Dropdown convertFrom_drop;
    public TMP_Dropdown convertTo_drop;

    public GameObject SettingsLayer;
    public GameObject ValueLayer;
    public GameObject OutputLine;
    public GameObject ClassicInput;
    public GameObject SliderInput;
    public GameObject resultTextO;
    public GameObject ExitSettingsLine;


    public List<string> dropStrings;

    public GameObject sampleSliderInput;
    public List<GameObject> slidersForInput;

    //settings layer
    public TMP_Dropdown language_drop;
    public TMP_Dropdown color_drop;
    public LabelPosition language_drop_fieldName;
    public Slider decimalPlaces_slider;
    public Slider decimalSlider_slider;
    public Slider sliderNumbersQ_slider;
    public GameObject SliderSettingsHide;

    public TMP_Text decimalPlaces_slider_value;
    public TMP_Text decimalSlider_slider_value;
    public TMP_Text sliderNumbersQ_slider_value;

    public Toggle Toggle_inputLayer;
    public Toggle Toggle_invertInputSlider;

    public Sprite[] SpriteToggle;
    public Image ToggleInputImage;
    public Image ToggleInvertInputImage;
    public Color[] ColorSwap;
    public float SuperWidth;

    private void Awake()
    {
        Settings.Instance.ui = FindObjectOfType<UI>();
        
        SuperWidth = ValueLayer.GetComponent<RectTransform>().rect.width;
        SetWidth();

        language_drop.value = Array.IndexOf(Settings.Instance.myLangs, Settings.Instance.language);
        decimalPlaces_slider.value = Settings.Instance.decimalPlaces;
        decimalSlider_slider.value = Settings.Instance.decimalSlider;
        sliderNumbersQ_slider.value = Settings.Instance.sliderNumbersQ;
        decimalPlaces_slider_value.text = Convert.ToString(Settings.Instance.decimalPlaces);
        decimalSlider_slider_value.text = Convert.ToString(Settings.Instance.decimalSlider);
        sliderNumbersQ_slider_value.text = Convert.ToString(Settings.Instance.sliderNumbersQ);

        Toggle_inputLayer.isOn = Settings.Instance.inputLayer;
        Toggle_invertInputSlider.isOn = Settings.Instance.invertInputSlider;

        if (Settings.Instance.invertInputSlider)
        {
            ToggleInvertInputImage.sprite = SpriteToggle[1];
            ToggleInvertInputImage.color = ColorSwap[Settings.Instance.themeColor]; 
        }
        else
        {
            ToggleInvertInputImage.sprite = SpriteToggle[0];
            ToggleInvertInputImage.color = new Color(0.5019608f, 0.5019608f, 0.5019608f);
        }

        if (Settings.Instance.inputLayer)
        {
            ToggleInputImage.sprite = SpriteToggle[1];
            ToggleInputImage.color = ColorSwap[Settings.Instance.themeColor];
            SliderSettingsHide.SetActive(false);
        }
        else
        {
            ToggleInputImage.sprite = SpriteToggle[0];
            ToggleInputImage.color = new Color(0.5019608f, 0.5019608f, 0.5019608f);
            SliderSettingsHide.SetActive(true);
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
            ToggleInputImage.color = ColorSwap[Settings.Instance.themeColor];
            SliderSettingsHide.SetActive(false);
        }
        else
        {
            ToggleInputImage.sprite = SpriteToggle[0];
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
            ToggleInvertInputImage.color = ColorSwap[Settings.Instance.themeColor];
        }
        else
        {
            ToggleInvertInputImage.sprite = SpriteToggle[0];
            ToggleInvertInputImage.color = new Color(0.5019608f, 0.5019608f, 0.5019608f);
        }

        SaveSystem.Instance.SettingsSave();
    }
}
