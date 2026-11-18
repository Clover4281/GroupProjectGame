using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishGame : MonoBehaviour
{
    //  set finished to false
   private bool finished = false;

   //  if the player collides with the flag pole at the end of the level, the scene quits
   private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !finished)
        {
            finished = true;
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}
