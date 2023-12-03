using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderController : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    private Slider mSlider;
    [SerializeField] private TextMeshProUGUI mSliderValue;

    [SerializeField] private Camera mCameraController;
    [SerializeField] private GameObject mFDSV; //_videoModeか_fdsvModeのどちらかが入る

    // Start is called before the first frame update
    void Start()
    {
        mSlider = GetComponent<Slider>();
        mSlider.wholeNumbers = true;
        mSlider.gameObject.SetActive(false);

        //_cameraController = GameObject.Find("Main Camera");

        ////_sliderText.gameObject.SetActive(false);
        mSlider.onValueChanged.AddListener((v) =>
        {
            mSliderValue.text = v.ToString("0");
        });
        mSliderValue.gameObject.SetActive(false);
    }

    // ドラックが開始したとき呼ばれる.
    public void OnBeginDrag(PointerEventData eventData)
    {   //カメラが動かないように
        mCameraController.GetComponent<CameraController>().SetActive(false);
        //動画を止める
        ExecuteEvents.Execute<EventCaller>(
            target: mFDSV,
            eventData: null,
            functor: (reciever, eventData) => reciever.WaitForSliderChanging());
    }

    // ドラックが終了したとき呼ばれる.
    public void OnEndDrag(PointerEventData eventData)
    {   //カメラを動かしてもOK
        mCameraController.GetComponent<CameraController>().SetActive(true);
        //スライダーの値をビデオに反映
        ExecuteEvents.Execute<EventCaller>(
            target: mFDSV,
            eventData: null,
            functor: (reciever, eventData) => reciever.ReflectFrameFromSlider((int)mSlider.value));
    }

    //VideoBaseから呼び出す
    public void PrepareSlider(int min, int max)
    {
        mSlider.enabled = true;
        mSliderValue.enabled = true;
        mSlider.gameObject.SetActive(true);
        mSliderValue.gameObject.SetActive(true);
        mSlider.minValue = min;
        mSlider.maxValue = max;
        mSlider.value = 0;
    }

}
