using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
    
{
    public ButtControls ButtControls;
    public GameObject Butterbean;
    public GameController gameController;

    public AudioClip whiff;
    public AudioClip strike;
    public AudioClip thud;
    private float prevLevel;

    private float hitPosition;

    public bool batting = false;

    private float xDistance;
    private float permXDistance;
    private float butterWidth;

// /            AudioSource.PlayClipAtPoint(doubleJumpSounds[Random.Range(0,3)], transform.position);

    // Start is called before the first frame update
    void Start()
    {
        xDistance = Mathf.Abs(transform.position.x - Butterbean.transform.position.x);
        permXDistance = xDistance;

        butterWidth = Butterbean.GetComponent<SpriteRenderer>().bounds.size.x;

        setPosDown();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.isDead == false){



            if (Input.GetMouseButtonDown(1)  && ButtControls.shouldWalk && Level.level > 0){
                
                ButtControls.shouldWalk = false;
                Level.level--;


                if (!ButtControls.checkClickSide()){
                    transform.position = new Vector2(Butterbean.transform.position.x + xDistance,transform.position.y);
                }
                else{
                    transform.position = new Vector2(transform.position.x - 2*xDistance,transform.position.y);
                }

                StartCoroutine(hit());   

            }

        
        }
    }

    void setPosDown(){
        transform.position = new Vector2(transform.position.x, transform.position.y - 100f);
    }

      private IEnumerator hit(){

        prevLevel = Level.level;
        yield return new WaitForSeconds(ButtControls.cricketWind);
        batting = true;
        transform.position = new Vector2(transform.position.x,transform.position.y+100f);
        StartCoroutine(checkHit());
        yield return new WaitForSeconds(ButtControls.cricketAttack);
        batting = false;
        setPosDown();
    }

    private IEnumerator checkHit(){
        yield return new WaitForSeconds(0.03f);
        if (prevLevel != Level.level && !GameController.isDead){
            AudioSource.PlayClipAtPoint(strike, transform.position);
            prevLevel = Level.level;
        }
        else if (!GameController.isDead){
            AudioSource.PlayClipAtPoint(whiff, transform.position);
        }
        while (batting){
            yield return new WaitForSeconds(0.01f);
            if (prevLevel != Level.level){
                if (!GameController.isDead){
                    AudioSource.PlayClipAtPoint(strike, transform.position);
                    prevLevel = Level.level;
                }
            }
        }
        
    }

}
