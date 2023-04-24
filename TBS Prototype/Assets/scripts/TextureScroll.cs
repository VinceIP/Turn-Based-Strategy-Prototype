using UnityEngine;
using System.Collections;

public class TextureScroll : MonoBehaviour {

	public float speed = 0.5f;
    public float offsetX = 0.5f;
    public float offsetY = 0.5f;
    public bool windEffect;

    public float windTime = 3000f;
	public Renderer rend;
    public float timer;
    public float maxX;
    public float maxY;

	void Start ()
	{
		rend = GetComponent<Renderer>();
        timer = Time.time;
    }
	
	void Update ()
	{
		float scroll = Time.time * -speed;
        if(windEffect == true)
        {
            
            if(Time.time - windTime > timer)
            {
                Debug.Log("changing course");
                timer = Time.time;
                maxX = offsetX + 1;
                maxY = offsetY + 1;
                
                StartCoroutine(OffsetChange());
            }
        }

        rend.material.mainTextureOffset = new Vector2(offsetX * scroll,offsetY * scroll);
	}

    public  IEnumerator OffsetChange()
    {
        
        while(offsetX < maxX + 1)
        {
            offsetX += 0.01f;
            while(offsetY > maxY + 1)
            {
                offsetY -= 0.01f;
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
               
        
    }
}
