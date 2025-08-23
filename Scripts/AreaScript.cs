using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AreaScript : MonoBehaviour
{

    public string territoryName;
    public string owner;
    public GameObject country;
    public bool isOwned;
    public int population;
    public float totalPower;
    public int preArmyCount = 0;
    public int armyCount = 0;
    public GameManager gameManager;
    bool isClickedIt;
    public bool isInvestigated = false;

    private void Start()
    {
        country = null;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        territoryName = this.name;
        owner = "None";
        population = 0;
        armyCount = 0;
    }

}
