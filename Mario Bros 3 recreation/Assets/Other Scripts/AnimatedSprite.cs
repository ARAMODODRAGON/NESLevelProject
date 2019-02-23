using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour {
    private bool play = true;
    public float speed;
    public Sprite[] sprites;

    private SpriteRenderer sp;
    private int i;

    private void Start() {
        sp = GetComponent<SpriteRenderer>();
        i = 0;
        StartCoroutine("RunAnim");
    }

    IEnumerator RunAnim() {
        while (true) {
            yield return new WaitForSeconds(1 / speed);
            if (play) {
            sp.sprite = sprites[i];
            i++;
            if (i == sprites.Length) i = 0;
            } 
        }
    }

    public void Play() {
        play = true;
    }

    public void Stop() {
        play = false;
    }

    public void Stop(int n) {
        Stop();
        SetSprite(n);
    }

    public void SetSprite(int n) {
        if (n < sprites.Length && n >= 0) i = n;
        else Debug.LogError("The sprite could not be found");
        sp.sprite = sprites[i];
    }
}
