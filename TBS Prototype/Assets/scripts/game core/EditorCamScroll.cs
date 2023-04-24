using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
/*
namespace Busted
{
    public class EditorCamScroll : MonoBehaviour
    {
        public static EditorCamScroll instance;

        public float zoomFactor = 2f;
        //public float zoomLimit = 5f;
        public float zoomOuterLimit = 8f;
        public float zoomInnerLimit = 3f;
        public static float zoomLevel;
        public float camPad = 3.0f;
        public Camera cam;
        public static bool moving = false;
        //public Transform cursor;

        public float panSpeed = 3f;
        public float zoomMin;
        public float zoomMax;
        public float zoomStart;
        public float scrollSpeed = 8.0f;
        public float scrollThresholdX;
        public float scrollThreshholdY;
        public float scrollThresholdModifierX = -7f;
        public float scrollThreshholdModifierY = 0.5f;
        public Vector3 target;

        public bool allowScroll = true;

        [SerializeField]
        public float xDiff;
        [SerializeField]
        public float yDiff;

        public int screenWidth;
        public int screenHeight;
        public float mouseX;
        public float mouseY;


        void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(this);
            zoomLevel = 5f;
            target = Camera.main.transform.position;
            cam = GetComponent<Camera>();
            allowScroll = true;
        }

        void Start()
        {
            //cursor = GameObject.FindGameObjectWithTag("Curs").GetComponent<Transform>();
        }

        void Update()
        {
            mouseX = Input.mousePosition.x;
            mouseY = Input.mousePosition.y;
            screenWidth = Screen.width;
            screenHeight = Screen.height;
            Camera.main.orthographicSize = zoomLevel;
            PixelScaler.PixelScale();

            if (!EventSystem.current.IsPointerOverGameObject() && GRID.Map.cursor.MouseInBounds() &&
                allowScroll == true)
            {

                //Camera zoom
                if (GRID.Map.cursor.MouseInBounds())
                {
                    //In
                    if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        if (zoomLevel - zoomFactor > zoomInnerLimit)
                        {
                            zoomLevel -= zoomFactor;
                        }
                    }
                    //Out
                    else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    {
                        if (zoomLevel + zoomFactor < zoomOuterLimit)
                        {
                            zoomLevel += zoomFactor;
                        }
                    }




                    //Camera scroll
                    //Button scrolling
                    if (!InputController.IsMouseActive())
                    {
                        scrollThresholdX = cam.orthographicSize - 2.0f;
                        scrollThreshholdY = cam.orthographicSize - 2.0f;
                        xDiff = GRID.Map.cursor.transform.position.x - Camera.main.transform.position.x;
                        yDiff = GRID.Map.cursor.transform.position.y - Camera.main.transform.position.y;
                    }
                    //Mouse Scrolling
                    /. TBD: Make the screen scroll when mouse is on the edge of the screen, if the cursor is locked to the window 
                    else if (InputController.IsMouseActive())
                    {

                        scrollThresholdX = cam.orthographicSize - scrollThresholdModifierX;
                        scrollThreshholdY = cam.orthographicSize - scrollThreshholdModifierY;
                        xDiff = InputController.mousePos.x - Camera.main.transform.position.x;
                        yDiff = InputController.mousePos.y - Camera.main.transform.position.y;

                    }

                    if (xDiff >= scrollThresholdX)
                    {
                        CameraTranslate("e");
                    }
                    else if (yDiff >= scrollThreshholdY)
                    {
                        CameraTranslate("n");
                    }
                    else if (xDiff <= -scrollThresholdX)
                    {
                        CameraTranslate("w");
                    }
                    else if (yDiff <= -scrollThreshholdY)
                    {
                        CameraTranslate("s");
                    }


                }
            }
        }
        /// <summary>
        /// Translate the camera in a cardinal direction (N,S,E,W)
        /// </summary>
        /// <param name="direction"></param>
        public void CameraTranslate(string direction)
        {
            switch (direction)
            {
                case "n":
                    Camera.main.transform.Translate(Vector3.up * Time.deltaTime * scrollSpeed);
                    break;
                case "s":
                    Camera.main.transform.Translate(Vector3.down * Time.deltaTime * scrollSpeed);
                    break;
                case "e":
                    Camera.main.transform.Translate(Vector3.right * Time.deltaTime * scrollSpeed);
                    break;
                case "w":
                    Camera.main.transform.Translate(Vector3.left * Time.deltaTime * scrollSpeed);
                    break;
            }
        }









        public static void CenterCameraOnMap()
        {
            //Camera.main.transform.position = Map.instance.centerTile;
            Camera.main.transform.position = new Vector3(GRID.Map.cursor.transform.position.x, GRID.Map.cursor.transform.position.y, -1f);
        }

        public static void CenterCameraOnTarget(GameObject target)
        {
            Vector3 t = target.GetComponent<Transform>().position;
            EditorCamScroll.instance.StartCoroutine(EditorCamScroll.instance.PanTo(t));
            //Camera.main.transform.position = new Vector3(t.x, t.y, Camera.main.transform.position.z);

        }

        public IEnumerator PanTo(Vector3 target)
        {
            Vector3 c = Camera.main.GetComponent<Transform>().position;
            float panSpeed = 35f;
            Debug.Log("in coroutine");
            while (Camera.main.transform.position != target)
            {
                Debug.Log("in loop");
                Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, target, Time.deltaTime * panSpeed);
                yield return null;
            }
            yield return null;
        }

    }

*/