using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public StartManager startmanager;

    public Slider Sli_BGM, Sli_SE;
    private AudioSource SE, BGM;
    private GameObject audio_BGM;
    private float volume_SE, volume_BGM, SE_default, BGM_default;

    // Start is called before the first frame update
    void Start()
    {
        audio_BGM = GameObject.Find("BGM(Clone)");
        BGM = audio_BGM.GetComponent<AudioSource>();
        SE = startmanager.GetComponent<AudioSource>();

        BGM_default = BGM.volume;
        SE_default = SE.volume;

        Sli_BGM.value = PlayerPrefs.GetFloat("BGM", 1.0f);
        Sli_SE.value = PlayerPrefs.GetFloat("SE", 1.0f);

        volume_BGM = PlayerPrefs.GetFloat("BGM", 1.0f);
        BGM.volume *= volume_BGM;

        volume_SE = PlayerPrefs.GetFloat("SE", 1.0f);
        SE.volume *= volume_SE;
    }

     void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.Return))
        {
            ExitMenu();
        }   
    }

    public void ExitMenu()
    {
        startmanager.keyflag = true;
        gameObject.SetActive(false);
    }
    public void BGM_Setting()
    {
        float volume;

        volume = Sli_BGM.value;
        BGM.volume = BGM_default * volume;

        PlayerPrefs.SetFloat("BGM", volume);
    }

    //効果音の音量調整
    public void SE_Setting()
    {
        float volume;

        volume = Sli_SE.value;
        SE.volume = SE_default * volume;

        PlayerPrefs.SetFloat("SE", volume);
    }
}
