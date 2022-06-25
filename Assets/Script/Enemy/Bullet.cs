using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject playerTarget;
    public float speed;
    private Rigidbody2D bulletRB;

    // Start is called before the first frame update
    void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();
        FindTarget();

    }

    private void Update()
    {
        //
    }
    private void FindTarget()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player");
        Vector2 moveDir = (playerTarget.transform.position - this.transform.position).normalized * speed;
        bulletRB.velocity = new Vector2(moveDir.x, moveDir.y);
        Destroy(this.gameObject, 2);
    }
}
