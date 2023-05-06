using UnityEngine;
using System.Collections;

public class TextFadeIn : MonoBehaviour {
    public float fadeInTime = 1.0f; // The time it takes to fade in
    private SpriteRenderer spriteRenderer; // Reference to the sprite renderer
    public GameController GameController;

    void Start () {
        // Get the sprite renderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the sprite's alpha value to 0
        Color spriteColor = spriteRenderer.color;
        spriteColor.a = 0f;
        spriteRenderer.color = spriteColor;
        // Start the fading in coroutine
    }

    public IEnumerator FadeInCoroutine () {
        // Loop while the alpha value is less than 1
        yield return new WaitForSeconds(2f);
        spriteRenderer.sortingOrder = 23;
        while (spriteRenderer.color.a < 1.0f && GameController.isDead) {
            // Increase the alpha value over time
            Color spriteColor = spriteRenderer.color;
            spriteColor.a += Time.deltaTime / fadeInTime;
            spriteRenderer.color = spriteColor;

            // Wait for the next frame
            yield return null;
        }
    }

    public void textVisible(){
        spriteRenderer.sortingOrder = 23;
        Color spriteColor = spriteRenderer.color;
        spriteColor.a = 1f;
        spriteRenderer.color = spriteColor;
    }

    public void textInvisible(){
        spriteRenderer.sortingOrder = -2;
        Color spriteColor = spriteRenderer.color;
        spriteColor.a = 0f;
        spriteRenderer.color = spriteColor;
        }
}