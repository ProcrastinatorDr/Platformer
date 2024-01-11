using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MushroomStates
{
    idle,
    run
}

public class Mushroom : ObjectEntity
{
    [SerializeField] float speed = 3.0f;
    [SerializeField] int lives = 3;
    private Vector3 dir;
    Animator anim;
    SpriteRenderer sprite;

    private void Start()
    {
        dir = transform.right;
    }
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        //rigidbody = GetComponent<Rigidbody2D>();
    }
    private HeroStates State
    {
        get { return (HeroStates)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage();
            lives--;
            Debug.Log("У гриба " + lives);
        }
        if (lives < 1)
        {
            Die();
        }
    }
    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.1f + transform.right * dir.x * 0.7f, 0.1f);

        if(colliders.Length > 0)
        {
            dir*=-1f;
        }
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, Time.deltaTime);
    }
    private void Run()
    {
        State = HeroStates.run;
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        sprite.flipX = dir.x > 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        State = HeroStates.idle;
        Move();
        //if (Input.GetButton("Horizontal"))
        //    Run();
    }
}
