using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public static int currentScore=0;
    public static int BestScore;
    public static int current_attempts;
    private static bool isNewScore = false;

    public static Text score;
    public void start(){
        BestScore = PlayerPrefs.GetInt("Best Score");
        current_attempts = PlayerPrefs.GetInt("Current Attempts");
    }
    public static void resetScore(){
        currentScore =0;
        isNewScore = false;
        updateScore();
    }
    public static void addAttempt(){
        if (!isNewScore){
            current_attempts++;
            PlayerPrefs.SetInt("Current Attempts",current_attempts);
        }
        else{
            PlayerPrefs.SetInt("Current Attempts",0);
            current_attempts=0;
        }
    }
    public static void addScore(Text t,int value){
        score = t;
        currentScore += value;
        if (BestScore < currentScore){
            isNewScore = true;
            PlayerPrefs.SetInt("Best Score",currentScore);
            PlayerPrefs.SetInt("Current Attempts",0);
            BestScore = currentScore;
        }
        updateScore(t);
    }
    private static void updateScore(Text t){
        t.text = "Score: "+currentScore;
    }
    private static void updateScore(){
        score.text = "Score: "+currentScore;
    }
    public static void StartScore()
    {
        score = GameObject.FindWithTag("score").GetComponent<Text>();
    }

}
