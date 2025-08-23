using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsScript : MonoBehaviour
{

    public SendLetter sendLetter;

    void Start()
    {
        sendLetter = GameObject.Find("GameManager").GetComponent<GameManager>().sendLetter;
        if (gameObject.name != "ExpeditionButton")
        {
            GetComponent<Button>().onClick.AddListener(TaskOnClick);
        }
        else
        {
            if (GetComponent<Button>())
            {
                GetComponent<Button>().onClick.AddListener(TaskOnClick);
            }
        }
    }

    
    void TaskOnClick()
    {
        sendLetter.OpenMessageMenu(this.gameObject);
    }
   

    public IEnumerator destroyObject()
    {
        yield return new WaitForSeconds(0f);
        Destroy(gameObject);
    }
}
