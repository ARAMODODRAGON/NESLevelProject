using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paragoomba : Enemies {

    public static List<Microgoomba> allMG = new List<Microgoomba>();

    [Header("Wings")]
    public AnimatedSprite leftWing;
    public AnimatedSprite rightWing;

    [Header("Movement")]
    public float blocksAboveMario;
    public float blocksAwayFromMario;
    public float groundTime;
    public float airTime;
    public float Xvel;
    public float flyingXvel;
    public float riseSpeed;
    public float fallingSpeed;
    public float fallingAccel;

    [Header("MicroGoombas")]
    public GameObject Microgoomba;
    [Min(1)] public int numberOfMicroGoombas;
    public int numbertoSpawn;

    [Header("Goomba")]
    public GameObject Goomba;

    //private variables
    private AnimatedSprite ass; //because I cant use 'as'
    private float timer;
    private enum MovStates { Walking, Flying, Hovering };
    private MovStates curState;
    private Vector2 distToMario;
    private Vector2 vel;
    private int numOfSpawned;

    protected override void Awake() {
        base.Awake();
        timer = 0f;
        numOfSpawned = 0;
        curState = MovStates.Walking;
    }

    protected override void Start() {
        base.Start();
        GetDistToMario();
        ass = GetComponent<AnimatedSprite>();
    }

    private void GetDistToMario() {
        distToMario = player.Center - transform.position;
    }

    protected override void ActiveUpdate() {
        base.ActiveUpdate();
        if (destroyOnExit) return;
        GetDistToMario();

        if (isFacingRight && distToMario.x < -blocksAwayFromMario) Flip();
        if (!isFacingRight && distToMario.x > blocksAwayFromMario) Flip();

        if (curState == MovStates.Walking) {
            if (ec.IsGrounded) timer += Time.fixedDeltaTime;
            Walking();
        } else if (curState == MovStates.Flying) {
            Flying();
        } else if (curState == MovStates.Hovering) {
            timer += Time.fixedDeltaTime;
            Hovering();
        }

        rb.velocity = vel;
    }

    public override void TakeDamage(string killType) {
        if (killType.Equals("fire")) {
            hitPoints = 0;
            ass.Stop();
            leftWing.Stop();
            rightWing.Stop();
            col.enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            if (isFacingRight) rb.velocity = new Vector2(1f, 10f) * Xvel;
            else rb.velocity = new Vector2(-1f, 10f) * Xvel;
            transform.localScale = new Vector2(transform.localScale.x, -transform.localScale.y);
            destroyOnExit = true;
            rb.gravityScale = 7;
        } else if (killType.Equals("stomp")) {
            Goomba g = Instantiate(Goomba, transform.position, Quaternion.identity).GetComponent<Goomba>();
            g.StartCoroutine(g.StartFromPara(isFacingRight));
            Destroy(gameObject);
        }
    }

    #region movement

    protected void Walking() {
        if (isFacingRight) {
            vel.x = Xvel;
        } else {
            vel.x = -Xvel;
        }
        vel.y -= fallingAccel * Time.fixedDeltaTime;
        if (ec.IsGrounded) vel.y = 0f;
        else if (vel.y < -fallingSpeed) vel.y = -fallingSpeed;

        if (timer >= groundTime) {
            curState = MovStates.Flying;
            timer = 0f;
        }
    }

    protected void Flying() {
        if (isFacingRight) {
            vel.x = flyingXvel;
        } else {
            vel.x = -flyingXvel;
        }
        vel.y = riseSpeed;

        if (distToMario.y < -blocksAboveMario || (ec.IsCeiling && ec.IsColliding)) {
            curState = MovStates.Hovering;
            timer = 0f;
            vel.y = 0f;
        }
    }

    protected void Hovering() {
        if (isFacingRight) {
            vel.x = flyingXvel;
        } else {
            vel.x = -flyingXvel;
        }

        if (timer > (airTime / numbertoSpawn) * (numOfSpawned + 1)) {
            int numOfFallingMG = 0;
            foreach (var item in allMG) {
                if (!item.isAttached) numOfFallingMG++;
                if (numOfFallingMG >= numbertoSpawn) break;
            }
            if (numOfFallingMG < numOfSpawned + 1) {
                Instantiate(Microgoomba, transform.position, transform.rotation);
            }
            numOfSpawned++;
        }

        if (timer >= airTime) {
            curState = MovStates.Walking;
            timer = 0f;
            vel.y = 0f;
            numOfSpawned = 0;
        }
    }
    #endregion

}
