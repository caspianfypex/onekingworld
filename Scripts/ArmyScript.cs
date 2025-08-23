using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyScript : MonoBehaviour
{

    public GameObject armyMenu;
    public Slider armySlider;
    public Text coinText;
    public Text armyText;
    public GameManager gameManager;

    void Start()
    {
        

    }


    void Update()
    {
        if (gameManager.playerArea != "Yok" && gameManager.selectedArea != null)
        {
            armySlider.maxValue = (500 - gameManager.selectedArea.GetComponent<AreaScript>().armyCount);
            armyText.text = armySlider.value.ToString();
            coinText.text = (armySlider.value * 20).ToString();
            if(gameManager.playerCountry.GetComponent<Country>().coin >= armySlider.value * 20)
            {
                coinText.color = Color.black;
            }
            else
            {
                coinText.color = Color.red;
            }
        }
    }

    public void ArmyButton()
    {
        if (armySlider.value != 0)
        {
            if (armySlider.value * 20 <= gameManager.playerCountry.GetComponent<Country>().coin)
            {
                gameManager.playerCountry.GetComponent<Country>().coin -= armySlider.value * 20;
                gameManager.selectedArea.GetComponent<AreaScript>().armyCount += Mathf.FloorToInt(armySlider.value);
                gameManager.UpdateArmyCountry(gameManager.playerCountry.GetComponent<Country>(), gameManager.selectedArea.GetComponent<AreaScript>(), false);
            }
            else
            {
                gameManager.sendLetter.AddLetterAndOpenMessageMenu("Warning!...", "You Do Not Have Enough Coin To Get " + armySlider.value + " Warriors");
            }
            ArmyMenuClose();
        }
    }


    public void ArmyMenuOpen()
    {
        if (500 - gameManager.selectedArea.GetComponent<AreaScript>().armyCount > 0)
        {
            armyMenu.SetActive(true);
        }
    }

    public void ArmyMenuClose()
    {
        armyMenu.SetActive(false);
    }

}
