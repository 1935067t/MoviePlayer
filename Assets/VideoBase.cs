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

    //----------------�O���Ƃ̘A�g�p�̊֐�--------------------------

    //�r�f�I�@���� �X���C�_�[�@�փt���[���𔽉f
    protected void SetFrameToSlider()
    {
        mSlider.value = mPlayers[mCounter].frame;
    }

    //SliderController����Ă�
    //�X���C�_�[�����삳��Ă���Ƃ��ɓ���𑀍�o���Ȃ��悤�ɂ���
    public void WaitForSliderChanging()
    {
        mPlayers[mCounter].Pause();
        mActive = false;
    }

    //SliderController����Ă�
    //frame-1�ł������v�`�F�b�N
    public void ReflectFrameFromSlider(int frame)
    {
        mPlayers[mCounter].frame = frame;
        mActive = true;
    }

    //LoopBox����Ă�
    public void IsLoop(bool loop)
    {
        mPlayers[mCounter].isLooping = loop;
    }
}
