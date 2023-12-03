using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class player2 : MonoBehaviour
{
    //[SerializeField] RawImage rawImage = null;
    [SerializeField] VideoPlayer videoPlayer = null;
    [SerializeField] Material material = null;

    int idx = 0;
    bool loadfirst;

    /// <summary>
    /// URLを指定して再生する
    /// </summary>
    public void Play(string url)
    {
        // 動画の設定
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = url;

        // 再生準備の開始
        videoPlayer.prepareCompleted += OnPrepareCompleted;
        videoPlayer.Prepare();
    }


    void OnFrameReady(VideoPlayer vp, long frame)
    {
        if (loadfirst)
        {
            vp.Pause();
            loadfirst = false;
        }
        Debug.Log(vp.frame);
    }

    /// <summary>
    /// ビデオクリップを指定して再生する
    /// </summary>
    public void Play()
    {
        // 動画の設定
        //videoPlayer.source = VideoSource.VideoClip;
        //videoPlayer.clip = videoClip;

        // 再生準備の開始
        videoPlayer.frameReady += OnFrameReady;
        // この設定をしないとFrameReadyのイベントは発火されないので注意
        videoPlayer.sendFrameReadyEvents = true;
        videoPlayer.prepareCompleted += OnPrepareCompleted;
        videoPlayer.Prepare();
        Debug.Log("a");
    }

    /// <summary>
    /// 再生準備が完了した
    /// </summary>
    void OnPrepareCompleted(VideoPlayer vp)
    {
        // イメージに動画テクスチャをセットする
        //material.mainTexture = vp.texture;

        // イメージサイズを動画と同じ大きさにする
        //RectTransform rt = GetComponent<RectTransform>();
        //rt.sizeDelta = new Vector2(videoPlayer.texture.width, videoPlayer.texture.height);

        // イベントハンドラをセットして再生する
        //videoPlayer.started += OnMovieStarted;
        vp.Play();
        loadfirst = true;

        Debug.Log(vp.frameCount);
    }

    private void Start()
    {
        Play();
    }

    private void Update()
    {
        //if(u && videoPlayer.isPlaying)
        //{ u= false; 
        //    videoPlayer.Pause();
        //}

        if (Input.GetKeyDown(KeyCode.Return))
        {
            videoPlayer.StepForward();
            //Debug.Log(videoPlayer.frame);
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            videoPlayer.frame--;
            //Debug.Log(videoPlayer.frame);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (videoPlayer.isPlaying)
                videoPlayer.Pause();

            else videoPlayer.Play();
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            idx++;
            if(idx%2==1)
            {
                material.mainTexture = videoPlayer.texture;
                //videoPlayer.StepForward();
            }
        }
    }

}