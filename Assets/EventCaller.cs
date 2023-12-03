using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface EventCaller : IEventSystemHandler
{
    //VideoBaseä÷êî
    void WaitForSliderChanging();
    void ReflectFrameFromSlider(int frame);
    void IsLoop(bool loop);
}
