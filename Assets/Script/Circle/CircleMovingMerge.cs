using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UI;

public class CircleMovingMerge : CircleController
{

    SpriteRenderer circle1SR, circle2SR, circle3SR, circle4SR;
    CircleCollider2D circle1Atk, circle2Atk, circle3Atk, circle4Atk;
    private int circle_CNT;

    void Start()
    {
        circle1SR = Circle[0].GetComponent<SpriteRenderer>();
        circle2SR = Circle[1].GetComponent<SpriteRenderer>();
        circle3SR = Circle[2].GetComponent<SpriteRenderer>();
        circle4SR = Circle[3].GetComponent<SpriteRenderer>();
        circle1Atk = Circle[0].GetComponent<CircleCollider2D>();
        circle2Atk = Circle[1].GetComponent<CircleCollider2D>();
        circle3Atk = Circle[2].GetComponent<CircleCollider2D>();
        circle4Atk = Circle[3].GetComponent<CircleCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        stop = false;
        CircleEnergyCheck1 = false; //1207 from jeongik delete CircleEnergyCheck2
        circleactive = true;
        //CircleSize_var=CircleSize; // 1214 from jeongik
        circle_CNT = circleCount;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))//1207 from jeongik
        {
            if (circleactive)
            {
                CircleUnActive(circleCount);
                circleactive = false;
            }
            else
            {
                CircleActive(circleCount);
                CancelInvoke();
                circleactive = true;
            }
        }        
        if (CircleControl)
        {
            circleactive = true;
            CircleActive(circleCount);
            CancelInvoke();
        }
        else
        {
            //Invoke("CircleUnActive", 5);

        }
        if (CircleControl && Mathf.Abs(Dir) == 1)
        {
            //Debug.Log(Dir);
            CircleControl = false; //1207 from jeongik
        }
        if (circle_CNT != circleCount)
        {
            circle_CNT = circleCount;
            CircleUnActive(circleCount);
            circleactive = false;
            CircleActive(circleCount);
            CancelInvoke();
            circleactive = true;
        }

        //에너지 충전
        CircleSkillFill();//1207 from jeongik
        //Debug.Log(currentTime);//1207 from jeongik
        //Debug.Log(CircleSize_var);//1207 from jeongik

        if (PlayerMoving.CurrentEnergy < 100.1f)
        {
            PlayerMoving.CurrentEnergy += Time.deltaTime * EnergyFillSpeed; ;
        }
        // 카이팅
        GetComponent<CircleController>()?.WSkill(KeyCode.W, WRadius, WSpeed, WSize);
        GetComponent<CircleController>()?.ADSkill(KeyCode.D, clockwise, ADSpeed, EnergyDrainSpeed);
        GetComponent<CircleController>()?.ADSkill(KeyCode.A, counterclockwise, ADSpeed, EnergyDrainSpeed);
        GetComponent<CircleController>()?.FeverSkill(KeyCode.D, clockwise, CircleSkillSpeed, CircleEnergyDrainSpeed);
        GetComponent<CircleController>()?.FeverSkill(KeyCode.A, counterclockwise, CircleSkillSpeed, CircleEnergyDrainSpeed);

        // if (Input.GetKey(KeyCode.S)){
        //     Radius = Mathf.Lerp(Radius,1,Time.deltaTime*10);
        //     rotdir = Mathf.Lerp(rotdir,Mathf.Sign(rotdir) * 0.5f,Time.deltaTime*10);
        //     PlayerMoving.Damagefix = Mathf.Lerp(PlayerMoving.Damagefix,0,Time.deltaTime*10);

        // }
        if (!Input.GetKey(KeyCode.S) && Radius < 3)
        {
            Radius = Mathf.Lerp(Radius, 3, Time.deltaTime * 10);
            rotdir = Mathf.Lerp(rotdir, Mathf.Sign(rotdir) * 1.0f, Time.deltaTime * 10);
            PlayerMoving.Damagefix = Mathf.Lerp(PlayerMoving.Damagefix, 1, Time.deltaTime * 10);
        }

        // 서클 사이즈
        transform.GetChild(0).localScale = new Vector3(CircleSize, CircleSize, 1);
        transform.GetChild(1).localScale = new Vector3(CircleSize, CircleSize, 1);
        transform.GetChild(2).localScale = new Vector3(CircleSize, CircleSize, 1);
        transform.GetChild(3).localScale = new Vector3(CircleSize, CircleSize, 1);

        // 서클 회전
        //this.transform.rotation = Quaternion.Euler(0, 0, Angle);
        PPX = player.position.x;
        PPY = player.position.y;
        Angle += CircleController.AngleSpeed * rotdir * Time.deltaTime * 30;

 

        CircleChange(circleCount);

        // 360도 마다 저장된 각도 0으로 초기화
        if (Angle > 360 || Angle < -360) //1207 from jeongik
        {
            Angle = 0;
        }

    }
    public void CircleChange(int circle_Count)
    {
        switch (circle_Count)
        {
            case 2:
                transform.GetChild(0).position = new Vector3(PPX + Radius * Mathf.Cos(Angle * Mathf.Deg2Rad), PPY + Radius * Mathf.Sin(Angle * Mathf.Deg2Rad), -1);
                transform.GetChild(1).position = new Vector3(PPX + Radius * Mathf.Cos((Angle + 180) * Mathf.Deg2Rad), PPY + Radius * Mathf.Sin((Angle + 180) * Mathf.Deg2Rad), -1);
                break;
            case 3:
                transform.GetChild(0).position = new Vector3(PPX + Radius * Mathf.Cos(Angle * Mathf.Deg2Rad), PPY + Radius * Mathf.Sin(Angle * Mathf.Deg2Rad), -1);
                transform.GetChild(1).position = new Vector3(PPX + Radius * Mathf.Cos((Angle + 120) * Mathf.Deg2Rad), PPY + Radius * Mathf.Sin((Angle + 120) * Mathf.Deg2Rad), -1);
                transform.GetChild(2).position = new Vector3(PPX + Radius * Mathf.Cos((Angle + 240) * Mathf.Deg2Rad), PPY + Radius * Mathf.Sin((Angle + 240) * Mathf.Deg2Rad), -1);
                break;
            case 4:
                transform.GetChild(0).position = new Vector3(PPX + Radius * Mathf.Cos(Angle * Mathf.Deg2Rad), PPY + Radius * Mathf.Sin(Angle * Mathf.Deg2Rad), -1);
                transform.GetChild(1).position = new Vector3(PPX + Radius * Mathf.Cos((Angle + 90) * Mathf.Deg2Rad), PPY + Radius * Mathf.Sin((Angle + 90) * Mathf.Deg2Rad), -1);
                transform.GetChild(2).position = new Vector3(PPX + Radius * Mathf.Cos((Angle + 180) * Mathf.Deg2Rad), PPY + Radius * Mathf.Sin((Angle + 180) * Mathf.Deg2Rad), -1);
                transform.GetChild(3).position = new Vector3(PPX + Radius * Mathf.Cos((Angle + 270) * Mathf.Deg2Rad), PPY + Radius * Mathf.Sin((Angle + 270) * Mathf.Deg2Rad), -1);
                break;
        }
    }
    public void CircleActive(int circle_Count)//1218 WJ
    {
        switch (circle_Count)
        {

            case 2:
                circle1SR.enabled = true;
                circle2SR.enabled = true;
                circle3SR.enabled = false;
                circle4SR.enabled = false;
                circle1Atk.enabled = true;
                circle2Atk.enabled = true;
                circle3Atk.enabled = false;
                circle4Atk.enabled = false;
                break;
            case 3:

                circle1SR.enabled = true;
                circle2SR.enabled = true;
                circle3SR.enabled = true;
                circle4SR.enabled = false;
                circle1Atk.enabled = true;
                circle2Atk.enabled = true;
                circle3Atk.enabled = true;
                circle4Atk.enabled = false;
                break;
            case 4:
                circle1SR.enabled = true;
                circle2SR.enabled = true;
                circle3SR.enabled = true;
                circle4SR.enabled = true;
                circle1Atk.enabled = true;
                circle2Atk.enabled = true;
                circle3Atk.enabled = true;
                circle4Atk.enabled = true;
                break;
        }
    }
    public void CircleUnActive(int circle_Count)//1218 WJ
    {
        switch (circle_Count)
        {
            case 2:
                circle1SR.enabled = false;
                circle2SR.enabled = false;
                circle3SR.enabled = false;
                circle4SR.enabled = false;
                circle1Atk.enabled = false;
                circle2Atk.enabled = false;
                circle3Atk.enabled = false;
                circle4Atk.enabled = false;
                circleactive = false;
                break;
            case 3:
                circle1SR.enabled = false;
                circle2SR.enabled = false;
                circle3SR.enabled = false;
                circle4SR.enabled = false;
                circle1Atk.enabled = false;
                circle2Atk.enabled = false;
                circle3Atk.enabled = false;
                circle4Atk.enabled = false;
                circleactive = false;
                break;
            case 4:
                circle1SR.enabled = false;
                circle2SR.enabled = false;
                circle3SR.enabled = false;
                circle4SR.enabled = false;
                circle1Atk.enabled = false;
                circle2Atk.enabled = false;
                circle3Atk.enabled = false;
                circle4Atk.enabled = false;
                circleactive = false;
                break;
        }

    }
}
