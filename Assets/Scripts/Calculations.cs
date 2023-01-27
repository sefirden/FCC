using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Calculations : MonoBehaviour
{
    double doubleInput;
    public double staticvalue;
    private bool languageLabel;
    private UI ui; //������ ��
    private List<FormulaList> formulas;

    private void Awake() //����������� �� ���� �������
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
                slider_temp.name = "slider_temp"; //����������� ���
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
        if (Settings.Instance.sliderNumbersQ < 5 && Settings.Instance.decimalSlider > 0)
        {
            TMP_Text number = ui.slidersForInput[Settings.Instance.sliderNumbersQ].transform.Find("firstNumber").GetComponent<TMP_Text>();
            TMP_Text decimal_t = ui.slidersForInput[Settings.Instance.sliderNumbersQ].transform.Find("decimal").GetComponent<TMP_Text>();

            while(number.fontSize == 18f)
            {
                yield return new WaitForFixedUpdate();
            }
            decimal_t.fontSize = number.fontSize;///1.5f;

            // RectTransform rt = decimal_t.GetComponent<RectTransform>();
            number.GetComponent<RectTransform>().offsetMax = new Vector2(-1, number.GetComponent<RectTransform>().offsetMax.y);
            number.GetComponent<RectTransform>().offsetMin = new Vector2(9, number.GetComponent<RectTransform>().offsetMin.y);

           Debug.Log(number.textInfo.characterInfo[0].ascender);


            decimal_t.GetComponent<RectTransform>().offsetMax = new Vector2(-number.textInfo.characterInfo[0].ascender/2f, number.GetComponent<RectTransform>().offsetMax.y);
            decimal_t.GetComponent<RectTransform>().offsetMin = new Vector2(-number.textInfo.characterInfo[0].ascender/2f, number.GetComponent<RectTransform>().offsetMin.y);

            // rt.localPosition -= new Vector3(ui.slidersForInput[Settings.Instance.sliderNumbersQ].GetComponent<RectTransform>().rect.width/2.5f,0,0);
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
            ui.resultText.text = "Enter valid data";
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

    public void OnInputChange() //��� ����� ����
    {        
        try
        {
            doubleInput = Convert.ToDouble(ui.inputValue.text);            
        }
        catch
        {
            ui.inputValue.text = ui.inputValue.text.Replace('.', ',');
            try
            {
                doubleInput = Convert.ToDouble(ui.inputValue.text);
            }
            catch
            {
                doubleInput = 0;
                ui.inputValue.text = "";
            }
        }
        if (doubleInput != 0)
        {
            ConvertValue();
        }
        else
        {
            ui.resultText.text = "Enter valid data";
        }
    }

    public void ConvertValue()
    {
        double value;
        //��� �������� ��������� ���� ����� ������ 62 �� ����� ��� �� ��������� ��������, ���� ������ �� ������
        if (staticvalue > 62)
        {
            value = Math.Round(staticvalue / doubleInput, Settings.Instance.decimalPlaces);
        }
        else
        {
            value = Math.Round(doubleInput * staticvalue, Settings.Instance.decimalPlaces);
        }
        
        string result = value.ToString();
        ui.resultText.text = result;

        if (Settings.Instance.inputLayer)
        {
            Settings.Instance.inputValue = ui.inputValue.text;
        }
    }

    public void FindStaticvalue()
    {
        //��� ������ ��� ����� ��� � �� ��� �������������� ���� �������-����� �� ������� ������� ��� ������� ������, ����������� ��� ��������� ����������, ��������� �� ������� � ����
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
    }

    public void Quit() //����� �� ����
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
    public FormulaList(string F, string T, double Fr)//� ����� ������� ��������� ������ ��� �����
    {
        ConvertFrom = F;
        ConvertTo = T;
        Formula = Fr;
    }
}
