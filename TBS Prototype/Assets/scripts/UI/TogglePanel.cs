using UnityEngine;
using System.Collections;

public class TogglePanel : MonoBehaviour
{
    public int startIndex;
    //public RectTransform startParent;
    public RectTransform panelsParent;
    public RectTransform faderTrans;
    public RectTransform editorGUI;
    public CanvasGroup pnl_blackout;
    public bool toggled;
    
    /*
    void Start()
    {
        pnl_blackout = GameObject.Find("EditorGUI/pnl_blackout").GetComponent<CanvasGroup>();
        editorGUI = GameObject.Find("EditorGUI").GetComponent<RectTransform>();
        faderTrans = pnl_blackout.GetComponent<RectTransform>();
        panelsParent = transform.parent.GetComponent<RectTransform>();
        //startParent = GetComponentInParent<RectTransform>();
        startIndex = GetComponent<Transform>().GetSiblingIndex();
    }

    public void Toggle(bool useFader)     //Toggle panel on/off
    {
        if(pnl_blackout.alpha == 0 || pnl_blackout.alpha == 0.65f) //Run only if the fader isn't transitioning; prevents rapid fire clicks
        {
            RectTransform rect = gameObject.GetComponent<RectTransform>();
            CanvasGroup canvas = gameObject.GetComponent<CanvasGroup>();
            CanvasGroup turnThisOff;

            if (canvas.interactable == false) //Turn on
            {
                bool ignoreFade = false;
                foreach (CanvasGroup c in GUIEditorController.instance.panelList) //Check if any other panels are active, and turn it off, without messing with blackout
                {
                    if(c.alpha == 1 && c != gameObject.GetComponent<CanvasGroup>())
                    {
                        turnThisOff = c;
                        turnThisOff.GetComponent<TogglePanel>().toggled = false;
                        turnThisOff.alpha = 0;
                        turnThisOff.interactable = false;
                        turnThisOff.blocksRaycasts = false;
                        print("Turned off: " + turnThisOff.gameObject.name);
                        ignoreFade = true;
                    }
                }

                canvas.alpha = 1;
                canvas.interactable = true;
                canvas.blocksRaycasts = true;
                if(useFader == true && ignoreFade == false)
                {
                    toggled = true;
                    StartCoroutine(Fade());
                    rect.SetParent(editorGUI);
                    rect.SetAsLastSibling();
                    GUIEditorController.instance.inMenu = true;
                }

            }
            else
            {                               //Turn off
                toggled = false;

                canvas.alpha = 0;
                canvas.interactable = false;
                canvas.blocksRaycasts = false;
                if(useFader == true)
                {
                    StartCoroutine(Fade());
                    rect.SetParent(panelsParent);
                    rect.SetSiblingIndex(startIndex);
                    GUIEditorController.instance.inMenu = false;
                }


            }
        }



    }

    public IEnumerator Fade()
    {
        float value;
        if (pnl_blackout.alpha > 0) //Fadout

        {
            value = -0.01f;
        }
        else
        {
            value = 0.01f;

        }

        for(float i = 0f; i < 0.65f; i += 0.01f)
        {
            pnl_blackout.alpha += value;
            yield return null;
        }
        if (pnl_blackout.alpha < 0) pnl_blackout.alpha = 0;                 //Weird number prevention
        else if (pnl_blackout.alpha > 0.65f) pnl_blackout.alpha = 0.65f;

    }
    */

}
