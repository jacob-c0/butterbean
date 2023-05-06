using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{    
    public ButtControls ButtControls;
    public Level Level;
    public BackgroundManager BackgroundManager;
    public ShopWhistle ShopWhistle;
    public RubberMove RubberMove;
    public TalkScript TalkScript;
    public EnemyTracker EnemyTracker;
    public TextFadeIn jumpText;
    public TextFadeIn winThumb;

    public MRLOVERMAN mr1;
    public MRLOVERMAN mr2;

    public int minMovementLevel;
    public static int maxLevel;


    public AudioSource gameMusic;
    public AudioSource deathMusic;
    public AudioSource winMusic;
    public static bool isDead;
    private bool canRestart;

    void Start()
    {
        Screen.SetResolution(880, 400, true);

        maxLevel = 100;
        canRestart = false;
        isDead = false;
        gameMusic.volume = 0f;
        gameMusic.Play();
        deathMusic.Stop();
        winMusic.Stop();
        StartCoroutine(startState());
        Screen.SetResolution(880, 400, false);
        
    }

    public IEnumerator startState(){
        yield return new WaitUntil(() => jumpText != null);
        jumpText.textVisible();
        yield return new WaitUntil(() => Input.GetButtonDown("Jump"));
        jumpText.textInvisible();
        TalkScript.onBegin();
        RubberMove.onBegin();
        ButtControls.onBegin();
        ShopWhistle.onBegin();
    }

     void Update()
    {
        if (isDead == true && !deathMusic.isPlaying && Level.level < maxLevel){
            gameMusic.Stop();
            deathMusic.Play();
            StartCoroutine(deathPause());
            StartCoroutine(jumpText.FadeInCoroutine());

        }
        else if (isDead == true && !winMusic.isPlaying && Level.level >= maxLevel){
            ButtControls.shouldWalk = false; // For win
            gameMusic.Stop();
            winMusic.Play();
            //AudioSource.PlayClipAtPoint(winMusic, transform.position);
            StartCoroutine(deathPause());
            StartCoroutine(winThumb.FadeInCoroutine());

        }
        if (canRestart && Input.GetButtonDown("Jump")){
            StartCoroutine(reset());

        }
        
    }

    private IEnumerator deathPause(){
        yield return new WaitForSeconds(0.5f);
        canRestart = true;
    }

    private IEnumerator reset(){
            yield return new WaitForSeconds(0.05f);
            jumpText.textInvisible();
            winThumb.textInvisible();
            isDead = false;
            EnemyTracker.resetEnemies();
            BackgroundManager.resetBackgrounds();
            deathMusic.Stop();
            gameMusic.Play();
            canRestart = false;
            ButtControls.justDied = 0;
            isDead = false;
            Level.level = 0;
            winMusic.Stop();
            ButtControls.onBegin();
      //      mr1.reset();
      //      mr2.reset();
            ButtControls.shouldWalk = true;
    }

}

