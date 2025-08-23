using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Country : MonoBehaviour
{
    public string countryName;
    public string owner;
    public GameObject capitalArea;
    public float armyPower;
    public int level;
    public int army;
    public float coin;
    public int happiness;
    public string technology;
    public int countryPopulation;
    public bool inBattle;
    public int warCount;
    public List<GameObject> countryTerritories = new List<GameObject>();

    void Start()
    {

    }

    void Update()
    {
        army = armyCount();
        countryPopulation = crPopulationCount();
    }

    private int armyCount()
    {
        int i = 0;
        foreach (GameObject trt in countryTerritories)
        {
            i += trt.GetComponent<AreaScript>().armyCount;
        }
        return i;
    }

    private int crPopulationCount()
    {
        int i = 0;
        foreach (GameObject trt in countryTerritories)
        {
            i += trt.GetComponent<AreaScript>().population;
        }
        return i;
    }

    public static void CountryCreate(string owner, string cn, GameObject ca, int arpow, int coi, int happns, string techn)
    {
        GameObject country = new GameObject();
        Country newCr = country.AddComponent<Country>();
        newCr.owner = owner;
        newCr.countryName = cn;
        newCr.capitalArea = ca;
        newCr.armyPower = arpow;
        newCr.coin = coi;
        newCr.happiness = happns;
        newCr.technology = techn;
        newCr.level = 1;
        newCr.countryTerritories.Add(ca);
        ca.GetComponent<AreaScript>().country = country;
        country.name = cn;
        country.transform.position = ca.transform.position;
        GameObject.Find("GameManager").GetComponent<GameManager>().countries.Add(country);
    }

    public static void AddTerritory(GameObject country, GameObject territory)
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gameManager.countries.Contains(country))
        {
            AreaScript areaScr = territory.GetComponent<AreaScript>();
            Country cr = country.GetComponent<Country>();
            cr.countryTerritories.Add(territory);
            if (gameManager.populationGenerate(territory) != -1)
            {
                areaScr.population = gameManager.populationGenerate(territory);
            }
            territory.GetComponent<SpriteRenderer>().color = cr.capitalArea.GetComponent<SpriteRenderer>().color;
            areaScr.owner = cr.owner;
            areaScr.country = country;
            if (!areaScr.isOwned)
            {
                areaScr.isOwned = true;
            }

        }
    }

    public static void SendArmyToTerritoryFromTerritory(GameObject territory1, GameObject territory2, int count)
    {
        if (territory2.GetComponent<AreaScript>().armyCount >= count)
        {
            territory2.GetComponent<AreaScript>().armyCount -= count;
            territory2.GetComponent<AreaScript>().armyCount += count;
        }
    }

    public IEnumerator GiveCoinToCountry(Country country, int coin)
    {
        while (true)
        {
            this.coin -= coin;
            country.coin += coin;
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator GetCoinFromCountry(Country country, int coin)
    {
        while (true)
        {
            this.coin += coin;
            country.coin -= coin;
            yield return new WaitForSeconds(1f);
        }
    }
}
