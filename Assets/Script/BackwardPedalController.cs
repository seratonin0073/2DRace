using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BackwardPedalController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Sprite pedalUp;
    public Sprite pedalDown;
    public GameObject Car;

    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = pedalDown;
        Car.GetComponent<CarController>().moveBackward = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = pedalUp;
        Car.GetComponent<CarController>().moveBackward = false;
    }
}
