using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{

    private int charindex=0;

    public Sprite[] framecoler = new Sprite[2];

    public Image[] frame = new Image[2];

    public CharGenerator charGenerator;
    public GameManager gameManager;

    public Slider Sli_BGM, Sli_SE;
    private AudioSource SE, BGM;

    public AudioSource[] result_SE = new AudioSource[3];
    public AudioSource[] bonus_SE = new AudioSource[3];

    private GameObject audio_BGM;
    private float volume_SE, volume_BGM, SE_default, BGM_default;

    void Awake()
    {
        audio_BGM = GameObject.Find("BGM(Clone)");
        BGM = audio_BGM.GetComponent<AudioSource>();
        SE = gameManager.GetComponent<AudioSource>();

        BGM_default = BGM.volume;
        SE_default = SE.volume;

        Sli_BGM.value = PlayerPrefs.GetFloat("BGM", 1.0f);
        Sli_SE.value = PlayerPrefs.GetFloat("SE", 1.0f);

        volume_BGM = PlayerPrefs.GetFloat("BGM", 1.0f);
        volume_SE = PlayerPrefs.GetFloat("SE", 1.0f);

        BGM.volume *= volume_BGM;
        SE.volume *= volume_SE;
        for (int i = 0; i < 3; i++)
        {
            result_SE[i].volume = SE_default * volume_SE;
            bonus_SE[i].volume = SE_default * volume_SE;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            charindex = 0;
            ActiveFrame(charindex);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            charindex = 1;
            ActiveFrame(charindex);
        }
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            ProcessButton(charindex);
        }
    }

    public void ProcessButton(int index)
    {
        switch (index)
        {
            case 0:
                ExitMenu();
                break;
            case 1:
                gameManager.RetryButton();
                break;
            default:
                break;
        }

    }

    public void ExitMenu()
    {
        charGenerator.keyflag = true;
        gameObject.SetActive(false);
    }



    private void ActiveFrame(int index)
    {
        switch (index)
        {
            case 0:
                frame[0].sprite = framecoler[1];
                frame[1].sprite = framecoler[0];
                break;
            case 1:
                frame[0].sprite = framecoler[0];
                frame[1].sprite = framecoler[1];
                break;
            default:
                break;
        }
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

        for (int i = 0; i < 3; i++)
        {
            result_SE[i].volume = SE_default * volume;
            bonus_SE[i].volume = SE_default * volume;
        }

        PlayerPrefs.SetFloat("SE", volume);
    }



}
