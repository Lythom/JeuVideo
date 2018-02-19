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

    // liste des animations configurables
    public Animation[] anims;

    // nombre d'images par seconde à laquelle l'animation doit être jouée
    public int imagesParSecondes = 12;

    // références interne
    private SpriteRenderer spriteRenderer;

    // état privé
    // indice de la frame actuellement affichée
    private int currentSpriteIdx = 0;
    // indice de l'animation actuellement affichée    
    private int currentAnim = 0;
    // accumulateur pour mesurer le temps cumulé qui passe
    private float accumulateur = 0;

    // Use this for initialization
    void Start () {
        // Récupère une référence au script AnimationCourse attaché au même GameObject
        spriteRenderer = GetComponent<SpriteRenderer> ();
    }

    public void SetAnimationFromSpeed (float vitesse) {
        // Si la vitesse est négative, on va vers la gauche et le sprite devrait être inversé
        // Si la vitesse est positive, on va vers la droite et le sprite ne devrait pas être inversé
        // Si on allait précédemment vers la gauche (flipX est vrai) et que la vitesse tombe à 0, on reste vers la gauche
        this.spriteRenderer.flipX = vitesse < 0 || this.spriteRenderer.flipX && vitesse == 0;
        // Si le personnage se déplace et qu'il n'est pas déjà en train de courir, le faire courir
        if (vitesse != 0 && currentAnim != GetAnimIndex ("Run")) {
            ChangeAnimation ("Run");
        }
        // Si le personnage est arrêté et qu'il n'est pas déjà en animation d'attente, déclencher l'animation d'attente.       
        if (vitesse == 0 && currentAnim != GetAnimIndex ("Iddle")) {
            ChangeAnimation ("Iddle");
        }
    }

    public void ChangeAnimation (string anim) {
        this.currentAnim = GetAnimIndex (anim);
        // Lors d'une nouvelle animation, repartir à la première image de cette animation
        this.currentSpriteIdx = 0;
    }

    // Update is called once per frame
    void Update () {
        // durée souhaitée d'une frame
        float frameDuration = GetFrameDurationInSec ();

        // durée accumulée depuis le dernier changement de frame
        accumulateur += Time.deltaTime;

        // vide l'accumulateur et fait avancer les frames        
        while (accumulateur > frameDuration && frameDuration > 0) {
            NextFrame ();
            accumulateur -= frameDuration;
        }
    }

    private int GetAnimIndex (string animName) {
        // cherche dans un tableau par condition sur le nom de l'animation
        return System.Array.FindIndex (anims, anim => {
                return anim.name == animName;
            });
    }

    private float GetFrameDurationInSec () {
        // 1 seconde divisée par le nombre de frame à afficher par secondes nous donne le temps
        // qu'il convient d'allouer à chaque frame (en seconde).
        return 1f / imagesParSecondes;
    }

    private void NextFrame () {
        // boucle du début à la fin du tableau, revient au début lorsqu'on dépasse la fin
        // grace à la fonction modulo (%).
        currentSpriteIdx = (currentSpriteIdx + 1) % anims[currentAnim].sprites.Length;
        // Affiche dans le sprite renderer la frame en cours de l'animation en cours.
        spriteRenderer.sprite = anims[currentAnim].sprites[currentSpriteIdx];
    }
}