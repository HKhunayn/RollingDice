using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public static int currentScore=0;

    public static Text score;
    public static void resetScore(){
        currentScore =0;
        updateScore();
    }
    public static void addScore(Text t,int value){
        score = t;
        currentScore += value;
        updateScore(t);
    }
    private static void updateScore(Text t){
        t.text = "Score: "+currentScore;
    }
    private static void updateScore(){
        score.text = "Score: "+currentScore;
    }
    void Start()
    {
        score = GameObject.FindWithTag("score").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
