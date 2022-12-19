using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scene0Manager : MonoBehaviour
{
    public GameObject GameLoadPanel;
    public void GameLoadButton()
    {
        GameLoadPanel.gameObject.SetActive(true);
    }
}
