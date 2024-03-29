using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroStates
{
    idle,
    run,
    jump
}
public class Hero : ObjectEntity
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private int lives = 5;
    [SerializeField] private float jumpForce = 2.3f;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;
    private bool isGrounded = false;
    public static Hero Instance { get; set; }
    public override void GetDamage()
    {
        lives--;
        Debug.Log(lives);
        if (lives < 1)
        {
            Die();
        }
    }
    
    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    private HeroStates State
    {
        get { return (HeroStates)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }
    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
            State = HeroStates.idle;
        if (Input.GetButton("Horizontal"))
            Run();
        if (isGrounded && Input.GetButton("Jump"))
            Jump();
    }
    private void FixedUpdate()
    {
        CheckGround();
    }
    private void Run()
    {
        if (isGrounded)
            State = HeroStates.run;
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        sprite.flipX = dir.x < 0.0f;
    }
    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = collider.Length > 1;
        if (!isGrounded)
            State = HeroStates.jump;
    }
    //// Start is called before the first frame update
    //void Start()
    //{

    //}
}
