using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stagesSystem : MonoBehaviour
{
    public GameObject[] stages; 
    private float[] lengths = {22f,10f,60f,30f,45f,30f};  //real lengths = 15 5 54 24 35 20
    public GameObject point;
    private float currentstageLocation=25f;
    // Start is called before the first frame update
    void Start()
    {
        for(int i =0; i < 120; i++){
            int r = Random.Range(0,stages.Length);
            
            Instantiate(stages[r], new Vector3(currentstageLocation, 0,0), Quaternion.identity);
            currentstageLocation += lengths[r];
            currentstageLocation += 10f;
            if (currentstageLocation > 3750){
                Instantiate(point, new Vector3(currentstageLocation + 10f, 10,0), Quaternion.identity);
                break;
            }
        }
        // for(int i =0; i < stages.Length; i++){
        //     Instantiate(stages[i], new Vector3(100*i+100,0,0), Quaternion.identity);

        // }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
