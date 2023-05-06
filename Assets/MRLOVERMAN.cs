using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRLOVERMAN : MonoBehaviour
{
    public Sprite[] spriteArray;
    private SpriteRenderer spriteRenderer;
    public int fadeInTime;

    public AudioSource mrNoise;
    public int levelToSpawn;

    private bool canRespawn;
    public MRLOVERMAN mainMr;

    private float currentSpeed;

    public GameObject Butterbean;

    public GameController gameController;
    public ShopWhistle ShopWhistle;
    public ButtControls ButtControls;
    public Level Level;
    

    void Start () {
        // Get the sprite renderer component
        canRespawn = true;
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the sprite's alpha value to 0
        Color spriteColor = spriteRenderer.color;
        spriteColor.a = 0f;
        spriteRenderer.color = spriteColor;
        // Start the fading in coroutine
        mrNoise.Stop();
    }

    public void reset(){
         Color spriteColor = spriteRenderer.color;
        spriteColor.a = 0f;
        spriteRenderer.color = spriteColor;
        canRespawn = true;
        spriteRenderer.sortingOrder = -2;
        mrNoise.Stop();
    }

    void Update()
    {
        if (GameController.isDead == false && Level.level >= levelToSpawn && canRespawn){
            StartCoroutine(FadeInCoroutine());
            mrNoise.Play();
        }
    }

    
    public IEnumerator FadeInCoroutine () {
        // Loop while the alpha value is less than 1
        canRespawn = false;
        spriteRenderer.sortingOrder = 22;
        StartCoroutine(waitForSanity());
        while (spriteRenderer.color.a < 1.0f && !GameController.isDead && !ShopWhistle.whistled) {
            // Increase the alpha value over time
            Color spriteColor = spriteRenderer.color;
            spriteColor.a += Time.deltaTime / fadeInTime;
            spriteRenderer.color = spriteColor;

            // Wait for the next frame
            yield return null;
        }
    }

    public IEnumerator respawn(){
        if (mainMr == null){
            yield return new WaitForSeconds(Random.Range(5,15));
        }
        else{
            yield return new WaitUntil(() => mainMr.canRespawn);
        }
        canRespawn = true;
    }

    public IEnumerator waitForSanity(){
        yield return new WaitUntil(() => ShopWhistle.whistled);
        Level.level++;
        mrNoise.Stop();
        Color spriteColor = spriteRenderer.color;
        spriteColor.a = 0f;
        spriteRenderer.color = spriteColor;
        StartCoroutine(respawn());

    }

}