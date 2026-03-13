using UnityEngine;

public class SoccerBallSounds : MonoBehaviour
{
    public float velocityThreshold = 0.1f; // Minimum hit strength to play sound
    public float soundCooldown = 0.2f;    // Time to wait before playing again
    private float _lastSoundTime;
    [Header("Physics Settings")]
    public float kickStrength = 15f;
    public float liftAmount = 0.8f;

    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // --- 1. PHYSICS PUSH (IF PLAYER) ---
        if (collision.gameObject.CompareTag("Player"))
        {
            // Reset energy to prevent "pinning" or double-stacking force
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Calculate direction away from the player
            Vector3 pushDir = (transform.position - collision.transform.position).normalized;
            pushDir.y = 0; // Keep horizontal direction clean

            // Apply the "Punch" with a lift to clear ground friction
            rb.velocity = (pushDir + Vector3.up * liftAmount) * kickStrength;

            Debug.Log("Ball Kicked by Player!");
        }

        // --- 2. SOUND LOGIC ---
        // Global cooldown check (prevents "machine gun" sounds on constant contact)
        if (Time.time - _lastSoundTime < soundCooldown) return;

        // Check if the impact was strong enough to warrant a sound
        if (collision.relativeVelocity.magnitude > velocityThreshold)
        {
            Debug.Log("Ball Hit! Impact Strength: " + collision.relativeVelocity.magnitude);

            PlayBallSound(collision.gameObject);
            _lastSoundTime = Time.time;
        }
    }

    private void PlayBallSound(GameObject hitObject)
    {

        Debug.Log("Ball Hit! Play Wwise Event here.");
        // Example for Wwise:
        // AkSoundEngine.PostEvent("Play_Ball_Kick", gameObject);
    }
}