using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    static public Player Instance; //��Ŭ��

    
    public GameManager manager;

    public string currentMapName; //transferMap ��ũ��Ʈ�� �ִ� transferMapName ������ ���� ����.

    //�޸���, �ȱ�, �����ϱ�
    public float runSpeed;
    public float walkSpeed;
    public float currentSpeed;
    public float jumpPower;
    //�ִ� ���¹̳�, ���� ���¹̳�
    public float maxStamina = 100f;
    public float currentStamina;
    //�޷����� ���¹̳� �Ҹ�, �����Ͽ����� ���¹̳� �Ҹ�
    public float runStamina = 10f; //�ʴ� 5
    public float jumpStamina = 10f; //Ƚ���� 10
    //������ ������ ȸ���Ǵ� ���¹̳� ��
    public float recoveryStamina = 2f; //�ʴ� 2
    public float downRecoveryStamina = 10;
    //HP ��� �߰�
    public float currentHp;
    public float maxHp = 100f;

    //��ɵ� �־��ֱ� ������ٵ�(����), ��������Ʈ������(�÷��̾� ȸ��),�ִϸ�����(�÷��̾� �����϶� ���)
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
        
        if (!Input.GetButtonUp("Horizontal") && (!Input.GetKey(KeyCode.Space))) //���¹̳� ȸ��
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

                DeJumpStamina(jumpStamina); //������ ���¹̳� ����
            }
        }
        //�ɱ�
        if (Input.GetKey(KeyCode.S))
        {
            currentSpeed = 0f;
            anim.SetBool("isCrouch",true);
            IncDownrecovery(downRecoveryStamina * Time.deltaTime);

        }
        //�ɴ� ���϶� ���¹̳� ȸ��

        

        //�ִϸ��̼�
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

        //������
        if(Input.GetKeyDown(KeyCode.F) && scanObject != null)
        {
            manager.Action(scanObject);
        }
    }
    void FixedUpdate()
    {
        

        Movement();


        //����ĳ��Ʈ ���� ���̰� �ϱ�
        if (rigid.velocity.y < 0)
        {
            //Debug.DrawRay(rigid.position, Vector3.down * 2f, new Color(0, 1, 0));
            //���� ĳ��Ʈ�� ����Ͽ� �������� ����
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
        
    void Movement() //���� ������
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
            if (Input.GetKey(KeyCode.LeftShift)) //�޸���
            {
                currentSpeed = runSpeed;

                DeRunStamina(runStamina * Time.deltaTime); //���¹̳� ����


            }
        }
        
    }   
    
    

    //���¹̳� ����
    void DeRunStamina(float amount) //�޷����� ���¹̳� ����
    {
        currentStamina = Mathf.Clamp(currentStamina - amount, 0f, maxStamina);
        UpdateUI();
    }
    void DeJumpStamina(float amount) //���������� ���¹̳� ����
    {
        currentStamina = Mathf.Clamp(currentStamina - amount, 0f, maxStamina);
        UpdateUI();
    }
    void Increcovery(float amount) //������������ ���¹̳� ȸ��
    {
        currentStamina = Mathf.Clamp(currentStamina + amount, 0f, maxStamina);
        UpdateUI();
    }

    void IncDownrecovery(float amount) //�ɾ����� ���¹̳� ȸ��
    {

        currentStamina = Mathf.Clamp(currentStamina + amount, 0f, maxStamina);
        UpdateUI();
    }
    private void UpdateUI() //ui������Ʈ
    {
        staminaSlider.value = currentStamina;
        HpSlider.value = currentHp;
    }

    //private IEnumerator StaminaLatecortine(float amount)
    //{
    //    yield return new WaitForSeconds(1f);
    //}
}
