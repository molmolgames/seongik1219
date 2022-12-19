using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class UIPanelMouseEvent : MonoBehaviour
{
    public GameObject UIPanel;
    void Start()
    {

    }

    // Update is called once per frame

    public void OnMouseEnter()
    {
        UIPanel.SetActive(true);
    }

    public void OnMouseExit()
    {
        UIPanel.SetActive(false);
    }

}
