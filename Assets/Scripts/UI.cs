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
using System.Globalization;
using System.Linq;



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
    public GameObject ExitSpacing;
    public GameObject loadingIndicator;

    public GameObject noDataIndicator;

    public GameObject Toast;

    public List<string> dropStrings;
    private List<Data> dataList;
    private List<Data> filtredDataList;

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

    private int index;
    private Calculations calculations;

    public ScrollRect scrollRect;
    public Scrollbar scrollbarVertical;
    public float loadThreshold = 0.01f;

    private bool loading;

    public GameObject sort_filter;

    //sortlayer ui
    public Button to_sort_layer;
    public GameObject sort_layer;
    public TMP_Dropdown sort_by_drop;
    public bool sort_by_drop_padding;
    public bool sort_desc_bool;
    public string[] sort_by_values;
    public Button sort_asc;
    public Button sort_desc;
    public GameObject sort_spacing;


    //filterlayer ui
    public Button to_filter_layer;
    public GameObject filter_layer;
    public Button filter_date_min;
    public GameObject filter_date_min_layer;
    public Button filter_date_max;
    public GameObject filter_date_max_layer;
    public Button filter_from_drop;
    public GameObject filter_from_drop_value_layer;
    public GameObject filter_from_drop_content;
    public TMP_InputField filter_from_min;
    public TMP_InputField filter_from_max;
    public Button filter_to_drop;
    public GameObject filter_to_drop_value_layer;
    public GameObject filter_to_drop_content;
    public TMP_InputField filter_to_min;
    public TMP_InputField filter_to_max;
    public GameObject filter_spacing;
    public GameObject sample_drop_item;


    private void Awake()
    {
        Settings.Instance.ui = FindObjectOfType<UI>();
        calculations = FindObjectOfType<Calculations>();

        SuperWidth = ValueLayer.GetComponent<RectTransform>().rect.width;
        SetWidth();

        language_drop.value = Array.IndexOf(Settings.Instance.myLangs, Settings.Instance.language);

        for (int i = 0; i < ColorSwap.Length; i++)
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
        float spacing = 10f;
        convertFrom_drop.GetComponent<RectTransform>().sizeDelta = new Vector2((width - height - spacing * 2f) / 2f, height);
        convertTo_drop.GetComponent<RectTransform>().sizeDelta = new Vector2((width - height - spacing * 2f) / 2f, height);
        resultTextO.GetComponent<RectTransform>().sizeDelta = new Vector2((width - height - spacing), height);

        ExitSpacing.GetComponent<RectTransform>().sizeDelta = new Vector2((width - height * 3 - spacing * 3f), height);
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

        scrollRect.onValueChanged.AddListener(OnScrollValueChanged);

        sort_by_drop_padding = true;
        sort_desc_bool = true;
        foreach (string s in sort_by_values)
        {
            var item = new TMP_Dropdown.OptionData(SaveSystem.GetText(s)); // creating dropdown item and converting texture to sprite
            sort_by_drop.options.Add(item);
        }
    }

    private void OnScrollValueChanged(Vector2 value)
    {
        ScrollValue();
    }

    private async void ScrollValue()
    {
        float currentPosition = scrollRect.verticalScrollbar.value;
        float maxPosition = scrollRect.verticalScrollbar.size;

        if (currentPosition <= maxPosition - loadThreshold && !loading)
        {
            if (index < filtredDataList.Count)
            {
                Debug.Log("load");
                loadingIndicator.SetActive(true);
                await LoadDataAsync();                
            }
        }
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

    //sortlayer
    public void ToSortLayer()
    {
        if(sort_layer.activeSelf) //если слой сортировки уже открыт
        {
                ColorBlock colorBlock;
                colorBlock = to_sort_layer.colors;
                colorBlock.normalColor = DarkModeColors[3];
                colorBlock.highlightedColor = DarkModeColors[3];
                colorBlock.selectedColor = DarkModeColors[3];
                to_sort_layer.colors = colorBlock;
            
            sort_layer.SetActive(false);
        }
        else
        {
            if (Settings.Instance.darkMode)
            {
                ColorBlock colorBlock;
                colorBlock = to_sort_layer.colors;
                colorBlock.normalColor = DarkModeColors[1];
                colorBlock.highlightedColor = DarkModeColors[1];
                colorBlock.selectedColor = DarkModeColors[1];
                to_sort_layer.colors = colorBlock;                
            }                
            else
            {
                ColorBlock colorBlock;
                colorBlock = to_sort_layer.colors;
                colorBlock.normalColor = ColorSwap[Settings.Instance.themeColor];
                colorBlock.highlightedColor = ColorSwap[Settings.Instance.themeColor];
                colorBlock.selectedColor = ColorSwap[Settings.Instance.themeColor];
                to_sort_layer.colors = colorBlock;
            }

            sort_layer.SetActive(true);
            sort_by_drop.GetComponent<RectTransform>().sizeDelta = new Vector2(SuperWidth - (16f + 36f + 10f) * 2, 52f);
            sort_spacing.GetComponent<RectTransform>().sizeDelta = new Vector2(SuperWidth - (36f + 50f) * 3 - 16f * 2, 36f);

            if (sort_by_drop_padding)
            {
                StartCoroutine(SetPaddingRight(sort_by_drop.transform.GetChild(0).gameObject));
                sort_by_drop_padding = false;
            }

            if (sort_desc_bool)
                SortDesc();
            else
                SortAsc();
        }
    }

    public void SortAsc()
    {
        sort_desc_bool = false;
        ColorBlock colorBlock;

        colorBlock = sort_desc.colors;
        colorBlock.normalColor = DarkModeColors[3];
        colorBlock.highlightedColor = DarkModeColors[3];
        colorBlock.selectedColor = DarkModeColors[3];
        sort_desc.colors = colorBlock;

        if (Settings.Instance.darkMode)
        {
            colorBlock = sort_asc.colors;
            colorBlock.normalColor = DarkModeColors[1];
            colorBlock.highlightedColor = DarkModeColors[1];
            colorBlock.selectedColor = DarkModeColors[1];
            sort_asc.colors = colorBlock;
        }
        else
        {
            colorBlock = sort_asc.colors;
            colorBlock.normalColor = ColorSwap[Settings.Instance.themeColor];
            colorBlock.highlightedColor = ColorSwap[Settings.Instance.themeColor];
            colorBlock.selectedColor = ColorSwap[Settings.Instance.themeColor];
            sort_asc.colors = colorBlock;
        }

    }

    public void SortDesc()
    {
        sort_desc_bool = true;
        ColorBlock colorBlock;

        colorBlock = sort_asc.colors;
        colorBlock.normalColor = DarkModeColors[3];
        colorBlock.highlightedColor = DarkModeColors[3];
        colorBlock.selectedColor = DarkModeColors[3];
        sort_asc.colors = colorBlock;

        if (Settings.Instance.darkMode)
        {
            colorBlock = sort_desc.colors;
            colorBlock.normalColor = DarkModeColors[1];
            colorBlock.highlightedColor = DarkModeColors[1];
            colorBlock.selectedColor = DarkModeColors[1];
            sort_desc.colors = colorBlock;
        }
        else
        {
            colorBlock = sort_desc.colors;
            colorBlock.normalColor = ColorSwap[Settings.Instance.themeColor];
            colorBlock.highlightedColor = ColorSwap[Settings.Instance.themeColor];
            colorBlock.selectedColor = ColorSwap[Settings.Instance.themeColor];
            sort_desc.colors = colorBlock;
        }
    }

    public async void ApplySort()
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

        if (sort_desc_bool) //сортируем по убыванию
        {
            switch(sort_by_drop.value)
            {
                case 0:
                    filtredDataList.Sort((x, y) => y.date.CompareTo(x.date));
                    break;
                case 1:
                    filtredDataList.Sort((x, y) => y.inputValue.CompareTo(x.inputValue));
                    break;
                case 2:
                    filtredDataList.Sort((x, y) => y.resultText.CompareTo(x.resultText));
                    break;
            }

            loadingIndicator.SetActive(true);
            index = 0;
            await LoadDataAsync();
        }
        else
        {
            switch (sort_by_drop.value)
            {
                case 0:
                    filtredDataList.Sort((x, y) => x.date.CompareTo(y.date));
                    break;
                case 1:
                    filtredDataList.Sort((x, y) => x.inputValue.CompareTo(y.inputValue));
                    break;
                case 2:
                    filtredDataList.Sort((x, y) => x.resultText.CompareTo(y.resultText));
                    break;
            }

            loadingIndicator.SetActive(true);
            index = 0;
            await LoadDataAsync();
        }
    }

    public void ResetSort()
    {
        SortDesc();
        sort_by_drop.value = 0;
        ApplySort();
    }


    //filterlayer
    public void ToFilterLayer()
    {
        if (filter_layer.activeSelf) //если слой сортировки уже открыт
        {
            ColorBlock colorBlock;
            colorBlock = to_filter_layer.colors;
            colorBlock.normalColor = DarkModeColors[3];
            colorBlock.highlightedColor = DarkModeColors[3];
            colorBlock.selectedColor = DarkModeColors[3];
            to_filter_layer.colors = colorBlock;

            filter_layer.SetActive(false);
        }
        else
        {
            if (Settings.Instance.darkMode)
            {
                ColorBlock colorBlock;
                colorBlock = to_filter_layer.colors;
                colorBlock.normalColor = DarkModeColors[1];
                colorBlock.highlightedColor = DarkModeColors[1];
                colorBlock.selectedColor = DarkModeColors[1];
                to_filter_layer.colors = colorBlock;
            }
            else
            {
                ColorBlock colorBlock;
                colorBlock = to_filter_layer.colors;
                colorBlock.normalColor = ColorSwap[Settings.Instance.themeColor];
                colorBlock.highlightedColor = ColorSwap[Settings.Instance.themeColor];
                colorBlock.selectedColor = ColorSwap[Settings.Instance.themeColor];
                to_filter_layer.colors = colorBlock;
            }

            filter_layer.SetActive(true);

            //set items size
            filter_from_drop.GetComponent<RectTransform>().sizeDelta = new Vector2((SuperWidth - (16f + 16f + 40f)) * 0.4f, 52f);
            filter_from_min.GetComponent<RectTransform>().sizeDelta = new Vector2((SuperWidth - (16f + 16f + 40f)) * 0.3f, 48f);
            filter_from_max.GetComponent<RectTransform>().sizeDelta = new Vector2((SuperWidth - (16f + 16f + 40f)) * 0.3f, 48f);

            filter_to_drop.GetComponent<RectTransform>().sizeDelta = new Vector2((SuperWidth - (16f + 16f + 40f)) * 0.4f, 52f);
            filter_to_min.GetComponent<RectTransform>().sizeDelta = new Vector2((SuperWidth - (16f + 16f + 40f)) * 0.3f, 48f);
            filter_to_max.GetComponent<RectTransform>().sizeDelta = new Vector2((SuperWidth - (16f + 16f + 40f)) * 0.3f, 48f);

            filter_date_min.GetComponent<RectTransform>().sizeDelta = new Vector2((SuperWidth - (16f + 16f + 30f)) * 0.5f, 48f);
            filter_date_max.GetComponent<RectTransform>().sizeDelta = new Vector2((SuperWidth - (16f + 16f + 30f)) * 0.5f, 48f);


            filter_spacing.GetComponent<RectTransform>().sizeDelta = new Vector2(SuperWidth - (36f + 50f) * 3 - 16f * 2, 36f);

            SetBasicValueToFilter();

        }
    }

    private void SetBasicValueToFilter()
    {
        //filter_from_min.text = filtredDataList.FindAll(x => x.from > 18 && x.gender == "Male");
        double maxInputValue = double.MinValue;
        double minInputValue = double.MaxValue;
        double maxResultText = double.MinValue;
        double minResultText = double.MaxValue;
        DateTime maxDate = DateTime.MinValue;
        DateTime minDate = DateTime.MaxValue;

        foreach (var item in filtredDataList)
        {
            if (item.inputValue > maxInputValue)
                maxInputValue = item.inputValue;
            if (item.inputValue < minInputValue)
                minInputValue = item.inputValue;
            if (item.resultText > maxResultText)
                maxResultText = item.resultText;
            if (item.resultText < minResultText)
                minResultText = item.resultText;
            if (item.date > maxDate)
                maxDate = item.date;
            if (item.date < minDate)
                minDate = item.date;
        }

        filter_from_min.text = Convert.ToString(minInputValue);
        filter_from_max.text = Convert.ToString(maxInputValue);

        filter_to_min.text = Convert.ToString(minResultText);
        filter_to_max.text = Convert.ToString(maxResultText);

        filter_date_min.GetComponentInChildren<TMP_Text>().text = minDate.ToString("dd MMM yy", CultureInfo.CurrentCulture);
        filter_date_max.GetComponentInChildren<TMP_Text>().text = maxDate.ToString("dd MMM yy", CultureInfo.CurrentCulture);

        var uniqueConvertFromValues = filtredDataList.Select(data => data.convertFrom_drop).Distinct().ToList();
        string filter_from_drop_text = "";
        if (uniqueConvertFromValues.Count > 1)
        {
            filter_from_drop_text = SaveSystem.GetText("multiple_values");
        }
        else 
        { 
            foreach (var item in uniqueConvertFromValues)
            {
                filter_from_drop_text = item;
            }
        }

        foreach (Transform child in filter_from_drop_content.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in uniqueConvertFromValues)
        {
            GameObject item_temp = Instantiate(sample_drop_item, filter_from_drop_content.transform.position, Quaternion.identity, filter_from_drop_content.transform);
            item_temp.name = "item_temp";
            item_temp.GetComponentInChildren<TMP_Text>().text = item.ToString();
            item_temp.GetComponent<RectTransform>().sizeDelta = new Vector2(filter_from_drop.GetComponent<RectTransform>().rect.width, 28f);
        }

        filter_from_drop.GetComponentInChildren<TMP_Text>().text = filter_from_drop_text;
        filter_from_drop_value_layer.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 28f * (uniqueConvertFromValues.Count)+4f);




        var uniqueConvertToValues = filtredDataList.Select(data => data.convertTo_drop).Distinct().ToList();
        string filter_to_drop_text = "";
        if (uniqueConvertToValues.Count > 1)
        {
            filter_to_drop_text = SaveSystem.GetText("multiple_values");
        }
        else
        {
            foreach (var item in uniqueConvertToValues)
            {
                filter_to_drop_text = item;
            }
        }

        foreach (Transform child in filter_to_drop_content.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in uniqueConvertToValues)
        {
            GameObject item_temp = Instantiate(sample_drop_item, filter_to_drop_content.transform.position, Quaternion.identity, filter_to_drop_content.transform);
            item_temp.name = "item_temp";
            item_temp.GetComponentInChildren<TMP_Text>().text = item.ToString();
            item_temp.GetComponent<RectTransform>().sizeDelta = new Vector2(filter_to_drop.GetComponent<RectTransform>().rect.width, 28f);
        }

        filter_to_drop.GetComponentInChildren<TMP_Text>().text = filter_to_drop_text;
        filter_to_drop_value_layer.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 28f * (uniqueConvertToValues.Count) + 4f);
    }

    public void FilterFromDropOpenClose()
    {
        if (filter_from_drop_value_layer.activeSelf)
        {
            filter_from_drop_value_layer.SetActive(false);
        }
        else
        {
            filter_from_drop_value_layer.SetActive(true);
        }
    }

    public void FilterToDropOpenClose()
    {
        if (filter_to_drop_value_layer.activeSelf)
        {
            filter_to_drop_value_layer.SetActive(false);
        }
        else
        {
            filter_to_drop_value_layer.SetActive(true);
        }
    }

    public async void ApplyFilter()
    {
        Debug.Log("Apply filter");
        //await 
    }

    public void ResetFilter()
    {
        Debug.Log("Reset filter");
    }



    public void ForCloseDataLayer()
    {
        if (sort_layer.activeSelf)
            ToSortLayer();

        if (filter_layer.activeSelf)
            ToFilterLayer();

        SortDesc();
        sort_by_drop.value = 0;
    }

    private IEnumerator SetPaddingRight(GameObject tempObject)
    {
        yield return new WaitForEndOfFrame();
        tempObject.transform.localPosition = new Vector3(tempObject.transform.localPosition.x + (tempObject.gameObject.GetComponent<RectTransform>().rect.width / 2), tempObject.transform.localPosition.y, tempObject.transform.localPosition.z);   
    }

    public async void SetDataToList()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
            string filePath = Path.Combine(Application.persistentDataPath, "data.json");
        #else
            string filePath = Path.Combine(Application.dataPath, "data.json"); //путь к файлу с сейвами
        #endif

        dataList = new List<Data>();
        filtredDataList = new List<Data>();


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

            string json = await Task.Run(() => File.ReadAllText(filePath));
            dataList = JsonConvert.DeserializeObject<List<Data>>(json);
            filtredDataList = dataList;

            if (dataList.Count == 0)
            {
                NoDataIndicator();
            }
            else
            {
                index = 0;
                sort_filter.SetActive(true);
                await LoadDataAsync();
            }

        }
        else
        {
            NoDataIndicator();
        }

    }

    private void NoDataIndicator()
    {
        GameObject nodata = Instantiate(noDataIndicator, noDataIndicator.transform.position, Quaternion.identity, dataitemparent.transform);
        nodata.name = "nodataindicator";

        sort_filter.SetActive(false);
        loadingIndicator.SetActive(false);
        ValueLayer.SetActive(false);
        DataListLayer.SetActive(true);
    }

    private async Task LoadDataAsync()
    {
        loading = true;
        // Load portion of data
        List<Data> portionOfData = await GetDataPortionAsync(index,20);
        index += 20;
        // Create data items using portion of data
        foreach (Data data in portionOfData)
        {
            GameObject data_temp = Instantiate(dataitem, dataitem.transform.position, Quaternion.identity, dataitemparent.transform);
            data_temp.name = "dataitem";

            DataItem setdata = data_temp.GetComponent<DataItem>();
            setdata.dataValue.text = data.date.ToString("HH:mm\ndd MMM yy", CultureInfo.CurrentCulture);

            setdata.fromLabel.text = data.convertFrom_drop;
            setdata.fromValue.text = data.inputValue.ToString();

            setdata.toLabel.text = data.convertTo_drop;
            setdata.toValue.text = data.resultText.ToString();
        }
        ValueLayer.SetActive(false);
        DataListLayer.SetActive(true);
        StartCoroutine(UpdateLayoutGroup());
    }

    private async Task<List<Data>> GetDataPortionAsync(int startIndex, int count)
    {
        return await Task.Run(() =>
        {
            // Код загрузки данных
            count = Math.Min(count, filtredDataList.Count - startIndex);
            List<Data> portionOfData = new List<Data>();
            try
            {
                portionOfData = filtredDataList.GetRange(startIndex, count);
            }
            catch
            {
                Debug.Log("error load portion data");
            }
            return portionOfData;
            
        });
    }

    public async void DeleteData(int indexD)
    {
        loadingIndicator.SetActive(true);

    #if UNITY_ANDROID && !UNITY_EDITOR
            string filePath = Path.Combine(Application.persistentDataPath, "data.json");
    #else
        string filePath = Path.Combine(Application.dataPath, "data.json"); //путь к файлу с сейвами
#endif

        Data itemToRemove = filtredDataList[indexD];
        filtredDataList.RemoveAt(indexD);
        dataList.RemoveAll(item => ReferenceEquals(item, itemToRemove));

        index--;
        string newJson = JsonConvert.SerializeObject(dataList);
        await WriteTextAsync(filePath, newJson);

        if (dataList.Count == 0)
        {
            NoDataIndicator();
        }

        StartCoroutine(ToastShow("delete_data"));

        if (dataitemparent.transform.childCount < 10 && filtredDataList.Count > dataitemparent.transform.childCount)
        {
            Debug.Log("try to load");
            if (indexD < filtredDataList.Count)
            {
                loadingIndicator.SetActive(true);
                await LoadDataAsync();
            }
        }

        Destroy(dataitemparent.transform.GetChild(indexD).gameObject);
        StartCoroutine(UpdateLayoutGroup());
    }

    IEnumerator UpdateLayoutGroup()
    {
        yield return new WaitForEndOfFrame();

        forcerebuildlayer.gameObject.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();
        LayoutRebuilder.ForceRebuildLayoutImmediate(forcerebuildlayer.gameObject.GetComponent<RectTransform>());

        yield return new WaitForEndOfFrame();
        loadingIndicator.SetActive(false);
        loading = false;
    }

    private IEnumerator ToastShow(string temptext)
    {
        Toast.SetActive(true);
        string toasttext = SaveSystem.GetText(temptext);
        Toast.GetComponentInChildren<TMP_Text>().text = toasttext;
        yield return new WaitForSecondsRealtime(2f);
        Toast.SetActive(false);
    }

    public void ToDataList()
    {
        loadingIndicator.SetActive(true);
        SetDataToList();
    }

    public void CollectData()
    {
        string input = Convert.ToString(calculations.doubleInput);

        Data data = new Data
        {
            inputValue = Convert.ToDouble(input),
            resultText = Convert.ToDouble(resultText.text),
            convertFrom_drop = dropStrings[convertFrom_drop.value],
            convertTo_drop = dropStrings[convertTo_drop.value],
            date = DateTime.Now
        };       

        SaveData(data);
    }

    public async void SaveData(Data data)
    {
        loadingIndicator.SetActive(true);

        #if UNITY_ANDROID && !UNITY_EDITOR
            string filePath = Path.Combine(Application.persistentDataPath, "data.json");
        #else
            string filePath = Path.Combine(Application.dataPath, "data.json"); //путь к файлу с сейвами
        #endif

        List<Data> dataListforsave = new List<Data>();
        if (File.Exists(filePath))
        {   
            string json = await ReadFileAsync(filePath);
            dataListforsave = JsonConvert.DeserializeObject<List<Data>>(json);
        }
        dataListforsave.Insert(0, data);
        string newJson = JsonConvert.SerializeObject(dataListforsave);

        await WriteTextAsync(filePath, newJson);
        loadingIndicator.SetActive(false);

        StartCoroutine(ToastShow("save_data"));
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
