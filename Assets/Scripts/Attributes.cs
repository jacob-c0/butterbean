using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attributes : MonoBehaviour
{

    public Sprite[] spriteArray;
    public Sprite[] damagedSprites;
    public AudioClip hitSound;
    private SpriteRenderer spriteRenderer;

    public bool isDead = false;

    public bool commonDeath;
    public int health;
    private int baseHealth;

    public bool canMove = false;

    public EnemyTracker EnemyTracker;
    public GameObject butterbean;
    public ButtControls ButtControls;
    public Bat Bat;

    public static Level Level;

    public float walkSpeed; // Speed of movement
    public int levelIncrease;
    public int levelToSpawn;
    public int minSpawnTime;
    public int maxSpawnTime;
    private bool canSpawn; // to prevent spawning in some circumstances

    public Attributes self;


    public float baseY;
    public float baseX;

    public int enemyNum;
    public int spawnNum;
    public Attributes next; // For linked list

    // Start is called before the first frame update
    void Start()
    {
        canSpawn = true;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();     
        baseY = transform.position.y;
        baseX = transform.position.x;
        baseHealth = health;
        spawnNum = 100000;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator die(){
    

        if (ButtControls.falling && !Bat.batting){
            yield return new WaitForSeconds(0.06f); //some extra leeway for jump
        }

        EnemyTracker.enemies[enemyNum] = null;

        isDead = true;
        changeSprite(1);
        if(GameController.isDead){yield break;}
        Level.level+=levelIncrease;
        transform.position = new Vector2(transform.position.x, transform.position.y - 0.5f);
        yield return new WaitForSeconds(0.125f);
        changeSprite(2);
        if(GameController.isDead){yield break;}
        transform.position = new Vector2(transform.position.x, transform.position.y - 0.25f);

        
    }

    public IEnumerator spawnEnemy()
    {
        yield return new WaitUntil(() => Level.level >= levelToSpawn-2);
        EnemyTracker.toSpawn.Add(self);
        yield return new WaitUntil(() => Level.level >= levelToSpawn);
            canMove = true;
            transform.position = new Vector2(baseX+0.2f,baseY);
            EnemyTracker.spawn(self);

            while (true)
            {
                yield return new WaitUntil(() => transform.position.x == baseX);
                if (!commonDeath){
                    StartCoroutine(delaySpawn(25f));
                }
                yield return new WaitUntil(() => Level.level >= levelToSpawn);
                spawnNum = Random.Range(minSpawnTime,maxSpawnTime);
                while (spawnNum > 0 && !canMove){
                    yield return new WaitForSeconds(1f);
                    spawnNum--;
                }
                yield return new WaitUntil(() => canSpawn);
                if (GameController.isDead){yield return new WaitUntil(()=> !GameController.isDead);}
                spawn();
            }
    }

    public void respawn(){

        EnemyTracker.die(self);
        canMove = false;
        transform.position = new Vector2(baseX, baseY);
    }

    public void spawn(){
        EnemyTracker.spawn(self);
        canMove = true;
        transform.position = new Vector2(transform.position.x,baseY);
        if(commonDeath || (!commonDeath && health <= 0)){
            changeSprite(0);
            isDead = false;
            health = baseHealth;
        }
    }
    
    private void changeSprite(int arrayNum){
        if(GameController.isDead){return;}
        spriteRenderer.sprite = spriteArray[arrayNum];
    }

    public void reset(){

        spriteRenderer.sprite = spriteArray[0];
        canMove = false;
        transform.position = new Vector2(baseX,baseY);
        health = baseHealth;

    }

    public bool moveLogic(){
        if (canMove && Level.level >= levelToSpawn || transform.position.x < baseX){
            return true;
    }
    return false;
    }

    public void damage(){
        health--;
        if (commonDeath && health == 0){
            StartCoroutine(die());
        }
        else if (!commonDeath && health > 0){
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
            spriteRenderer.sprite = damagedSprites[baseHealth-health-1];
        }
    }

    private IEnumerator delaySpawn(float time){
        canSpawn = false;
        yield return new WaitForSeconds(time);
        canSpawn = true;

    }

    public int getBaseHealth(){
        return baseHealth;
    }


}