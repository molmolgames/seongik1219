using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MainCameraMove : MonoBehaviour
{
   

    public GameObject Target_A;
    public GameObject Target_B;
    public GameObject Target_C;  // 카메라가 따라다닐 타겟
    public GameObject Target_D;

    public float offsetX = 0.0f;            // 카메라의 x좌표
    public float offsetY = 10.0f;           // 카메라의 y좌표
    public float offsetZ = -10.0f;          // 카메라의 z좌표

    public float CameraSpeed = 15.0f;       // 카메라의 속도
    Vector3 TargetPos;                      // 타겟의 위치
    int count ;
    private void Awake()
    {
        
    }
    void Update()
    {

    }
    void FixedUpdate()
    {
        count++;
        if (count >= 200 && count < 500)
        {
            CameraMoving1();
        }
        else if(count >= 500 && count <800)
        {
            CameraMoving2();
        }
        else if (count >= 800 && count < 1100)
        {
            CameraMoving3();
        }
        else if (count >= 1100 && count < 1400)
        {
            CameraMoving4();
        }
        else if (count >= 1400)
        {
            count = 200;
        }
        // 카메라의 움직임을 부드럽게 하는 함수(Lerp)
        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);
    }


    void CameraMoving1()
    {
        TargetPos = new Vector3(
            Target_A.transform.position.x + offsetX,
            Target_A.transform.position.y + offsetY,
            Target_A.transform.position.z + offsetZ
            );
    }
    void CameraMoving2()
    {
        TargetPos = new Vector3(
            Target_B.transform.position.x + offsetX,
            Target_B.transform.position.y + offsetY,
            Target_B.transform.position.z + offsetZ
            );
    }
    void CameraMoving3()
    {
        TargetPos = new Vector3(
            Target_C.transform.position.x + offsetX,
            Target_C.transform.position.y + offsetY,
            Target_C.transform.position.z + offsetZ
            );
    }
    void CameraMoving4()
    {
        TargetPos = new Vector3(
            Target_D.transform.position.x + offsetX,
            Target_D.transform.position.y + offsetY,
            Target_D.transform.position.z + offsetZ
            );
    }
}

