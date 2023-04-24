using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Busted
{
    public class Unit_HealthCounter : MonoBehaviour
    {
        public List<Sprite> sprites;
        private Unit _unit;
        private SpriteRenderer _rend;
        private int _index;

        void Start()
        {
            _unit = transform.parent.GetComponent<Unit>();
            _rend = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            if (_unit.Health > 94)
            {
                _index = 9;
            }
            else if (_unit.Health <= 94 && _unit.Health >= 85)
            {
                _index = 8;
            }
            else if (_unit.Health <= 84 && _unit.Health >= 75)
            {
                _index = 7;
            }
            else if (_unit.Health <= 74 && _unit.Health >= 65)
            {
                _index = 6;
            }
            else if (_unit.Health <= 64 && _unit.Health >= 55)
            {
                _index = 5;
            }
            else if (_unit.Health <= 54 && _unit.Health >= 45)
            {
                _index = 4;
            }
            else if (_unit.Health <= 44 && _unit.Health >= 35)
            {
                _index = 3;
            }
            else if (_unit.Health <= 34 && _unit.Health >= 25)
            {
                _index = 2;
            }
            else if (_unit.Health <= 24 && _unit.Health >= 15)
            {
                _index = 1;
            }
            else if (_unit.Health <= 14 && _unit.Health >= 4)
            {
                _index = 0;
            }

            _rend.sprite = sprites[_index];
        }

    }

}
