using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeDetection : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    GameObject tempGameObject;
    Calculations calculations;

    [SerializeField]
    private float minimumDistance = 0.2f;
    [SerializeField]
    private float maximumTime = 1f;
    [SerializeField, Range(0f, 1f)]
    private float directionTreshold = 0.5f;

    private InputManager inputManager;

    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;

    public float WaitForSeconds;
    private bool animate;
    public float animationSpeed = 1.0f;

    private void Awake()
    {
        inputManager = InputManager.Instance;
    }

    void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
        calculations = FindObjectOfType<Calculations>();
        animate = false;
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;
        CheckIsNumber();
    }

    private void SwipeEnd(Vector2 position, float time)
    {
        endPosition = position;
        endTime = time;
        Vector2 swipeDirection = InputManager.Instance.SwipeDirection();
        DetectSwipe();
    }


    private void CheckIsNumber()
    {
        tempGameObject = null;

        m_PointerEventData = new PointerEventData(m_EventSystem);
        m_PointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        m_Raycaster.Raycast(m_PointerEventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.tag == "Player")
            {
                tempGameObject = result.gameObject;
                return;
            }
        }
    }

    private void DetectSwipe()
    {
        if (Vector3.Distance(startPosition, endPosition) >= minimumDistance &&
            (endTime - startTime) <= maximumTime &&
            tempGameObject != null)
        {
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
            Vector3 direction = endPosition - startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
        }
    }

    private void SwipeDirection(Vector2 direction)
    {
        if(Vector2.Dot(Vector2.up, direction) > directionTreshold)
        {
            StartCoroutine(NumberPlus());
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionTreshold)
        {
            StartCoroutine(NumberMinus());
        }
    }

    private IEnumerator NumberPlus()
    {
        while(animate)
        {
            yield return new WaitForFixedUpdate();
        }
       
        animate = true;
        TMP_Text tempText = tempGameObject.GetComponent<TMP_Text>();
        RectTransform textTransform = tempText.GetComponent<RectTransform>();
        RectTransform parentTransform = tempText.transform.parent.GetComponent<RectTransform>();
        TMP_TextInfo textInfo = tempText.textInfo;
        tempText.ForceMeshUpdate();

        float letterHeight = textInfo.characterInfo[0].ascender + 20f;
        float parentHeight = parentTransform.rect.height;
        float animationDistance = (parentHeight / 2) - (letterHeight / 2);

        float currentPosition = textTransform.anchoredPosition.y;

        float thisPosition = textTransform.anchoredPosition.y;
        float targetTopPosition = currentPosition + animationDistance;
        float targetDownPosition = currentPosition - animationDistance;

        while (Mathf.Abs(textTransform.anchoredPosition.y - targetTopPosition) > 0.1f)
        {
            targetTopPosition = currentPosition + animationDistance;
            thisPosition = Mathf.MoveTowards(thisPosition, targetTopPosition, animationSpeed * Time.deltaTime);
            textTransform.anchoredPosition = new Vector2(textTransform.anchoredPosition.x, thisPosition);
            yield return null;
        }

        int currentNumber = Convert.ToInt32(tempGameObject.GetComponent<TMP_Text>().text);
        tempGameObject.GetComponent<TMP_Text>().text = "";
        yield return null;

        textTransform.anchoredPosition = new Vector2(textTransform.anchoredPosition.x, targetDownPosition);

        if (Settings.Instance.invertInputSlider)
        {
            if (currentNumber == 0)
            {
                currentNumber = 9;
            }
            else
            {
                currentNumber -= 1;
            }
        }
        else
        {
            if (currentNumber == 9)
            {
                currentNumber = 0;
            }
            else
            {
                currentNumber += 1;
            }
        }

        yield return null;

        tempGameObject.GetComponent<TMP_Text>().text = Convert.ToString(currentNumber);
        calculations.OnSliderChange();
        yield return null;

        thisPosition = textTransform.anchoredPosition.y;
        while (Mathf.Abs(textTransform.anchoredPosition.y - targetTopPosition) > 0.1f)
        {
            targetTopPosition = currentPosition;
            thisPosition = Mathf.MoveTowards(thisPosition, targetTopPosition, animationSpeed * Time.deltaTime);
            textTransform.anchoredPosition = new Vector2(textTransform.anchoredPosition.x, thisPosition);
            yield return null;
        }

        animate = false;
    }

    private IEnumerator NumberMinus()
    {
        while (animate)
        {
            yield return new WaitForFixedUpdate();
        }

        animate = true;
        TMP_Text tempText = tempGameObject.GetComponent<TMP_Text>();
        RectTransform textTransform = tempText.GetComponent<RectTransform>();
        RectTransform parentTransform = tempText.transform.parent.GetComponent<RectTransform>();
        TMP_TextInfo textInfo = tempText.textInfo;
        tempText.ForceMeshUpdate();

        float letterHeight = textInfo.characterInfo[0].ascender + 20f;
        float parentHeight = parentTransform.rect.height;
        float animationDistance = (parentHeight / 2) - (letterHeight / 2);

        float currentPosition = textTransform.anchoredPosition.y;

        float thisPosition = textTransform.anchoredPosition.y;
        float targetTopPosition = currentPosition + animationDistance;
        float targetDownPosition = currentPosition - animationDistance;

        while (Mathf.Abs(textTransform.anchoredPosition.y - targetDownPosition) > 0.1f)
        {
            targetDownPosition = currentPosition - animationDistance;
            thisPosition = Mathf.MoveTowards(thisPosition, targetDownPosition, animationSpeed * Time.deltaTime);
            textTransform.anchoredPosition = new Vector2(textTransform.anchoredPosition.x, thisPosition);
            yield return null;
        }

        int currentNumber = Convert.ToInt32(tempGameObject.GetComponent<TMP_Text>().text);
        tempGameObject.GetComponent<TMP_Text>().text = "";
        yield return null;

        textTransform.anchoredPosition = new Vector2(textTransform.anchoredPosition.x, targetTopPosition);

        if (Settings.Instance.invertInputSlider)
        {
            if (currentNumber == 9)
            {
                currentNumber = 0;
            }
            else
            {
                currentNumber += 1;
            }
        }
        else
        {
            if (currentNumber == 0)
            {
                currentNumber = 9;
            }
            else
            {
                currentNumber -= 1;
            }

        }
        yield return null;

        tempGameObject.GetComponent<TMP_Text>().text = Convert.ToString(currentNumber);
        calculations.OnSliderChange();
        yield return null;

        thisPosition = textTransform.anchoredPosition.y;
        while (Mathf.Abs(textTransform.anchoredPosition.y - currentPosition) > 0.1f)
        {
            thisPosition = Mathf.MoveTowards(thisPosition, currentPosition, animationSpeed * Time.deltaTime);
            textTransform.anchoredPosition = new Vector2(textTransform.anchoredPosition.x, thisPosition);
            yield return null;
        }

        animate = false;
    }
}
