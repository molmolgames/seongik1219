using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMapController : WaterObjectData
{
    public static GameObject waterobject;
    static int ChestKeyCount;
    bool UpExcelZoneOn;
    bool UpExcelZoneOff;
    
    // Start is called before the first frame update
    void Start()
    {
        UpExcelZoneOn = true;
        UpExcelZoneOff = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(UpExcelZoneOn); //1214 from jeongik
        if (UpExcelZoneOn){
            UpExcelZone.SetActive(false);
            UpExcelZoneOn = false;
            StartCoroutine(DelayUpExcelZoneOff());
        }
        if (UpExcelZoneOff){
            UpExcelZone.SetActive(true);
            UpExcelZoneOff = false;
            StartCoroutine(DelayUpExcelZoneOn());
        }
        
        
    }
    public static void ScanObject(GameObject scanObject)
    {
        waterobject = scanObject;
        WaterMapController Waterdata = waterobject.GetComponent<WaterMapController>();
        if(Waterdata.isChest == true){
            if (ChestKeyCount > 5){
                Debug.Log("WaterChest");
                Animator anim = waterobject.GetComponent<Animator>();
                anim.SetTrigger("ChestOpen");
                WaterMapController dropitem = waterobject.GetComponent<WaterMapController>();
                dropitem.DropItem();
                Debug.Log("drop");
                CircleCollider2D collision = waterobject.GetComponent<CircleCollider2D>();
                collision.enabled = false;
            }
            else {
                Debug.Log(waterobject.GetComponent<WaterMapController>().id);
                Debug.Log(waterobject.GetComponent<WaterMapController>().isSign);
                
                waterobject.GetComponent<WaterMapController>().Gamemanager.Talk(waterobject.GetComponent<WaterMapController>().id, waterobject.GetComponent<WaterMapController>().isSign);
                waterobject.GetComponent<WaterMapController>().Gamemanager.talkPanel.SetActive(waterobject.GetComponent<WaterMapController>().Gamemanager.isTalkPanelActive);
            }
            
        }
        if(Waterdata.isKey== true){
            Debug.Log("isKey");
            if (Waterdata.id == 11){
                WaterMapController dropitem = waterobject.GetComponent<WaterMapController>();
                dropitem.DropItem();
                Debug.Log("UpLevelExcelZone");
            }
            ChestKeyCount ++;
            waterobject.SetActive(false);
        }
        
    }
    public void DropItem() //아이템드랍함수
    {
            GameObject item = Instantiate(dropItem);
            Transform objectposition = waterobject.GetComponent<Transform>();
            item.transform.position = objectposition.position;
            item.SetActive(true);
    }
    public IEnumerator DelayUpExcelZoneOn()
    {
        yield return new WaitForSeconds(5.0f);
        UpExcelZoneOn = true;
    }
    public IEnumerator DelayUpExcelZoneOff()
    {
        yield return new WaitForSeconds(1.0f);
        UpExcelZoneOff = true;
    }
}

