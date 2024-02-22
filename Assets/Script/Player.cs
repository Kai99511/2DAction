using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField, Header("移動速度")]
    private float moveSpeed;
    [SerializeField, Header("ジャンプ速度")]
    private float jumpSpeed;
    [SerializeField, Header("体力")]
    private int HP;
    [SerializeField, Header("無敵時間")]
    private float damageTime;
    [SerializeField, Header("点滅時間")]
    private float flashTime;
    [SerializeField, Header("ジャンプSE")]
    private GameObject jumpSE;
    [SerializeField, Header("ダメージSE")]
    private GameObject damageSE;

    private Vector2 inputDirection;
    private Rigidbody2D rigid;
    private Animator anim;
    private bool bjump;
    private SpriteRenderer spriteRenderer;
    


    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bjump = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        LookMoveDirec();
        Hitfloor();
       
    }

    private void Move()
    {
        //if (bjump) return;
        rigid.velocity = new Vector2( inputDirection.x * moveSpeed,rigid.velocity.y);
        anim.SetBool("Walk",inputDirection.x !=0.0f);
    }

    private void LookMoveDirec()
    {
        if (inputDirection.x > 0.0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (inputDirection.x < 0.0f)
        {
            transform .eulerAngles = new Vector3(0.0f,180.0f,0.0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if(collision.gameObject.tag == "Enemy")
        {
            HitEnemy(collision.gameObject);
        }
        else if(collision.gameObject.tag == "Goal")
        {
            FindObjectOfType<MainManager>().ShowGameClearUI();
            enabled = false;
            GetComponent<PlayerInput>().enabled = false;
        }
    }

    private void Hitfloor()
    {
        int layerMask = LayerMask.GetMask("Floor");
        Vector3 rayPos = transform.position - new Vector3(0.0f,transform.lossyScale.y/2.0f);
        Vector3 raySize = new Vector3(transform.lossyScale.x - 0.1f, 0.1f);
        RaycastHit2D rayHit = Physics2D.BoxCast(rayPos,raySize,0.0f,Vector2.zero,0.0f,layerMask);
        if(rayHit.transform == null)
        {
            bjump = true;
            anim.SetBool("Jump", bjump);
            return;
        }
        
        if(rayHit.transform.tag == "Floor" && bjump)
        {
            bjump = false;
            anim.SetBool("Jump",bjump);
        }
    }

    private void HitEnemy(GameObject enemy)
    {
        float halfScaleY = transform.lossyScale.y / 2.0f;
        float enemyHalfScaleY = enemy.transform.lossyScale.y / 2.0f;
        if(transform.position.y - (halfScaleY - 0.1f)>= enemy.transform.position.y + (enemyHalfScaleY - 0.1f))
        {
            Destroy(enemy);
            rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            Instantiate(jumpSE);
        }
        else
        {
            enemy.GetComponent<Enemy>().PlayerDamage(this);
            gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
            StartCoroutine(Damage());
        }
    }

    IEnumerator Damage()
    {
        Color color = spriteRenderer.color;
        for(int i = 0; i < damageTime; i++)
        {
            yield return new WaitForSeconds(flashTime);
            spriteRenderer.color = new Color(color.r,color.g,color.b,0.0f);

            yield return new WaitForSeconds(flashTime);
            spriteRenderer.color = new Color(color.r, color.g, color.b, 1.0f);
        }
        spriteRenderer.color = color;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    private void Dead()
    {
        if(  HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Camera camera = Camera.main;
        if(camera.name == "Main Camera" && camera.transform.position.y > transform.position.y)
        {
            Destroy(gameObject);
        }
    }

    public void Onmove(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
    }

    public void Onjump(InputAction.CallbackContext context)
    {
        if (!context.performed || bjump) return;

        rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        Instantiate(jumpSE);
      
    }


    public void Damage(int damage)
    {
        HP = Mathf.Max(HP - damage,0);
        Instantiate(damageSE);
        Dead();
    }

    public int GetHP()
    {
        return HP;
    }
}
