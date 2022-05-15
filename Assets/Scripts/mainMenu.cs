using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public GameObject cube;
    public float x;
    public float y;
    public float z;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cube.transform.Rotate(x,y,z);
    }

    public void loadScene(){
        SceneManager.LoadScene(1);
    }
}
