using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Busted
{
    public class CamScroll : MonoBehaviour
    {
        private Map m_map;
        private Camera m_cam;

        private bool m_mapLoaded;
        private bool m_scrollAllowed;

        private float m_zoomLevel = 6;

        public static float CAM_BORDER = 15; //Camera scrolls if mouse touches the edge of the screen within this amount of pixels
        public static float CAM_SPEED = 10;
        public static float CAM_ZOOMFACTOR = 1;
        public static float CAM_ZOOMLIMIT_UPPER = 9;
        public static float CAM_ZOOMLIMIT_LOWER = 5;

        private void OnEnable()
        {
            Map.onMapLoaded += OnMapLoaded;
            Map.onMapUnloaded += OnMapUnloaded;
        }
        private void OnDisable()
        {
            Map.onMapLoaded -= OnMapLoaded;
            Map.onMapUnloaded -= OnMapUnloaded;

        }

        void OnMapLoaded()
        {
            m_map = GRID.Map;
            m_cam = Camera.main;
            m_mapLoaded = true;
            m_scrollAllowed = true;
            PanCamera("n");
        }

        void OnMapUnloaded()
        {
            m_map = null;
            m_cam = null;
            m_mapLoaded = false;
            m_scrollAllowed = false;
        }

        private void Update()
        {
            if (m_mapLoaded && m_scrollAllowed)
            {
                string targetDir = null;
                if (InputController.IsMouseActive()) targetDir = GetMouseScroll();
                else if (InputController.IsKeyboardActive()) targetDir = GetKeyboardScroll();
                HandleZoom();
                if (targetDir != null) PanCamera(targetDir);
            }
        }

        void HandleZoom()
        {
            //Zoom in/out
            if (InputController.IsMouseInWindow())
            {
                if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
                {
                    m_cam.orthographicSize -= CAM_ZOOMFACTOR;
                }
                else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
                {
                    m_cam.orthographicSize += CAM_ZOOMFACTOR;
                }
            }
            m_cam.orthographicSize = Mathf.Clamp(m_cam.orthographicSize, CAM_ZOOMLIMIT_LOWER, CAM_ZOOMLIMIT_UPPER);
        }

        string GetMouseScroll()
        {
            string targetDir = null;
            if (InputController.IsMouseInWindow())
            {
                //Mouse scrolling
                if (Input.mousePosition.x <= CAM_BORDER) targetDir = "w"; //If mouse is on edges of screen, set a target direction to pan
                else if (Input.mousePosition.x >= (Screen.currentResolution.width - CAM_BORDER)) targetDir = "e";
                else if (Input.mousePosition.y <= CAM_BORDER) targetDir = "s";
                else if (Input.mousePosition.y >= (Screen.currentResolution.height - CAM_BORDER)) targetDir = "n";
                
            }
            return targetDir;

        }

        string GetKeyboardScroll()
        {
            string targetDir = null;
            float axisX = Input.GetAxisRaw("X_Keyboard");
            float axisY = Input.GetAxisRaw("Y_Keyboard");
            if (axisX > 0) targetDir = "e";
            else if (axisX < 0) targetDir = "w";
            if (axisY > 0) targetDir = "s";
            else if (axisY < 0) targetDir = "n";
            return targetDir;
        }



        public static void PanCamera(string direction)
        {
            int padding = 3;
            Vector3 dir = new Vector3();
            switch (direction)
            {
                case "n":
                    dir = Vector3.up;
                    break;
                case "s":
                    dir = Vector3.down;
                    break;
                case "e":
                    dir = Vector3.right;
                    break;
                case "w":
                    dir = Vector3.left;
                    break;
            }
            Tile bottomLeft = GRID.Map.tiles[0, 0];
            Tile topLeft = GRID.Map.tiles[0, GRID.Map.height-1];
            Tile bottomRight = GRID.Map.tiles[GRID.Map.width-1, 0];
            Tile topRight = GRID.Map.tiles[GRID.Map.width-1, GRID.Map.height-1];

            Camera.main.transform.Translate(dir * CamScroll.CAM_SPEED * Time.deltaTime);

            //float x = Mathf.Clamp(Camera.main.transform.position.x, -2, Camera.main.transform.position.x);
            //float y = Mathf.Clamp(Camera.main.transform.position.y, -2, Camera.main.transform.position.y);
            float x = Mathf.Clamp(Camera.main.transform.position.x, bottomLeft.transform.position.x + padding, topRight.transform.position.x - padding);
            float y = Mathf.Clamp(Camera.main.transform.position.y, bottomLeft.transform.position.y + padding, topRight.transform.position.y - padding);
            Camera.main.transform.position = new Vector3(x, y, 0);

        }


        public bool ScrollAllowed
        {
            get { return m_scrollAllowed; }
            set { m_scrollAllowed = value; }
        }


        /*
        public static IEnumerator PanCamera(string direction)
        {
            Vector3 dir = new Vector3();
            switch(direction)
            {
                case "n":
                    dir = Vector3.up;
                    break;
                case "s":
                    dir = Vector3.down;
                    break;
                case "e":
                    dir = Vector3.right;
                    break;
                case "w":
                    dir = Vector3.left;
                    break;
            }
            Camera.main.transform.Translate(dir * CamScroll.CAM_SPEED * Time.deltaTime);
            return null;
        }
        */
    }
}