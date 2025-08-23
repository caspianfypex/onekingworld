using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{

    public ItemType itemType;
    public int value;
    public float advValue;
    public int requiredLevel = 1;
    public Text valueText;
    public Button upgradeButton;
    public Text percentText;
    public int percent = 0;
    public int advValue2;
    public Text advValueText;
    public GameManager gameManager;
    public Coroutine thisCoroutine = null;
    public Text levelText;
    

    public enum ItemType {

        military,coin,population
    }


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        advValueText = transform.Find("Advantages").Find("advantageText").GetComponent<Text>();
        if (itemType == ItemType.coin || itemType == ItemType.population)
        {
            advValueText.text = "+ 0/sec";
        }
        else if (itemType == ItemType.military)
        {
            advValueText.text = "+ " + advValue.ToString();
        }
        valueText = transform.Find("Value").Find("valueText").gameObject.GetComponent<Text>();
        upgradeButton = transform.Find("Upgrade").gameObject.GetComponent<Button>();
        upgradeButton.onClick.AddListener(TaskOnClick);
        percentText = upgradeButton.gameObject.transform.Find("percentText").gameObject.GetComponent<Text>();
        levelText = transform.Find("levelText").gameObject.GetComponent<Text>();
    }


    public void ColorChangeByColor()
    {
        if(gameManager.playerCountry.GetComponent<Country>().level < requiredLevel)
        {
            transform.Find("Value").gameObject.SetActive(false);
            transform.Find("Advantages").gameObject.SetActive(false);
            upgradeButton.gameObject.SetActive(false);
            levelText.enabled = true;
            levelText.text = "Required Level Is " + requiredLevel.ToString();
        }
        else
        {
            if (percent < 100)
            {
                transform.Find("Value").gameObject.SetActive(true);
                transform.Find("Advantages").gameObject.SetActive(true);
                upgradeButton.gameObject.SetActive(true);
                levelText.enabled = false;
            }
        }
    }

    void Update()
    {
        valueText.text = value.ToString();
        ColorChangeByColor();
    }

    void TaskOnClick()
    {
        Country pc = gameManager.playerCountry.GetComponent<Country>();
        if (percent < 100 && pc.coin >= value)
        {
            gameManager.GetComponent<LevelSystem>().exp += 10;
        }
        if (itemType == ItemType.coin && pc.coin >= value && percent < 100)
        {
            pc.coin -= value;
            if (percent != 0) {
                gameManager.removeMoneyPerSecond(Mathf.FloorToInt(advValue));
            }
            advValue = advValue * 1.5f;
            gameManager.addMoneyPerSecond(Mathf.FloorToInt(advValue));
            AddToAdvCoin();
            startIEnumerator1();
        }
        if(itemType == ItemType.military && pc.coin >= value && percent < 100)
        {
            pc.coin -= value;
            startIEnumerator2();
            AddToAdvMilitary();
            gameManager.UpdateArmyCountry(gameManager.playerCountry.GetComponent<Country>(), gameManager.playerCountry.GetComponent<Country>().capitalArea.GetComponent<AreaScript>(), true);
        }
        if (itemType == ItemType.population && pc.coin >= value && percent < 100)
        {
            pc.coin -= value;
            advValue = advValue * 1.5f;
            startIEnumerator3();
            AddToAdvPopulation();

        }
        if(percent >= 100)
            {
            transform.Find("Value").gameObject.SetActive(false);
            if (itemType == ItemType.military)
            {
                transform.Find("Advantages").gameObject.SetActive(false);
            }
        }
    }

    public void startIEnumerator1()
    {
        gameManager.runAltFunction1(GetComponent<ShopItem>());
    }

    public void startIEnumerator2()
    {
        gameManager.runAltFunction2(GetComponent<ShopItem>());
    }

    public void startIEnumerator3()
    {
        gameManager.runAltFunction3(GetComponent<ShopItem>());
    }


    public IEnumerator coinFunction()
    {
        Country pc = gameManager.playerCountry.GetComponent<Country>();
        if (percent != 80)
        {
            value = Mathf.FloorToInt((value * 1.5f));
        }
        while (true)
        {
            valueText.text = value.ToString();
            pc.coin += Mathf.FloorToInt(advValue);
            yield return new WaitForSeconds(1f);
        }

    }

    public void AddToAdvCoin()
    {
        if (percent < 100) {
            advValueText.text = "+ " + Mathf.FloorToInt(advValue) + "/sec";
            percent += 20;
            percentText.text = percent.ToString() + "/100";
        }
    }

    public void AddToAdvMilitary()
    {
        if (percent < 100)
        {
            advValueText.text = "+ " + advValue;
            percent += 20;
            percentText.text = percent.ToString() + "/100";
        }
    }


    public void AddToAdvPopulation()
    {
        if (percent < 100)
        {
            advValueText.text = "+ " + Mathf.FloorToInt(advValue) + "/sec";
            percent += 20;
            percentText.text = percent.ToString() + "/100";
        }
    }
}
