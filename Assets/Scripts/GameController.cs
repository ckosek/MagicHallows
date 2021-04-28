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

    public List<string> list;

    private void Awake()
    {
        battleMenu = GameObject.Find("ActionMenu");
        list = null;
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
        this.battleMenu.SetActive(true);

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
                if (currentFighterStats.SceneBefore == "The End" && list == null)
                {
                    list = new List<string>{"Is this guy wearing Crocs?", "He has an 8 pack...", "Lazers?!?!", "I wonder what Shampoo he uses", "I have to end this.", "Where is his shirt", "That's all he's got?", "So this is THE Chris Summer", "Ouch", "He is too powerful", "This is a strategy game", "He keeps insulting someone named Chase", "What even is a Loft Bed"};
                } else if (list == null)
                {
                    list = new List<string>{ "Hello there","This guy doesn't quit","What now?","Ouch", "I can't wait to tell Mr. South about this", "...", "Not again", "But what about the Economy", "I can do this", "All this fighting is making me hungry", "Do you think Wendy's is still open", "What the", "Wanna see a magic trick?"};
                }
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
