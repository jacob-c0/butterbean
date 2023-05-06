using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class boxDebug : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private Color boxColor = Color.green;

    public GameObject Butterbean;

    private void Start()
    {
        // Get the BoxCollider2D component of the object

        // Create a new GameObject to hold the sprite renderer
        GameObject spriteHolder = new GameObject("BoxCollider2DVisualizer");
        spriteHolder.transform.parent = transform;
        spriteHolder.transform.localPosition = Vector3.zero;

        // Add a SpriteRenderer to the new GameObject
        spriteRenderer = spriteHolder.AddComponent<SpriteRenderer>();
        spriteRenderer.color = boxColor;

        // Calculate the size of the sprite based on the size of the collider
        Vector2 colliderSize = boxCollider.size;
        spriteRenderer.size = colliderSize;

        // Set the sorting order of the sprite to be behind other objects
        spriteRenderer.sortingOrder = 20;
    }

    private void Update()
    {
        // Set the position of the sprite to match the position of the collider
        Gizmos.color = Color.green;
        spriteRenderer.transform.position = boxCollider.transform.position;
    }

    private void OnDrawGizmos()
    
    {if (!Application.isPlaying)
        {
            // Draw box in green in editor mode
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);
        }
    }
}