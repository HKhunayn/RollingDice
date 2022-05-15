using UnityEngine;
using System.Collections;
public class mainMenu : MonoBehaviour
{
    public GameObject menubar;
    public GameObject clickable;
    public GameObject score;
    public GameObject gameTitle;
    public GameObject start_animation;
    public Rigidbody p;

    public void startGame(){
        StartCoroutine(startAnimation());
    }
    
    public void start(){
        
        if (PlayerPrefs.GetInt("isMainMenu") != 0)
            startGame();
    }

    IEnumerator startAnimation(){
        
        start_animation.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        menubar.SetActive(false);
        clickable.SetActive(false);
        gameTitle.SetActive(false);
        score.SetActive(true);
        player.isMainMenu = false;
        ScoreSystem.StartScore();
        p.position = new Vector3(0, p.position.y,p.position.z);
        yield return new WaitForSeconds(1f);
        start_animation.SetActive(false);
    }
   
}
