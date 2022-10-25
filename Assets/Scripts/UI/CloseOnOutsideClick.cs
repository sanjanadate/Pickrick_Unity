﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseOnOutsideClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }


    public void OnDeselect(BaseEventData eventData)
    {
        gameObject.SetActive(false);
    }
}
