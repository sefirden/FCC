using System;
using System.Collections;
using System.Collections.Generic;
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
            SwipeDirection(direction2D); //��� �������� ��� ������
        }
    }

    private void SwipeDirection(Vector2 direction)
    {
        if(Vector2.Dot(Vector2.up, direction) > directionTreshold)
        {
            NumberPlus();
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionTreshold)
        {
            NumberMinus();
        }
    }

    private void NumberPlus()
    {
        int currentNumber = Convert.ToInt32(tempGameObject.GetComponent<TMP_Text>().text);
        if(currentNumber == 9)
        {
            currentNumber = 0;
        }
        else
        {
            currentNumber += 1;
        }
        tempGameObject.GetComponent<TMP_Text>().text = Convert.ToString(currentNumber);
    }

    private void NumberMinus()
    {
        int currentNumber = Convert.ToInt32(tempGameObject.GetComponent<TMP_Text>().text);
        if (currentNumber == 0)
        {
            currentNumber = 9;
        }
        else
        {
            currentNumber -= 1;
        }
        tempGameObject.GetComponent<TMP_Text>().text = Convert.ToString(currentNumber);
    }
}