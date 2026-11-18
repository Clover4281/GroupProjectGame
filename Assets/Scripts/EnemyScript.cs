using UnityEngine;


public class EnemyScript : MonoBehaviour
{
    //  set enemy speed 
    public float speed = 2f;

    //  sets two points for the enemy to walk between
    public Transform[] points;

    //  the index of the point the enemy is going towards
    private int i;
    private SpriteRenderer spriteRenderer;

    //  gets the spriterenderer, so the sprite can be flipped
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //  the enemy walks to one point and once it reaches it, flips the sprite and walks to the other point
    void Update()
    {
        if (Vector2.Distance(transform.position, points[i].position) < 0.25f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);

        spriteRenderer.flipX = (transform.position.x - points[i].position.x) < 0f;
    }
}