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
    void Start(){
                print(" start chaning...");
        int q = PlayerPrefs.GetInt("Quality");
        QualitySettings.SetQualityLevel(q);
        changeQualityto(q);
        ///
        int h = PlayerPrefs.GetInt("Haptic");
        changeHapticto(h);
        //
        int f = PlayerPrefs.GetInt("FPS");
        changeFPSto(f);
        //
        int s = PlayerPrefs.GetInt("Sound");
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
        quality.text = qq[q];
    }

    public void changeHaptic(){
        int h = PlayerPrefs.GetInt("Haptic",1);
        if (h == 0)
            h=1;
        else 
            h=0;
        changeHapticto(h);

        
    }
    private void changeHapticto(int h){
        PlayerPrefs.SetInt("Haptic",h);
        if (h == 0) haptic.text="Off";
        else  haptic.text="On";
    }

    public void changeFPS(){
        int f = PlayerPrefs.GetInt("FPS",1)+1;
        if (f > 2)
            f=0;
        changeFPSto(f);
    }
    private void changeFPSto(int f){
        PlayerPrefs.SetInt("FPS",f);
        int []ff = {20,30,60};
        fps.text = ff[f]+"";
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
}
