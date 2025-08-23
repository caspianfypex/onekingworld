using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{

    public Text profileText;
    public GameManager gameManager;
    public GameObject profileMenu;
    public Text countryText;
    public Text populationText;
    public GameObject huntingDes;
    public GameObject educationDes;
    public GameObject soldierDes;
    public GameObject investigationDes;
    public GameObject huntsliderObj;
    public GameObject educationsliderObj;
    public GameObject soldiersliderObj;
    public GameObject investigationsliderObj;
    public Coroutine coroutine = null;

    public int hunting;
    public int education;
    public int military;
    public int investigation;

    void Update()
    {
        profileUpdate();
        advAndDisUpdate(huntingDes, huntsliderObj);
        advAndDisUpdate(educationDes, educationsliderObj);
        advAndDisUpdate(soldierDes, soldiersliderObj);
        advAndDisUpdate(investigationDes, investigationsliderObj);
    }

    public void profileUpdate()
    {
        if (gameManager.playerArea != "Yok")
        {
            Country cr = gameManager.playerCountry.GetComponent<Country>();
            if (cr.capitalArea != null)
            {
                countryText.text = cr.capitalArea.name;
            }
            populationText.text = cr.countryPopulation.ToString();
            profileText.text = "Profile - Level " + cr.level;
        }
    }

    public void OpenProfile()
    {
        profileMenu.SetActive(true);
    }

    public void CloseProfile()
    {
        profileMenu.SetActive(false);
    }

    public void advAndDisUpdate(GameObject obj, GameObject sliderObj)
    {
        Text desText = obj.transform.Find("desText").GetComponent<Text>();
        Text advText = null;
        if (obj.transform.Find("advImage"))
        {
            advText = obj.transform.Find("advImage").transform.Find("avText").GetComponent<Text>();
        }
        Slider slider = sliderObj.GetComponent<Slider>();
        desText.text = "-" + Mathf.FloorToInt((slider.value * 100 * 2)).ToString() + "/sec";
        if (obj.name.Contains("soldierDes"))
        {
            desText.text = "-" + Mathf.FloorToInt((slider.value * 100 * 2 * 60)).ToString() + "/min";
        }
        if (obj.name.Contains("huntingDes"))
        {
            desText.text = "-" + Mathf.FloorToInt((slider.value * 100)).ToString() + "/sec";
        }
        if (obj.name.Contains("educationDes"))
        {
            desText.text = "-" + Mathf.FloorToInt((slider.value * 85)).ToString() + "/sec";
        }
        if (advText != null)
        {
            if (!obj.name.Contains("soldierDes") && !obj.name.Contains("educationDes") && !obj.name.Contains("huntingDes"))
            {
                advText.text = "+" + Mathf.FloorToInt((slider.value * 100)).ToString() + "/sec";
            }
            else if(obj.name.Contains("soldierDes"))
            {
                advText.text = "+" + (Mathf.FloorToInt((slider.value * 100)) / 10).ToString() + "/min";
            }
            else if (obj.name.Contains("educationDes"))
            {
                advText.text = "+" + Mathf.FloorToInt((slider.value * 120)).ToString() + "/sec";
            }
            else if (obj.name.Contains("huntingDes"))
            {
                advText.text = "+" + Mathf.FloorToInt((slider.value * 65)).ToString() + "/sec";
            }
        }
    }

    public IEnumerator updateVariables1()
    {
        while(gameManager.playerArea == "Yok")
        {

            yield return new WaitForSeconds(0.1f);
        }
        Country pc = gameManager.playerCountry.GetComponent<Country>();
        while (true)
        {
            int populationTRFET;
            if (pc.countryPopulation >= Mathf.FloorToInt((huntsliderObj.GetComponent<Slider>().value * 100)))
            {
                populationTRFET = Mathf.FloorToInt(Mathf.FloorToInt((huntsliderObj.GetComponent<Slider>().value * 100)) / pc.countryTerritories.Count);

                foreach (GameObject areObj in pc.countryTerritories)
                {
                    
                    AreaScript areaScr = areObj.GetComponent<AreaScript>();
                    if (areaScr.population >= populationTRFET)
                    {
                        areaScr.population -= populationTRFET;
                    }
                }
                pc.coin += Mathf.FloorToInt((huntsliderObj.GetComponent<Slider>().value * 65));
            }
            if (pc.coin >= Mathf.FloorToInt((educationsliderObj.GetComponent<Slider>().value * 85)))
            {
                populationTRFET = Mathf.FloorToInt(Mathf.FloorToInt((educationsliderObj.GetComponent<Slider>().value * 120)) / pc.countryTerritories.Count);
                foreach (GameObject areObj in pc.countryTerritories)
                {
                    AreaScript areaScr = areObj.GetComponent<AreaScript>();
                    areaScr.population += populationTRFET;
                }
                pc.coin -= Mathf.FloorToInt((educationsliderObj.GetComponent<Slider>().value * 85));
            }
            if(pc.coin >= Mathf.FloorToInt((investigationsliderObj.GetComponent<Slider>().value * 100 * 2)))
            {
                pc.coin -= Mathf.FloorToInt((investigationsliderObj.GetComponent<Slider>().value * 100 * 2));
            }
            yield return new WaitForSeconds(1);
        }
    }

    public IEnumerator updateVariables2()
    {
        while (gameManager.playerArea == "Yok")
        {

            yield return new WaitForSeconds(0.1f);
        }
        while (true)
        {
            yield return new WaitForSeconds(60);
            Country pc = gameManager.playerCountry.GetComponent<Country>();
            if (pc.coin >= Mathf.FloorToInt((soldiersliderObj.GetComponent<Slider>().value * 100 * 2 * 60)))
            {
                pc.armyPower += (Mathf.FloorToInt((soldiersliderObj.GetComponent<Slider>().value * 100) / 10));
                pc.coin -= Mathf.FloorToInt((soldiersliderObj.GetComponent<Slider>().value * 100 * 2 * 60));
            }
        }
    }

    public int coinPerSecond()
    {
        int i = Mathf.FloorToInt((huntsliderObj.GetComponent<Slider>().value * 65)) - Mathf.FloorToInt((educationsliderObj.GetComponent<Slider>().value * 85)) - Mathf.FloorToInt((investigationsliderObj.GetComponent<Slider>().value * 100 * 2));

        return i;
    }

    public int coinPerMinute()
    {
        return Mathf.FloorToInt((soldiersliderObj.GetComponent<Slider>().value * 100 * 2 * 60));
    }
}
