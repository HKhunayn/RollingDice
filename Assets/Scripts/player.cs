using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    public Rigidbody p;
    public float startedSpeed=0.3f;
    public float speed;
    public float speedFactor=0.00015f;
    public float jump=500;

    public Rigidbody cam;
    private bool clicked = false;
    private float camhight;
    private float xPositonDiff;
    public float flipSpeed = 2f;
    public GameObject scripts;
    public bool isDoublejump = false;
    private float []perfectRot = {1,0,0.7071074f,-0.7071074f,-1}; 
    // Start is called before the first frame update
    public Transform[] diceFaces = new Transform[6];
    private bool Isflipping = false;
    public bool isMainMenu;
    public Text speedText;
    private Vector3 LastjumpLocation;
    private bool moreMoiwning=false;
    private float remaining = 0;
    public Rigidbody jumpEffect ;
    public Rigidbody walkEffect ;
    public Rigidbody deathEffect ;
    private bool isDead = false; 
    
    void Start()
    { 
        camhight = cam.position.y;
        xPositonDiff = Mathf.Abs(p.position.x - cam.position.x);
        //speed = startedSpeed;
        
    }

    private void Update() {
        if (Input.acceleration.sqrMagnitude > 5f || Input.GetKeyDown(KeyCode.Space)){ // shake it
            //p.position = p.position + new Vector3(Random.Range(-10f,-4f),Random.Range(-10f,-4f),Random.Range(-10f,-4f));
            //Debug.Log("before random rotating "+p.transform.localRotation);
            //Quaternion q = p.transform.rotation;
            int x = ((int)Random.Range(0f,4f))*90;
            int y = ((int)Random.Range(0f,4f))*90;
            int z = ((int)Random.Range(0f,4f))*90;
            p.transform.Rotate(new Vector3(90,0,0));
            //p.transform.rotation = q;
            //Debug.Log("After random rotating "+p.transform.localRotation);
            Handheld.Vibrate();
            
        }
        if(Input.GetKey(KeyCode.Mouse0) && !clicked && !Isflipping && !isMainMenu){
            clicked=true;
            LastjumpLocation = p.position;
            p.AddForce(0,jump,0,ForceMode.Impulse);
            remaining += 90;
            StartCoroutine(flip());
            //print("isFlipping: "+ Isflipping);
        }else if(Input.GetKeyDown(KeyCode.Mouse0)  && !isMainMenu && !isDoublejump && LastjumpLocation.x  + 3f< p.position.x  && p.velocity.y > 10f  ){ // double jump
            isDoublejump = true;
            p.AddForce(jump/2,2*jump,0,ForceMode.Impulse);
            jumpEffect.position = p.position + new Vector3(3f,-1.5f,0);
            jumpEffect.AddForce(-p.velocity.x,-p.velocity.y,0,ForceMode.Impulse);
            remaining += 90;
            StartCoroutine(flip());
            
                
            float f = 0;
            while(f < 1f)
                f += Time.deltaTime;
            jumpEffect.velocity =new Vector3(0,0,0);
            moreMoiwning=true;
            print ("Double jump !");
        }
            
    }
    private void FixedUpdate() {
        
        if (Isflipping && !isDead)
            p.position += new Vector3((startedSpeed+speed)/2,0,0);
        else
            p.position += new Vector3(speed,0,0);
        //p.AddForce(speed,0,0,ForceMode.Impulse);
        if (!isMainMenu)
            speed += speedFactor;
        speedText.text = "Speed: " + (Mathf.Floor(1000*speed)/100) + " m/s";

        if (p.velocity.y<10 && !isDoublejump || moreMoiwning){
            p.AddForce(-speed,-(speedFactor+(jump/4)),0,ForceMode.Impulse);
        }
        cam.position = new Vector3(p.position.x - xPositonDiff,camhight,cam.position.z);
        if (!clicked && !isDead){ // walkEffect
            Instantiate(walkEffect, p.position + new Vector3(-2f,-1.5f,0), Quaternion.identity);
        } 
    }
    
    IEnumerator flip(){
        Isflipping = true;
        yield return new WaitForSeconds(0.1f);
        //Vector3 speed = new Vector3(0,0,-5);
        
        int total = 0;
        float _speed = 250*Time.deltaTime;
        bool afterMid = false;
        Transform v = p.transform;
        for(int i =0;i<diceFaces.Length;i++){
            if (diceFaces[i].position.x > v.position.x)
                v = diceFaces[i];
        }
        //Debug.Log(v.position);
        while(remaining > 0){
            float amount = Mathf.Min(_speed,remaining);
            p.transform.Rotate(new Vector3(-p.transform.rotation.x,-p.transform.rotation.y,-amount));
            //p.transform.rotation *= Quaternion.identity;
            //p.AddTorque(0,0,-1f);
            yield return new WaitForSeconds(0.01f);
            total++;
            if (_speed > 5)
                afterMid = true;
            if (afterMid && _speed > 1+ flipSpeed/10f)
                _speed -= flipSpeed/10;
            else if (!afterMid)
                _speed += flipSpeed/10f;
            remaining -= amount;
            //print("time:"+ Time.deltaTime + " , total:"+total + " , speed:"+ _speed);
        }
        //print("time:"+ Time.deltaTime + " , total:"+total + " , speed:"+ _speed);
        Isflipping=false;
        
    }
    private IEnumerator waitt(){
        yield return new WaitForSeconds(0.1f);
        
    }
    public Text score;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("+1") && !isMainMenu){
            //ScoreSystem.addScore(score,1);
            print("Score +1 ?");
            ScoreSystem.addScore(score,1);
        }
        else if (other.gameObject.CompareTag("Damgable")){
            ScoreSystem.resetScore();
            SceneManager.LoadSceneAsync("game");            
        }
        else if (other.gameObject.CompareTag("jump") && isMainMenu){
            clicked = true;
            p.AddForce(0,jump,0,ForceMode.Impulse);
            remaining += 90;
            StartCoroutine(flip());        
        }else if (other.gameObject.CompareTag("the_end")){
            p.position = new Vector3(0,p.position.y,0);
        }
    }
    private void OnCollisionEnter(Collision other) {
        if(clicked){
            //Debug.Log("hit!");
            //StartCoroutine(waitt());
            clicked = false;
            isDoublejump = false;
            moreMoiwning =false;
        }
        if (other.gameObject.CompareTag("Damgable")){// death
            if (isMainMenu){
                Debug.LogWarning("hit in mainmenu mode!");
                Time.timeScale =0f;
            }
            
            Death(other.transform.position);
            


        }

    }

    private void Death(Vector3 v){
        
        if (isDead)
            return;
        isDead = true;
        
        StartCoroutine(_Death());

    }
    IEnumerator _Death(){
        
        
        Instantiate(deathEffect, p.position + new Vector3(-3f,3f,-0f), Quaternion.identity);
        // yield return new WaitForSeconds(0.05f);
        Time.timeScale =0.5f;
        p.GetComponent<Renderer>().enabled = false;
        p.GetComponent<Collider>().enabled = false;
        for(int i=0;i<diceFaces.Length;i++)
            diceFaces[i].gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Time.timeScale =1f;
        
        ScoreSystem.resetScore();
        SceneManager.LoadSceneAsync("game");
    }
    
}
