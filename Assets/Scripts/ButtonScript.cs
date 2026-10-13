using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public GameObject door;
    public SpriteRenderer sprite;
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            sprite.color = Color.lawnGreen;
            Destroy(door);
        }
    }
}
