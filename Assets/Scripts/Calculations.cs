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
    public TMP_InputField inputValue;
    public TMP_Text resultText;
    public TMP_Dropdown convertFrom_drop;
    public TMP_Dropdown convertTo_drop;
    

    private List<FormulaList> formulas;
    double doubleInput;

    public GameObject SettingsLayer;
    public GameObject ValueLayer;

    public GameObject ClassicInput;
    public GameObject SliderInput;

    public double staticvalue;

    public List<string> dropStrings;

    public GameObject sampleSliderInput;
    public List<GameObject> slidersForInput;

    private void Awake() //запускается до всех стартов
    {
        FillFormulas();      

        convertFrom_drop.AddOptions(dropStrings);
        convertFrom_drop.value = dropStrings.IndexOf(Settings.Instance.convertFrom);
        convertFrom_drop.onValueChanged.AddListener(delegate {
            Settings.Instance.convertFrom = dropStrings[convertFrom_drop.value];
            FindStaticvalue();
            ConvertValue();
        });

        convertTo_drop.AddOptions(dropStrings);
        convertTo_drop.value = dropStrings.IndexOf(Settings.Instance.convertTo);
        convertTo_drop.onValueChanged.AddListener(delegate
        {
            Settings.Instance.convertTo = dropStrings[convertTo_drop.value];
            FindStaticvalue();
            ConvertValue();
        });

        if (Settings.Instance.inputLayer)
        {
            ClassicInput.SetActive(true);
            SliderInput.SetActive(false);

            inputValue.text = Settings.Instance.inputValue;
            if (inputValue.text != "")
                ConvertValue();
        }
        else
        {
            ClassicInput.SetActive(false);
            SliderInput.SetActive(true);

            SetSliderInput();
            OnSliderChange();
        }

        FindStaticvalue();
    }


    public void SetSliderInput()
    {
        slidersForInput.Clear();
        try
        {
            foreach (Transform child in SliderInput.transform)
            {
                Destroy(child.gameObject);
            }
        }
        catch
        {
            Debug.LogError("No child found");
        }

        if (slidersForInput.Count < Settings.Instance.sliderNumbersQ + Settings.Instance.decimalSlider)
        {
            for (int i = 0; i < (Settings.Instance.sliderNumbersQ + Settings.Instance.decimalSlider); i++)
            {
                GameObject slider_temp = Instantiate(sampleSliderInput, sampleSliderInput.transform.position, Quaternion.identity, SliderInput.transform);
                slider_temp.name = "slider_temp"; //присваиваем имя
                slidersForInput.Add(slider_temp);
            }
        }

        if (Settings.Instance.inputSliderValue != "0")
        {
            char[] characters = Settings.Instance.inputSliderValue.ToCharArray();

            if (characters.Length < Settings.Instance.sliderNumbersQ + Settings.Instance.decimalSlider)
            {
                for (int i = 0; i < characters.Length; i++)
                {
                    slidersForInput[i].GetComponentInChildren<TMP_Text>().text = Convert.ToString(characters[i]);
                }
            }
            else
            {
                for (int i = 0; i < (Settings.Instance.sliderNumbersQ + Settings.Instance.decimalSlider); i++)
                {
                    slidersForInput[i].GetComponentInChildren<TMP_Text>().text = Convert.ToString(characters[i]);
                }
            }
        }
        StartCoroutine(SetSliderDecimal());
    }

    public IEnumerator SetSliderDecimal()
    {
        if (Settings.Instance.sliderNumbersQ < 5 && Settings.Instance.decimalSlider > 0)
        {
            TMP_Text number = slidersForInput[Settings.Instance.sliderNumbersQ - 1].transform.Find("firstNumber").GetComponent<TMP_Text>();
            TMP_Text decimal_t = slidersForInput[Settings.Instance.sliderNumbersQ - 1].transform.Find("decimal").GetComponent<TMP_Text>();

            while(number.fontSize == 18f)
            {
                yield return new WaitForFixedUpdate();
            }            
            decimal_t.fontSize = number.fontSize;

            RectTransform rt = decimal_t.GetComponent<RectTransform>();
            rt.localPosition += new Vector3(slidersForInput[Settings.Instance.sliderNumbersQ - 1].GetComponent<RectTransform>().rect.width/2,0,0);
        }
    }


    public void OnSliderChange()
    {
        string temp = "";
        for (int i = 0; i < slidersForInput.Count; i++)
        {
            temp += slidersForInput[i].GetComponentInChildren<TMP_Text>().text;
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
            resultText.text = "Enter valid data";
        }
    }

    public void ChangeFromTo()
    {
        string tempFrom = Settings.Instance.convertFrom;
        convertFrom_drop.value = dropStrings.IndexOf(Settings.Instance.convertTo);
        convertTo_drop.value = dropStrings.IndexOf(tempFrom);
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
            doubleInput = Convert.ToDouble(inputValue.text);            
        }
        catch
        {
            inputValue.text = inputValue.text.Replace('.', ',');
            try
            {
                doubleInput = Convert.ToDouble(inputValue.text);
            }
            catch
            {
                doubleInput = 0;
                inputValue.text = "";
            }
        }
        if (doubleInput != 0)
        {
            ConvertValue();

        }
        else
        {
            resultText.text = "Enter valid data";
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
        
        string result = value.ToString();
        resultText.text = result;

        if (Settings.Instance.inputLayer)
        {
            Settings.Instance.inputValue = inputValue.text;
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
        SettingsLayer.SetActive(true);
        ValueLayer.SetActive(false);
    }

    public void BackToInput()
    {
        ValueLayer.SetActive(true);
        SettingsLayer.SetActive(false);

        if (Settings.Instance.inputLayer)
        {
            ClassicInput.SetActive(true);
            SliderInput.SetActive(false);

            inputValue.text = Settings.Instance.inputValue;
        }
        else
        {
            ClassicInput.SetActive(false);
            SliderInput.SetActive(true);

            SetSliderInput();
            OnSliderChange();
        }
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
