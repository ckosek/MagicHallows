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
   public bool isMoving;
   private Vector2 input;
   private Animator animator;
   SavePlayerPos playerPosData;
   public float PerChance;


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
               var targetPos = transform.position;
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
       if(Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) != null)
       {
           return false;
       }else
       {
            return true;
       }
   }

   private void CheckForEncounters()
   {
       if (Physics2D.OverlapCircle(transform.position, 0.2f, battleTrigger) != null)
       {
           if (Random.Range(1,101) <= PerChance)
           {
               Debug.Log("Encountered Battle.");
               playerPosData.PlayerPosSave();
               SceneManager.LoadScene("BattleScene");
           }
       }
       if (Physics2D.OverlapCircle(transform.position, 0.2f, blockedTrigger) != null)
       {
            Debug.Log("Encountered Battle.");
            playerPosData.PlayerPosSave();
            SceneManager.LoadScene("BattleScene");
       }
   }

}
