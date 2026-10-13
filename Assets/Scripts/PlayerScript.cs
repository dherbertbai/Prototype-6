using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public List<PlayerSnapshot> playerSnapshots;
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public CircleCollider2D hitbox;
    private bool isRewinding = false;
    
    private bool isRecording =  false;

    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vel = Vector2.zero;
        if (Keyboard.current.dKey.isPressed)
            vel.x = speed;
        else if (Keyboard.current.aKey.isPressed)
            vel.x = -speed;
        if (Keyboard.current.wKey.isPressed)
            vel.y = speed;
        else if (Keyboard.current.sKey.isPressed)
            vel.y = -speed;
        rb.linearVelocity = vel;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && !isRewinding)
        {
            isRecording = true;
        }
        if (Keyboard.current.spaceKey.wasReleasedThisFrame)
        {
            isRecording = false;
            Do();
        }

        if (isRecording)
        {
            sprite.color = Color.aquamarine;
            playerSnapshots.Add(new PlayerSnapshot
            {
                position = transform.position,
                velocity = rb.linearVelocity,
            });
        }
        if (isRewinding)
        {
            sprite.color = Color.khaki;
            return;
        }
        if (!isRewinding && !isRecording)
        {
            sprite.color = Color.white;
            hitbox.isTrigger = false;
        }
        else
        {
            hitbox.isTrigger = true;
        }
        
    }

    public void Do()
    {
        if (isRewinding)
        {
            return;
        }
        StartCoroutine(RewindCoroutine());
    }

    public IEnumerator RewindCoroutine()
    {
        rb.simulated = false;
        isRewinding = true;

        for (int i = playerSnapshots.Count - 1; i >= 0; i--)
        {
            rb.linearVelocity = playerSnapshots[i].velocity;
            transform.position = playerSnapshots[i].position;
            yield return null;
        }
        rb.simulated = true;
        isRewinding = false;
        playerSnapshots.Clear();
    }
    [System.Serializable]
    public class PlayerSnapshot
    {
        public Vector2  position;
        public Vector2  velocity;
    }
}
