
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public string playerArea;
    public Profile profile;
    public GameObject playerCountry;
    bool yan_son_anim_azal = true;
    public Color defaultTerritoryColor;
    bool willColorChange = true;
    public GameObject altMenu;
    public GameObject altMenu2;
    public GameObject armyMenu;
    public SendLetter sendLetter;
    public Color preAreaColor = Color.white;
    public GameObject selectedArea;
    public List<GameObject> countries = new List<GameObject>();
    public List<ShopItem> shopItems = new List<ShopItem>();
    Coroutine altFunc1;
    bool willUpdateVariables2Run = true;
    public List<Text> coinTexts = new List<Text>();
    float moneyFromShop;
    public Toggle battleMode;

    void Start()
    {
        selectedArea = null;
        sendLetter = GetComponent<SendLetter>();
        playerArea = "Yok";
        altFunc1 = null;
        StartCoroutine(profile.updateVariables1());
        profile.coroutine = StartCoroutine(profile.updateVariables2());
    }

    void Update()
    {
        RaycastHit2D hit = new RaycastHit2D();
        if (Input.GetMouseButtonDown(0))
        {
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), -Vector2.up);
        }
        
        if (Input.GetMouseButtonDown(0) && hit.collider != null && hit.collider.gameObject.name.Contains("area") && Time.timeScale != 0) 
        {
            AreaScript areaScr = hit.collider.gameObject.GetComponent<AreaScript>();
            if (playerArea.Equals("Yok"))
            {
                playerArea = areaScr.gameObject.name;
                Country.CountryCreate("Suleyman Sinan Hz.", "Undabek", areaScr.gameObject, 15, 12000, 50, "bad");
                areaScr.population = populationGenerate(areaScr.gameObject);
                areaScr.isOwned = true;
                areaScr.armyCount = 100;
                playerCountry = GameObject.Find("Undabek");
                GameObject textObj = new GameObject("soldierText");
                textObj.transform.parent = areaScr.gameObject.transform;
                textObj.transform.position = areaScr.gameObject.transform.position;
                TextMesh textMesh = textObj.AddComponent<TextMesh>();
                textMesh.characterSize = 0.1f;
                textMesh.fontSize = 105;
                textMesh.color = Color.black;
                textMesh.fontStyle = FontStyle.Bold;
                textMesh.anchor = TextAnchor.MiddleCenter;
                textMesh.text = areaScr.totalPower.ToString();
                areaScr.GetComponent<SpriteRenderer>().color = Color.yellow;
                StartCoroutine(botsTerritoriesActivate());
            }
            if (!GetComponent<War>().isBattle && !GetComponent<ArmyScript>().armyMenu.active)
            {
                if (!GetComponent<War>().isTryingToBattle)
                {
                    if (preAreaColor == Color.white)
                    {
                        preAreaColor = areaScr.GetComponent<SpriteRenderer>().color;
                    }
                    if (selectedArea != null)
                    {
                        selectedArea.GetComponent<SpriteRenderer>().color = preAreaColor;
                        preAreaColor = areaScr.GetComponent<SpriteRenderer>().color;
                    }
                    selectedArea = areaScr.gameObject;
                    openAltMenu();
                }
                if (GetComponent<War>().isTryingToBattle && playerCountry.GetComponent<Country>().countryTerritories.Contains(areaScr.gameObject))
                {
                    selectedArea = areaScr.gameObject;
                }
            }
        }

        if (playerArea.Equals("Yok"))
        {
            for (int i = 1; i != 20; i++)
            {
                GameObject obj = GameObject.Find("area" + i.ToString());
                playColorAnimation(obj.GetComponent<SpriteRenderer>());
            }
        }
        else
        {
            if (willColorChange)
            {
                for (int i = 1; i != 20; i++)
                {
                    GameObject obj = GameObject.Find("area" + i.ToString());
                    if (!obj.GetComponent<AreaScript>().isOwned)
                    {
                        obj.GetComponent<SpriteRenderer>().color = defaultTerritoryColor;
                    }
                }
                willColorChange = false;
            }
        }

        if (playerArea != "Yok")
        {
            foreach (Text coinT in coinTexts)
            {
                coinT.text = playerCountry.GetComponent<Country>().coin.ToString();
                if (coinT.transform.Find("coinPerSecondText")){
                    Text altText = coinT.transform.Find("coinPerSecondText").gameObject.GetComponent<Text>();
                    altText.text = (profile.coinPerSecond() + moneyFromShop).ToString() + "/sec  <color=red>-" + profile.coinPerMinute().ToString() + "/min </color>";
                    if ((profile.coinPerSecond() + moneyFromShop) < 0)
                    {
                        altText.color = Color.red;
                    }
                    else
                    {
                        altText.color = Color.blue;
                    }
                }
            }
        }

        profile.advAndDisUpdate(profile.huntingDes, profile.huntsliderObj);
        profile.advAndDisUpdate(profile.educationDes, profile.educationsliderObj);
        profile.advAndDisUpdate(profile.soldierDes, profile.soldiersliderObj);
        profile.advAndDisUpdate(profile.investigationDes, profile.investigationsliderObj);
        profile.profileUpdate();
        if (profile.soldiersliderObj.GetComponent<Slider>().value == 0)
        {
            StopCoroutine(profile.coroutine);
            willUpdateVariables2Run = true;
        }
        else
        {
            if (willUpdateVariables2Run)
            {
                profile.coroutine = StartCoroutine(profile.updateVariables2());
                willUpdateVariables2Run = false;
            }
        }
        if (selectedArea)
        {
            playColorAnimation(selectedArea.GetComponent<SpriteRenderer>());
        }

        if (playerArea != "Yok")
        {
            foreach (GameObject tr in playerCountry.GetComponent<Country>().countryTerritories)
            {
                if (tr.GetComponent<AreaScript>().armyCount > 500)
                {
                    tr.GetComponent<AreaScript>().armyCount = 450;
                    UpdateArmyCountry(playerCountry.GetComponent<Country>(), tr.GetComponent<AreaScript>(), false);
                }
            }
        }

    }

    public void UpdateArmy()
    {
        if (countries.Count >= 8)
        {
            foreach (GameObject obj in countries)
            {
                Country cr = obj.GetComponent<Country>();
                foreach (GameObject crt in cr.countryTerritories)
                {
                    AreaScript areaScr = crt.GetComponent<AreaScript>();
                    areaScr.totalPower = Mathf.FloorToInt(cr.armyPower * areaScr.armyCount);
                    areaScr.transform.Find("soldierText").GetComponent<TextMesh>().text = areaScr.totalPower.ToString();
                }
            }
        }
    }

    public void UpdateArmyCountry(Country cr, AreaScript areaScr, bool all)
    {
        if (all)
        {
            foreach (GameObject crt in cr.countryTerritories)
            {
                AreaScript areaSc = crt.GetComponent<AreaScript>();
                areaSc.totalPower = Mathf.FloorToInt(cr.armyPower * areaSc.armyCount);
                areaSc.transform.Find("soldierText").GetComponent<TextMesh>().text = areaSc.totalPower.ToString();
            }
        }
        else
        {
            areaScr.totalPower = Mathf.FloorToInt(cr.armyPower * areaScr.armyCount);
            areaScr.transform.Find("soldierText").GetComponent<TextMesh>().text = areaScr.totalPower.ToString();
        }
    }

    public void addMoneyPerSecond(float i)
    {
        moneyFromShop += i;
    }

    public void removeMoneyPerSecond(float i)
    {
        moneyFromShop -= i;
    }

    public void runAltFunction1(ShopItem si)
    {
        if (si.thisCoroutine != null)
        {
            StopCoroutine(si.thisCoroutine);
        }
        si.thisCoroutine = StartCoroutine(activateAltFunction1(si));
    }

    public IEnumerator activateAltFunction1(ShopItem si)
    {
        Country pc = playerCountry.GetComponent<Country>();
        si.value = Mathf.FloorToInt((si.value * 1.5f));
        si.valueText.text = si.value.ToString();
        while (true)
        {
            pc.coin += Mathf.FloorToInt(si.advValue);
            yield return new WaitForSeconds(1f);
        }
    }

    public void runAltFunction2(ShopItem si)
    {
        if (si.thisCoroutine != null)
        {
            StopCoroutine(si.thisCoroutine);
        }
        si.thisCoroutine = StartCoroutine(activateAltFunction2(si));
    }

    public IEnumerator activateAltFunction2(ShopItem si)
    {
        Country pc = playerCountry.GetComponent<Country>();
        if (si.percent != 80)
        {
            si.value = Mathf.FloorToInt((si.value * 1.5f));
            si.valueText.text = si.value.ToString();
        }
        pc.armyPower += si.advValue;
        yield return new WaitForSeconds(0f);
    }

    public void runAltFunction3(ShopItem si)
    {
        if (si.thisCoroutine != null)
        {
            StopCoroutine(si.thisCoroutine);
        }
        si.thisCoroutine = StartCoroutine(activateAltFunction3(si));
    }

    public IEnumerator activateAltFunction3(ShopItem si)
    {
        Country pc = playerCountry.GetComponent<Country>();
        si.value = Mathf.FloorToInt((si.value * 1.5f));
        si.valueText.text = si.value.ToString();
        while (true)
        {
            int populationTATET;
            populationTATET = Mathf.FloorToInt(Mathf.FloorToInt(si.advValue) / pc.countryTerritories.Count);
            foreach (GameObject areaObj in pc.countryTerritories)
            {
                AreaScript areaScr = areaObj.GetComponent<AreaScript>();
                areaScr.population += populationTATET;
            }
            yield return new WaitForSeconds(1f);
        }
    }


    public IEnumerator botsTerritoriesActivate()
    {
        for(int i = 1; i < 9; i++)
        {
            GameObject obj = GameObject.Find("area" + Random.Range(1, 19).ToString());
            while (obj.name.Equals(playerArea))
            {
                obj = GameObject.Find("area" + Random.Range(1, 19).ToString());
            }
            while (obj.GetComponent<AreaScript>().isOwned)
            {
                obj = GameObject.Find("area" + Random.Range(1, 19).ToString());
            }
            obj.GetComponent<AreaScript>().population = populationGenerate(obj);
            obj.GetComponent<AreaScript>().isOwned = true;
            obj.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
            obj.GetComponent<AreaScript>().owner = ("Bot" + i.ToString());
            obj.GetComponent<AreaScript>().armyCount = Random.Range(100, 500);
            GameObject textObj = new GameObject("soldierText");
            textObj.transform.parent = obj.transform;
            textObj.transform.position = obj.transform.position;
            TextMesh textMesh = textObj.AddComponent<TextMesh>();
            textMesh.characterSize = 0.1f;
            textMesh.fontSize = 105;
            textMesh.color = Color.white;
            textMesh.fontStyle = FontStyle.Bold;
            textMesh.anchor = TextAnchor.MiddleCenter;
            Country.CountryCreate(obj.GetComponent<AreaScript>().owner, obj.GetComponent<AreaScript>().owner, obj, Random.Range(30, 55), 1500, 50, "good");
            textMesh.text = obj.GetComponent<AreaScript>().totalPower.ToString();
            yield return new WaitForSeconds(0.2f);
        }
        UpdateArmy();
    }

    
    public void playColorAnimation(SpriteRenderer spr, bool c = default(bool))
    {
        if (!c)
        {
            bool yanipSon = true;
            float yanSonAnimA = spr.color.a;
            if (yan_son_anim_azal)
            {
                if (yanSonAnimA > 0.1f)
                {
                    yanSonAnimA -= 0.005f;
                }
                else
                {
                    yan_son_anim_azal = false;
                }
            }
            else
            {
                if (yanSonAnimA < 0.9f)
                {
                    yanSonAnimA += 0.005f;
                }
                else
                {
                    yan_son_anim_azal = true;
                }
            }
            spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, yanSonAnimA);
        }
        else if (c)
        {
            bool yanipSon = true;
            float yanSonAnimA = spr.color.r;
            if (yan_son_anim_azal)
            {
                if (yanSonAnimA > 0.1f)
                {
                    yanSonAnimA -= 0.005f;
                }
                else
                {
                    yan_son_anim_azal = false;
                }
            }
            else
            {
                if (yanSonAnimA < 0.9f)
                {
                    yanSonAnimA += 0.005f;
                }
                else
                {
                    yan_son_anim_azal = true;
                }
            }
            spr.color = new Color(yanSonAnimA, spr.color.g, spr.color.b, spr.color.a);
        }
    }

    public int populationGenerate(GameObject obj)
    {
        if (!obj.GetComponent<AreaScript>().isOwned)
        {
            SpriteRenderer spr = obj.GetComponent<SpriteRenderer>();
            float i = (950f * (spr.bounds.size.x * spr.bounds.size.y) * (spr.bounds.size.x * spr.bounds.size.y));
            return Mathf.FloorToInt(i);
        }
        else
        {
            return -1;
        }
    }

    public void openAltMenu()
    {
        if (selectedArea != null && !GetComponent<ArmyScript>().armyMenu.active)
        {
            Debug.Log(selectedArea.GetComponent<AreaScript>().country);
            altMenu2.SetActive(false);
            altMenu.SetActive(false);
            if (selectedArea.GetComponent<AreaScript>().country != playerCountry)
            {
                altMenu.SetActive(true);
            }
            else
            {
                altMenu2.SetActive(true);
            }
        }
    }

    public void closeAltMenu()
    {
        altMenu.SetActive(false);
        altMenu2.SetActive(false);
        armyMenu.SetActive(false);
    }

    public void sendExpedition()
    {
        AreaScript areaScr = selectedArea.GetComponent<AreaScript>();
        if (areaScr.country != null)
        {
            Country areaCr = areaScr.country.GetComponent<Country>();
            sendLetter.AddLetterAndOpenMessageMenu("Owner: " + areaScr.owner + " Population: " + areaScr.population, "Owner: " + areaScr.owner + "\nPopulation: " + areaScr.population
            + "\nCountry: " + areaScr.country.name + "\nCountry Coin: " + areaCr.coin.ToString() + "\nCountry Capital: " + areaCr.capitalArea.name + "\nCountry Army Power: " + areaCr.armyPower.ToString());
        }
        else
        {
           sendLetter.AddLetterAndOpenMessageMenu("Owner: " + areaScr.owner + " Population: " + areaScr.population, "Owner: " + areaScr.owner + "\nPopulation: " + areaScr.population);
        }
        
        areaScr.isInvestigated = true;
    }






}
