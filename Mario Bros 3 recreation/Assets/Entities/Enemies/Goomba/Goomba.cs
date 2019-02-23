using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : Enemies {

    private AnimatedSprite ass; //because I cant use 'as'
    public Sprite DeadGoomb;
    public float timeToDeath;
    public float speed;

    protected override void Start() {
        base.Start();

        if (Player.instance.transform.position.x <= transform.position.x) {
            isFacingRight = false;
        } else {
            isFacingRight = true;
        }
        ass = GetComponent<AnimatedSprite>();
    }

    // this is only called when instantiated by the paragoomba
    public IEnumerator StartFromPara(bool faceright) {
        ec.CheckMain = false;
        yield return new WaitForFixedUpdate();
        ec.CheckMain = true;
        isFacingRight = faceright;
    }

    protected override void OnActivate() {
        base.OnActivate();
        if (player.transform.position.x <= transform.position.x) {
            isFacingRight = false;
        } else {
            isFacingRight = true;
        }
    }

    protected override void ActiveUpdate() {
        if (isFacingRight) {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        } else {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
    }

    public override void TakeDamage(string killType) {
        if (killType.Equals("stomp")) {
            ass.Stop();
            rb.simulated = false;
            sp.sprite = DeadGoomb;
            hitPoints = 0;
            Destroy(gameObject, timeToDeath);
        } else if (killType.Equals("fire")) {
            hitPoints = 0;
            ass.Stop();
            GetComponent<BoxCollider2D>().enabled = false;
            if (isFacingRight) rb.velocity = new Vector2(1f, 10f) * speed;
            else rb.velocity = new Vector2(-1f, 10f) * speed;
            transform.localScale = new Vector2(transform.localScale.x, -transform.localScale.y);
            destroyOnExit = true;
        } else {
            base.TakeDamage(killType);
        }
    }
}
