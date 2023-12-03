using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using System.IO;
using System;
using UnityEngine.Video;

public class FDSV : VideoBase
{

    bool mLoad;
    bool mLoadfirst;
    bool mIsPlaying;
    bool mIsLooping;
    int mLoadcount;
    string[] mVideoUrls;
    int[] mPosition = new int[3];
    int[] mDimension = new int[3];
    int[] mMoveIdx = new int[6];
    int mUrlIdx;
    // Start is called before the first frame update
    void Start()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false);
        //string extension = Path.GetExtension(paths[0]);

        //Array.Resize(ref _isPrepared, 0);
        //_videoNum = 0;
        //_preparedCount = 0;
        string[] content = File.ReadAllLines(paths[0]);

        string directoryPath = content[0];
        string extension = "*." + content[1];
        string[] dimension = content[2].Split(' ');
        string[] initialPosition = content[3].Split(' ');
        string frameRate = content[4];

        //ファイルパスのチェック
        if (directoryPath == "same")
        {
            directoryPath = Directory.GetParent(paths[0]).FullName;
        }
        else if (Directory.Exists(directoryPath) == false) return;

        mVideoUrls = Directory.GetFiles(directoryPath, extension);

        try
        {
            mDimension[0] = int.Parse(dimension[0]);
            mDimension[1] = int.Parse(dimension[1]);
            mDimension[2] = int.Parse(dimension[2]);

            mPosition[0] = int.Parse(initialPosition[0]);
            mPosition[1] = int.Parse(initialPosition[1]);
            mPosition[2] = int.Parse(initialPosition[2]);

            mUrlIdx = mPosition[0] + mDimension[0] * mPosition[1] + mDimension[1] * mDimension[0] * mPosition[2];
        }
        catch (FormatException)
        {
            mDimension[0] = mVideoUrls.Length;
            mDimension[1] = 1;
            mDimension[2] = 1;

            mPosition[0] = 0;
            mPosition[1] = 0;
            mPosition[2] = 0;

            mUrlIdx = 0;
        }

        mMoveIdx[0] = 1;
        mMoveIdx[1] = -1;
        mMoveIdx[2] = mDimension[0];
        mMoveIdx[3] = -mDimension[0];
        mMoveIdx[4] = mDimension[1] * mDimension[0];
        mMoveIdx[5] = -mDimension[1] * mDimension[0];

        mFrame = 0;
        mLoadfirst = true;
        mLoadcount = 0;

        for (int i = 0; i < 2; i++)
        {
            VideoPlayer player = this.gameObject.AddComponent<VideoPlayer>();
            player.renderMode = VideoRenderMode.APIOnly;
            player.audioOutputMode = VideoAudioOutputMode.None;
            player.waitForFirstFrame = false;
            player.playOnAwake = false;
            player.isLooping = true;
            player.url = mVideoUrls[0];
            player.prepareCompleted += Prepared;
            player.frameReady += Onframeready;
            player.sendFrameReadyEvents = true;
            player.Prepare();
        }

        mPlayers = gameObject.GetComponents<VideoPlayer>();
        //mActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && mLoad == false)
        {
            mFrame = mPlayers[mCounter].frame;
            mCounter = (mCounter + 1) % 2;
            mUrlIdx++;
            mPlayers[mCounter].url = mVideoUrls[mUrlIdx];

            mPlayers[mCounter].Play();
            mPlayers[mCounter].frame = mFrame;
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log(mPlayers[0].isPaused);
            Debug.Log(mPlayers[1].isPaused);
            Debug.Log(mPlayers[0].frame);
            Debug.Log(mPlayers[1].frame);
        }
        if (mActive == false) return;

        BasicVideoOperation();
        SwitchCamera();
    }

    void Prepared(VideoPlayer vp)
    {

        if (mLoadfirst)
        {
            vp.Play();
            return;
        }

        mLoad = true;
    }

    void Onframeready(VideoPlayer vp, long frame)
    {

        if (mLoadfirst)
        {
            mLoadcount++;
            vp.Pause();
            vp.frame = 0;
            if (mLoadcount < 2) return;
            mMaterial.mainTexture = mPlayers[mCounter].texture;
            mSlider.GetComponent<SliderController>().PrepareSlider(0, (int)mPlayers[mCounter].frameCount - 1);
            mLoadfirst = false;
            mActive = true;
            DisplayInfomation();
        }

        else if (mLoad && frame == this.mFrame && vp.url == mPlayers[mCounter].url)
        {

            mMaterial.mainTexture = mPlayers[mCounter].texture;
            mPlayers[(mCounter + 1)%2].Pause();
            if (mIsPlaying == false) mPlayers[mCounter].Pause(); 
            mLoad = false;
            DisplayInfomation();
        }
        mSlider.value = frame;
    }

    void SwitchCamera()
    {
        if(mLoad) { return; }
        int moveIdx = CalcMoveIdx();
        if(moveIdx == 0)
        {
            return;
        }

        mLoad = true;
        mIsPlaying = mPlayers[mCounter].isPlaying;
        mIsLooping = mPlayers[mCounter].isLooping;
        mFrame = mPlayers[mCounter].frame;
        //mPlayers[mVideoIdx].Pause();

        mUrlIdx += moveIdx;

        mCounter = (mCounter + 1) % 2;
        mPlayers[mCounter].isLooping = mIsLooping;
        mPlayers[mCounter].url = mVideoUrls[mUrlIdx];

        mPlayers[mCounter].Play();
        mPlayers[mCounter].frame = mFrame;

        Debug.Log(mUrlIdx);
        Debug.Log(mPosition[0]);
        Debug.Log(mPosition[1]);
        Debug.Log(mPosition[2]);

        //DisplayInfomation();
    }

    int CalcMoveIdx()
    {
        //+x方向への移動
        if(Input.GetKeyDown(KeyCode.RightArrow) && CheckP_X())
        {
            mPosition[0]++;
            return mMoveIdx[0];
        }

        //-x方向への移動
        if(Input.GetKeyDown(KeyCode.LeftArrow) && CheckM_X())
        {
            mPosition[0]--;
            return mMoveIdx[1];
        }

        //+y方向への移動
        if(Input.GetKeyDown(KeyCode.UpArrow) && CheckP_Y())
        {
            mPosition[1]++;
            return mMoveIdx[2];
        }

        //-y方向へ移動
        if( Input.GetKeyDown(KeyCode.DownArrow) && CheckM_Y())
        {
            mPosition[1]--;
            return mMoveIdx[3];
        }

        //+z方向へ移動
        if(Input.GetKeyDown(KeyCode.Comma) && CheckP_Z())
        {
            mPosition[2]++;
            return mMoveIdx[4];
        }

        //-z方向へ移動
        if(Input.GetKeyDown(KeyCode.Period) && CheckM_Z())
        {
            mPosition[2]--;
            return mMoveIdx[5];
        }

        return 0;
    }

    bool CheckP_X() { return mPosition[0] + 1 < mDimension[0]; }
    bool CheckM_X() { return mPosition[0] - 1 >= 0; }
    bool CheckP_Y() { return mPosition[1] + 1 < mDimension[1]; }
    bool CheckM_Y() { return mPosition[1] - 1 >= 0; }
    bool CheckP_Z() { return mPosition[2] + 1 < mDimension[2]; }
    bool CheckM_Z() { return mPosition[2] - 1 >= 0; }

    private void DisplayInfomation()
    {
        mInfomationTxt.text =
            "File: " + Path.GetFileName(mPlayers[mCounter].url) + "\n"
            + "Dimension: " + $"({mDimension[0]},{mDimension[1]},{mDimension[2]})\n"
            + "Position: " + ($"({mPosition[0]},{mPosition[1]},{mPosition[2]})\n");
    }
}
