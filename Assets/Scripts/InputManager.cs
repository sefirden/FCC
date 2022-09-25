using UnityEngine;
using UnityEngine.InputSystem;


[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{

    #region Events
    public delegate void StartTouch(Vector2 position, float time);
    public event StartTouch OnStartTouch;

    public delegate void EndTouch(Vector2 position, float time);
    public event EndTouch OnEndTouch;
    #endregion

    private SwipeControls swipeControls;
    private Camera mainCamera;
    public static InputManager Instance { get; private set; }

    private void Awake()
    {
            if (Instance == null) //если объекта ещё нет
            {
                Instance = this; //говорим что вот кагбе он
                //DontDestroyOnLoad(gameObject); //и говорим что его нельзя ломать между уровнями, иначе он нахер не нужен
            }

        swipeControls = new SwipeControls();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        swipeControls.Enable();
    }

    private void OnDisable()
    {
        swipeControls.Disable();
    }

    void Start()
    {
        swipeControls.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        swipeControls.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
    }

    private void StartTouchPrimary(InputAction.CallbackContext context)
    {
        if (OnStartTouch != null) OnStartTouch(Utils.ScreenToWorld(mainCamera, swipeControls.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)context.startTime);
        //if (OnStartTouch != null) OnStartTouch(swipeControls.Touch.PrimaryPosition.ReadValue<Vector2>(), (float)context.startTime);
    }

    private void EndTouchPrimary(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null) OnEndTouch(Utils.ScreenToWorld(mainCamera, swipeControls.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)context.time);
        //if (OnEndTouch != null) OnEndTouch(swipeControls.Touch.PrimaryPosition.ReadValue<Vector2>(), (float)context.time);
    }

    public Vector2 PrimaryPosition()
    {
        return Utils.ScreenToWorld(mainCamera, swipeControls.Touch.PrimaryPosition.ReadValue<Vector2>());
    }

}
