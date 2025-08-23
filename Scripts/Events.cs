using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Events : MonoBehaviour
{

    public GameManager gameManager;
    public War war;
    public SendLetter sendLetter;
    public Profile profile;
    public GameObject finishMenu1;
    public GameObject finishMenu2;

    void Start()
    {
        war = gameManager.GetComponent<War>();
        StartCoroutine(coinEvent());
        StartCoroutine(warEvent());
        StartCoroutine(warEvent2());
        StartCoroutine(finishGame());
        StartCoroutine(deleteCountry());
        StartCoroutine(showAd());
    }



    public IEnumerator coinEvent()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (gameManager.playerArea != "Yok")
            {
                break;
            }
        }
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(180, 420));
            int coinToGive = Random.Range(1000, 5000);
            sendLetter.AddLetter("A New Ore Has Been Found", "The Ore That Has Been Found Is Very Special" +
                "\nBecause It Is Very Rare And Very Beatiful" +
                "\nYou Sold It To One Of The Countries And You Got " + coinToGive.ToString() + " Coin");
            gameManager.playerCountry.GetComponent<Country>().coin += coinToGive;
        }
    }

    public IEnumerator warEvent()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (gameManager.playerArea != "Yok")
            {
                break;
            }
        }
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(300f, 360f));
            Country rc1 = new Country();
            while (true)
            {
                Country rcc = gameManager.countries[Random.Range(0, gameManager.countries.Count - 1)].GetComponent<Country>();
                if (!rcc.inBattle && rcc.gameObject != gameManager.playerCountry && rcc.gameObject.active)
                {
                    rc1 = rcc;
                    break;
                }
                yield return new WaitForSeconds(0.01f);
            }
            if (rc1 != null) {
                sendLetter.AddLetterAndOpenMessageMenu("The War Is Very Close...", "The New War Is Very Close" +
                    "\nYour Spies Brought New Information" +
                    "\nCountry " + rc1.name + " Will Attack You In 60 Seconds" +
                    "\nBe Ready For The War If Your Army Is Powerful You Will Win" +
                    "\nElse You Will Lose!!!");
                yield return new WaitForSeconds(60f);
                if (!war.isBattle) {
                    Country pc = gameManager.playerCountry.GetComponent<Country>();
                    AreaScript rArea = pc.countryTerritories[Random.Range(0, pc.countryTerritories.Count - 1)].GetComponent<AreaScript>();
                    StartCoroutine(war.StartBattle(rc1, gameManager.playerCountry.GetComponent<Country>(), rc1.capitalArea.GetComponent<AreaScript>(), rArea));
                }
            }
            else
            {
                Debug.LogError("Every Country Is In Battle");
                break;
            }
        }
    }

    public IEnumerator warEvent2()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (gameManager.playerArea != "Yok")
            {
                break;
            }
        }
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(240f, 300f));

            Country rc1 = new Country();
            while(true)
            {
                Country rcc = gameManager.countries[Random.Range(0, gameManager.countries.Count - 1)].GetComponent<Country>();
                if (!rcc.inBattle && rcc.gameObject != gameManager.playerCountry && rcc.gameObject.active)
                {
                    rc1 = rcc;
                    break;
                }
                yield return new WaitForSeconds(0.01f);
            }
            Country rc2 = new Country();
            while (true)
            {
                Country rcc = gameManager.countries[Random.Range(0, gameManager.countries.Count - 1)].GetComponent<Country>();
                if (!rcc.inBattle && rcc.gameObject != gameManager.playerCountry && rcc.gameObject != rc1.gameObject && rcc.gameObject.active)
                {
                    rc2 = rcc;
                    break;
                }
                yield return new WaitForSeconds(0.01f);
            }

            StartCoroutine(war.StartBattle(rc1, rc2, rc1.countryTerritories[Random.Range(0, rc1.countryTerritories.Count - 1)].GetComponent<AreaScript>(), rc2.countryTerritories[Random.Range(0, rc2.countryTerritories.Count - 1)].GetComponent<AreaScript>()));
        }
    }


    // if someone does a lot of attacks so another countries will attack him for stopping it.
    public IEnumerator warEvent3(Country cr)
    {
        yield return new WaitForSeconds(3f);
        Country cra = new Country();
        if(cr.warCount >= 3 && !war.isBattle)
        {
            cr.warCount -= 1;
            cra = gameManager.countries[Random.Range(0, gameManager.countries.Count - 1)].GetComponent<Country>();
            while(true)
            {

                cra = gameManager.countries[Random.Range(0, gameManager.countries.Count - 1)].GetComponent<Country>();
                if(cra != gameManager.playerCountry.GetComponent<Country>() && cra != cr && cra.countryTerritories.Count >= 1)
                {
                    break;
                }
                yield return new WaitForSeconds(0.1f);
            }
            while (cr.inBattle)
            {
                yield return new WaitForSeconds(0.1f);
            }
            while (cra.inBattle)
            {
                yield return new WaitForSeconds(0.1f);
            }
            StartCoroutine(war.StartBattle(cra, cr, cra.capitalArea.GetComponent<AreaScript>(), cr.countryTerritories[Random.Range(0, cr.countryTerritories.Count - 1)].GetComponent<AreaScript>()));
            yield return new WaitForSeconds(1.8f);
        }
            
        
    }

    public IEnumerator territoryEvent1()
    {
        while (true)
        {
            if (gameManager.countries.Count >= 6)
            {
                for(int i = 0; i < gameManager.countries.Count; i++)
                {
                    GameObject crObj = gameManager.countries[i];
                    Country cr = crObj.GetComponent<Country>();
                    for(int a = 0; a < cr.countryTerritories.Count; a++)
                    {
                        GameObject trObj = cr.countryTerritories[i];
                        if (cr.countryTerritories.Contains(trObj))
                        {
                            AreaScript areaScr = trObj.GetComponent<AreaScript>();
                            if (areaScr.armyCount == 0)
                            {
                                yield return new WaitForSeconds(Random.Range(1, 3));
                                if (areaScr.country != gameManager.playerCountry)
                                {
                                    areaScr.armyCount += Random.Range(50, 100);
                                    gameManager.UpdateArmyCountry(cr, areaScr, false);
                                }
                            }
                        }
                        if(cr.countryTerritories.Count == 1)
                        {
                            break;
                        }
                    }
                }
            }
            yield return new WaitForSeconds(10f);
        }
    }

    public IEnumerator territoryEvent2(AreaScript areaScr)
    {
        yield return new WaitForSeconds(Random.Range(10f, 15f));
        if (areaScr.country != gameManager.playerCountry)
        {
            if (areaScr.totalPower <= 3000)
            {
                areaScr.totalPower += Random.Range(1500, 2000);
            }
            else
            {
                areaScr.totalPower += Random.Range(500, 1000);
            }
            areaScr.transform.Find("soldierText").GetComponent<TextMesh>().text = areaScr.totalPower.ToString();
        }
    }

    public IEnumerator deleteCountry()
    {
        while (true)
        {
            foreach(GameObject crObj in gameManager.countries)
            {
                Country cr = crObj.GetComponent<Country>();
                if(cr.countryTerritories.Count <= 0)
                {
                    crObj.SetActive(false);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator finishGame()
    {
        while (true)
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                if (gameManager.playerArea != "Yok")
                {
                    break;
                }
            }
            int i = 0;
            foreach(GameObject crObj in gameManager.countries)
            {
                if (!crObj.active)
                {
                    i++;
                }
            }
            if(i == 8)
            {
                Time.timeScale = 0;
                finishMenu1.SetActive(true);
            }
            if (!gameManager.playerCountry.active)
            {
                Time.timeScale = 0;
                finishMenu2.SetActive(true);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator showAd()
    {
        while (true)
        {
            GameMonetize.Instance.ShowAd();
            yield return new WaitForSeconds(90);
        }
    }
}
