using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Busted.UI
{
    /// <summary>
    /// Modal panel for yes, no, cancel type questions
    /// </summary>
    public class ModalPanel : MonoBehaviour
    {
        private static ModalPanel _modalPanel;
        public static ModalPanel Instance()
        {
            if (_modalPanel == null)
            {
                _modalPanel = FindObjectOfType(typeof(ModalPanel)) as ModalPanel;
            }
            if (_modalPanel == null) Debug.Log("Scene is missing a modal panel.");
            return _modalPanel;
        }

        private int m_blackoutOrigIndex; //blackout panel original sibling index

        public Text question;
        public Button yesButton;
        public Button noButton;
        public Button cancelButton;
        public GameObject pnl_blackout;
        public GameObject panels;

        public EditorControllerUI editorControllerUI;

        void Awake()
        {

        }

        void OnEnable()
        {
            editorControllerUI = GameObject.FindGameObjectWithTag("EditorControllerUI").GetComponent<EditorControllerUI>();
            pnl_blackout = editorControllerUI.pnl_blackout;
            m_blackoutOrigIndex = pnl_blackout.transform.GetSiblingIndex();
            panels = editorControllerUI.panels;
            yesButton.gameObject.SetActive(false);
            noButton.gameObject.SetActive(false);
            cancelButton.gameObject.SetActive(false);
        }
        /// <summary>
        /// Yes, no, and cancel
        /// </summary>
        public void Choice(string question, UnityAction yesEvent, UnityAction noEvent, UnityAction cancelEvent)
        {
            gameObject.SetActive(true);
            BlackoutToFront();
            yesButton.onClick.RemoveAllListeners();
            noButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();

            if (yesEvent != null)
            {
                yesButton.gameObject.SetActive(true);
                yesButton.onClick.AddListener(yesEvent);
                yesButton.onClick.AddListener(ClosePanel);
            }
            else yesButton.gameObject.SetActive(false);

            if (noEvent != null)
            {
                noButton.gameObject.SetActive(true);
                noButton.onClick.AddListener(noEvent);
                noButton.onClick.AddListener(ClosePanel);
            }
            else noButton.gameObject.SetActive(false);

            if (cancelEvent != null)
            {
                cancelButton.gameObject.SetActive(true);
                cancelButton.onClick.AddListener(cancelEvent);
                cancelButton.onClick.AddListener(ClosePanel);
            }
            else noButton.gameObject.SetActive(false);


            this.question.text = question;
        }

        /// <summary>
        /// Yes and no
        /// </summary>
        public void Choice(string question, UnityAction yesEvent, UnityAction noEvent)
        {
            gameObject.SetActive(true);
            BlackoutToFront();
            yesButton.onClick.RemoveAllListeners();
            noButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();

            yesButton.gameObject.SetActive(true);
            yesButton.onClick.AddListener(yesEvent);
            yesButton.onClick.AddListener(ClosePanel);

            noButton.gameObject.SetActive(true);
            noButton.onClick.AddListener(noEvent);
            noButton.onClick.AddListener(ClosePanel);
            pnl_blackout.transform.SetSiblingIndex(panels.transform.GetSiblingIndex());

            this.question.text = question;
        }


        public void Message(string message, UnityAction okEvent)
        {
            gameObject.SetActive(true);
            BlackoutToFront();
            noButton.gameObject.SetActive(false);
            cancelButton.gameObject.SetActive(false);
            yesButton.gameObject.SetActive(true);
            pnl_blackout.transform.SetSiblingIndex(panels.transform.GetSiblingIndex());

            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(okEvent);
            yesButton.gameObject.GetComponentInChildren<Text>().text = "OK";
            this.question.text = message;
        }

        public void ClosePanel()
        {
            yesButton.gameObject.SetActive(false);
            noButton.gameObject.SetActive(false);
            cancelButton.gameObject.SetActive(false);
            yesButton.gameObject.GetComponentInChildren<Text>().text = "Yes";
            BlackoutReset();
            gameObject.SetActive(false);
        }

        void BlackoutToFront()
        {
            pnl_blackout.transform.SetSiblingIndex(transform.GetSiblingIndex()); //Set sib index to modal panel's position
        }

        void BlackoutReset()
        {
            pnl_blackout.transform.SetSiblingIndex(m_blackoutOrigIndex);
        }
    }
}
