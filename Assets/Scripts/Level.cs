using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Sprite[] levelNumbers;
    public GameObject[] levelObjects;
    public static int level = 0;

    // Start is called before the first frame update
    void Start()
    {
        levelObjects[1].GetComponent<SpriteRenderer>().enabled = false;
        levelObjects[0].GetComponent<SpriteRenderer>().enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(level>=100){
            GameController.isDead = true;
            return;
        }
        if (level>0){
        levelObjects[0].GetComponent<SpriteRenderer>().enabled = true;
        levelObjects[0].GetComponent<SpriteRenderer>().sprite = levelNumbers[level%10];
        }
        else{
            levelObjects[0].GetComponent<SpriteRenderer>().enabled = false;
        }
        if (level>9){
            levelObjects[1].GetComponent<SpriteRenderer>().enabled = true;
            levelObjects[1].GetComponent<SpriteRenderer>().sprite = levelNumbers[(int)(Mathf.Floor(level/10.0f))];
        }
        else{
            levelObjects[1].GetComponent<SpriteRenderer>().enabled = false;
        }
        
    }

}
