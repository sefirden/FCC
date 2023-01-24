using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetColor : MonoBehaviour
{
    public UI ui; //скрипт уи
    public bool text;
    public bool darkText;
    public bool image;
    public bool darkImage;
    public bool allColors;



    private ColorBlock colorBlock;

    void Awake()
    {
        ui = FindObjectOfType<UI>();

        if (text)
            ColorText();

        if (image)
            ColorImage();

        if (allColors)
            ColorAllColors();
    }

    private void ColorText()
    {
        if(Settings.Instance.darkMode)
            gameObject.GetComponent<TMP_Text>().color = ui.DarkModeColors[1];
        else
        {
            if(darkText)
                gameObject.GetComponent<TMP_Text>().color = ui.DarkModeColors[2];
            else
                gameObject.GetComponent<TMP_Text>().color = ui.ColorSwap[Settings.Instance.themeColor];
        }
            
    }

    private void ColorImage()
    {
        if (Settings.Instance.darkMode)
        {
            if (gameObject.transform.name != "Background")
            {
                if(darkImage)
                    gameObject.GetComponent<Image>().color = ui.DarkModeColors[0];
                else
                    gameObject.GetComponent<Image>().color = ui.DarkModeColors[1];
            }
            else
            {
                Color temp = new Color(ui.DarkModeColors[1].r, ui.DarkModeColors[1].g, ui.DarkModeColors[1].b, 0.5f);
                gameObject.GetComponent<Image>().color = temp;
            }
        }
        else
        {
            if (gameObject.transform.name != "Background")
            {
                if (darkImage)
                    gameObject.GetComponent<Image>().color = ui.DarkModeColors[1];
                else
                    gameObject.GetComponent<Image>().color = ui.ColorSwap[Settings.Instance.themeColor];
            }            
            else
            {
                Color temp = new Color(ui.ColorSwap[Settings.Instance.themeColor].r, ui.ColorSwap[Settings.Instance.themeColor].g, ui.ColorSwap[Settings.Instance.themeColor].b, 0.5f);
                gameObject.GetComponent<Image>().color = temp;
            }
        }
    }



    private void ColorAllColors()
    {
        if (Settings.Instance.darkMode)
        {
            try
            {
                TMP_InputField temp;
                temp = gameObject.GetComponent<TMP_InputField>();
                colorBlock = temp.colors;
                colorBlock.normalColor = ui.DarkModeColors[1];
                colorBlock.highlightedColor = ui.DarkModeColors[1];
                colorBlock.selectedColor = ui.DarkModeColors[1];
                temp.colors = colorBlock;
            }
            catch
            {
                try
                {
                    TMP_Dropdown temp;
                    temp = gameObject.GetComponent<TMP_Dropdown>();
                    colorBlock = temp.colors;
                    colorBlock.normalColor = ui.DarkModeColors[1];
                    colorBlock.highlightedColor = ui.DarkModeColors[1];
                    colorBlock.selectedColor = ui.DarkModeColors[1];
                    temp.colors = colorBlock;
                }
                catch
                {
                    try
                    {
                        Button temp;
                        temp = gameObject.GetComponent<Button>();
                        colorBlock = temp.colors;
                        colorBlock.normalColor = ui.DarkModeColors[1];
                        colorBlock.highlightedColor = ui.DarkModeColors[1];
                        colorBlock.selectedColor = ui.DarkModeColors[1];
                        temp.colors = colorBlock;
                    }
                    catch
                    {
                        Slider temp;
                        temp = gameObject.GetComponent<Slider>();
                        colorBlock = temp.colors;
                        colorBlock.normalColor = ui.DarkModeColors[1];
                        colorBlock.highlightedColor = ui.DarkModeColors[1];
                        colorBlock.selectedColor = ui.DarkModeColors[1];
                        colorBlock.pressedColor = ui.DarkModeColors[1];
                        temp.colors = colorBlock;
                    }
                }
            }
        }
        else
        {
            try
            {
                TMP_InputField temp;
                temp = gameObject.GetComponent<TMP_InputField>();
                colorBlock = temp.colors;
                colorBlock.normalColor = ui.ColorSwap[Settings.Instance.themeColor];
                colorBlock.highlightedColor = ui.ColorSwap[Settings.Instance.themeColor];
                colorBlock.selectedColor = ui.ColorSwap[Settings.Instance.themeColor];
                temp.colors = colorBlock;
            }
            catch
            {
                try
                {
                    TMP_Dropdown temp;
                    temp = gameObject.GetComponent<TMP_Dropdown>();
                    colorBlock = temp.colors;
                    colorBlock.normalColor = ui.ColorSwap[Settings.Instance.themeColor];
                    colorBlock.highlightedColor = ui.ColorSwap[Settings.Instance.themeColor];
                    colorBlock.selectedColor = ui.ColorSwap[Settings.Instance.themeColor];
                    temp.colors = colorBlock;
                }
                catch
                {
                    try
                    {
                        Button temp;
                        temp = gameObject.GetComponent<Button>();
                        colorBlock = temp.colors;
                        colorBlock.normalColor = ui.ColorSwap[Settings.Instance.themeColor];
                        colorBlock.highlightedColor = ui.ColorSwap[Settings.Instance.themeColor];
                        colorBlock.selectedColor = ui.ColorSwap[Settings.Instance.themeColor];
                        temp.colors = colorBlock;
                    }
                    catch
                    {
                        Slider temp;
                        temp = gameObject.GetComponent<Slider>();
                        colorBlock = temp.colors;
                        colorBlock.normalColor = ui.ColorSwap[Settings.Instance.themeColor];
                        colorBlock.highlightedColor = ui.ColorSwap[Settings.Instance.themeColor];
                        colorBlock.selectedColor = ui.ColorSwap[Settings.Instance.themeColor];
                        colorBlock.pressedColor = ui.ColorSwap[Settings.Instance.themeColor];
                        temp.colors = colorBlock;
                    }
                }
            }
        }

    }

}
