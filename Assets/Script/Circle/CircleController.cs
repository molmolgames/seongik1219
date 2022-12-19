using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

public class CircleController : MonoBehaviour
{
    public Rigidbody2D rigid;
    public float Angle; // 회전 각
    public static float AngleSpeed = 1;  //  Real Speed Movement of Circle
    public float Radius; // 회전 반경
    public float EnergyFillSpeed;
    public float EnergyDrainSpeed;
    public PlayerMoving Player;
    public float coolTime;
    public static float currentTime;
    
    

    public Transform player;
    public GameManger gameManger;
    public Image CircleSkillImage;
    public GameObject[] Circle; //회전 서클
    //public Slider CircleSlider; //기력 바 //1215 from jeongik 주석처리
    public float rotdir; //회전 방향 // RotSpeed 1211 from jeongik
    public float WRadius;
    public float WSpeed;
    public float WSize;
    public float CircleSkillSpeed;
    public float ADSpeed;
    public float CircleEnergyDrainSpeed; //Circle Energy Drain Speed;
    protected int Dir; //스킬 디렉션 //1207 from jeongik
    
    
    protected float PPX; //player position x //1207 from jeongik
    protected float PPY; //player position y //1207 from jeongik
    protected float doubleclickedtime = -1.0f; //1207 from jeongik
    protected float interval = 0.25f; //1207 from jeongik
    protected bool IsDoubleClicked = false; //1207 from jeongik
    protected bool stop; //1207 from jeongik
    protected bool CircleEnergyCheck1; //1207 from jeongik
    protected int clockwise = -1; //1207 from jeongik
    protected int counterclockwise = 1; //1207 from jeongik
    [HideInInspector] //1207 from jeongik
    public static bool CircleSkillCheck;
    public bool circleactive = false; //1207 from jeongik
    public static bool CircleControl; //1207 from jeongik

    public float CircleSize;  // Real Size of Circle
    [HideInInspector]public static float CircleSize_var = 1;   // value for calculation // 1215 from jeongik
    [HideInInspector]public static int circleCount = 2;  //1218 WJ Total Circle Object count


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void WSkill(KeyCode Key, float WRadius,float WSpeed,float WSize)
    {
        if (Input.GetKey(Key) && PlayerMoving.CurrentEnergy > 0 && !stop){
            //ActiveSlider(); //1207 from jeongik
            CircleControl = true; //1207 from jeongik
            Radius = Mathf.Lerp(Radius,WRadius,Time.deltaTime*10);
            PlayerMoving.CurrentEnergy -= Time.deltaTime * EnergyDrainSpeed;
            /* rotdir = Mathf.Lerp(rotdir,Mathf.Sign(rotdir) * WSpeed,Time.deltaTime*10);*/ // 서클 속도 증가
            rotdir = Mathf.Lerp(rotdir, Mathf.Sign(rotdir) * WSpeed, Time.deltaTime * 10);  // change with AngleSpeed  From WJ
            CircleSize = Mathf.Lerp(CircleSize, CircleSize_var*(WSize), Time.deltaTime*10); //서클 크기 증가   // change with CircleSize  From WJ
            if (PlayerMoving.CurrentEnergy<0.001f){
                //Invoke("UnActiveSlider",1); //1207 from jeongik
                CircleControl = false; //1207 from jeongik
                stop = true;
            }
        }
        if (Input.GetKeyUp(Key)){
            //Invoke("UnActiveSlider",1); //1207 from jeongik
            CircleControl = false; //1207 from jeongik
            stop = false;
        }
        else if(!Input.GetKey(Key) && Radius > 3  ||  CircleSize < CircleSize_var || stop){
            Radius = Mathf.Lerp(Radius, 3, Time.deltaTime*10);
            //rotdir = Mathf.Lerp(rotdir,Mathf.Sign(rotdir) * 1f, Time.deltaTime*10);   
            rotdir = Mathf.Lerp(rotdir, Mathf.Sign(rotdir), Time.deltaTime * 10);  // change with AngleSpeed  From WJ
            //PlayerMoving.Size = Mathf.Lerp(PlayerMoving.Size,1f,Time.deltaTime*10);
            CircleSize = Mathf.Lerp(CircleSize, CircleSize_var, Time.deltaTime * 10); // change with CircleSize  From WJ
            if (Radius < 3.001f && CircleSize < CircleSize_var + 0.01f)
            {
                //Invoke("UnActiveSlider",1); //1207 from jeongik
                CircleControl = false; //1207 from jeongik
            }
            // PlayerMoving.AngleSpeed = Mathf.Lerp(PlayerMoving.AngleSpeed,PlayerMoving.AngleSpeed+3,Time.deltaTime*10);
        } 
    }
    public virtual void ADSkill(KeyCode Key, int dir, float ADSpeed, float EnergyDrainSpeed)
    {
        //서클 속도 증가 패시브
        if (Input.GetKey(Key)&& PlayerMoving.CurrentEnergy > 0 && !stop){
            Dir = dir ; //1207 from jeongik
            rotdir = Mathf.Sign(Dir)*ADSpeed;  // ADSpeed + AngleSpeed change From WJ
            PlayerMoving.CurrentEnergy -= Time.deltaTime * EnergyDrainSpeed;
            //ActiveSlider(); //1207 from jeongik
            CircleControl = true;
            if (PlayerMoving.CurrentEnergy <0.01f){
                stop = true;
                rotdir = Mathf.Sign(Dir);
                //Debug.Log(stop);
                //Invoke("UnActiveSlider",1); //1207 from jeongik
                CircleControl = false; //1207 from jeongik
            }
        }
        if (Input.GetKeyUp(Key)){
            stop = false;
            //Invoke("UnActiveSlider",1); //1207 from jeongik
            CircleControl = false; //1207 from jeongik
        }
    }
    public virtual void FeverSkill(KeyCode Key, int dir, float CircleSkillSpeed, float CircleEnergyDrainSpeed)
    {
        // 서클 속도 증가 액티브 스킬
        if (Input.GetKeyDown(Key)){
            if((Time.time-doubleclickedtime) < interval)
            {
                IsDoubleClicked = true;
                doubleclickedtime = -1.0f;
                Dir=dir; //1207 from jeongik
                //Debug.Log(IsDoubleClicked); //1207 from jeongik
            }
            else{
                IsDoubleClicked =false;
                doubleclickedtime = Time.time;
                //Debug.Log(IsDoubleClicked);
            }
        }
        if (IsDoubleClicked && currentTime >= coolTime || CircleEnergyCheck1){
            //ActiveSlider(); //1207 from jeongik
            CircleControl = true; //1207 from jeongik

            if (currentTime > 0.1f){
                currentTime -= Time.deltaTime*CircleEnergyDrainSpeed;
                rotdir = Dir*CircleSkillSpeed;
                CircleSkillFill();
                CircleEnergyCheck1 = true;
            }
            else if (currentTime <= 0.1f){//1214 from jeongik
                CircleEnergyCheck1 = false;
                CircleSkillCheck = false;
                //Invoke("UnActiveSlider",1); //1207 from jeongik
                CircleControl = false; //1207 from jeongik
                rotdir = Dir;
            }
        }
        if(Input.GetKeyUp(Key)){
            IsDoubleClicked = false;
            rotdir = Mathf.Sign(Dir);
            CircleControl = false; //1207 from jeongik
        }
    }
    // public virtual void ActiveSlider() //기력활성함수 // 1215 from jeongik 주석처리
    // {
    //     CircleSlider.gameObject.SetActive(true);
    // }
    // public virtual void UnActiveSlider() //기력활성화함수
    // {
    //     CircleSlider.gameObject.SetActive(false);
    // }
    public virtual void ActiveCircle() //1207 from jeongik
    {
        rigid.gameObject.SetActive(true);
    }
    public virtual void UnActiveCircle() //1207 from jeongik
    {
        rigid.gameObject.SetActive(false);
    }
    public virtual void CircleSkillFill() //1207 from jeongik
    {
        if (currentTime < coolTime && !CircleSkillCheck){
            currentTime += Time.deltaTime;
        }
        else if (currentTime > coolTime){
            currentTime = coolTime;
            CircleSkillCheck = true;
            //Debug.Log("CircleSkillCheck = true");
        }
        CircleSkillImage.fillAmount = (currentTime / coolTime);
    }



}
