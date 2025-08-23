using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendLetter : MonoBehaviour
{
    [TextArea(15, 20)]
    public string letter;
    public GameObject lettersContent;
    public GameObject letterBoxMenu;
    public GameObject messagePrefab;
    public Scrollbar letterScrollBar;
    public GameManager gameManager;
    public Vector3 letterContentFirstTransform;
    public GameObject messageMenu;

    void Start()
    {
        letterContentFirstTransform.x = lettersContent.GetComponent<RectTransform>().position.x;
        letterContentFirstTransform.y = lettersContent.GetComponent<RectTransform>().position.y;
        letterContentFirstTransform.z = lettersContent.GetComponent<RectTransform>().position.z;
        gameManager = GetComponent<GameManager>();
    }

    void Update()
    {
        
    }

    public void CloseMenu()
    {
        letterBoxMenu.SetActive(false);
        lettersContent.GetComponent<RectTransform>().position = letterContentFirstTransform;
        letterScrollBar.value = 1;
    }

    public void OpenMenu()
    {
        if (gameManager.playerArea != "Yok")
        {
            if (!letterBoxMenu.active)
            {
                letterBoxMenu.SetActive(true);
            }
        }
    }

    public void AddLetter(string letterName, string message)
    {
        if (gameManager.playerArea != "Yok")
        {
            GameObject newLetter = Instantiate(messagePrefab, lettersContent.transform);
            newLetter.name = letterName;
            Text letterGameObjectName = newLetter.transform.Find("Text").gameObject.GetComponent<Text>();
            letterGameObjectName.text = letterName;
            if (lettersContent.transform.childCount >= 2)
            {
                lettersContent.transform.GetChild(lettersContent.transform.childCount - 1).SetSiblingIndex(0);
            }
            newLetter.transform.Find("altText").GetComponent<Text>().text = message;
        }
    }

    public void AddLetterAndOpenMessageMenu(string letterName, string message)
    {
        if (gameManager.playerArea != "Yok")
        {
            GameObject newLetter = Instantiate(messagePrefab, lettersContent.transform);
            newLetter.name = letterName;
            Text letterGameObjectName = newLetter.transform.Find("Text").gameObject.GetComponent<Text>();
            letterGameObjectName.text = letterName;
            if (lettersContent.transform.childCount >= 2)
            {
                lettersContent.transform.GetChild(lettersContent.transform.childCount - 1).SetSiblingIndex(0);
                for (int i = 0; i != lettersContent.transform.childCount; i++)
                {
                    GameObject ltr = lettersContent.transform.GetChild(i).gameObject;
                }
            }
            newLetter.transform.Find("altText").GetComponent<Text>().text = message;
            messageMenu.SetActive(true);
            messageMenu.transform.Find("messageText").GetComponent<Text>().text = newLetter.transform.Find("altText").GetComponent<Text>().text;
        }
    }

    public void OpenMessageMenu(GameObject gmj)
    {
        messageMenu.SetActive(true);
        messageMenu.transform.Find("messageText").GetComponent<Text>().text = gmj.transform.Find("altText").GetComponent<Text>().text;
    }

    public void CloseMessageMenu()
    {
        messageMenu.SetActive(false);
    }
}
