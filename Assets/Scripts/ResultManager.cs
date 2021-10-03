using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManger : MonoBehaviour
{
    public GameManager gameManager;

    int charindex=1;

    public Sprite[] framecoler = new Sprite[2];

    public Image[] frame = new Image[3];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)||charindex>0)
        {
            charindex --;
            ActiveFrame(charindex);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)||charindex<2)
        {
            charindex++;
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
                gameManager.RankingButton();
                break;
            case 2:
                gameManager.RetryButton();
                break;
            default:
                break;
        }

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

}
