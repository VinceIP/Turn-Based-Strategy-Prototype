using UnityEngine;

namespace Busted
{
    [RequireComponent(typeof(SpriteHolder))]
    public class Entity : MonoBehaviour
    {
        public int xCord;
        public int yCord;
        public bool canBeNeutral;

        public PlayerData.TeamColor teamColor;

        public SpriteHolder spriteHolder;

        protected virtual void Awake()
        {
            spriteHolder = GetComponent<SpriteHolder>();
            //BoxCollider2D col = gameObject.AddComponent<BoxCollider2D>();
            //col.size = new Vector2(1, 1);
            //col.isTrigger = true;
        }

        protected virtual void Start()
        {
            UpdateTeamColorSprite();
        }

        public virtual void OnSelect()
        {

        }

        public void UpdateTeamColorSprite()
        {
            SpriteRenderer rend = GetComponent<SpriteRenderer>();
            if (canBeNeutral)
            {
                switch (teamColor)
                {
                    case PlayerData.TeamColor.BLUE:
                        rend.sprite = spriteHolder.sprites[0];
                        break;
                    case PlayerData.TeamColor.RED:
                        rend.sprite = spriteHolder.sprites[1];
                        break;
                    case PlayerData.TeamColor.YELLOW:
                        rend.sprite = spriteHolder.sprites[2];
                        break;
                    case PlayerData.TeamColor.GREEN:
                        rend.sprite = spriteHolder.sprites[3];
                        break;
                    case PlayerData.TeamColor.NEUTRAL:
                        rend.sprite = spriteHolder.sprites[4];
                        break;
                }
            }
            else
            {
                switch (teamColor)
                {
                    case PlayerData.TeamColor.BLUE:
                        rend.sprite = spriteHolder.sprites[0];
                        break;
                    case PlayerData.TeamColor.RED:
                        rend.sprite = spriteHolder.sprites[1];
                        break;
                    case PlayerData.TeamColor.YELLOW:
                        rend.sprite = spriteHolder.sprites[2];
                        break;
                    case PlayerData.TeamColor.GREEN:
                        rend.sprite = spriteHolder.sprites[3];
                        break;
                }
            }
        }
    }
}
