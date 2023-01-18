using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LabelPosition : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SetPaddingRight());        
    }

    public IEnumerator SetPaddingRight()
    {
            while (gameObject.GetComponent<RectTransform>().rect.width == 0)
            {
            yield return new WaitForFixedUpdate();
            }
            transform.localPosition = new Vector3(transform.localPosition.x + (gameObject.GetComponent<RectTransform>().rect.width / 2), transform.localPosition.y, transform.localPosition.z);
    }
}
