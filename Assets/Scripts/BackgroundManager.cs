using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public Sprite[] backgrounds;
    public GameObject[] backgroundObjects;
    public GameController GameController;
    public Level Level;
    public float scrollSpeed = 1.0f;
    public float resetPosition = 0f;
    public float respawnPosition = 52.21f;
    public int uniqueBackgroundChance;

    private float prevLevel = 0;
    private float baseScrollSpeed;

    private int randomEmpty = 0;
    private int backgroundLength;
    private int beginEntry;

    private int lastIndexUsed = 0;
    private int currentIndex = 0;

    public sizePrinter sizePrinter;
    float spriteWidth;

    void Start()
    {
        foreach (GameObject backgroundObject in backgroundObjects)
        {
            Sprite randomBackground = backgrounds[0];
            backgroundObject.GetComponent<SpriteRenderer>().sprite = randomBackground;
            baseScrollSpeed = scrollSpeed;
        }
    }

    void Update()
    {
        if (GameController.isDead == false){

            if (prevLevel != Level.level && Level.level > GameController.minMovementLevel){
               scrollSpeed = baseScrollSpeed + 0.08f*(Level.level - GameController.minMovementLevel);
                prevLevel = Level.level;
            }

            float spriteWidth = sizePrinter.spriteWidth;
            foreach (GameObject backgroundObject in backgroundObjects)
            {
                backgroundObject.transform.Translate(Vector2.left * scrollSpeed * Time.deltaTime);

                if (backgroundObject.transform.position.x <= resetPosition)
                {
                    backgroundObject.transform.position += new Vector3(respawnPosition, 0.0f, 0.0f);

                    randomEmpty = Random.Range(0,uniqueBackgroundChance-1);
                    if (randomEmpty != 1){
                        backgroundLength = 0;
                        beginEntry = 0;
                        currentIndex = 0;
                    }
                    else{
                        backgroundLength = backgrounds.Length;
                        beginEntry = 1;
                    }
                    
                    if (backgroundLength>0){
                        currentIndex = Random.Range(beginEntry,backgroundLength);
                        while (lastIndexUsed == currentIndex){
                            currentIndex = Random.Range(beginEntry,backgroundLength);
                        }
                        lastIndexUsed = currentIndex;
                    }

                    Sprite randomBackground = backgrounds[currentIndex];
                    if (Level.level < 85){
                        backgroundObject.GetComponent<SpriteRenderer>().sprite = randomBackground;
                    }
                    else{
                        backgroundObject.GetComponent<SpriteRenderer>().sprite = backgrounds[1];
                    }
                }
            }
        }
        else{
            scrollSpeed = baseScrollSpeed;
        }
    }

   public void resetBackgrounds(){
        foreach (GameObject backgroundObject in backgroundObjects)
            backgroundObject.GetComponent<SpriteRenderer>().sprite = backgrounds[0];   
    }

}