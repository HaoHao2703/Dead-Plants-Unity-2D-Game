using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 4;

    [SerializeField]
    private Rigidbody2D rb;
    private Vector3 movePos;

    private Animator animator;

    [SerializeField]
    private float bunchWaitTime = 0.3f;
    private float waitBeforeBunching;

    [SerializeField]
    private float moveWaitTime = 0.3f;
    private float waitBeforeMoving;

    private bool canMove = true;

    //Bunch action
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    public void Update()
    {
        HandleMovement();
        HandleAnimation();
        HandleFacingDirection();

        HandleBunching();
        CheckIfCanMove();
    }

    public void HandleMovement()
    {
        movePos.x = Input.GetAxisRaw(TagManager.HORIZONTAL_AXIS);
        movePos.y = Input.GetAxisRaw(TagManager.VERTICAL_AXIS);

        if (!canMove)
            return;

        rb.MovePosition(transform.position + movePos * moveSpeed * Time.deltaTime);
    }

    void HandleAnimation()
    {
        if (!canMove)
            return;

        if (Mathf.Abs(movePos.x) > 0 && Mathf.Abs(movePos.y) == 0)
            animator.Play(TagManager.RUN_ANIMATION_NAME);
        if(movePos.x == 0 && movePos.y == 0)
            animator.Play(TagManager.IDLE_ANIMATION_NAME);
        if (movePos.x == 0 && movePos.y > 0)
            animator.Play(TagManager.UP_ANIMATION_NAME);
        if (movePos.x == 0 && movePos.y < 0)
            animator.Play(TagManager.DOWN_ANIMATION_NAME);
    }

    void HandleFacingDirection()
    {
        Debug.Log(movePos.x);
        
        if (movePos.x >= 0)
            transform.localScale = Vector3.one;
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void StopMovement()
    {
        canMove = false;
        waitBeforeMoving = Time.time + moveWaitTime;
    }

    void CheckIfCanMove()
    {
        if (Time.time > waitBeforeMoving)
            canMove = true;
    }

    void Bunch()
    {
        waitBeforeBunching = Time.time + bunchWaitTime;
        StopMovement();
        animator.Play(TagManager.BUNCH_ANIMATION_NAME);

        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach(Collider2D enemy in hitEnemy)
        {
            Debug.Log("Hit Enemy");
        }
    }

    void HandleBunching()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (Time.time > waitBeforeBunching)
                Bunch();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
