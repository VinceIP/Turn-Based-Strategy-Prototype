/*
using UnityEngine;
using System.Collections;

namespace Busted
{
    public class Curs_Old : MonoBehaviour
    {
        public static Curs_Old instance = null;
        public static Transform trans;

        public float jumpDist = 0.5f;
        public static Vector3 mousePos;

        public float depth = 0f;         //For layering

        public Tile onTile;             //The particular tile the curs is selecting

        public bool canMove;
        public bool movePause;
        public float moveDelay = 0.05f;

        public Vector3 location;

        public AudioSource cursorAudio;

        private InputController inputController;

        void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);

            trans = GetComponent<Transform>();
        }

        void Start()
        {
            inputController = InputController.instance;
            cursorAudio = GetComponent<AudioSource>();
            InputController.SetMouseActive(false);
            ResetCursorPosition();
            EditorCamScroll.CenterCameraOnMap();
            movePause = true;
            canMove = true;
        }

        void Update()
        {
            onTile = GRID.Map.tiles[(int)trans.position.x, (int)trans.position.y].GetComponent<Tile>();
            if (CanMove()) //Only run if a map exists and the curs is allowed to move
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                //Mouse movement
                if (InputController.IsMouseActive() && MouseInBounds()
                   && InputController.instance.xAxis == 0 && InputController.instance.yAxis == 0) //If the mouse is in the window and active
                {
                    transform.position = new Vector3(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y), -depth); //Set the curs to the mouse position but locked to the grid
                }
                //Gamepad/Keyboard movement
                else if (!InputController.IsMouseActive())
                {
                    if (InputController.instance.xAxis > 0)
                    {
                        StartCoroutine(MoveCursor("right"));
                    }
                    else if (InputController.instance.xAxis < 0)
                    {
                        StartCoroutine(MoveCursor("left"));
                    }
                    else if (InputController.instance.yAxis > 0)
                    {
                        StartCoroutine(MoveCursor("down"));
                    }
                    else if (InputController.instance.yAxis < 0)
                    {
                        StartCoroutine(MoveCursor("up"));
                    }
                }
                
            }

        }

        void UpdateOnTile()
        {
            onTile = GRID.Map.tiles[(int)transform.position.x, (int)transform.position.y].GetComponent<Tile>();
        }

        public static bool CanMove()
        {
            if (GRID.GameController.sceneState == GameController.SceneState.Game ||
                GRID.GameController.sceneState == GameController.SceneState.Editor)
            {
                if (GRID.Map != null && GRID.Map.cursor.movePause == true && GRID.Map.cursor.canMove == true) return true;
            }
            return false;
        }

        public static void ResetCursorPosition()
        {
            GRID.Map.cursor.transform.position = new Vector3(GRID.Map.width / 2, GRID.Map.height / 2, -GRID.Map.cursor.depth);  //Center the curs on the map
        }

        public IEnumerator MoveCursor(string dir)   //Check whether or not we would leave the bounds of the map. If not, move the curs 1 unit
        {
            if (movePause == true)
            {
                switch (dir)
                {
                    case "up":
                        if (transform.position.y + 1 < GRID.Map.height) //Compare potential curs location to the location of bounding map tiles
                        {
                            cursorAudio.Play();
                            transform.Translate(Vector3.up);
                            if (!InputController.IsMouseActive()) movePause = false;    //If the mouse isn't active set a move delay
                        }
                        break;
                    case "down":
                        if (transform.position.y - 1 >= 0)
                        {
                            cursorAudio.Play();
                            transform.Translate(Vector3.down);
                            if (!InputController.IsMouseActive()) movePause = false;
                        }
                        break;
                    case "right":
                        if (transform.position.x + 1 < GRID.Map.width)
                        {
                            cursorAudio.Play();
                            transform.Translate(Vector3.right);
                            if (!InputController.IsMouseActive()) movePause = false;
                        }
                        break;
                    case "left":
                        if (transform.position.x - 1 >= 0)
                        {
                            cursorAudio.Play();
                            transform.Translate(Vector3.left);
                            if (!InputController.IsMouseActive()) movePause = false;
                        }
                        break;
                }
                yield return new WaitForSeconds(moveDelay);     //Wait moveDelay seconds before you can move again
                movePause = true;
            }
        }
        public static bool MouseInBounds()  //Return true if mouse curs is within the bounds of the map
        {
            if (GRID.Map != null) //Only run if a map exists
            {
                if (mousePos.x >= 0 &&
                   mousePos.x <= GRID.Map.width - 1 &&
                   mousePos.y <= GRID.Map.height - 1 &&
                   mousePos.y >= 0)
                {
                    return true;
                }
                else return false;
            }
            return false;

        }



    }
}
*/
