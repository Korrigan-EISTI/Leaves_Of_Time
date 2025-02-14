using UnityEngine;

public class TimeController : MonoBehaviour
{
    public Animator playerAnimator;
    public Transform playerTransform;
    public float moveSpeed = 5f;

    private bool isPaused = false;

    void Start()
    {
        playerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime; // Permet aux animations de continuer
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) // Active/Désactive la pause
        {
            TogglePause();
        }

        if (isPaused)
        {
            HandlePausedMovement(); // Permet le déplacement en mode pause
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
    }

    void HandlePausedMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(h, 0, v).normalized;

        // Déplacement manuel du joueur en mode pause
        if (moveDirection.magnitude > 0)
        {
            playerTransform.position += moveDirection * moveSpeed * Time.unscaledDeltaTime;
        }
    }
}
