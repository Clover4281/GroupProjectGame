using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class BuffUps : MonoBehaviour
{

    public float AmountToGive = 5;
    public float Duration = 3;

    private PlayerMovement player;
    
    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player.moveSpeed += AmountToGive;

            Destroy(gameObject);
        }
    }
}
