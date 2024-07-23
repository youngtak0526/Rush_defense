using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    static public Player Instance; //싱클톤

    
    public GameManager manager;

    public string currentMapName; //transferMap 스크립트에 있는 transferMapName 변수의 값을 저장.

    //달리기, 걷기, 점프하기
    public float runSpeed;
    public float walkSpeed;
    public float currentSpeed;
    public float jumpPower;
    //최대 스태미나, 현재 스태미나
    public float maxStamina = 100f;
    public float currentStamina;
    //달렸을때 스태미나 소모량, 점프하였을때 스태미나 소모량
    public float runStamina = 10f; //초당 5
    public float jumpStamina = 10f; //횟수당 10
    //가만히 있을때 회복되는 스태미나 량
    public float recoveryStamina = 2f; //초당 2
    public float downRecoveryStamina = 10;
    //HP 기능 추가
    public float currentHp;
    public float maxHp = 100f;

    //기능들 넣어주기 리지드바디(물리), 스프라이트렌더러(플레이어 회전),애니메이터(플레이어 움직일때 모습)
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    GameObject scanObject;

    Vector3 dirVec;
    public Slider staminaSlider;
    public Slider HpSlider;

    void Awake()
    {
        
    }
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(this.gameObject);
            rigid = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
            
        }
        else
        {
            Destroy(this.gameObject);
        }

        currentStamina = maxStamina;
        currentHp = maxHp;
        UpdateUI();
    }
    void Update()
    {
        
        if (!Input.GetButtonUp("Horizontal") && (!Input.GetKey(KeyCode.Space))) //스태미나 회복
        {
            
            Increcovery(recoveryStamina * Time.deltaTime);
        }
        //stop speed
        //if (Input.GetButtonUp("Horizontal"))
        //{
        //    rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);

        ////}
        //if (Input.GetButton("Horizontal"))
        //{
        //     = Input.GetAxisRaw("Horizontal") == -1;

        //}
        //jump
        if (currentStamina > jumpStamina)
        {
            if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
            {
                rigid.velocity = new Vector2(rigid.velocity.x, 0);
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("isJumping", true);

                DeJumpStamina(jumpStamina); //점프시 스태미나 감소
            }
        }
        //앉기
        if (Input.GetKey(KeyCode.S))
        {
            currentSpeed = 0f;
            anim.SetBool("isCrouch",true);
            IncDownrecovery(downRecoveryStamina * Time.deltaTime);

        }
        //앉는 중일때 스태미나 회복

        

        //애니메이션
        if (Mathf.Abs(rigid.velocity.x) < 0.2)
        {
            anim.SetBool("iswalking", false);
        }
        else
        {
            anim.SetBool("iswalking", true);
            anim.SetBool("isCrouch", false);
        }

        //Direction
        bool hDown = Input.GetButtonDown("Horizontal");
        bool hUp = Input.GetButtonUp("Horizontal");
        float h = Input.GetAxisRaw("Horizontal");

        if (hDown && h == -1)
            dirVec = Vector3.left;
        else if (hDown && h == 1)
            dirVec = Vector3.right;

        //대사출력
        if(Input.GetKeyDown(KeyCode.F) && scanObject != null)
        {
            manager.Action(scanObject);
        }
    }
    void FixedUpdate()
    {
        

        Movement();


        //레이캐스트 눈에 보이게 하기
        if (rigid.velocity.y < 0)
        {
            //Debug.DrawRay(rigid.position, Vector3.down * 2f, new Color(0, 1, 0));
            //레이 캐스트를 사용하여 무한점프 막기
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 2f, LayerMask.GetMask("platfrom"));

            if (rayHit.collider != null)
            {
                //Debug.DrawRay(rigid.position, rayHit.point - rigid.position, Color.yellow);
                //if (rayHit.distance < GetComponent<CapsuleCollider2D>().size.y + 0.5f)
                if (rayHit.distance < 5f)
                {
                    anim.SetBool("isJumping", false);
                }

            }
        }
        


        Debug.DrawRay(rigid.position, dirVec * 1f, new Color(0,1,0));
        RaycastHit2D rayHitObject = Physics2D.Raycast(rigid.position, dirVec, 1f, LayerMask.GetMask("Object"));

        if (rayHitObject.collider != null)
        {
            scanObject = rayHitObject.collider.gameObject;
        }
        else
            scanObject = null;
    }
        
    void Movement() //가로 움직임
    {
        float h = manager.isAction ? 0 : Input.GetAxis("Horizontal");
        if (h != 0f)
        {
            spriteRenderer.flipX = h < 0 ? true : false;
        }

        rigid.velocity = new Vector2(currentSpeed * h, rigid.velocity.y);

        currentSpeed = walkSpeed;

        if (currentStamina > 5f)
        {
            if (Input.GetKey(KeyCode.LeftShift)) //달리기
            {
                currentSpeed = runSpeed;

                DeRunStamina(runStamina * Time.deltaTime); //스태미나 감소


            }
        }
        
    }   
    
    

    //스태미너 증감
    void DeRunStamina(float amount) //달렸을때 스태미나 감소
    {
        currentStamina = Mathf.Clamp(currentStamina - amount, 0f, maxStamina);
        UpdateUI();
    }
    void DeJumpStamina(float amount) //점프했을때 스태미나 감소
    {
        currentStamina = Mathf.Clamp(currentStamina - amount, 0f, maxStamina);
        UpdateUI();
    }
    void Increcovery(float amount) //가만히있을때 스태미나 회복
    {
        currentStamina = Mathf.Clamp(currentStamina + amount, 0f, maxStamina);
        UpdateUI();
    }

    void IncDownrecovery(float amount) //앉았을때 스태미나 회복
    {

        currentStamina = Mathf.Clamp(currentStamina + amount, 0f, maxStamina);
        UpdateUI();
    }
    private void UpdateUI() //ui업데이트
    {
        staminaSlider.value = currentStamina;
        HpSlider.value = currentHp;
    }

    //private IEnumerator StaminaLatecortine(float amount)
    //{
    //    yield return new WaitForSeconds(1f);
    //}
}
