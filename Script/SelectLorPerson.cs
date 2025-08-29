using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectLorPerson : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text lor;
    public void OnPointerEnter(PointerEventData eventData)
    {
        lor.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        lor.gameObject.SetActive(false);
        
    }
}
