using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// Make sure this script is attached on one of the UI object
/// </summary>
public class InputManager : MonoBehaviour, IPointerDownHandler,
    IPointerUpHandler, IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        //if (eventData.pointerCurrentRaycast.gameObject != null)
        //{
        //    Debug.Log("OnDrag " + eventData.pointerCurrentRaycast.gameObject.name);
        //}

        if (CardManager.instance.SelectedCard != null)
        {
            CardManager.instance.MoveSelectedCard(eventData.position);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            //Debug.Log("OnPointerDown " + eventData.pointerCurrentRaycast.gameObject.name);
            if (eventData.pointerCurrentRaycast.gameObject.GetComponent<CardView>() != null)
            {
                CardManager.instance.SelectCard(eventData.pointerCurrentRaycast.gameObject.GetComponent<CardView>());
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            //Debug.Log("OnPointerUp " + eventData.pointerCurrentRaycast.gameObject.name);
        }

        CardManager.instance.OnCardRelease();
    }
}
