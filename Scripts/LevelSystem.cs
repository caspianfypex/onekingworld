using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{

    public GameManager gameManager;
    public GameObject shopMenu;
    public Slider expSlider;
    public Text expText;
    public int exp;

    void Start()
    {
        gameManager = GetComponent<GameManager>();
        StartCoroutine(giveXPPerMinute());
    }


    void Update()
    {
        expSlider.value = exp;
        expText.text = expSlider.value.ToString() + "/" + expSlider.maxValue.ToString() + " EXP";
        if (exp >= expSlider.maxValue)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        Country pc = gameManager.playerCountry.GetComponent<Country>();
        exp = 0;
        pc.level += 1;
        if (pc.level == 2) {
            expSlider.maxValue = 150;
        }
        if(pc.level == 3)
        {
            expSlider.maxValue = 250;
        }
        gameManager.sendLetter.AddLetterAndOpenMessageMenu("LEVEL UP!!!...",
        "You Have Leveled Up. Your New Level: " + pc.level.ToString() +
         "\nOpen Shop For New Things" +
         "\nWhen you level up your enemies start to be more harder than normal" +
         "\nSo upgrade your country using new items for fighting with them");
    }

    public void OpenShop()
    {
        if (!shopMenu.active)
        {
            shopMenu.SetActive(true);
        }
    }

    public void CloseShop()
    {
        if (shopMenu.active)
        {
            shopMenu.SetActive(false);
        }
    }

    IEnumerator giveXPPerMinute()
    {
        while (true)
        {
            yield return new WaitForSeconds(60);
            exp += 1;
        }
    }
}
