using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterAction : MonoBehaviour
{
    private GameObject hero;
    private GameObject enemy;
    private GameObject display;

    [SerializeField]
    private GameObject meleePrefab;

    [SerializeField]
    private GameObject rangePrefab;

    [SerializeField]
    private GameObject specialPrefab;

    [SerializeField]
    private Sprite faceIcon;

    private GameObject currentAttack;

    void Awake()
    {
        hero = GameObject.FindGameObjectWithTag("Hero");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        display = GameObject.Find("ActionMenu");
    }

    public void SelectAttack(string btn)
    {
        display.SetActive(false);
        GameObject victim = hero;
        if (tag == "Hero")
        {
            victim = enemy;
        }
        if (btn.CompareTo("melee") == 0)
        {
            meleePrefab.GetComponent<AttackScript>().Attack(victim);

        } else if (btn.CompareTo("range") == 0)
        {
            rangePrefab.GetComponent<AttackScript>().Attack(victim);

        } else if (btn.CompareTo("heal") == 0)
        {
            specialPrefab.GetComponent<AttackScript>().Heal(hero);

        }else if(btn.CompareTo("run") == 0)
        {
            //UnityEngine.SceneManagement.SceneManager.LoadScene("Tedshire");
            FighterStats f = victim.GetComponent<FighterStats>();
            UnityEngine.SceneManagement.SceneManager.LoadScene(f.SceneBefore);
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
    }
}
