using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CharGenerator : MonoBehaviour
{
   
   public int click = 0, stock=20;

    private bool selection = true;
    private bool movement = false;
    private bool back = false;
    private bool clock = false;

    public bool keyflag=true;//キーボードの入力を受けつけるかどうか

    [SerializeField] GameObject[] chuPrefabs = new GameObject[3];
    [SerializeField]GameObject canvas;
    public Sprite[] framecoler = new Sprite[2];

    public Image[] frame = new Image[4];

    public Text stock_text;

    private AudioSource audioSource;

    public AudioClip[] drum = new AudioClip[3];

    private GameObject currentchu,charframe;

    public GameObject selectionPanel, menuPanel;

    public GameManager gameManager;

    private RectTransform rect;

    private int charindex = 1;

    private float time = 0.0f;

    private float scale, rotation,posx=0.0f,posy=0.0f,speed;

    private float char_x, char_y, char_size;




    // Start is called before the first frame update
    void Start()
    {
        stock = PlayerPrefs.GetInt("STOCK", 20);
        stock_text.text = stock.ToString();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (selection && stock > 0)
        {
            SelectionChar();
            if (Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.Return)&&keyflag)
            {
                if (charindex != 3)
                {
                    GenerateChar();
                }
                else
                {
                    if(keyflag)
                    MenuButton();
                }
            }
        }
        else if (movement)
        {
            selectionPanel.SetActive(false);
            MovementChar();
        }

    }

    //どの文字を選ぶかの処理
    private void SelectionChar()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow) && charindex<3 && keyflag)
        {
            charindex++;
            ActiveFrame(charindex);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && charindex>0 && keyflag)
        {
            charindex--;
            ActiveFrame(charindex);
        }
        
    }

    private void ActiveFrame(int index)
    {
        switch (index)
        {
            case 0:
                frame[0].sprite = framecoler[1];
                frame[1].sprite = framecoler[0];
                frame[2].sprite = framecoler[0];
                frame[3].sprite = framecoler[0];
                break;
            case 1:
                frame[0].sprite = framecoler[0];
                frame[1].sprite = framecoler[1];
                frame[2].sprite = framecoler[0];
                frame[3].sprite = framecoler[0];
                break;
            case 2:
                frame[0].sprite = framecoler[0];
                frame[1].sprite = framecoler[0];
                frame[2].sprite = framecoler[1];
                frame[3].sprite = framecoler[0];
                break;
            case 3:
                frame[0].sprite = framecoler[0];
                frame[1].sprite = framecoler[0];
                frame[2].sprite = framecoler[0];
                frame[3].sprite = framecoler[1];
                break;
            default:
                break;
        }

    }

    private void GenerateChar()
    {
        currentchu = Instantiate(chuPrefabs[charindex]);

        if (currentchu != null)
        {
            currentchu.transform.SetParent(canvas.transform, false);
            currentchu.transform.SetAsFirstSibling();
            currentchu.transform.GetChild(0).GetComponent<frameColider>().gamemanager = gameManager;
            currentchu.GetComponent<Text>().color = new Vector4(200, 0, 0, 255);
            charframe = currentchu.transform.GetChild(0).gameObject;
            charframe.SetActive(false);

            rect = currentchu.GetComponent<RectTransform>();
            char_x = currentchu.GetComponent<CharData>().move_x;
            char_y = currentchu.GetComponent<CharData>().move_y;
            char_size = currentchu.GetComponent<CharData>().size;
            speed = currentchu.GetComponent<CharData>().speed;


            selection = false;
            movement = true;
            time = 0.5f;
            click= 0;
        }
    }
    

    private void MovementChar()
    {
        if ((Input.GetKeyDown(KeyCode.Space)||Input.GetMouseButtonDown(0)|| Input.GetKeyDown(KeyCode.Return))&&keyflag)
        {
            time = 0.5f*speed;
            click++;
            back = false;
            audioSource.PlayOneShot(drum[0]);
            if (click == 3)
                time = 0.0f;
        }

        if (!back)
            time += Time.deltaTime;
        else if (back)
            time -= Time.deltaTime;

        if (time/speed >= 1.0f)
        {
            if (clock)
            {
                time = 0.0f;
            }
            else
            {
                back = true;
                time = 1.0f*speed;
            }

        }
        else if (time <= 0.0f)
        {
            back = false;
            time = 0.0f;
        }

        switch (click)
        {
            case 0:
                posx = Mathf.Lerp(-char_x, char_x, time/speed);
                break;
            case 1:
                //rect.DOLocalMove((new Vector3(0, -100f, 0)), 1.0f).SetLoops(-1, LoopType.Yoyo);
                posy = Mathf.Lerp(-char_y, char_y, time/speed);
                //rect.localPosition = Vector3.Lerp(new Vector3(0,-100, 0), new Vector3(0, 100, 0),time);
                break;
            case 2:
                scale = Mathf.Lerp(0.8f, 1.0f, time);
                rect.localScale = new Vector3(char_size*scale, char_size * scale, char_size * scale);
                break;
            case 3:
                // rect.DOLocalRotate(new Vector3(0, 0, 360f), 3f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
                rotation = Mathf.Lerp(0, 360.0f, time/speed);
                rect.localRotation = Quaternion.Euler(0, 0, rotation);
                clock = true;
                break;
            case 4:
                StartCoroutine("CompleteSet");
                click++;
                break;
            case 5:
                StartCoroutine("FinishOneTurn");
                click = -1;
                break;
            default:
                break;

        }
        if (rect != null)
        {
            rect.localPosition = new Vector3(posx, posy, 0);
        }

    }

    IEnumerator FinishOneTurn()
    {
        stock--;
        stock_text.text = stock.ToString();

        clock = false;
        movement = false;
        if (stock > 0)
        {
            selection = true;
            selectionPanel.SetActive(true);
        }

        currentchu.GetComponent<Text>().color = new Vector4(0, 0, 0, 255);

        yield return new WaitForSeconds(0.3f);

        currentchu = null;
        rect = null;
        posx = 0.0f;
        posy = 0.0f;

        //frame.GetComponent<frameColider>().enabled = false;
        Destroy(charframe.GetComponent<frameColider>());

     }
    
    IEnumerator CompleteSet()
    {
        charframe.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        gameManager.PointCheck(currentchu,scale);
    }

    public void SelectButton(int index)
    {
        if (selection && stock > 0)
        {
            charindex = index;
            GenerateChar();
        }
    }

    public void MenuButton()
    {
        keyflag = false;
        menuPanel.SetActive(true);
    }
}
