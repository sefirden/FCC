using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LabelPosition : MonoBehaviour
{


    void Start()
    {
        //StartCoroutine(SetPaddingRight());

    }

    public IEnumerator SetPaddingRight()
    {
            while (gameObject.GetComponent<RectTransform>().rect.width == 0)
            {
                yield return new WaitForFixedUpdate();
                Debug.Log(gameObject.GetComponent<RectTransform>().rect.width);
            }

            transform.position = new Vector3(transform.position.x + (gameObject.GetComponent<RectTransform>().rect.width / 2), transform.position.y, transform.position.z);
           
            Debug.Log(transform.position);
    }
}
