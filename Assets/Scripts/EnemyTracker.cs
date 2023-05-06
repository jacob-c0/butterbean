using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    // Start is called before the first frame update

    public Attributes[] enemies;
    private int k;
    public Attributes[] allEnemies = new Attributes[6];
    public List<Attributes> toSpawn = new List<Attributes>();
    public Attributes head;

    void Start()
    {

        k = 0;
        foreach (Attributes num in enemies){
            num.enemyNum = k;
            allEnemies[k] = num;
            k++;


        }

        for(int i = 0; i < k; i++){
            enemies[i] = null;
        }
        k=0; 
        
    }

    // Update is called once per frame
    void Update()
    {
        if (toSpawn.Count>0){
        int max = 100;
        foreach (Attributes Attributes in toSpawn){
            if (Attributes.spawnNum < max){
                max = Attributes.spawnNum;
                head = Attributes;
            }
        }
    }
    }

    public void spawn(Attributes Attributes){
        enemies[Attributes.enemyNum] = Attributes;
        toSpawn.Remove(Attributes);
    }

    public void die(Attributes Attributes){
        enemies[Attributes.enemyNum] = null;
        toSpawn.Add(Attributes);
    }

    public void resetEnemies(){

        foreach(Attributes enemy in allEnemies){
            if (enemy){
                enemy.reset();
                enemies[enemy.enemyNum] = null;
                
                if(enemy.levelToSpawn <= 2){
                    toSpawn.Add(enemy);
                }
            }
        }
    
    }

public Attributes findLowestSpawn(){
    
    if (toSpawn.Count == 0){
        return null;
    }

    int lowestSpawn = toSpawn[0].spawnNum;

    //Go through it once to find the lowest spawn
    foreach (Attributes enemy in toSpawn)
    {
        if (enemy.spawnNum < lowestSpawn){
            lowestSpawn = enemy.spawnNum;
        }
    }

    //Go through it again to find the enemy
    foreach (Attributes enemy in toSpawn)
    {
        if (enemy.spawnNum == lowestSpawn){
            return enemy;
        }
    }

    return null;

}

}
