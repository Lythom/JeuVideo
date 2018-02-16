using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct Animation {
    public string name;

    public Sprite[] sprites;
}
public class AnimationCourse : MonoBehaviour {

    public Animation[] anims;
    public int imagesParSecondes = 24;

    private SpriteRenderer spriteRenderer;

    private int currentSpriteIdx = 0;
    private int currentAnim = 0;
    private float accumulateur = 0;

    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer> ();
    }

    public void SetSpeed (float v) {
        this.spriteRenderer.flipX = v < 0 || this.spriteRenderer.flipX && v == 0;
        if (Mathf.Abs (v) > 0 && currentAnim != GetAnimIndex ("Run")) {
            ChangeAnimation ("Run");
        }
        if (Mathf.Abs (v) == 0 && currentAnim != GetAnimIndex ("Iddle")) {
            ChangeAnimation ("Iddle");
        }
    }

    void ChangeAnimation (string anim) {
        this.currentAnim = GetAnimIndex (anim);
        this.currentSpriteIdx = 0;
    }

    // Update is called once per frame
    void Update () {
        // durée souhaitée d'une frame
        float frameDuration = GetFrameDurationInSec ();

        // durée accumulée depuis le dernier changement de frame
        accumulateur += Time.deltaTime;

        // vide l'accumulateur et fait avancer les frames        
        if (accumulateur > frameDuration) {
            NextFrame ();
            accumulateur -= frameDuration;
        }

    }

    int GetAnimIndex (string animName) {
        return System.Array
            .FindIndex (anims, anim => {
                return anim.name == animName;
            });
    }

    float GetFrameDurationInSec () {
        return 1f / imagesParSecondes;
    }

    public void NextFrame () {
        currentSpriteIdx = (currentSpriteIdx + 1) % anims[currentAnim].sprites.Length;
        spriteRenderer.sprite = anims[currentAnim].sprites[currentSpriteIdx];
    }
}