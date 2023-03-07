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

public class InputChecker : MonoBehaviour
{
    public TMP_InputField inputField;
    public double doubleInput;

    public void OnInputChange() //��� ����� ����
    {
        try
        {
            doubleInput = Convert.ToDouble(inputField.text);
        }
        catch
        {
            //Debug.Log("�� ������������ ������ ���");
            inputField.text = Regex.Replace(inputField.text, "[^0-9.,]", "");

            if (inputField.text.Count(x => x == '.') + inputField.text.Count(x => x == ',') > 1)
            {
                //Debug.Log("����� ��� ����� ��� �������");
                int lastIndex = inputField.text.LastIndexOfAny(new char[] { '.', ',' });
                if (inputField.text.Substring(lastIndex).Contains(",") || inputField.text.Substring(lastIndex).Contains("."))
                {
                    //Debug.Log("����� ������ ����� ��� �������");
                    int firstIndex = inputField.text.IndexOfAny(new char[] { '.', ',' });
                    inputField.text = inputField.text.Remove(firstIndex, 1);
                }
            }
            try
            {
                //Debug.Log("������� �������������� � ���� ������ ���");
                doubleInput = Convert.ToDouble(inputField.text.Replace('.', ','));
            }
            catch
            {
                // Debug.Log("�� ������������ ������ ���");
                doubleInput = 0;
                inputField.text = "";
            }
        }
    }
}
