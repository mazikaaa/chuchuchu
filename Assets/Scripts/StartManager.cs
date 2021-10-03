using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    public Image[] frame = new Image[2];

    public Sprite[] framecolor = new Sprite[2];

    private int charindex,music;

    public bool keyflag=true;

    public GameObject AudioPrefab, setting_frame,menuPanel;
    //public Slider Sli_BGM, Sli_SE;
   // static private float SE_default, BGM_default;
    //private float BGM_volume, SE_volume;
    public AudioClip button_SE;

    AudioSource SE_audio, BGM_audio;

    private void Awake()
    {
       SE_audio = GetComponent<AudioSource>();


            //ゲーム開始時に音楽を流すオブジェクトを生成
            if (GameObject.Find("BGM(Clone)")==null)
            {
                GameObject Audio = Instantiate(AudioPrefab);
                BGM_audio = Audio.GetComponent<AudioSource>();
            }
            else
            {
                GameObject Audio = GameObject.Find("BGM(Clone)");
                BGM_audio = Audio.GetComponent<AudioSource>();
            }
        }
    // Start is called before the first frame update
    void Start()
    { 

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)&&charindex>0 && keyflag)
        {
            charindex--;
            ActiveFrame(charindex);

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && charindex<2 && keyflag)
        {
            charindex ++;
            ActiveFrame(charindex);
        }

        if (Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.Return))
        {
            switch (charindex)
            {
                case 0:
                    GoGameButton(20);
                    break;
                case 1:
                    GoGameButton(50);
                    break;
                case 2:
                    if(keyflag)
                    MenuButton();
                    break;

            }
        }
    }

    public void GoGameButton(int stock)
    {
        SE_audio.PlayOneShot(button_SE);
        PlayerPrefs.SetInt("STOCK", stock);

        switch (stock) {
            case 20:
                PlayerPrefs.SetInt("RANK", 0);
                break;
            case 50:
                PlayerPrefs.SetInt("RANK", 1);
                break;
            default:
                break;
        }

        //1.0秒後に実行する
        StartCoroutine(DelayMethod(0.8f, () =>
        {
            SceneManager.LoadScene("GameScene");
        }));
    }

    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    private void ActiveFrame(int index)
    {
        switch (index)
        {
            case 0:
                frame[0].sprite = framecolor[1];
                frame[1].sprite = framecolor[0];
                frame[2].sprite = framecolor[0];
                break;
            case 1:
                frame[0].sprite = framecolor[0];
                frame[1].sprite = framecolor[1];
                frame[2].sprite = framecolor[0];
                break;
            case 2:
                frame[0].sprite = framecolor[0];
                frame[1].sprite = framecolor[0];
                frame[2].sprite = framecolor[1];
                break;
            default:
                break;
        }
    }

    public void MenuButton()
    {
        keyflag = false;
        menuPanel.SetActive(true);
    }
}
