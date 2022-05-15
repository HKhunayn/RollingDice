using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class deathMenu : MonoBehaviour
{   
    public static GameObject menu;
    public static Text score;
    public static Text best_score;
    public static Text current_attempts;
    public GameObject start_animation;

    public static void openDeathMenu(GameObject menu,Text s,Text best,Text attps){
        
        ScoreSystem.addAttempt();
        s.text = "Score: " + ScoreSystem.currentScore;
        best.text = "Best score: " + ScoreSystem.BestScore;
        attps.text = "Current attempts: " + ScoreSystem.current_attempts;
        menu.SetActive(true);
    } 
    public void playAgain(){
        PlayerPrefs.SetInt("isMainMenu",1);
        StartCoroutine( loadScene());

    }
    public void backtoMainMenu(){
        PlayerPrefs.SetInt("isMainMenu",0);
        StartCoroutine( loadScene());
    }

    IEnumerator loadScene(){
        
        start_animation.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        ScoreSystem.resetScore();
        SceneManager.LoadSceneAsync("game");
        yield return new WaitForSeconds(1f);
        start_animation.SetActive(false);
    }
}
