using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandESC : MonoBehaviour
{
    public void ESCButton()
    {
        this.transform.parent.gameObject.SetActive(false);
    }
}
