using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtControls : MonoBehaviour
{
    public Sprite[] spriteArray;
    public Sprite[] MuzzleFlare;
    public AudioClip shootSound;
    public AudioClip[] doubleJumpSounds;
    public AudioClip[] killLandSounds;
    public AudioClip landSound;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    public GameController GameController;
   // public boolean jumped = false;
   float jumpForce = 16f;
   public bool canJump = true;

    public float speed = 5f;
    public float leftBoundary;
    public float rightBoundary;


    public float cricketWind = 0.4f;
    public float cricketAttack = 0.4f;
    private SpriteRenderer[] clickedSprites = new SpriteRenderer[10]; // For gun to find which sprite to shoot

    public Level Level;
    public ShopWhistle ShopWhistle;

    private int currentSpriteIndex = 0; // index of the current sprite in spriteArray
    private float timer = 0f; // timer to keep track of the time since the last sprite change
    public float spriteChangeInterval = 0.4f; // the time interval between sprite changes
    private float baseChangeInterval;

    public bool shouldWalk = true;
    public static bool falling = false;
    public int justDied = 0;

    public LayerMask Ground; // layer mask to specify the ground layer
    public float groundCheckDistance = 0.1f; // distance to check for the ground

    private Collider2D coll; // reference to the sprite's collider
    private bool isGrounded; // flag to store if the sprite is on the ground

    private int prevLevel;
    private float baseX;


//   bool falling = false;

   public float baseY;

    // Start is called before the first frame update
    public void Start()
    {

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();    
        spriteRenderer.sortingOrder = -2; 
        
        //For jumping action
        canJump = true;
        
        leftBoundary = transform.position.x;

        prevLevel = 0;
        baseChangeInterval = spriteChangeInterval;
        baseX = transform.position.x;


        coll = GetComponent<Collider2D>();
        
        rb = gameObject.GetComponent<Rigidbody2D>();

        baseY = spriteRenderer.transform.position.y;

        leftBoundary = spriteRenderer.transform.position.x;  
        rightBoundary = spriteRenderer.transform.position.x;
    }

    public void onBegin(){
        spriteRenderer.sortingOrder = 23;
        jump();
    }

    // Update is called once per frame
       void Update()
    {               
         
         
        transform.position = new Vector2(baseX+Mathf.Max(0,(Level.level-GameController.minMovementLevel))/10f,transform.position.y);

        if (GameController.isDead == false){
            isGrounded = Physics2D.OverlapBox(coll.bounds.center, coll.bounds.size, 0, Ground) != null;

            // Increase position based on level
            if (prevLevel != Level.level && Level.level > GameController.minMovementLevel){
                spriteChangeInterval = baseChangeInterval-0.003f*(GameController.minMovementLevel-10);
                prevLevel = Level.level;
            }
         

            timer += Time.deltaTime;

            if (transform.position.y < baseY){
                transform.position = new Vector2(transform.position.x,baseY);
            }

            if (Input.GetMouseButtonDown(1) && shouldWalk && Level.level > 0){
                canJump = false;
                checkClickSide();
                StartCoroutine(cricketBat());
            }
          else if(Input.GetMouseButtonDown(0) && shouldWalk && Level.level > 1){
                if (ShopWhistle.getCurrentSprite().sprite != ShopWhistle.defaultSprite){
                    return;
                }
                OnMouseDown();
            }

            // if the timer has reached the sprite change interval, change the sprite and reset the timer
            if (timer >= spriteChangeInterval && shouldWalk)
            {
                currentSpriteIndex = (currentSpriteIndex+1)%2; // increment the sprite index and wrap around to the beginning if necessary
                changeSprite(currentSpriteIndex);
                timer = 0f; // reset the timer
            }

            if (Input.GetButtonDown("Jump") && canJump){
                canJump = false;
                shouldWalk = false;
                jump();
            }
            else{
                 float horizontal = Input.GetAxis("Horizontal");
            }   
        }
        else if (GameController.isDead == true && justDied == 0){
                justDied++;
        }
        else if (justDied == 1){
            StartCoroutine(die());
        }
        // To reset it after player dies
        else{
            spriteChangeInterval = baseChangeInterval;
            prevLevel = 0;
        }

    }

    public void jump(){
        if (transform.position.y > baseY+0.1f){
            AudioSource.PlayClipAtPoint(doubleJumpSounds[Random.Range(0,3)], transform.position);
        }
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        spriteRenderer.sprite = spriteArray[2];
        StartCoroutine(land());
    }
    
    private IEnumerator land(){
        

        
        if (GameController.isDead){yield break;}
        yield return new WaitForSeconds(0.4f);
        int prevLandLevel = Level.level;
        if (GameController.isDead){yield break;}
        falling = true;
        rb.velocity = new Vector2(rb.velocity.x, -jumpForce);
        while (transform.position.y != baseY && !GameController.isDead){
            if (rb.velocity == new Vector2 (rb.velocity.x, jumpForce)){
                yield break;
            }
            yield return new WaitForSeconds(0.01f);
         
        }
        rb.velocity = new Vector2(rb.velocity.x, 0);
        if (prevLandLevel < Level.level){
            if (GameController.isDead){yield break;} //Code does not check it fast enough if it were 1 line above the if-else
            AudioSource.PlayClipAtPoint(killLandSounds[Random.Range(0,3)], transform.position);
        }
        else{
            if (GameController.isDead){yield break;}
            AudioSource.PlayClipAtPoint(landSound, transform.position);
        }
        changeSprite(0);
        transform.position = new Vector2(transform.position.x,baseY); //so its exact numbers
        falling = false;
        shouldWalk = true;
        canJump = true;
    //    falling = true;
    }

     private IEnumerator landAnim(){

        yield return new WaitUntil(() => transform.position.y == baseY);
        changeSprite(0);
    }

    private IEnumerator die(){
        if (Level.level < GameController.maxLevel){
            justDied++;
            spriteRenderer.sprite = spriteArray[3];
            rb.velocity = new Vector2(rb.velocity.x, 0);
            transform.position = new Vector2(transform.position.x, Mathf.Max(0.5f,transform.position.y));
            falling = false;
            yield return new WaitForSeconds(0.4f);
            rb.velocity = new Vector2(rb.velocity.x, -1f);
            yield return new WaitUntil(() => (transform.position.y <= baseY-5f) || !GameController.isDead);
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        
        yield return new WaitUntil(() => !GameController.isDead);
        changeSprite(2);
        resetNormal();
        transform.position = new Vector2(transform.position.x, baseY);
        yield return new WaitForSeconds(0.03f);
        jump();
        
    }

        private IEnumerator cricketBat(){

        changeSprite(4);
        yield return new WaitForSeconds(cricketWind);
        changeSprite(5);
        ButtControls.falling = true;
        yield return new WaitForSeconds(cricketAttack+0.01f);
        if (transform.position.y > baseY+0.1f){yield break;}
        changeSprite(0);
        resetNormal();


    }

    private IEnumerator shoot(){
        shouldWalk = false;
        int k = 0;
        if (spriteRenderer.sprite == spriteArray[1]){
            k+=2;
        }
        for(int i = 0; i<2; i++){

            changeSpriteMuzzle(k);
            yield return new WaitForSeconds(0.08f);
            k++;
        }
        
        k=6;

        if(spriteRenderer.sprite == MuzzleFlare[3]){
            k++;
        }
        
        changeSprite(k);
        yield return new WaitForSeconds(0.3f);
        k=0;
        if(k == 7){
            k++;
        }
        changeSprite(k);

        resetNormal();
        }

    private void OnMouseDown()
    {

        Collider2D[] colliders = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        int k = 0;

        // Loop through all colliders that were clicked on
        foreach (Collider2D collider in colliders)
        {
            SpriteRenderer spriteRenderer = collider.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                // Add sprite to array
                clickedSprites[k] = spriteRenderer;
                k++;
            }
        }

        // Find sprite with highest sorting order
        SpriteRenderer topSprite = null;
        int highestSortingOrder = int.MinValue;

        foreach (SpriteRenderer sprite in clickedSprites)
        {
            if (sprite != null && sprite.sortingOrder > highestSortingOrder && sprite.sortingOrder >= 5 && sprite.sortingOrder < 20 && !sprite.GetComponent<Attributes>().isDead)
            {
                topSprite = sprite;
                highestSortingOrder = sprite.sortingOrder;
            }
        }

        clearSprites();

        if (topSprite){
            AudioSource.PlayClipAtPoint(shootSound, transform.position);
            checkClickSide();
            StartCoroutine(shoot());
            topSprite.GetComponent<Attributes>().damage();
            Level.level-=2;        
        }
    }

    private void clearSprites(){
        for(int i = 0; i < 10; i++){
            clickedSprites[i] = null;
        }
    }

    private void changeSprite(int arrayNum){
        if(GameController.isDead || transform.position.y > baseY+0.1f){return;}
        spriteRenderer.sprite = spriteArray[arrayNum];
    }

    private void changeSpriteMuzzle(int arrayNum){
        if(GameController.isDead || transform.position.y > baseY){return;}
        spriteRenderer.sprite = MuzzleFlare[arrayNum];
    }

    public bool checkClickSide(){
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float playerX = transform.position.x;
            bool isLeftSide = clickPos.x < playerX;

            if (isLeftSide)
            {
                spriteRenderer.flipX = true;
                return true;
            }
            else
            {
                return false;
            }
    }

    private void resetNormal(){
        spriteRenderer.flipX = false;
        ButtControls.falling = false;
        shouldWalk = true;
        canJump = true;
    }


}
