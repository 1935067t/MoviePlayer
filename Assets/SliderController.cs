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
    [SerializeField] private GameObject mFDSV; //_videoMode��_fdsvMode�̂ǂ��炩������

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

    // �h���b�N���J�n�����Ƃ��Ă΂��.
    public void OnBeginDrag(PointerEventData eventData)
    {   //�J�����������Ȃ��悤��
        mCameraController.GetComponent<CameraController>().SetActive(false);
        //������~�߂�
        ExecuteEvents.Execute<EventCaller>(
            target: mFDSV,
            eventData: null,
            functor: (reciever, eventData) => reciever.WaitForSliderChanging());
    }

    // �h���b�N���I�������Ƃ��Ă΂��.
    public void OnEndDrag(PointerEventData eventData)
    {   //�J�����𓮂����Ă�OK
        mCameraController.GetComponent<CameraController>().SetActive(true);
        //�X���C�_�[�̒l���r�f�I�ɔ��f
        ExecuteEvents.Execute<EventCaller>(
            target: mFDSV,
            eventData: null,
            functor: (reciever, eventData) => reciever.ReflectFrameFromSlider((int)mSlider.value));
    }

    //VideoBase����Ăяo��
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
