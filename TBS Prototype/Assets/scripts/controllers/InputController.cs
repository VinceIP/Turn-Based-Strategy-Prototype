using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Busted
{
    public class InputController : MonoBehaviour
    {
        //public static InputController instance;
        public static bool mouseActive;
        public static Vector3 mousePos;
        public static Vector3 screenSizeWorld;

        public float xAxis = 0;
        public float yAxis = 0;

        public bool joystickActive;
        public bool keyboardActive;

        public EditorController editorController;
        public GameController gameController;

        private InputControllerState _inputControllerState;

        public string stateName;

        public InputControllerState InputControllerState
        {
            get { return _inputControllerState; }
            set
            {
                if (_inputControllerState == null) _inputControllerState = new InputControllerState();
                _inputControllerState.OnStateExit();
                _inputControllerState = value;
                value.OnStateEnter();
                stateName = value.stateName;

            }
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneFinishedLoading;
        }
        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneFinishedLoading;
        }

        public void OnSceneFinishedLoading(Scene scene, LoadSceneMode loadSceneMode)
        {
            if(scene.name == "editor")
            {
                editorController = GameObject.FindGameObjectWithTag("EditorController").GetComponent<EditorController>();
                InputControllerState = new InputControllerState_Editor();
                InputControllerState.editorController = editorController;
            }
            else if (scene.name == "game")
            {
                //gameController = GameObject.FindGameObjectWithTag("Gamecontroller").GetComponent<GameController>();
                gameController = GRID.GameController;
                InputControllerState = new InputControllerState_Game();
                InputControllerState.gameController = gameController;
            }
        }

        void Awake()
        {
            //DontDestroyOnLoad(transform.gameObject);
        }

        void OnLevelWasLoaded(int level)
        {

        }

        void Start()
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

        void Update()
        {

            if(Input.GetKeyDown(KeyCode.Tab))
            {
                if (Cursor.lockState == CursorLockMode.Confined) Cursor.lockState = CursorLockMode.None;
                else Cursor.lockState = CursorLockMode.Confined;
            }

            if (SceneManager.GetActiveScene().name != "preload")
            {

                mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
                screenSizeWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

                if (!IsMouseActive())
                {
                    //Toggle mouse active state on mouse movement
                    if (Input.GetAxisRaw("X_Mouse") != 0 || Input.GetAxisRaw("Y_Mouse") != 0)
                        SetMouseActive(true);
                }
                else if (IsMouseActive())
                {
                    /*
                    if (Input.GetAxisRaw("X_Mouse") == 0 && Input.GetAxisRaw("Y_Mouse") == 0)
                    {
                        SetMouseActive(false);
                    }
                    */
                    if (IsKeyboardActive()) SetMouseActive(false);
                }

                if (Input.GetAxisRaw("X_Keyboard") != 0)
                {
                    xAxis = Input.GetAxisRaw("X_Keyboard");
                    joystickActive = false;
                    keyboardActive = true;
                }
                else if (Input.GetAxisRaw("X_Stick") != 0 || Input.GetAxisRaw("X_Dpad") != 0)
                {
                    if (Input.GetAxisRaw("X_Stick") != 0) xAxis = Input.GetAxisRaw("X_Stick");
                    else xAxis = Input.GetAxisRaw("X_Dpad");
                    joystickActive = true;
                    keyboardActive = false;
                }

                else if (Input.GetAxisRaw("Y_Keyboard") != 0)
                {
                    yAxis = Input.GetAxisRaw("Y_Keyboard");
                    joystickActive = false;
                    keyboardActive = true;
                }
                else if (Input.GetAxisRaw("Y_Stick") != 0 || Input.GetAxisRaw("Y_Dpad") != 0)
                {
                    if (Input.GetAxisRaw("Y_Stick") != 0) yAxis = Input.GetAxisRaw("Y_Stick");
                    else yAxis = Input.GetAxisRaw("Y_Dpad");
                    joystickActive = true;
                    keyboardActive = false;
                }
                else
                {
                    xAxis = 0;
                    yAxis = 0;
                }

                if (Input.GetButton("Confirm_Gamepad") || Input.GetButton("Confirm_Keyboard") || Input.GetButton("Confirm_Mouse"))
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        InputControllerState.OnPressedConfirm();
                    }
                }
            }

        }

        public static bool IsMouseActive()
        {
            if (mouseActive == true)
                return true;
            else return false;
        }

        public static void SetMouseActive(bool active)
        {
            if (active == true)
            {
                mouseActive = true;
                GRID.InputController.joystickActive = false;
                GRID.InputController.keyboardActive = false;
            }

            else mouseActive = false;
        }

        public static bool IsJoystickActive()
        {
            if (GRID.InputController.joystickActive)
            {
                return true;
            }
            else return false;
        }

        public static bool IsKeyboardActive()
        {
            if (GRID.InputController.keyboardActive)
            {
                return true;
            }
            else return false;
        }

        public static bool IsMouseInWindow()
        {
            if (Input.mousePosition.x > Screen.currentResolution.width || Input.mousePosition.y > Screen.currentResolution.height ||
                Input.mousePosition.x < 0 || Input.mousePosition.y < 0)
            {
                return false;
            }
            else return true;
        }

        public static void ToggleCursorLock()
        {

        }
    }

}