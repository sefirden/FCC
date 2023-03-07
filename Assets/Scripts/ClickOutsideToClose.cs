using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOutsideToClose : MonoBehaviour
{
    public GameObject objectsToClose;
    public GameObject objectToExclude;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bool clickedOnObjectToExclude = RectTransformUtility.RectangleContainsScreenPoint(
                objectToExclude.GetComponent<RectTransform>(), Input.mousePosition);

            if (!clickedOnObjectToExclude)
            {

                    if (!RectTransformUtility.RectangleContainsScreenPoint(
                        objectsToClose.GetComponent<RectTransform>(), Input.mousePosition))
                    {
                        objectsToClose.SetActive(false);
                    }

            }
        }
    }
}


