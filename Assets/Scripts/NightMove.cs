using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightMove : MonoBehaviour
{
    public Sprite[] spriteArray;
    public Sprite[] damagedSprites;
    private SpriteRenderer spriteRenderer;

    public AudioClip landSound;
    public AudioClip smashSound;

    public int enemyNum; //For use of tracking enemy in Enemies gameobject
    public EnemyTracker EnemyTracker;

    public Attributes Attributes;
    private Rigidbody2D rb;
    private Collider2D col;

    public GameController gameController;
    public ButtControls ButtControls;
    public Level Level;

    private bool fell;

    void Start()
    {

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();     
        rb = gameObject.GetComponent<Rigidbody2D>();
        col = gameObject.GetComponent<BoxCollider2D>();

        StartCoroutine(Attributes.spawnEnemy());
    }

    void Update()
    {
        if (Attributes.health == 0 && !Attributes.isDead){
            Attributes.isDead = true;
            changeSprite(1);
            rb.velocity = new Vector2(rb.velocity.x, -16f);

            StartCoroutine(customDie());
        }

        if (GameController.isDead == false){
                if (Attributes.moveLogic()){
                    transform.Translate(Vector2.left * Attributes.walkSpeed * Time.deltaTime);
                }

                if (transform.position.x <= -2.04f)
                {
                    Attributes.respawn();
                }
        }
    }


   private void OnTriggerEnter2D(Collider2D other)
    {
        if (!Attributes.isDead && other.transform.name == "Butterbean"){
            GameController.isDead = true;
        }

    }

     public IEnumerator customDie(){

       yield return new WaitUntil(() => transform.position.y < ButtControls.baseY);
        killDrop();
        rb.velocity = new Vector2(rb.velocity.x, 0);
        StartCoroutine(Attributes.die());
     }  

     public void killDrop(){
        int prevLevel = Level.level;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, spriteRenderer.bounds.size.x / 2f-0.2f);
        foreach (Collider2D c in colliders) {
            GameObject gameObject = c.gameObject;
            if (gameObject.GetComponent<Attributes>() != null && gameObject.transform.position.y < ButtControls.baseY+4f){
                gameObject.GetComponent<Attributes>().damage();
            }
        }
        if (Level.level > prevLevel){
            AudioSource.PlayClipAtPoint(smashSound,transform.position);
        }
        else{
            AudioSource.PlayClipAtPoint(landSound,transform.position);
        }
     }

    private void changeSprite(int arrayNum){
        if(GameController.isDead){return;}
            spriteRenderer.sprite = Attributes.spriteArray[arrayNum];
    }

    
}            
