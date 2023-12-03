using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    //スライダーを動かしているときは視線方向が回転しないようにする
    private bool _active = true;

    //スクリーンサイズ
    private float width;
    private float height;
    private float w_hMax; //widthとheightのうち大きい方の値を入れる
    private Vector2 sceneCenter = Vector2.zero;

    //スクリーンの中心から見た座標
    private Vector2 preMousePos = Vector2.zero;
    private Vector2 MousePos = Vector2.zero;

    //回転するときの感度
    [SerializeField] float sensitivityXY = 0.1f;
    [SerializeField] float sensitivityZ = 0.5f;

    private void Start()
    {
        Screen.SetResolution(Screen.width, Screen.height, false);
    }

    // Update is called once per frame
    void Update()
    {
        width = Screen.width;
        height = Screen.height;
        sceneCenter.x = width / 2.0f;
        sceneCenter.y = height / 2.0f;

        if (_active)
        {
            //Debug.Log(Input.GetAxis("Mouse X"));
            if (Input.GetMouseButtonDown(0))
            {
                preMousePos = Input.mousePosition;
                preMousePos -= sceneCenter;
                //Debug.Log(preMousePos);

                w_hMax = width > height ? width : height;
            }
            else if (Input.GetMouseButton(0))
            {
                MousePos = Input.mousePosition;
                MousePos -= sceneCenter;

                //float xAngle = sensitivityXY * (MousePos.y - preMousePos.y) / (1.0f + 10 * Mathf.Abs(MousePos.x) / Screen.width);
                float xAngle = sensitivityXY * (MousePos.y - preMousePos.y) / Mathf.Pow((1.0f + (Mathf.Abs(MousePos.x * 2) / width)), 2);
                float yAngle = -sensitivityXY * (MousePos.x - preMousePos.x) / Mathf.Pow((1.0f + (Mathf.Abs(MousePos.y * 2) / height)), 2);
                float zAngle = sensitivityZ * (MousePos.x * preMousePos.y - MousePos.y * preMousePos.x) / w_hMax;
                transform.Rotate(xAngle, yAngle, zAngle);

                preMousePos = MousePos;
            }
        }
    }

    public void ResetCamera()
    {
        transform.localEulerAngles = Vector3.zero;
    }

    public void SetActive(bool active)
    {
        _active = active;
    }
}