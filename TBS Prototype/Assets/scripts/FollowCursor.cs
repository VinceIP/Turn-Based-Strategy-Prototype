using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Busted
{
    public class FollowCursor : MonoBehaviour
    {
        public Curs cursor;

        void Start()
        {
            //print(cursor);
            transform.position = cursor.transform.position + Vector3.back;
        }
    }
}
