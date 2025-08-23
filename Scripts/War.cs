using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class War : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject battleMenu;
    public GameObject battleEndMenu;
    public GameObject battleEndMenu2;
    public bool isBattle = false;
    public Slider battleSlider;
    public Button battleButton;
    public Text attackerName;
    public Text defenseName;
    public Coroutine botAttack;
    public Country bot;
    public GameObject horse;
    public GameObject knobPrefab;
    public bool playColorAnimation = false;
    public SoundManager soundManager;
    AreaScript areaToAttack;
    public bool isTryingToBattle;

    public float warCount;

    public void BattleButton()
    {
        if (gameManager.battleMode.isOn)
        {
            bot = gameManager.selectedArea.GetComponent<AreaScript>().country.GetComponent<Country>();
            areaToAttack = gameManager.selectedArea.GetComponent<AreaScript>();
            if (gameManager.playerCountry.GetComponent<Country>().countryTerritories.Count == 1)
            {
                StartCoroutine(StartBattle(gameManager.playerCountry.GetComponent<Country>(), gameManager.selectedArea.GetComponent<AreaScript>().country.GetComponent<Country>(), gameManager.playerCountry.GetComponent<Country>().capitalArea.GetComponent<AreaScript>(),
                    gameManager.selectedArea.GetComponent<AreaScript>()));
            }
            else if (gameManager.playerCountry.GetComponent<Country>().countryTerritories.Count > 1)
            {
                gameManager.selectedArea.GetComponent<SpriteRenderer>().color = gameManager.preAreaColor;
                gameManager.selectedArea = null;
                playColorAnimation = true;
                isTryingToBattle = true;
                StartCoroutine(checkSelectedArea());
            }
        }
        else
        {
            gameManager.sendLetter.AddLetterAndOpenMessageMenu("You Need Activate Battle Mode...", "If You Want To Start Battle Firstly You\nNeed To Activate Battle Mode");
        }
    }

    
    public IEnumerator checkSelectedArea()
    {
        GameObject areaFrom;
        while(gameManager.selectedArea == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        areaFrom = gameManager.selectedArea;
        gameManager.closeAltMenu();
        gameManager.selectedArea = null;
        playColorAnimation = false;
        foreach (GameObject obj in gameManager.playerCountry.GetComponent<Country>().countryTerritories)
        {
            obj.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        StartCoroutine(StartBattle(gameManager.playerCountry.GetComponent<Country>(), bot, areaFrom.GetComponent<AreaScript>(),
            areaToAttack));
    }

    public IEnumerator StartBattle(Country attacker, Country defense, AreaScript from, AreaScript to)
    {
        if(attacker.gameObject == gameManager.playerCountry || defense.gameObject == gameManager.playerCountry)
        {
            soundManager.PlayHornSound();
        }
        gameManager.selectedArea = null;
        foreach(GameObject obj in gameManager.playerCountry.GetComponent<Country>().countryTerritories)
        {
            obj.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        gameManager.closeAltMenu();
        StartCoroutine(HorseAnimation(from, to));
        battleButton.interactable = false;
        attacker.inBattle = true;
        defense.inBattle = true;
        battleButton.interactable = false;
        isBattle = true;
        yield return new WaitForSeconds(1.8f);
        battleButton.interactable = true;
        bool win = false;
        isBattle = false;
        if (from.totalPower > to.totalPower)
        {
            defense.countryTerritories.Remove(to.gameObject);
            if(defense.capitalArea == to.gameObject)
            {
                defense.capitalArea = null;
                foreach (GameObject tr in defense.countryTerritories)
                {
                    defense.capitalArea = tr;
                    break;
                }
            }
            Country.AddTerritory(attacker.gameObject, to.gameObject);
            from.totalPower -= to.totalPower;
            to.totalPower = 0;
            from.armyCount -= to.armyCount;
            to.armyCount = 0;
            win = true;
            from.transform.Find("soldierText").GetComponent<TextMesh>().text = from.totalPower.ToString();
            to.transform.Find("soldierText").GetComponent<TextMesh>().text = to.totalPower.ToString();
            StartCoroutine(EndBattle(attacker, defense));
        }
        else if(from.totalPower < to.totalPower)
        {
            to.totalPower -= from.totalPower;
            from.totalPower = 0;
            to.armyCount -= from.armyCount;
            from.armyCount = 0;
            from.transform.Find("soldierText").GetComponent<TextMesh>().text = from.totalPower.ToString();
            to.transform.Find("soldierText").GetComponent<TextMesh>().text = to.totalPower.ToString();
            if (!gameManager.playerCountry.GetComponent<Country>().countryTerritories.Contains(to.gameObject))
            {
                StartCoroutine(GetComponent<Events>().territoryEvent2(to));
            }
            StartCoroutine(EndBattle(defense, attacker));
        }
        else if (from.totalPower == to.totalPower)
        {
            to.totalPower = 0;
            from.totalPower = 0;
            to.armyCount = 0;
            from.armyCount = 0;
            from.transform.Find("soldierText").GetComponent<TextMesh>().text = from.totalPower.ToString();
            to.transform.Find("soldierText").GetComponent<TextMesh>().text = to.totalPower.ToString();
        }
        if (attacker.gameObject == gameManager.playerCountry)
        {
            gameManager.closeAltMenu();
            if (!win)
            {
                areaToAttack.GetComponent<SpriteRenderer>().color = gameManager.preAreaColor;
            }
            gameManager.selectedArea = null;
            gameManager.preAreaColor = Color.white;
        }
        if (from.country != gameManager.playerCountry && from.armyCount == 0)
        {
            from.armyCount += Random.Range(50, 100);
            gameManager.UpdateArmyCountry(attacker, from, false);
        }
        if (to.country != gameManager.playerCountry && to.armyCount == 0)
        {
            to.armyCount += Random.Range(50, 100);
            gameManager.UpdateArmyCountry(defense, to, false);
        }
        Debug.Log(gameManager.preAreaColor);
        Debug.Log(gameManager.selectedArea);
        attacker.warCount += 1;
        StartCoroutine(GetComponent<Events>().warEvent3(attacker));
        isTryingToBattle = false;
        attacker.inBattle = false;
        defense.inBattle = false;
        gameManager.preAreaColor = Color.white;
    }



    void Update()
    {
        if (playColorAnimation)
        {
            foreach (GameObject ct in gameManager.playerCountry.GetComponent<Country>().countryTerritories)
            {
                gameManager.playColorAnimation(ct.GetComponent<SpriteRenderer>(), true);
            }
        }
    }

    public IEnumerator EndBattle(Country winner, Country loser)
    {
        int winnerIncome = Random.Range(1500, 3500);
        winner.coin += winnerIncome;
        if (winner.gameObject == gameManager.playerCountry)
        {
            gameManager.sendLetter.AddLetter("Winner Of The Battle...", "You Got " + winnerIncome.ToString() + " Coin For Winning " + loser.countryName);
        }
        int loserOutCome = Random.Range(500, 1500);
        if(loserOutCome > loser.coin)
        {
            loser.coin = 0;
        }
        else
        {
            loser.coin -= loserOutCome;
        }
        yield return null;
    } 


    public void PlayerAttack()
    {
        if (bot.armyPower < gameManager.playerCountry.GetComponent<Country>().armyPower)
        {
            battleSlider.value += GiveAttackValue(gameManager.playerCountry.GetComponent<Country>().armyPower * 1.5f);
        }
        else
        {
            battleSlider.value += GiveAttackValue(gameManager.playerCountry.GetComponent<Country>().armyPower * 1.3f);
        }
    }

    public IEnumerator HorseAnimation(AreaScript from, AreaScript to)
    {
        List<GameObject> knobList = new List<GameObject>();
        horse.SetActive(true);
        horse.transform.position = from.transform.position;
        yield return new WaitForSeconds(1f);
        float xSpeed = from.transform.position.x - to.transform.position.x;
        float ySpeed = from.transform.position.y - to.transform.position.y;
        xSpeed /= 20;
        ySpeed /= 20;
        Vector2 speed = new Vector2(xSpeed, ySpeed);
        float distanceBetweenTwoKnobs = 0.2f;
        float distance = 0;
        for(int i = 0; i <= 20; i++)   
        {
            distance += speed.magnitude;
            while(distance >= distanceBetweenTwoKnobs)
            {
                distance -= distanceBetweenTwoKnobs;
                Vector3 posToAdd = (knobList.Count + 1) * (speed.normalized * distanceBetweenTwoKnobs);
                Vector3 aimPos = to.transform.position+posToAdd; 
                GameObject newKnob = Instantiate(knobPrefab, aimPos, knobPrefab.transform.rotation) as GameObject;
                newKnob.transform.position = horse.transform.position;
                knobList.Add(newKnob);
            }
            horse.transform.Translate(-xSpeed, -ySpeed, 0);
            yield return new WaitForSeconds(0.04f);
        }
        horse.SetActive(false);
        foreach (GameObject knb in knobList)
        {
            knb.SetActive(false);
        }
        foreach(GameObject knb in knobList)
        {
            Destroy(knb);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator BotAttack(float attackValue)
    {
        if(bot.armyPower > gameManager.playerCountry.GetComponent<Country>().armyPower)
        {
            attackValue *= 1.5f;
        }
        while (isBattle)
        {
            battleSlider.value -= attackValue;
            yield return new WaitForSeconds(0.3f);
        }
    }

    float GiveAttackValue(float i)
    {
        return i / 100;
    }



    // Choices

    public void Choice1()
    {
        Country pc = gameManager.playerCountry.GetComponent<Country>();
        pc.coin += bot.coin;
        bot.coin = 0;
        battleEndMenu.SetActive(false);
        gameManager.closeAltMenu();
        gameManager.selectedArea = null;
        gameManager.preAreaColor = Color.white;
    }

    public void Choice2()
    {
        bot.countryTerritories.Remove(gameManager.selectedArea);
        Country.AddTerritory(gameManager.playerCountry, gameManager.selectedArea);
        battleEndMenu.SetActive(false);
        gameManager.closeAltMenu();
        gameManager.selectedArea = null;
        gameManager.preAreaColor = Color.white;
    }

    public void Choice3()
    {
        StartCoroutine(bot.GiveCoinToCountry(gameManager.playerCountry.GetComponent<Country>(), 10));
        gameManager.addMoneyPerSecond(10);
        battleEndMenu.SetActive(false);
        gameManager.closeAltMenu();
        gameManager.selectedArea = null;
        gameManager.preAreaColor = Color.white;
    }

    public void Choice4()
    {
        Country pc = gameManager.playerCountry.GetComponent<Country>();
        bot.coin += pc.coin;
        pc.coin = 0;
        battleEndMenu2.SetActive(false);
        gameManager.closeAltMenu();
        gameManager.selectedArea = null;
        gameManager.preAreaColor = Color.white;
    }

    public void Choice5()
    {
        Country pc = gameManager.playerCountry.GetComponent<Country>();
        GameObject randomObj = pc.countryTerritories[Random.Range(0, pc.countryTerritories.Count - 1)];
        pc.countryTerritories.Remove(randomObj);
        Country.AddTerritory(bot.gameObject, randomObj);
        battleEndMenu2.SetActive(false);
        gameManager.closeAltMenu();
        gameManager.selectedArea = null;
        gameManager.preAreaColor = Color.white;
    }

    public void Choice6()
    {
        StartCoroutine(bot.GetCoinFromCountry(gameManager.playerCountry.GetComponent<Country>(), 10));
        gameManager.addMoneyPerSecond(-10);
        battleEndMenu2.SetActive(false);
        gameManager.closeAltMenu();
        gameManager.selectedArea = null;
        gameManager.preAreaColor = Color.white;
    }
}
