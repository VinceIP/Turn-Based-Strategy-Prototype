using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Busted
{

    public class Curs : MonoBehaviour
    {
        public bool canMove;
        Camera cam;
        FollowCursor followCursor;
        public Map map;
        public Tile onTile;
        public Vector3 mousePos;


        private void OnEnable()
        {
            Tile.updateOnTile += UpdateOnTile;
        }
        private void OnDisable()
        {
            Tile.updateOnTile -= UpdateOnTile;
        }

        void Start()
        {
            cam = Camera.main;
            followCursor = cam.gameObject.AddComponent<FollowCursor>();
            followCursor.cursor = this;
            //JumpToCenter();
            //canMove = true;
        }



        void Update()
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            /*
            if (canMove)
            {
                //onTile = map.tiles[(int)transform.position.x, (int)transform.position.y].GetComponent<Tile>();
                //Mouse movement
                if (MouseInBounds()) //If the mouse is in the window and active
                {
                    //transform.position = mousePos;
                    //transform.position = new Vector3(Mathf.Ceil(mousePos.x), Mathf.Ceil(mousePos.y), transform.position.z); //Set the curs to the mouse position but locked to the grid
                    transform.position = onTile.transform.position;
                }
                
            }
            */
        }

        void UpdateOnTile(Tile t)
        {
            //print("ontile: " + t.name);
            onTile = t;
            transform.position = onTile.transform.position;
        }

        public bool MouseInBounds()  //Return true if mouse curs is within the bounds of the map
        {

            if (mousePos.x >= 0 &&
                mousePos.x <= map.width - 1 &&
                mousePos.y <= map.height - 1 &&
                mousePos.y >= 0)
            {
                return true;
            }
            else return false;
        }

        public void JumpToCenter()
        {
            Vector2 jumpTo;
            jumpTo.x = map.width / 2;
            jumpTo.y = map.height / 2;
            transform.position = jumpTo;
        }
    }
}