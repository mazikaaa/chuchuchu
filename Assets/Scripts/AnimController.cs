using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    public GameManager gameManager;
    public CharGenerator charGenerator;

    GameObject audio_BGM;
    AudioSource BGM;
    float BGM_default;


    public void DeleteAnimation()
    {
        charGenerator.keyflag = true;
        this.gameObject.SetActive(false);
    }

    public void MovePointAnimation()
    {
        gameManager.PointAnimation();
    }

    public void BGMStop()
    {
        audio_BGM = GameObject.Find("BGM(Clone)");
        BGM = audio_BGM.GetComponent<AudioSource>();

        BGM_default = BGM.volume;
        BGM.volume *= 0;
    }

    public void BGMPlay()
    {
        BGM.volume = BGM_default;
    }


}
