using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField, Header("à⁄ìÆë¨ìx")]
    private float moveSpeed;

    [SerializeField,Header("çUåÇóÕ")]
    private int attackPower;

    private Rigidbody2D rigid;
    private Animator anim;
    private Vector2 moveDirection;
    private bool bFloor;


    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); 
        moveDirection = Vector2.left;
        bFloor = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        ChangeMoveDirection();
        LookMoveDirection();
        HitFloor();
    }

    private void Move()
    {
        if (!bFloor) return;
        rigid.velocity = new Vector2(moveDirection.x * moveSpeed, rigid.velocity.y);
    }

    private void ChangeMoveDirection()
    {
        Vector2 halfSize = transform.lossyScale / 2.0f;
        int layerMask = LayerMask.GetMask("Floor");
        RaycastHit2D ray = Physics2D.Raycast(transform.position, -transform.right, halfSize.x + 0.1f, layerMask);
        if (ray.transform == null) return;
        if(ray.transform.tag == "Floor")
        {
            moveDirection = -moveDirection;
        }
    }

    private void LookMoveDirection()
    {
        if (moveDirection.x < 0.0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if(moveDirection.x>0.0f)
        {
            transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
        }
    }

    private void HitFloor()
    {
        int layerMask = LayerMask.GetMask("Floor");
            Vector3 rayPos = transform.position - new Vector3(0.0f,transform.lossyScale.y / 2.0f);
        Vector3 raySize = new Vector3(transform.lossyScale.x - 0.1f, 0.1f);
        RaycastHit2D rayHit = Physics2D.BoxCast(rayPos,raySize,0.0f,Vector2.zero,0.0f,layerMask);

        if(rayHit.transform == null)
        {
            bFloor = false;
            anim.SetBool("IsIdle", true);
        }
        else if (rayHit.transform.tag == "Floor" && !bFloor)
        {
            bFloor = true;
            anim.SetBool("IsIdle",false);
        }
       
    }

    public void PlayerDamage(Player player)
    {
        player.Damage(attackPower);
    }
}
