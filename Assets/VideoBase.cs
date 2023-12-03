using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoBase : MonoBehaviour, EventCaller
{
    protected VideoPlayer[] mPlayers;
    protected long mFrame = 0;
    protected int mCounter = 0;
    protected bool mActive = false;

    [SerializeField] protected Material mMaterial;
    [SerializeField] protected Slider mSlider;
    [SerializeField] protected Toggle mLoopBox;
    [SerializeField] protected TextMeshProUGUI mInfomationTxt;

    protected void BasicVideoOperation()
    {   //スペースでplay・pause切り替え
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaySwitch();
        }
        //Rで最初のフレームに
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
        //Returnで1フレーム進む
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StepForward();
        }
        //BackSpaceで1フレーム戻る
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            StepBackward();
        }
        //Lでループ切り替え
        if (Input.GetKeyDown(KeyCode.L))
        {
            SwitchLoop();
        }
    }

    void PlaySwitch()
    {
        if(mPlayers[mCounter].isPlaying) mPlayers[mCounter].Pause();

        else mPlayers[mCounter].Play();
    }

    void Restart()
    {
        mPlayers[mCounter].frame = 0;
    }

    void StepForward()
    {
        mPlayers[mCounter].StepForward();
    }

    void StepBackward()
    {
        mPlayers[mCounter].frame--;
    }

    void SwitchLoop()
    {
        mPlayers[mCounter].isLooping = mPlayers[mCounter].isLooping ? false : true;
        mLoopBox.isOn = mPlayers[mCounter].isLooping;
    }

    //----------------外部との連携用の関数--------------------------

    //ビデオ　から スライダー　へフレームを反映
    protected void SetFrameToSlider()
    {
        mSlider.value = mPlayers[mCounter].frame;
    }

    //SliderControllerから呼ぶ
    //スライダーが操作されているときに動画を操作出来ないようにする
    public void WaitForSliderChanging()
    {
        mPlayers[mCounter].Pause();
        mActive = false;
    }

    //SliderControllerから呼ぶ
    //frame-1でいいか要チェック
    public void ReflectFrameFromSlider(int frame)
    {
        mPlayers[mCounter].frame = frame;
        mActive = true;
    }

    //LoopBoxから呼ぶ
    public void IsLoop(bool loop)
    {
        mPlayers[mCounter].isLooping = loop;
    }
}
