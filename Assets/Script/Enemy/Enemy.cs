using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform playerTarget;

    [SerializeField]
    private float moveSpeed = 2f;
    public float lineOfSite;
    public float fireRate = 0.1f;
    private float nextFireTime;

    //
    [SerializeField]
    public float shootingRange;
    public GameObject bullet;
    public GameObject bulletParent;

    //
    private Vector3 tempScale;

    

    private Animator enemyAnim;


    // Start is called before the first frame update
    void Awake()
    {
        playerTarget = GameObject.FindWithTag(TagManager.PLAYER_TAG).transform;
        enemyAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        SearchForPlayer();
    }

    void SearchForPlayer()
    {
        if (!playerTarget)
            return;

        float distanceFromPlayer = Vector2.Distance(this.transform.position, playerTarget.position);
        if (distanceFromPlayer < lineOfSite && distanceFromPlayer > shootingRange)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, playerTarget.position, moveSpeed * Time.deltaTime);

            enemyAnim.Play(TagManager.ENEMY_ATTACK_ANIMATION);
            HandleFacingDirecton();
        }
        else if (distanceFromPlayer <= shootingRange && nextFireTime < Time.time)
        {
            Instantiate(bullet, bulletParent.transform.position, Quaternion.identity);
            nextFireTime = Time.time + fireRate;
        }
        else
            enemyAnim.Play(TagManager.ENEMY_IDLE_ANIMATION);
    }

    void HandleFacingDirecton()
    {
        tempScale = transform.localScale;

        if (transform.position.x > playerTarget.position.x)
            tempScale.x = Mathf.Abs(tempScale.x);
        else
            tempScale.x = -Mathf.Abs(tempScale.x);

        transform.localScale = tempScale;
    }

    private void OnDrawGizmosSeleted()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}
