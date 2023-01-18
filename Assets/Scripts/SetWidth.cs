using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetWidth : MonoBehaviour
{
    public UI ui;

    void Awake()
    {
        ui = FindObjectOfType<UI>();

        float width = ui.SuperWidth;
        float height = gameObject.GetComponent<RectTransform>().rect.height;
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }
}
