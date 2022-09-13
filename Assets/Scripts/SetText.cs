using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetText : MonoBehaviour
{

    public string text_id;

    void Start()
    {
        GetComponent<TMP_Text>().text = SaveSystem.GetText(text_id); //получаем текст из файлов игры
    }

}
