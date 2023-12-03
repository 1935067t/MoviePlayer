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
    /// URL���w�肵�čĐ�����
    /// </summary>
    public void Play(string url)
    {
        // ����̐ݒ�
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = url;

        // �Đ������̊J�n
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
    /// �r�f�I�N���b�v���w�肵�čĐ�����
    /// </summary>
    public void Play()
    {
        // ����̐ݒ�
        //videoPlayer.source = VideoSource.VideoClip;
        //videoPlayer.clip = videoClip;

        // �Đ������̊J�n
        videoPlayer.frameReady += OnFrameReady;
        // ���̐ݒ�����Ȃ���FrameReady�̃C�x���g�͔��΂���Ȃ��̂Œ���
        videoPlayer.sendFrameReadyEvents = true;
        videoPlayer.prepareCompleted += OnPrepareCompleted;
        videoPlayer.Prepare();
        Debug.Log("a");
    }

    /// <summary>
    /// �Đ���������������
    /// </summary>
    void OnPrepareCompleted(VideoPlayer vp)
    {
        // �C���[�W�ɓ���e�N�X�`�����Z�b�g����
        //material.mainTexture = vp.texture;

        // �C���[�W�T�C�Y�𓮉�Ɠ����傫���ɂ���
        //RectTransform rt = GetComponent<RectTransform>();
        //rt.sizeDelta = new Vector2(videoPlayer.texture.width, videoPlayer.texture.height);

        // �C�x���g�n���h�����Z�b�g���čĐ�����
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