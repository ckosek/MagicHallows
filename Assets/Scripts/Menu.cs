using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 2f;
    SavePlayerPos playerPosData;
    
    // Start is called before the first frame update
    public void Quit()
    {
       Application.Quit();
       Debug.Log("Quit!");
    }

    public void Play()
   {
       PlayerPrefs.DeleteAll();
       StartCoroutine(LoadLevel("TedShire"));
       playerPosData = FindObjectOfType<SavePlayerPos>();
       //SceneManager.LoadScene("TedShire");
   }

   public IEnumerator LoadLevel(string level)
   {
       // Play Animation
        transition.SetTrigger("Start");
       // Wait
        yield return new WaitForSeconds(transitionTime);

       // Load Scene
        SceneManager.LoadScene(level);
        playerPosData.PlayerPosSave();

   }
}
