using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWhistle : MonoBehaviour
{
    public AudioClip clickSound;
    public Sprite defaultSprite;
    public Sprite hoverSprite;
    private SpriteRenderer spriteRenderer;

 //   private BoxCollider2D collider;
    private float topEdge;
    private float bottomEdge;
    private float leftEdge;
    private float rightEdge;

    public bool whistled;
    public ButtControls ButtControls;
    public GameController GameController;
    public EnemyTracker EnemyTracker;


    public BoxCollider2D collider;

    public void OnMouseDown()
    {
        if (!GameController.isDead && spriteRenderer.sortingOrder == 20)
        {
            AudioSource.PlayClipAtPoint(clickSound, transform.position);
            whistled = true;
            Debug.Log(EnemyTracker.findLowestSpawn());
            EnemyTracker.findLowestSpawn().spawn();
        }
    }

    void OnMouseEnter()
    {
        // Change the sprite when the mouse enters the object
        if (!GameController.isDead){
            spriteRenderer.sprite = hoverSprite;
        }
    }

    void OnMouseExit()
    {
        // Change the sprite back to the original when the mouse exits the object
        // Note: You may want to store the original sprite in a variable to prevent any issues
        spriteRenderer.sprite = defaultSprite;
        whistled = false;
    }

    bool onMouseOver(){
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x >= leftEdge && mousePos.x <= rightEdge && mousePos.y >= bottomEdge && mousePos.y <= topEdge) { // Bounds of shop whistle. No idea how to get them to correlate to shop whistle collider variables
           return true;
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {

    whistled = false;
    collider = GetComponent<BoxCollider2D>();
    topEdge = transform.position.y + collider.offset.y + collider.size.y / 2f;
    bottomEdge = transform.position.y + collider.offset.y - collider.size.y / 2f;
    leftEdge = transform.position.x + collider.offset.x -  collider.size.x / 2f;
    rightEdge = transform.position.x + collider.offset.x + collider.size.x / 2f;
      spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = defaultSprite;
        spriteRenderer.sortingOrder = -2;    
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.isDead){
            spriteRenderer.sprite = defaultSprite;
        }
        if (onMouseOver()){
            OnMouseEnter();
        }
        else if (spriteRenderer.sprite != defaultSprite){
            OnMouseExit();
        }
        
    }

    void Upgrade_butt(int tier){
        if(tier < 4){
            ButtControls.rightBoundary += 2.5f;
        }
        else if (tier  == 4){

        }
    }

    public void onBegin(){
        spriteRenderer.sortingOrder = 20;
    }

    public SpriteRenderer getCurrentSprite(){
        return spriteRenderer;
    }
}
