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
    {   //�X�y�[�X��play�Epause�؂�ւ�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaySwitch();
        }
        //R�ōŏ��̃t���[����
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
        //Return��1�t���[���i��
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StepForward();
        }
        //BackSpace��1�t���[���߂�
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            StepBackward();
        }
        //L�Ń��[�v�؂�ւ�
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

    //----------------�O���Ƃ̘A�g�p�̊֐�--------------------------

    //�r�f�I�@���� �X���C�_�[�@�փt���[���𔽉f
    protected void SetFrameToSlider()
    {
        mSlider.value = mVideoPlayer.frame;
    }

    //SliderController����Ă�
    //�X���C�_�[�����삳��Ă���Ƃ��ɓ���𑀍�o���Ȃ��悤�ɂ���
    public void WaitForSliderChanging()
    {
        mVideoPlayer.Pause();
        mActive = false;
    }

    //SliderController����Ă�
    //frame-1�ł������v�`�F�b�N
    public void ReflectFrameFromSlider(int frame)
    {
        mVideoPlayer.frame = frame;
        mActive = true;
    }

    //LoopBox����Ă�
    public void IsLoop(bool loop)
    {
        mVideoPlayer.isLooping = loop;
        mIsLooping = loop;
    }
}
