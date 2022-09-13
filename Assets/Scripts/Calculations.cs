using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Calculations : MonoBehaviour
{
    public TMP_InputField inputValue;
    public TMP_Text resultText;
    public TMP_Dropdown convertFrom_drop;
    public TMP_Dropdown convertTo_drop;
    public Slider decimalPlaces_slider;

    private List<FormulaList> formulas;
    double doubleInput;

    public double staticvalue;

    public List<string> dropStrings;

    private void Awake() //запускается до всех стартов
    {
        FillFormulas();

        inputValue.text = Settings.Instance.inputValue;
        decimalPlaces_slider.value = Settings.Instance.decimalPlaces;

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

        FindStaticvalue();
        if (inputValue.text !="")
        ConvertValue();
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

    public void OnInputChange()
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

    private void ConvertValue()
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
        Settings.Instance.inputValue = inputValue.text;
    }

    public void FindStaticvalue()
    {
        //при старте или смене что и во что конвертировать ищем формулу-число на которое множить или которое делить, присваиваем его отдельной переменной, конвертим из стринга в дабл
        FormulaList find = formulas.Find(x => x.ConvertFrom == Settings.Instance.convertFrom && x.ConvertTo == Settings.Instance.convertTo);
        staticvalue = find.Formula;
    }

    public void ChangeDecimalPlaces()
    {
        Settings.Instance.decimalPlaces = Convert.ToInt32(decimalPlaces_slider.value);
        ConvertValue();
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
