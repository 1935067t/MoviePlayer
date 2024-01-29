using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoBase : MonoBehaviour, EventCaller
{
    protected VideoPlayer mVideoPlayer;

    protected long mFrame = 0;
    protected double mTime;
    protected int mUrlIdx;
    protected bool mIsPlaying;
    protected bool mIsLooping;

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
        if (mVideoPlayer.isPlaying) mVideoPlayer.Pause();

        else mVideoPlayer.Play();

        mIsPlaying = mVideoPlayer.isPlaying;
    }

    void Restart()
    {
        mVideoPlayer.frame = 0;
    }

    void StepForward()
    {
        mVideoPlayer.StepForward();
    }

    void StepBackward()
    {
        mVideoPlayer.frame--;
    }

    void SwitchLoop()
    {
        mVideoPlayer.isLooping = mVideoPlayer.isLooping ? false : true;
        mLoopBox.isOn = mVideoPlayer.isLooping;

        mIsLooping = mVideoPlayer.isLooping;
    }

    //----------------外部との連携用の関数--------------------------

    //ビデオ　から スライダー　へフレームを反映
    protected void SetFrameToSlider()
    {
        mSlider.value = mVideoPlayer.frame;
    }

    //SliderControllerから呼ぶ
    //スライダーが操作されているときに動画を操作出来ないようにする
    public void WaitForSliderChanging()
    {
        mVideoPlayer.Pause();
        mActive = false;
    }

    //SliderControllerから呼ぶ
    //frame-1でいいか要チェック
    public void ReflectFrameFromSlider(int frame)
    {
        mVideoPlayer.frame = frame;
        mActive = true;
    }

    //LoopBoxから呼ぶ
    public void IsLoop(bool loop)
    {
        mVideoPlayer.isLooping = loop;
        mIsLooping = loop;
    }
}
