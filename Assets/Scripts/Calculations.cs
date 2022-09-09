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
    public TMP_Dropdown whatConvert_drop;
    public TMP_Dropdown convertValue_drop;

    private string[,] formulas;
    double doubleInput;

    private double staticvalue;


    private void Awake() //запускается до всех стартов
    {
        FillFormulas();
    }

    private void FillFormulas()
    {
        formulas = new string[,]
        {
            {"km/l","km/l", "1"},
            {"km/l","l/100km", "100"},
            {"km/l","mpg (UK)", "2.8248093627967"},
            {"km/l","mpg (US)", "2.3521458329476"},
            {"km/l","mi/l", "0.62137119223735"},

            {"l/100km","km/l", "100"},
            {"l/100km","l/100km", "1"},
            {"l/100km","mpg (UK)", "282.48093627967"},
            {"l/100km","mpg (US)", "235.21458329475"},
            {"l/100km","mi/l", "62.137119223734"},

            {"mpg (UK)","km/l", "0.35400619"},
            {"mpg (UK)","l/100km", "282.48093627967"},
            {"mpg (UK)","mpg (UK)", "1"},
            {"mpg (UK)","mpg (US)", "0.83267418464614"},
            {"mpg (UK)","mi/l", "0.2199692483397"},

            {"mpg (US)","km/l", "0.4251437075"},
            {"mpg (US)","l/100km", "235.21458329475"},
            {"mpg (US)","mpg (UK)", "1.2009499254801"},
            {"mpg (US)","mpg (US)", "1"},
            {"mpg (US)","mi/l", "0.26417205240148"},

            {"mi/l","km/l", "1.609344"},
            {"mi/l","l/100km", "62.137119223734"},
            {"mi/l","mpg (UK)", "4.5460899991607"},
            {"mi/l","mpg (US)", "3.7854117833791"},
            {"mi/l","mi/l", "1"},
        };
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
        Settings.Instance.input = inputValue.text;
    }

    public void FindStaticvalue()
    {
        //при старте или смене что и во что конвертировать ищем формулу-число на которое множить или которое делить, присваиваем его отдельной переменной, конвертим из стринга в дабл
    }


    

    /* string lang = Settings.Instance.language; //грузим какой язык

     int v = Array.IndexOf(myLangs, lang);
     lang_drop.value = v;

         lang_drop.onValueChanged.AddListener(delegate { //ставим его в дропдовн меню
             index = lang_drop.value;
             Settings.Instance.language = myLangs[index];
             ApplyLanguageChanges(); //применяем настройки смены языка при выборе другого
 });
    void ApplyLanguageChanges() //применяем настройки смены языка при выборе другого
    {
        SaveSystem.Instance.SettingsSave(); //сохраняем настройки с новым языком
        string lvl = SceneManager.GetActiveScene().name; //получаем имя активной сцены
        SceneManager.LoadScene(lvl); //и загружаем ее заново
    }*/
}
