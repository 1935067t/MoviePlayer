using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoopBox : MonoBehaviour
{
    private Toggle mToggle;

    [SerializeField] private GameObject mFDSV;
    // Start is called before the first frame update
    void Start()
    {
        mToggle = GetComponent<Toggle>();

        mToggle.gameObject.SetActive(true);
        mToggle.enabled = true;

        mToggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(mToggle);
        });

    }

    private void ToggleValueChanged(Toggle toggle)
    {
        ExecuteEvents.Execute<EventCaller>(
            target: mFDSV,
            eventData: null,
            functor: (reciever, eventData) => reciever.IsLoop(mToggle.isOn));
    }
}
