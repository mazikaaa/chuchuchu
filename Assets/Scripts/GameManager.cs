using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{ 

    public int inside = 0;
    public int outside = 0;

    private int rank;

    public bool bonusflag=false;

    public Text score_text,base_text ,bonus_text, bonuspoint_text, sum_text,result_text;

    public GameObject pointpanel,resultpanel,bonuspanel;

    public Animator anim_point,anim_bonus;

    public AudioClip drum;

    private AudioSource audioSource;

    CharGenerator charGenerator;

    public AudioSource[] result_SE = new AudioSource[3];
    public AudioSource[] bonus_SE = new AudioSource[3];
    private float volume_SE, volume_BGM;

    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
        }
    }
    private int score;

    public int Point {
        get
        {
            return point;
        }
        set
        {
            point = value;
        }
    }
    private int point;



    // Start is called before the first frame update
    void Start()
    {
        InitTouch();
        charGenerator = GetComponent<CharGenerator>();
        audioSource = GetComponent<AudioSource>();
        rank = PlayerPrefs.GetInt("Rank", 0);

        volume_BGM = PlayerPrefs.GetFloat("BGM", 1.0f);
        volume_SE = PlayerPrefs.GetFloat("SE", 1.0f);

        for (int i = 0; i < 3; i++)
        {
            result_SE[i].volume *=  volume_SE;
            bonus_SE[i].volume *= volume_SE;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //初期化(次の文字の配置に移る際の処理)
    private void InitTouch()
    {
        inside = 0;
        outside = 0;
    }

    private void GetPoint(int a)
    {
        Score += a;
        score_text.text = Score.ToString();
        Point = 0;
    }

    private void ScoreAnimation(int a)
    {
        float value = 0.0f;
        float b = 0;

        
        while (value <= 1.0f)
        {
            value += 0.0001f;
            b = Mathf.Lerp(Score, Score + a, value);
            score_text.text = b.ToString();
        }
        
        Score += a;
        score_text.text = Score.ToString();
        Point = 0;

    }

    //何点入るかの確認
    public void PointCheck(GameObject obj,float scale)
    {
        int basepoint = 0;
        float sizebonus=0;
        int chwbonus = 1;

        switch (obj.name)
        {
            case "big(Clone)":
                basepoint = 2000;
                break;
            case "normal(Clone)":
                basepoint = 500;
                break;
            case "small(Clone)":
                basepoint = 100;
                break;
        }

        if (outside != 0)
        {
            chwbonus=0;
            Destroy(obj);
        }
        else if (inside == 2)
        {
            bonusflag = true;
            chwbonus = 100;
        }
        else if (inside == 1)
        {
            chwbonus=10;
        }
        else
        {
            chwbonus=1;
        }

        sizebonus = 1.2f * scale;

        Point = (int)(basepoint *sizebonus)*chwbonus;

        ScoreAnimation(Point);

        PointDisplay(basepoint, sizebonus, chwbonus, Point);

        InitTouch();
    }

    private void PointDisplay(int basepoint,float size,int chw,int point)
    {
        string bonustext = "", bonuspoint = "";

        base_text.text = basepoint.ToString();

        switch (chw)
        {
            case 0:
                bonustext +="接触ペナルティ\n";
                bonuspoint += "×" + chw.ToString() + "\n";
                break;
            case 10:
                bonustext += "中ノ中\n";
                bonuspoint += "×" + chw.ToString() + "\n";
                break;
            case 100:
                bonustext += "中ノ中ノ中\n";
                bonuspoint += "×" + chw.ToString() + "\n";
                break;
            default:
                break;
        }

        bonustext += "サイズボーナス";
        bonuspoint += "×"+size.ToString("F2")+"\n";

        bonus_text.text = bonustext;
        bonuspoint_text.text = bonuspoint;

        sum_text.text = point.ToString();

        if (bonusflag)
        {
            bonuspanel.SetActive(true);
            anim_bonus.SetTrigger("Chw");
        }
        else
        {
            PointAnimation();
        }

    }

    public void RetryButton()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void RankingButton()
    {
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(Score, rank);
    }

    public void TweetButton()
    {
        resultpanel.SetActive(false);

        StartCoroutine(TweetWithScreenShot.TweetManager.TweetWithScreenShot("あなたのスコアは" + Score.ToString() + "でした！"));

        StartCoroutine(DelayMethod(1.5f, () =>
        {
            resultpanel.SetActive(true); StartCoroutine(TweetWithScreenShot.TweetManager.TweetWithScreenShot("あなたのスコアは" + Score.ToString() + "でした！"));

        }));
    }

    public void ResultOpen()
    {
        audioSource.PlayOneShot(drum);
        resultpanel.SetActive(true);
        result_text.text = Score.ToString();
    }

    public void PointAnimation()
    {
        if (charGenerator.stock == 0)
        {
            charGenerator.keyflag = false;
            ResultOpen();
            return;
        }
        else
        {
            charGenerator.keyflag = false;
            pointpanel.SetActive(true);
            anim_point.SetTrigger("Point");
        }
        bonusflag = false;
    }

    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();

    }
}
