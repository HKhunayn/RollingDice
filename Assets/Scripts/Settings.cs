using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Settings : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject settingsMenu;
    public Text quality;
    public Text haptic;
    public Text fps;
    public Text sound;
    int []ff = {20,30,60,120,144,160,200,240};
    void Start(){
        int q = PlayerPrefs.GetInt("Quality",1);
        QualitySettings.SetQualityLevel(q);
        changeQualityto(q);
        ///
        int h = PlayerPrefs.GetInt("Haptic",1);
        changeHapticto(h);
        //
        int f = PlayerPrefs.GetInt("FPS",1);
        changeFPSto(f);
        //
        int s = PlayerPrefs.GetInt("Sound",1);
        changeSoundto(s);
        print(" All changed!");
    }
    IEnumerator s(){
        

        yield return new WaitForSeconds(1);
    }
    public void toggleSettingsMenu(){
        if (settingsMenu.active)
            settingsMenu.SetActive(false);
        else
            settingsMenu.SetActive(true);
    }
    public void changeQuality(){
        int q = PlayerPrefs.GetInt("Quality",1)+1;
        if (q > 2)
            q=0;
        changeQualityto(q);

    }
    private void changeQualityto(int q){
        PlayerPrefs.SetInt("Quality",q);
        QualitySettings.SetQualityLevel(q);
        string []qq = {"Low","Mid","High"};
        float []qr = {0.4f,0.6f,0.8f};
        quality.text = qq[q];
        Screen.SetResolution((int)(qr[q]*Screen.resolutions[0].width),(int)(qr[q]*Screen.resolutions[0].height),Screen.fullScreen);
        
    }

    public void changeHaptic(){
        int h = PlayerPrefs.GetInt("Haptic",1);
        if (h == 0){
            h=1;
            Settings.doHaptic();
        }
            
        else 
            h=0;
        changeHapticto(h);

        
    }
    private void changeHapticto(int h){
        PlayerPrefs.SetInt("Haptic",h);
        if (h == 0) haptic.text="Off";
        else 
            haptic.text="On";
    }

    public void changeFPS(){
        int f = PlayerPrefs.GetInt("FPS",1)+1;
        
        int maxfps=-1;
        for(int i =0; ff[i] <=Screen.currentResolution.refreshRate ; i++){
            maxfps++;
            
        }
        if (f > maxfps)
            f=0;
        changeFPSto(f);
    }
    private void changeFPSto(int f){
        
        PlayerPrefs.SetInt("FPS",f);
        
        fps.text = ff[f]+"/"+Screen.currentResolution.refreshRate;
        Application.targetFrameRate=ff[f];
        
    }
    public void changeSound(){
        int s = PlayerPrefs.GetInt("Sound",1);
        if (s == 0)
            s=1;
        else 
            s=0;
        changeSoundto(s);

    }

    private void changeSoundto(int s){
        PlayerPrefs.SetInt("Sound",s);
        AudioListener.volume = s;
        if (s == 0) sound.text="Off";
        else  sound.text="On";
    }

    public static void doHaptic(){
        if (PlayerPrefs.GetInt("Haptic") == 1){
            Handheld.Vibrate();
        }
            
    }
}
