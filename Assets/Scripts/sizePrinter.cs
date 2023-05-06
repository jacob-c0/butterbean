using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sizePrinter : MonoBehaviour
{
        public SpriteRenderer spriteRenderer;
        public static float spriteWidth;
    // Start is called before the first frame update
    void Start()
    {        
    spriteRenderer = GetComponent<SpriteRenderer>();
    spriteWidth = spriteRenderer.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
