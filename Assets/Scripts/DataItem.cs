using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// —крипт элемента списка данных

public class DataItem : MonoBehaviour
{
    private UI ui;                  // ui - ссылка на скрипт UI, используетс€ дл€ удалени€ элемента из списка данных
    public GameObject date;         // date, from, to - пол€, представл€ющие данные, которые нужно отобразить    
    public GameObject from;
    public GameObject to;
    public TMP_Text dataValue;      // dataValue, fromValue, toValue - текстовые пол€ дл€ отображени€ соответствующих значений
    public TMP_Text fromLabel;
    public TMP_Text fromValue;
    public TMP_Text toLabel;        // fromLabel, toLabel - текстовые пол€ дл€ отображени€ подписей к соответствующим пол€м
    public TMP_Text toValue;

    void Start()
    {
        ui = FindObjectOfType<UI>();
        SetWidth();
    }

    private void SetWidth()         // SetWidth() - метод дл€ установки ширины элемента списка, который зависит от размеров родительского элемента и количества дочерних элементов
    {
        float width = ui.ValueLayer.GetComponent<RectTransform>().rect.width;
        float height = date.GetComponent<RectTransform>().sizeDelta.y;
        float spacing = gameObject.GetComponent<HorizontalLayoutGroup>().spacing * (gameObject.transform.childCount - 1) + gameObject.GetComponent<HorizontalLayoutGroup>().padding.right + gameObject.GetComponent<HorizontalLayoutGroup>().padding.left + 24f;

        date.GetComponent<RectTransform>().sizeDelta = new Vector2((width - spacing) * 0.4f, height);
        from.GetComponent<RectTransform>().sizeDelta = new Vector2((width - spacing) * 0.3f, height);
        to.GetComponent<RectTransform>().sizeDelta = new Vector2((width - spacing) * 0.3f, height);
    }

    public void DeleteItem()        // DeleteItem() - метод дл€ удалени€ элемента из списка данных по нажатию на кнопку "”далить"
    {
        int index = gameObject.transform.GetSiblingIndex();
        ui.DeleteData(index);    
    }    
}
