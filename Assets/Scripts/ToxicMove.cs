using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicMove : MonoBehaviour
{
    public Sprite[] spriteArray;
    private SpriteRenderer spriteRenderer;


    public int enemyNum; //For use of tracking enemy in Enemies gameobject

    public Attributes Attributes;

    public GameController gameController;
    public ButtControls ButtControls;
    public Level Level;
    
    void Start()
    {

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();     

        StartCoroutine(Attributes.spawnEnemy());
    }

    void Update()
    {
        if (!GameController.isDead){
                if (Attributes.moveLogic()){
                    transform.Translate(Vector2.left * Attributes.walkSpeed * Time.deltaTime);
                }

                if (transform.position.x <= 6f)
                {
                    Attributes.respawn();
                }
            }
    }

   private void OnTriggerEnter2D(Collider2D other)
    {
    if (spriteRenderer.sprite == spriteArray[0] && other.transform.name == "Butterbean"){
        if (ButtControls.falling && !Attributes.isDead){
            ButtControls.canJump = true;
            StartCoroutine(Attributes.die());
            }
        else if (spriteRenderer.sprite == spriteArray[0] && other.transform.name == "Butterbean"){
        GameController.isDead = true;
        Debug.Log(gameObject.name);
        }
    }
        // Perform actions when trigger entered
    
    }

}