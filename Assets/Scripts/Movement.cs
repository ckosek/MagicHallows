using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
   public float moveSpeed;
   public LayerMask solidObjectsLayer;
   public LayerMask battleTrigger;
   public LayerMask blockedTrigger;
   public LayerMask TransitionTrigger;
   public bool isMoving;
   private Vector2 input;
   private Animator animator;
   SavePlayerPos playerPosData;
   public float PerChance;

   private Vector3 targetPos;


   private void Awake()
   {
       animator = GetComponent<Animator>();
       playerPosData = FindObjectOfType<SavePlayerPos>();
       playerPosData.PlayerPosLoad();
   }

   private void Update()
   {
       if(!isMoving)
       {
           input.x = Input.GetAxisRaw("Horizontal");
           input.y = Input.GetAxisRaw("Vertical");

            //remove diagonal
            if(input.x != 0) input.y = 0;

           if (input != Vector2.zero)
           {
               animator.SetFloat("moveX",input.x);
               animator.SetFloat("moveY",input.y);
               targetPos = transform.position;
               targetPos.x += input.x;
               targetPos.y += input.y;
               if(isWalkable(targetPos))
               {
                    StartCoroutine(Move(targetPos));
               }
           }
       }
       animator.SetBool("isMoving", isMoving);
   }

   IEnumerator Move(Vector3 targetPos)
   {
       isMoving = true;
       while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
       {
           transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
           yield return null;
       }
       transform.position = targetPos;
       isMoving = false;
       CheckForEncounters();
   }

   private bool isWalkable(Vector3 targetPos)
   {
       if(Physics2D.OverlapCircle(targetPos + new Vector3(0, 0.5f, 0), 0.2f, solidObjectsLayer) != null)
       {
           return false;
       }else
       {
            return true;
       }
   }

   private void CheckForEncounters()
   {
       string scene;
       if (Physics2D.OverlapCircle(transform.position, 0.2f, battleTrigger) != null)
       {
           if (Random.Range(1,101) <= PerChance)
           {
               Debug.Log("Encountered Battle.");
               playerPosData.PlayerPosSave();
               int randomNum;
               randomNum = Random.Range(1,101);
               scene = GameObject.Find("EncounterChance").GetComponent<SceneVars>().SceneName;
               if (scene == "SummerIsles")
                    SceneManager.LoadScene("BattleSceneBat");
               else if (scene == "TedShire")
                    if (randomNum <= 20)
                        SceneManager.LoadScene("BattleScene");
                    else if (randomNum > 20)
                        SceneManager.LoadScene("BattleSceneBoar");
           }
       }
       else if (Physics2D.OverlapCircle(transform.position, 0.2f, blockedTrigger) != null)
       {
            Debug.Log("Encountered Blocked Battle.");
            scene = GameObject.Find("EncounterFoSho").GetComponent<SceneVars>().SceneName;
            playerPosData.PlayerPosSave();
            SceneManager.LoadScene(scene);
       }
       else if (Physics2D.OverlapCircle(transform.position, 0.2f, TransitionTrigger) != null)
       {
            Debug.Log("Encountered Transition.");
            scene = GameObject.Find("Transition").GetComponent<SceneVars>().SceneName;
            //playerPosData.PlayerPosSave();
            if (scene == "SummerIsles")
                playerPosData.PlayerPosSet(4.5f, 0f);
            else if (scene == "TedShire")
                playerPosData.PlayerPosSet(-49.5f, -57f);
            SceneManager.LoadScene(scene);
       }
   }

   void OnDrawGizmosSelected() {
       Debug.Log("something");
       Gizmos.color = Color.red;
       Gizmos.DrawSphere(targetPos - new Vector3(0, 0.5f, 0), 0.2f);
   }

}
