using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DataItem : MonoBehaviour
{
    private UI ui; //скрипт уи
    public GameObject date;
    public GameObject from;
    public GameObject to;
    public TMP_Text dataValue;
    public TMP_Text fromLabel;
    public TMP_Text fromValue;
    public TMP_Text toLabel;
    public TMP_Text toValue;

    void Start()
    {
        ui = FindObjectOfType<UI>();
        SetWidth();
    }

    private void SetWidth()
    {
        float width = ui.ValueLayer.GetComponent<RectTransform>().rect.width;
        float height = date.GetComponent<RectTransform>().sizeDelta.y;
        float spacing = gameObject.GetComponent<HorizontalLayoutGroup>().spacing * (gameObject.transform.childCount - 1) + gameObject.GetComponent<HorizontalLayoutGroup>().padding.right + gameObject.GetComponent<HorizontalLayoutGroup>().padding.left + 24f;

        date.GetComponent<RectTransform>().sizeDelta = new Vector2((width - spacing) * 0.4f, height);
        from.GetComponent<RectTransform>().sizeDelta = new Vector2((width - spacing) * 0.3f, height);
        to.GetComponent<RectTransform>().sizeDelta = new Vector2((width - spacing) * 0.3f, height);
    }

    public void DeleteItem()
    {
        int index = gameObject.transform.GetSiblingIndex();
        ui.DeleteData(index);
        Destroy(gameObject);

    }
}
