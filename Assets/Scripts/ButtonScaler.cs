using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonScaler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform buttonRectTransform;
    [SerializeField] private Vector2 buttonOriginalScale;
    [SerializeField] private float buttonScaleFactor = 1.2f;

    [SerializeField] private RectTransform textRectTransform;
    [SerializeField] private Vector2 textOriginalScale;
    [SerializeField] private float textScaleFactor = 1.2f;

    void Awake()
    {
        buttonRectTransform = GetComponent<RectTransform>();
        textRectTransform = GetComponent<RectTransform>();
        buttonOriginalScale = buttonRectTransform.sizeDelta;
        textOriginalScale = textRectTransform.sizeDelta;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonRectTransform.sizeDelta = buttonOriginalScale * buttonScaleFactor;
        textRectTransform.sizeDelta = textOriginalScale * textScaleFactor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonRectTransform.sizeDelta = buttonOriginalScale;
        textRectTransform.sizeDelta = textOriginalScale;
    }
}
