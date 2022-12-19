using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPortal : MonoBehaviour
{
    public PlayerMoving player;
    [HideInInspector]public bool isPortal;
    public float coolTime;
    [HideInInspector]public float currentTime;
    public Image PortalImage;
    public GameObject Portal;


    private void Awake()
    {
        isPortal = false;
        currentTime = 0;
    }
    private void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        PortalImage.fillAmount = (currentTime / coolTime);

        if (currentTime <= 0)
        {
            isPortal = true;
        }
    }


    public void PortalButton()
    {
        if (isPortal == true)
        {
            MakePortal();
        }
    }
    public void MakePortal()
    {      
            currentTime = coolTime;
            isPortal = false;
            GameObject portal = Instantiate(Portal);
            portal.transform.position = player.rigid.position;
            StartCoroutine(DestroyObject(portal));       
    }
    public IEnumerator DestroyObject(GameObject gameobject)
    {
        yield return new WaitForSeconds(15f);
        Destroy(gameobject);
        StopCoroutine(DestroyObject(null));
    }
}
