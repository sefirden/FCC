using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Calculations : MonoBehaviour
{
    public double doubleInput;
    public double staticvalue;
    private bool languageLabel;
    private UI ui; //скрипт уи
    private List<FormulaList> formulas;

    private void Awake() //запускается до всех стартов
    {
        ui = FindObjectOfType<UI>();
        languageLabel = true;
        FillFormulas();

        ui.convertFrom_drop.AddOptions(ui.dropStrings);
        ui.convertFrom_drop.value = ui.dropStrings.IndexOf(Settings.Instance.convertFrom);
        ui.convertFrom_drop.onValueChanged.AddListener(delegate {
            Settings.Instance.convertFrom = ui.dropStrings[ui.convertFrom_drop.value];
            FindStaticvalue();
            ConvertValue();
        });

        ui.convertTo_drop.AddOptions(ui.dropStrings);
        ui.convertTo_drop.value = ui.dropStrings.IndexOf(Settings.Instance.convertTo);
        ui.convertTo_drop.onValueChanged.AddListener(delegate
        {
            Settings.Instance.convertTo = ui.dropStrings[ui.convertTo_drop.value];
            FindStaticvalue();
            ConvertValue();
        });

        if (Settings.Instance.inputLayer)
        {
            ui.ClassicInput.SetActive(true);
            ui.SliderInput.SetActive(false);

            ui.inputValue.text = Settings.Instance.inputValue;
            if (ui.inputValue.text != "")
                ConvertValue();
        }
        else
        {
            ui.ClassicInput.SetActive(false);
            ui.SliderInput.SetActive(true);

            SetSliderInput();
            OnSliderChange();
        }

        FindStaticvalue();
    }

    private void Start()
    {
        if(Settings.Instance.toSettings)
        ToSettings();
        Settings.Instance.toSettings = false;
    }

    public void SetSliderInput()
    {
        ui.slidersForInput.Clear();
        try
        {
            foreach (Transform child in ui.SliderInput.transform)
            {
                Destroy(child.gameObject);
            }
        }
        catch
        {
            Debug.LogError("No child found");
        }

        if (ui.slidersForInput.Count < Settings.Instance.sliderNumbersQ + Settings.Instance.decimalSlider)
        {
            for (int i = 0; i < (Settings.Instance.sliderNumbersQ + Settings.Instance.decimalSlider); i++)
            {
                GameObject slider_temp = Instantiate(ui.sampleSliderInput, ui.sampleSliderInput.transform.position, Quaternion.identity, ui.SliderInput.transform);
                slider_temp.name = "slider_temp"; //присваиваем имя
                ui.slidersForInput.Add(slider_temp);
            }
        }

        if (Settings.Instance.inputSliderValue != "0")
        {
            char[] characters = Settings.Instance.inputSliderValue.ToCharArray();

            if (characters.Length < Settings.Instance.sliderNumbersQ + Settings.Instance.decimalSlider)
            {
                for (int i = 0; i < characters.Length; i++)
                {
                    ui.slidersForInput[i].GetComponentInChildren<TMP_Text>().text = Convert.ToString(characters[i]);
                }
            }
            else
            {
                for (int i = 0; i < (Settings.Instance.sliderNumbersQ + Settings.Instance.decimalSlider); i++)
                {
                    ui.slidersForInput[i].GetComponentInChildren<TMP_Text>().text = Convert.ToString(characters[i]);
                }
            }
        }        
        StartCoroutine(SetSliderDecimal());
    }

    public IEnumerator SetSliderDecimal()
    {
        yield return new WaitForFixedUpdate();
        float sizeDecimal = 0f;

        if (Settings.Instance.sliderNumbersQ < 5 && Settings.Instance.decimalSlider > 0)
        {
            GameObject decimal_temp = Instantiate(ui.DecimalPoint, ui.DecimalPoint.transform.position, Quaternion.identity, ui.SliderInput.transform);
            decimal_temp.name = "decimal_temp"; //присваиваем имя

            decimal_temp.gameObject.GetComponent<Transform>().SetSiblingIndex(Settings.Instance.sliderNumbersQ);
            sizeDecimal = decimal_temp.GetComponent<RectTransform>().sizeDelta.x - (Settings.Instance.sliderNumbersQ + Settings.Instance.decimalSlider)*5;
            decimal_temp.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeDecimal, sizeDecimal);
        }

        SetWidthSlider(sizeDecimal);
    }

    private void SetWidthSlider(float sizeDecimal)
    {
        float width = ui.ValueLayer.GetComponent<RectTransform>().rect.width;
        float height = ui.slidersForInput[0].GetComponent<RectTransform>().sizeDelta.y;
        float spacing = ui.SliderInput.GetComponent<HorizontalLayoutGroup>().spacing * (ui.SliderInput.transform.childCount-1);

        for (int i = 0; i < (Settings.Instance.sliderNumbersQ + Settings.Instance.decimalSlider); i++)
        {
            ui.slidersForInput[i].GetComponent<RectTransform>().sizeDelta = new Vector2(((width - spacing - sizeDecimal) / (Settings.Instance.sliderNumbersQ + Settings.Instance.decimalSlider)), height);
        }
    }

    public void OnSliderChange()
    {
        string temp = "";
        for (int i = 0; i < ui.slidersForInput.Count; i++)
        {
            temp += ui.slidersForInput[i].GetComponentInChildren<TMP_Text>().text;
        }

        try
        {
            doubleInput = Convert.ToDouble(temp);
        }
        catch
        {
            doubleInput = 0;            
        }

        if (doubleInput != 0)
        {
            Settings.Instance.inputSliderValue = temp;
            if(Settings.Instance.decimalSlider != 0)
            {
                for(int i = 0; i < Settings.Instance.decimalSlider; i++)
                doubleInput /= 10;
            }            
            ConvertValue();
        }
        else
        {
            ui.resultText.text = SaveSystem.GetText("valid_data");
        }
    }

    public void ChangeFromTo()
    {
        string tempFrom = Settings.Instance.convertFrom;
        ui.convertFrom_drop.value = ui.dropStrings.IndexOf(Settings.Instance.convertTo);
        ui.convertTo_drop.value = ui.dropStrings.IndexOf(tempFrom);
    }

    private void FillFormulas()
    {
        formulas = new List<FormulaList>();

        formulas.Add(new FormulaList("km/l", "km/l", 1));
        formulas.Add(new FormulaList("km/l", "l/100km", 100));
        formulas.Add(new FormulaList("km/l", "mpg (UK)", 2.8248093627967));
        formulas.Add(new FormulaList("km/l", "mpg (US)", 2.3521458329476));
        formulas.Add(new FormulaList("km/l", "mi/l", 0.62137119223735));

        formulas.Add(new FormulaList("l/100km", "km/l", 100));
        formulas.Add(new FormulaList("l/100km", "l/100km", 1));
        formulas.Add(new FormulaList("l/100km", "mpg (UK)", 282.48093627967));
        formulas.Add(new FormulaList("l/100km", "mpg (US)", 235.21458329475));
        formulas.Add(new FormulaList("l/100km", "mi/l", 62.137119223734));

        formulas.Add(new FormulaList("mpg (UK)", "km/l", 0.35400619));
        formulas.Add(new FormulaList("mpg (UK)", "l/100km", 282.48093627967));
        formulas.Add(new FormulaList("mpg (UK)", "mpg (UK)", 1));
        formulas.Add(new FormulaList("mpg (UK)", "mpg (US)", 0.83267418464614));
        formulas.Add(new FormulaList("mpg (UK)", "mi/l", 0.2199692483397));

        formulas.Add(new FormulaList("mpg (US)", "km/l", 0.4251437075));
        formulas.Add(new FormulaList("mpg (US)", "l/100km", 235.21458329475));
        formulas.Add(new FormulaList("mpg (US)", "mpg (UK)", 1.2009499254801));
        formulas.Add(new FormulaList("mpg (US)", "mpg (US)", 1));
        formulas.Add(new FormulaList("mpg (US)", "mi/l", 0.26417205240148));

        formulas.Add(new FormulaList("mi/l", "km/l", 1.609344));
        formulas.Add(new FormulaList("mi/l", "l/100km", 62.137119223734));
        formulas.Add(new FormulaList("mi/l", "mpg (UK)", 4.5460899991607));
        formulas.Add(new FormulaList("mi/l", "mpg (US)", 3.7854117833791));
        formulas.Add(new FormulaList("mi/l", "mi/l", 1));
        
    }

    public void OnInputChange() //для инпут поля
    {
        try
        {
            doubleInput = Convert.ToDouble(ui.inputValue.text);
            //Debug.Log("пробуем конвертировать в дабл");
        }
        catch
        {
            //Debug.Log("не конвертирует первый раз");
            ui.inputValue.text = Regex.Replace(ui.inputValue.text, "[^0-9.,]", "");

            if (ui.inputValue.text.Count(x => x == '.') + ui.inputValue.text.Count(x => x == ',') > 1)
            {
                //Debug.Log("нашли две точки или запятые");
                int lastIndex = ui.inputValue.text.LastIndexOfAny(new char[] { '.', ',' });
                if (ui.inputValue.text.Substring(lastIndex).Contains(",") || ui.inputValue.text.Substring(lastIndex).Contains("."))
                {
                    //Debug.Log("нашли вторую точку или запятую");
                    int firstIndex = ui.inputValue.text.IndexOfAny(new char[] { '.', ',' });
                    ui.inputValue.text = ui.inputValue.text.Remove(firstIndex, 1);
                }
            }
                try
            {
                //Debug.Log("пробуем конвертировать в дабл второй раз");
                doubleInput = Convert.ToDouble(ui.inputValue.text.Replace('.', ','));
            }
            catch
            {
               // Debug.Log("не конвертирует второй раз");
                doubleInput = 0;
                ui.inputValue.text = "";
            }
        }
        if (doubleInput != 0)
        {
           // Debug.Log("конвертируем");
            ConvertValue();
        }
        else
        {
           // Debug.Log("ничего не введено или введен 0");
            ui.resultText.text = SaveSystem.GetText("valid_data");
        }
    }

    public void ConvertValue()
    {
        double value;
        //при расчетах проверяем если число больше 62 то делим его на ввоодимое значение, если меньше то множим
        if (staticvalue > 62)
        {
            value = Math.Round(staticvalue / doubleInput, Settings.Instance.decimalPlaces);
        }
        else
        {
            value = Math.Round(doubleInput * staticvalue, Settings.Instance.decimalPlaces);
        }
        // получаем текущую культуру
        CultureInfo cultureInfo = CultureInfo.CurrentCulture;

        // устанавливаем формат вывода чисел
        NumberFormatInfo numberFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
        numberFormat.NumberDecimalDigits = Settings.Instance.decimalPlaces; // количество знаков после запятой
        string result = value.ToString("N", numberFormat);
        ui.resultText.text = result;

        if (Settings.Instance.inputLayer)
        {
            Settings.Instance.inputValue = ui.inputValue.text;
        }
    }

    public void FindStaticvalue()
    {
        //при старте или смене что и во что конвертировать ищем формулу-число на которое множить или которое делить, присваиваем его отдельной переменной, конвертим из стринга в дабл
        FormulaList find = formulas.Find(x => x.ConvertFrom == Settings.Instance.convertFrom && x.ConvertTo == Settings.Instance.convertTo);
        staticvalue = find.Formula;
    }

    public void ToSettings()
    {
        ui.SettingsLayer.SetActive(true);
        if (languageLabel)
        { 
            StartCoroutine(SetPaddingRight());
        }
        ui.ValueLayer.SetActive(false);
    }

    public IEnumerator SetPaddingRight()
    {
        yield return new WaitForEndOfFrame();
        ui.language_drop_fieldName.transform.localPosition = new Vector3(ui.language_drop_fieldName.transform.localPosition.x + (ui.language_drop_fieldName.gameObject.GetComponent<RectTransform>().rect.width / 2), ui.language_drop_fieldName.transform.localPosition.y, ui.language_drop_fieldName.transform.localPosition.z);
        ui.color_drop_fieldName.transform.localPosition = new Vector3(ui.color_drop_fieldName.transform.localPosition.x + (ui.color_drop_fieldName.gameObject.GetComponent<RectTransform>().rect.width / 2), ui.color_drop_fieldName.transform.localPosition.y, ui.color_drop_fieldName.transform.localPosition.z);
        languageLabel = false;
    }

    public void BackToInput()
    {
        ui.ValueLayer.SetActive(true);
        ui.SettingsLayer.SetActive(false);
        ui.DataListLayer.SetActive(false);

        if (Settings.Instance.inputLayer)
        {
            ui.ClassicInput.SetActive(true);
            ui.SliderInput.SetActive(false);

            ui.inputValue.text = Settings.Instance.inputValue;
        }
        else
        {
            ui.ClassicInput.SetActive(false);
            ui.SliderInput.SetActive(true);

            SetSliderInput();
            OnSliderChange();
        }
        ui.ResetSort();

    }

    public void Quit() //выход из игры
    {
        Application.Quit();
    }

}

public class FormulaList
{
    public string ConvertFrom
    {
        get;
        set;
    }
    public string ConvertTo
    {
        get;
        set;
    }
    public double Formula
    {
        get;
        set;
    } 
    public FormulaList(string F, string T, double Fr)//в таком порядке заполняем данные для пиццы
    {
        ConvertFrom = F;
        ConvertTo = T;
        Formula = Fr;
    }
}
