using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Transactions;
using UnityEngine.SocialPlatforms;
using TMPro;

public class GameController : MonoBehaviour
{
    private List<FighterStats> fighterStats;

    private GameObject battleMenu;

    public Text battleText;

    public TextMeshProUGUI heroComments;

    private void Awake()
    {
        battleMenu = GameObject.Find("ActionMenu");
    }
    void Start()
    {
        fighterStats = new List<FighterStats>();
        GameObject hero = GameObject.FindGameObjectWithTag("Hero");
        FighterStats currentFighterStats = hero.GetComponent<FighterStats>();
        currentFighterStats.CalculateNextTurn(0);
        fighterStats.Add(currentFighterStats);

        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        FighterStats currentEnemyStats = enemy.GetComponent<FighterStats>();
        currentEnemyStats.CalculateNextTurn(0);
        fighterStats.Add(currentEnemyStats);

        fighterStats.Sort();
        

        NextTurn();
    }

    public void NextTurn()
    {
        battleText.gameObject.SetActive(false);
        FighterStats currentFighterStats = fighterStats[0];
        fighterStats.Remove(currentFighterStats);
        if (!currentFighterStats.GetDead())
        {
            GameObject currentUnit = currentFighterStats.gameObject;
            currentFighterStats.CalculateNextTurn(currentFighterStats.nextActTurn);
            fighterStats.Add(currentFighterStats);
            fighterStats.Sort();
            if(currentUnit.tag == "Hero")
            {
                //Debug.Log("HELP");
                var random = new System.Random();
                var list = new List<string>{ "Hello there","This guy doesn't quit","What now?","Ouch", "I can't wait to tell Mr. South about this", "...", "Not again", "But what about the Economy", "I can do this", "All this fighting is making me hungry", "Do you think Wendy's is still open"};
                int index = random.Next(list.Count);
                heroComments.text = list[index];
                this.battleMenu.SetActive(true);
            } else
            {
                this.battleMenu.SetActive(false);
                string attackType = UnityEngine.Random.Range(0, 2) == 1 ? "melee" : "range";
                currentUnit.GetComponent<FighterAction>().SelectAttack(attackType);
            }
        } else
        {
            NextTurn();
        }
    }
}
