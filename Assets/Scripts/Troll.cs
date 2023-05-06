using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troll : MonoBehaviour
{
    public Sprite[] spriteArray;
    private SpriteRenderer spriteRenderer;

    public int enemyNum; //For use of tracking enemy in Enemies gameobject

    public Attributes Attributes;

    private float currentSpeed;

    private float pausePosition;
    public GameObject Butterbean;

    public GameController gameController;
    public ButtControls ButtControls;
    public Level Level;
    


    void Start()
    {

        currentSpeed = Attributes.walkSpeed;

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();     

        StartCoroutine(Attributes.spawnEnemy());
    }

    void Update()
    {
        if (GameController.isDead == false){
            pausePosition = Butterbean.transform.position.x + 3.5f;
                if(spriteRenderer.sprite  == spriteArray[0]  && transform.position.x <= pausePosition){
                    Attributes.canMove = false;
                    StartCoroutine(Launch());
                }
                else if ((spriteRenderer.sprite != spriteArray[3] && spriteRenderer.sprite != spriteArray[4]) && Attributes.moveLogic()){
                    transform.Translate(Vector2.left * currentSpeed * Time.deltaTime);
                }

                if (transform.position.x <= 6f)
                {
                    currentSpeed = Attributes.walkSpeed;
                    if (GameController.isDead){return;}
                    transform.position = new Vector2(transform.position.x, transform.position.y-0.5f);
                    Attributes.respawn();
                }
            }
        else{
            currentSpeed = Attributes.walkSpeed;
        }
    }

   private void OnTriggerEnter2D(Collider2D other)
    {
        if (ButtControls.falling && !Attributes.isDead && other.transform.name == "Butterbean"){
            ButtControls.canJump = true;
            StartCoroutine(Attributes.die());
            }
        else if (!Attributes.isDead && other.transform.name == "Butterbean"){
            GameController.isDead = true;
            Debug.Log(gameObject.name);
        }
        // Perform actions when trigger entered


    }

        IEnumerator Launch()
    {
        changeSprite(3);
        yield return new WaitForSeconds(0.5f);
        changeSprite(4);
        yield return new WaitForSeconds(0.5f);
        if(GameController.isDead || Attributes.isDead){yield break;}
        changeSprite(5);
        transform.position = new Vector2(transform.position.x, 0.5f+transform.position.y);
        Attributes.canMove = true;
        currentSpeed +=3f;
    }

      private void changeSprite(int arrayNum){
        if(GameController.isDead || Attributes.isDead){return;}
        spriteRenderer.sprite = spriteArray[arrayNum];
    }
}