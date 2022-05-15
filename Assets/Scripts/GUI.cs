using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GUI : MonoBehaviour
{   
    public Text score;
    public int currentScore=0;
    public Text fps;
    // Start is called before the first frame update
    void Start()
    {
        
        fps.text = "FPS: " + Mathf.Floor(1/Time.deltaTime);
        //fps.transform.position = new Vector3(Screen.width-Screen.safeArea.width+Screen.width/2+12,(Screen.height-Screen.safeArea.height)/2 + Screen.height-24,0);
        
        
    }
    public void addScore(int value){
        currentScore += value;
        updateScore();
    }

    private void updateScore(){
        score.text = "Score: "+currentScore;
    }
    // Update is called once per frame
    private float frameTime = 0;
    void Update()
    {
        frameTime += Time.deltaTime;
        
        
        if (frameTime >= 1){
            frameTime = 0;
            fps.text = "FPS: " + Mathf.Floor(1/Time.deltaTime);
            
        }
        
    }
}
