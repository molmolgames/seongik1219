using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Circle2MovingNew : CircleController
{

    // Rigidbody2D rigid;
    // float Angle; // 회전 각
    // public float AngleSpeed;
    // public float Radius; // 회전 반경
    // public float EnergyFillSpeed;
    // public float EnergyDrainSpeed;
    // public PlayerMoving Player;
    
    

    // public Transform player;
    // public GameManger gameManger;
    // public Transform[] Circle; //회전 서클

    // public float rotdir; //회전 방향
    // public float WRadius;
    // public float WSpeed;
    // public float WSize;
    // public float CSSpeed;
    // public float ADSpeed;
    // public float CEDS; //Circle Energy Drain Speed;
    
    
    // float PPX; //player position x
    // float PPY; //player position y
    // float doubleclickedtime = -1.0f;
    // float doubleclickedtime2 = -1.0f;
    // float interval = 0.25f;
    // bool IsDoubleClicked = false;
    // bool IsDoubleClicked2 = false;
    // bool stop;
    // bool CircleEnergyCheck1;
    // bool CircleEnergyCheck2;


    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        stop = false;
        CircleEnergyCheck1 = false; //1207 from jeongik delete CircleEnergyCheck2
        circleactive = true;
        //CircleSize_var=CircleSize; // 1214 from jeongik
    }

    // Update is called once per frame

    void Update() {
        // Debug.Log(CircleMove);
        // Debug.Log(Dir);
        // 서클 숨기기 //1207 from jeongik
        if (Input.GetKeyDown(KeyCode.X))//1207 from jeongik
        {
            if (circleactive){
                Circle2UnActive();
                circleactive = false;
            }
            else{
                Circle2Active();
                CancelInvoke();
                circleactive = true;
            }
        }
        if (CircleControl){
            circleactive = true;
            Circle2Active();
            CancelInvoke();
        }
        else{
            Invoke("Circle2UnActive",5);

        }
        if (CircleControl && Mathf.Abs(Dir)==1){
            //Debug.Log(Dir);
            CircleControl = false; //1207 from jeongik
        }

        //에너지 충전
        CircleSkillFill();//1207 from jeongik
        //Debug.Log(currentTime);//1207 from jeongik
        //Debug.Log(CircleSize_var);//1207 from jeongik

        if (PlayerMoving.CurrentEnergy <100.1f){
            PlayerMoving.CurrentEnergy += Time.deltaTime * EnergyFillSpeed;;
        }
        // 카이팅
        GetComponent<CircleController>()?.WSkill(KeyCode.W,WRadius,WSpeed,WSize);
        GetComponent<CircleController>()?.ADSkill(KeyCode.D,clockwise,ADSpeed,EnergyDrainSpeed);
        GetComponent<CircleController>()?.ADSkill(KeyCode.A,counterclockwise,ADSpeed,EnergyDrainSpeed);
        GetComponent<CircleController>()?.FeverSkill(KeyCode.D,clockwise,CircleSkillSpeed,CircleEnergyDrainSpeed);
        GetComponent<CircleController>()?.FeverSkill(KeyCode.A,counterclockwise,CircleSkillSpeed,CircleEnergyDrainSpeed);
        
        // if (Input.GetKey(KeyCode.S)){
        //     Radius = Mathf.Lerp(Radius,1,Time.deltaTime*10);
        //     rotdir = Mathf.Lerp(rotdir,Mathf.Sign(rotdir) * 0.5f,Time.deltaTime*10);
        //     PlayerMoving.Damagefix = Mathf.Lerp(PlayerMoving.Damagefix,0,Time.deltaTime*10);
            
        // }
        if(!Input.GetKey(KeyCode.S) && Radius <3){
            Radius = Mathf.Lerp(Radius,3,Time.deltaTime*10);
            rotdir = Mathf.Lerp(rotdir,Mathf.Sign(rotdir) * 1.0f,Time.deltaTime*10);
            PlayerMoving.Damagefix = Mathf.Lerp(PlayerMoving.Damagefix,1,Time.deltaTime*10);
        }

        //서클 사이즈
        transform.GetChild(0).localScale = new Vector3(CircleSize, CircleSize, 1);
        transform.GetChild(1).localScale = new Vector3(CircleSize, CircleSize, 1);
        transform.GetChild(2).localScale = new Vector3(CircleSize, CircleSize, 1);

        // 서클 회전
        //this.transform.rotation = Quaternion.Euler(0, 0, Angle);
        PPX = player.position.x;
        PPY = player.position.y;
        Angle += CircleController.AngleSpeed * rotdir * Time.deltaTime * 30;
        transform.GetChild(0).position = new Vector3 (PPX + Radius * Mathf.Cos(Angle * Mathf.Deg2Rad),PPY + Radius * Mathf.Sin(Angle * Mathf.Deg2Rad),-1);
        transform.GetChild(1).position = new Vector3 (PPX + Radius * Mathf.Cos((Angle+120)* Mathf.Deg2Rad),PPY + Radius * Mathf.Sin((Angle+120)* Mathf.Deg2Rad),-1);
        transform.GetChild(2).position = new Vector3 (PPX + Radius * Mathf.Cos((Angle+240)* Mathf.Deg2Rad),PPY + Radius * Mathf.Sin((Angle+240)* Mathf.Deg2Rad),-1);
        // 360도 마다 저장된 각도 0으로 초기화
        if (Angle  > 360 || Angle < -360) //1207 from jeongik
        {
            Angle = 0;
        }
    }
    public void Circle2Active()//1207 from jeongik
    {
        SpriteRenderer circle1SR = Circle[0].GetComponent<SpriteRenderer>();
        circle1SR.enabled = true;
        SpriteRenderer circle2SR = Circle[1].GetComponent<SpriteRenderer>();
        circle2SR.enabled = true;
        SpriteRenderer circle3SR = Circle[2].GetComponent<SpriteRenderer>();
        circle3SR.enabled = true;
        CircleCollider2D circle1Atk = Circle[0].GetComponent<CircleCollider2D>();
        circle1Atk.enabled = true;
        CircleCollider2D circle2Atk = Circle[1].GetComponent<CircleCollider2D>();
        circle2Atk.enabled = true;
        CircleCollider2D circle3Atk = Circle[2].GetComponent<CircleCollider2D>();
        circle3Atk.enabled = true;
    }
    public void Circle2UnActive()//1207 from jeongik
    {
        SpriteRenderer circle1SR = Circle[0].GetComponent<SpriteRenderer>();
        circle1SR.enabled = false;
        SpriteRenderer circle2SR = Circle[1].GetComponent<SpriteRenderer>();
        circle2SR.enabled = false;
        SpriteRenderer circle3SR = Circle[2].GetComponent<SpriteRenderer>();
        circle3SR.enabled = false;
        CircleCollider2D circle1Atk = Circle[0].GetComponent<CircleCollider2D>();
        circle1Atk.enabled = false;
        CircleCollider2D circle2Atk = Circle[1].GetComponent<CircleCollider2D>();
        circle2Atk.enabled = false;
        CircleCollider2D circle3Atk = Circle[2].GetComponent<CircleCollider2D>();
        circle3Atk.enabled = false;
        circleactive = false;
    }
    void FixedUpdate()
    {
        


        
        

        //Debug.Log(player.position);
        //Debug.Log(this.transform.position);

        // 서클 플레이어 추적 >>>>> 이거 지금은 position 따라가게 해놨는 데 물리적용해서 속도로 지정으로 딜레이도 고려 중
        // Vector3 dir = player.transform.position - rigid.transform.position;
        

        //Debug.Log(transform.GetChild(0).position);
        //Debug.Log(transform.GetChild(1).position);

    }

    // void OnCollisionEnter2D(Collision2D collision) {
    //     if(collision.gameObject.tag == "Enemy"){
    //         Debug.Log("hit");
    //         OnAttack(collision.transform);
    //     }
    // }

    // public void OnAttack(Transform Enemy)

    // {
    //     EnemyMove enemymove = Enemy.GetComponent<EnemyMove>();
    //     enemymove.EnemyDamaged();
    //     gameManger.Stage_Score += 100;
    // }
}
