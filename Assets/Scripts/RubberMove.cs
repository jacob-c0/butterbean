using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubberMove : MonoBehaviour
{
    public Sprite[] spriteArray;
    private SpriteRenderer spriteRenderer;

    public int enemyNum; //For use of tracking enemy in Enemies gameobject

    public Attributes Attributes;


    public float walkSpeed;

    public GameController gameController;
    public ButtControls ButtControls;
    public Level Level;


    void Start()
    {

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();     
    }

    public void onBegin(){
        StartCoroutine(Attributes.spawnEnemy());
    }

    void Update()
    {
    if (GameController.isDead == false){
            if (Attributes.canMove){
                transform.Translate(Vector2.left * walkSpeed * Time.deltaTime);
            }

            if (transform.position.x <= 6f)
            {
                    Attributes.respawn();
            }
        }
    }

   private void OnTriggerEnter2D(Collider2D other)
    {
        if (ButtControls.falling && other.transform.name == "Butterbean"){
            if (!Attributes.isDead){
            StartCoroutine(Attributes.die());
            ButtControls.canJump = true;
            }
            }
        else if (spriteRenderer.sprite == spriteArray[0] && other.transform.name == "Butterbean") {
        GameController.isDead = true;
        Debug.Log(gameObject.name);
        }
        // Perform actions when trigger entered
    
    }

}