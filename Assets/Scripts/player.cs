using UnityEngine;
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
    public float whenjump = 0;
    public float whenhit = 0;
    public bool isDoublejump = false;
    private float []perfectRot = {1,0,0.7071074f,-0.7071074f,-1}; 
    // Start is called before the first frame update
    public Transform[] diceFaces = new Transform[6];
    private bool Isflipping = false;
     public static bool isMainMenu;
    public Text speedText;
    private Vector3 LastjumpLocation;
    private bool moreMoiwning=false;
    private float remaining = 0;
    public Rigidbody jumpEffect ;
    public Rigidbody walkEffect ;
    public Rigidbody deathEffect ;
    private bool isDead = false; 
    public GameObject death_menu;

    public GameObject[] jumps_sounds;
    public GameObject[] death_sounds;
    public GameObject[] hit_sounds;
    
    private bool stopMoving = false;
    void Start()
    { 
        // Debug.LogWarning("fixed:"+Time.fixedDeltaTime+" , delta:"+ Time.deltaTime);
        if (PlayerPrefs.GetInt("inMainMenu") == 0)
            isMainMenu = true;
        else
            isMainMenu= false;
        camhight = cam.position.y;
        xPositonDiff = Mathf.Abs(p.position.x - cam.position.x);
        //speed = startedSpeed;
        
    }

    private void Update() {
        // if (Input.acceleration.sqrMagnitude > 5f || Input.GetKeyDown(KeyCode.Space)){ // shake it
        //     //p.position = p.position + new Vector3(Random.Range(-10f,-4f),Random.Range(-10f,-4f),Random.Range(-10f,-4f));
        //     //Debug.Log("before random rotating "+p.transform.localRotation);
        //     //Quaternion q = p.transform.rotation;
        //     int x = ((int)Random.Range(0f,4f))*90;
        //     int y = ((int)Random.Range(0f,4f))*90;
        //     int z = ((int)Random.Range(0f,4f))*90;
        //     p.transform.Rotate(new Vector3(90,0,0));
        //     //p.transform.rotation = q;
        //     //Debug.Log("After random rotating "+p.transform.localRotation);
        //     Handheld.Vibrate();
            
        // }

        
        float t = Time.deltaTime * (1/Time.fixedDeltaTime);
        // Debug.Log("detaTime: "+Time.deltaTime + "   ,fixe: "+ Time.fixedDeltaTime + "   ,t:"+ t);
        if (Isflipping && !isDead)
            p.position += new Vector3((startedSpeed+speed)/2*t,0,0);
        else if (!stopMoving)
            p.position += new Vector3(speed*t,0,0);
        else
            p.position = LastjumpLocation;
        //p.AddForce(speed,0,0,ForceMode.Impulse);
        if (!isMainMenu && !isDead)
            speed += speedFactor*t;
        if (!stopMoving)
            cam.position = new Vector3((p.position.x - xPositonDiff),cam.position.y,cam.position.z);

        if(Input.GetKey(KeyCode.Mouse0) && !clicked && !Isflipping && !isMainMenu && !isDead){
            clicked=true;
            whenjump = Time.time;
            isDoublejump = true;
            Debug.LogWarning("jump time: "+Time.time);
            StartCoroutine(doubleJumpDelay());
            LastjumpLocation = p.position;
            p.AddForce(0,jump,0,ForceMode.Impulse);
            remaining += 90;
            StartCoroutine(flip());
            //print("isFlipping: "+ Isflipping);

            // Debug.Log("you can double jump now lol");

        }else if(Input.GetKeyDown(KeyCode.Mouse0) && !isMainMenu && !isDoublejump && clicked && !isDead && whenjump> whenhit){ // double jump

             if (Physics.Raycast(p.position + new Vector3(0,-3f,0),Vector3.down,10f,3) && p.velocity.y < -30f){
                Debug.DrawRay(p.position + new Vector3(0,-1.5f,0),Vector3.down*1f,Color.red,200f);
                Debug.LogError("near to the ground");
                return;
             }
              Debug.LogWarning("djump time: "+Time.time);
            // Debug.DrawRay(p.position + new Vector3(0,-1.5f,0),Vector3.down*1,Color.green,200f);
            // Debug.Log("pos:"+ p.position + " ,rot:"+ p.rotation+" ,vel:" + p.velocity);
            isDoublejump = true;
            p.velocity = Vector3.zero;
            p.AddForce(jump/2,2*jump,0,ForceMode.Impulse);
            if (p.velocity.y < 10f){
                
                p.velocity = new Vector3(p.velocity.x,10f,0);
                
            }
                
            
            jumpEffect.position = p.position + new Vector3(3f,-1.5f,0);
            jumpEffect.AddForce(-p.velocity.x,-p.velocity.y,0,ForceMode.Impulse);
            remaining += 90f;
            StartCoroutine(flip());
            
                
            float f = 0;
            f += Time.deltaTime;
            jumpEffect.velocity =new Vector3(0,0,0);
            moreMoiwning=true;
        }
            
    }

    private void FixedUpdate() {

        speedText.text = "Speed: " + (Mathf.Floor(1000*speed)/100) + " m/s";

        if (p.velocity.y<10 ||  moreMoiwning){
            p.AddForce(-speed,-(speedFactor+(jump/4)),0,ForceMode.Impulse);
        }

        if (!clicked && !isDead){ // walkEffect
            Instantiate(walkEffect, p.position + new Vector3(-2f,-1.5f,0), Quaternion.identity);
        } 
        if (p.position.y<-10 ) // to avoid keep falling
            Death(p.position);
        // }else if (p.position.y - LastjumpLocation.y> 10.35f){ // to fix high jump
        //     Debug.LogWarning("to high p.y=" + p.position.y + ", dif="+(p.position.y - LastjumpLocation.y) + " ,p.vel:"+p.velocity);
        //     if (p.velocity.y > 0)
        //         p.velocity= new Vector3(p.velocity.x,0,0);
        //     p.AddForce(0,-jump,0,ForceMode.Impulse);

        // }
        // if (p.velocity.y> 50f){ // to detect high jump
        //     Debug.LogWarning("High jump detected! -> " + p.velocity.y);
        //     p.velocity = new Vector3(p.velocity.x,50,p.velocity.z);
        // }
            
    }
    IEnumerator doubleJumpDelay(){
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        isDoublejump = false;
    }
    IEnumerator flip(){
        Isflipping = true;
        int r = Random.Range(0,jumps_sounds.Length-1);
        GameObject s = Instantiate(jumps_sounds[r], p.position,Quaternion.identity);
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
            if (_speed > 5f)
                afterMid = true;
            if (afterMid && _speed > 1+ flipSpeed/10f)
                _speed -= flipSpeed/10;
            else if (!afterMid)
                _speed += flipSpeed/10f;
            remaining -= amount;
            //print("time:"+ Time.deltaTime + " , total:"+total + " , speed:"+ _speed);
        }
        Destroy(s);
        if (remaining < 0){ // to fix some rotation problems happend when double jump
            p.transform.Rotate(new Vector3(-p.transform.rotation.x,-p.transform.rotation.y,-remaining));
            remaining -= remaining;

        }
        
        // p.rotation. = Quaternion.EulerAngles(p.rotation.eulerAngles.x,p.rotation.eulerAngles.y,0f);
        Isflipping=false;
        
    }

    public Text score;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("+1") && !isMainMenu){
            //ScoreSystem.addScore(score,1);
            ScoreSystem.addScore(score,1);
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
        if(clicked && other.gameObject.CompareTag("road")){
            LastjumpLocation = p.position;
            whenhit = Time.time;
            // Debug.Log("hit! :" + other.transform.position);

            StartCoroutine(justHited());
            Debug.LogWarning("hit time: "+Time.time);
        }
        if (other.gameObject.CompareTag("Damgable")){// death
            if (isMainMenu){
                Debug.LogWarning("hit in mainmenu mode!");
                //Time.timeScale =0f;
                p.position = new Vector3(p.position.x+1f,p.position.y+1f,0);
            }
            
            Death(other.transform.position);
            


        }
        else{
            int r = Random.Range(0,hit_sounds.Length-1);
            GameObject s = Instantiate(hit_sounds[r], p.position,Quaternion.identity);
        }

    }
    IEnumerator justHited(){

            yield return new WaitForSeconds(0.001f);
            clicked = false;
            moreMoiwning =false;


    }

    private void Death(Vector3 v){
        if (isMainMenu)
            p.position = new Vector3(0,0,0);
        if (isDead)
            return;
        isDead = true;
        Settings.doHaptic();
        StartCoroutine(_Death());

    }
    IEnumerator _Death(){


        Instantiate(deathEffect, p.position + new Vector3(-3f,3f,-0f), Quaternion.identity);
        int r = Random.Range(0,death_sounds.Length-1);
        GameObject d = Instantiate(death_sounds[r], p.position,Quaternion.identity);
        // yield return new WaitForSeconds(0.05f);
        Time.timeScale =0.5f;
        p.GetComponent<Renderer>().enabled = false;
        p.GetComponent<Collider>().enabled = false;
        for(int i=0;i<diceFaces.Length;i++)
            diceFaces[i].gameObject.SetActive(false);
        for(int i =0;i<5;i++){
            yield return new WaitForSecondsRealtime(0.2f);
            speed *= 0.95f;
            LastjumpLocation = p.position;
        }
        Destroy(d);
        Time.timeScale =1f;
        speed /= 2;
        Text s = death_menu.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>();
        Text bs = death_menu.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>();
        Text ca = death_menu.transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<Text>();
        deathMenu.openDeathMenu(death_menu,s,bs,ca);
        while(speed > 0.01f){
            yield return new WaitForSecondsRealtime(0.2f);
            speed *= 0.95f;
            LastjumpLocation = p.position;
        }
        speed = 0f;
        cam.velocity = Vector3.zero;
        stopMoving =true;
    }
    
}
