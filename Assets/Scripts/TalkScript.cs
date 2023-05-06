using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkScript : MonoBehaviour
{
    private Sprite[] spriteArray;
    private SpriteRenderer spriteRenderer;
    public Level Level;
    public GameController GameController;
    int sprite = 0;
    float alternator = -0.2f;

    private float spriteAlternator = 0.2f;
    float baseAlternator;

    private float prevLevel;

    // Start is called before the first frame update
    void Start()
    {
        spriteArray = Resources.LoadAll<Sprite>("Talk");
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = -2;
        StartCoroutine(animate());
        StartCoroutine(fixAnimation());
        prevLevel = 0;
        baseAlternator = spriteAlternator;
        
    }

    public void onBegin(){
        spriteRenderer.sortingOrder = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if (prevLevel != Level.level && Level.level > GameController.minMovementLevel){
            spriteAlternator = baseAlternator-0.002f*(Level.level - GameController.minMovementLevel);
            prevLevel = Level.level;
        }
        else if (GameController.isDead){
            spriteAlternator = baseAlternator;
        }


        
    }

    private IEnumerator fixAnimation(){
        while (true){
                yield return new WaitForSeconds(spriteAlternator);
                if (GameController.isDead == false){
                transform.position  = new Vector2(transform.position.x, transform.position.y+alternator);
                if (alternator < 0){
                    alternator = Mathf.Abs(alternator);
                }else
                {
                    alternator = -(alternator);
                }
            }
        }
    }
    private IEnumerator animate(){

        while (true) {
                yield return new WaitForSeconds(spriteAlternator);
                if (GameController.isDead == false){
                spriteRenderer.sprite = spriteArray[sprite];
                sprite = (sprite + 1) % 2;
            }
        }
    }
}