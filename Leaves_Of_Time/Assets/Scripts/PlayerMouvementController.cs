using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class CustomPlayerMovement : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float jumpForce = 5f;
    public float turnSpeed = 10f;
    public float gravity = 9.81f;
    public Animator animator;
    public Transform cameraTransform;

    private CharacterController controller;
    private Vector3 moveDirection;
    private float verticalVelocity = 0f;
    private bool isPaused = false;
    private bool isJumping = false;
    private bool isTransitioning = false; // 🔹 Empêche de spammer `C`

    [Header("Post Processing Effects")]
    public PostProcessVolume postProcessingVolume;
    private Vignette vignette;
    private bool canBreakTime = false;

    public bool CanBreakTime
    {
        get => canBreakTime;
        set => canBreakTime = value;
    }

    [Header("Camera Effect")]
    public Transform cameraTarget; // 🎥 Objet vers lequel la caméra regarde (ex: Player)
    private Vector3 originalCameraOffset;
    public float cameraZoomOutDistance = 3f; // 🎥 Distance de recul
    public float cameraZoomDuration = 0.4f; // 🎥 Durée du zoom arrière
    public float cameraZoomInDuration = 0.1f; // 🎥 Durée du retour rapide

    [Header("Camera Smooth Settings")]
    public float cameraSmoothTime = 0.15f; // 🎥 Plus petit = plus rapide, plus grand = plus fluide
    private Vector3 cameraVelocity = Vector3.zero; // 🎥 Stocke la vitesse actuelle du SmoothDamp

    void LateUpdate()
    {
        // 🎥 Lissage du mouvement de la caméra avec SmoothDamp
        Vector3 targetPosition = cameraTarget.position + originalCameraOffset;
        cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, targetPosition, ref cameraVelocity, cameraSmoothTime);
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        if (postProcessingVolume && postProcessingVolume.profile)
        {
            postProcessingVolume.profile.TryGetSettings(out vignette);
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !isTransitioning) // 🔹 Bloque l'entrée si une transition est en cours
        {
            if (!isPaused)
                StartCoroutine(SlowDownTimeCoroutine(0.5f));
            else
                StartCoroutine(ResumeTimeCoroutine(0.5f));
        }

        if (!isPaused)
        {
            HandleMovement(Time.deltaTime);
        }
        else
        {
            HandleMovement(Time.unscaledDeltaTime);
        }
    }

    private void HandleMovement(float deltaTime)
    {
        float adjustedDeltaTime = isPaused ? Time.unscaledDeltaTime : deltaTime;

        bool moveForward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool moveBackward = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        bool moveLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool moveRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool jumpPressed = Input.GetKeyDown(KeyCode.Space);

        // Stopper complètement le mouvement horizontal en pause
        if (isPaused && !moveForward && !moveBackward && !moveLeft && !moveRight)
        {
            moveDirection = Vector3.zero;
            animator.SetFloat("Speed", 0f);
        }
        else
        {
            moveDirection = Vector3.zero;
            if (moveForward) moveDirection += Vector3.forward;
            if (moveBackward) moveDirection += Vector3.back;
            if (moveLeft) moveDirection += Vector3.left;
            if (moveRight) moveDirection += Vector3.right;
            moveDirection.Normalize();
        }

        // 🔹 Force la mise à jour de isGrounded en pause
        if (isPaused)
        {
            controller.Move(Vector3.down * 0.01f);  // Petit mouvement vers le bas pour forcer la mise à jour
        }

        bool grounded = controller.isGrounded; // Maintenant, isGrounded est à jour

        if (grounded)
        {
            HandleJump(adjustedDeltaTime, jumpPressed);
        }
        else
        {
            verticalVelocity -= gravity * adjustedDeltaTime;
        }

        if (moveDirection.magnitude > 0 || !grounded) // On bouge ou on tombe
        {
            float currentSpeed = isRunning ? runSpeed : walkSpeed;
            Vector3 targetDirection = cameraTransform.forward * moveDirection.z + cameraTransform.right * moveDirection.x;
            targetDirection.y = 0; // Empêcher l'inclinaison vers l'avant

            if (targetDirection != Vector3.zero) // Vérification AVANT d'appliquer LookRotation
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), turnSpeed * deltaTime);
            }

            Vector3 movement = targetDirection.normalized * currentSpeed + Vector3.up * verticalVelocity;
            controller.Move(movement * adjustedDeltaTime);
        }

        if (isPaused)
        {
            animator.SetFloat("Speed", moveDirection.magnitude); // Pas d'interpolation en pause
        }
        else
        {
            animator.SetFloat("Speed", moveDirection.magnitude, 0.1f, deltaTime); // Interpolation en temps normal
        }
    }
    private void HandleJump(float deltaTime, bool jumpPressed)
    {
        bool grounded = controller.isGrounded; // On se base uniquement sur le CharacterController

        if (isJumping && grounded) // Quand on touche le sol
        {
            isJumping = false;
            animator.SetBool("Jump", false);
            animator.SetBool("Grounded", true);
            verticalVelocity = 0f; // 🔹 Remettre la vélocité à zéro
        }

        if (jumpPressed && grounded) // Autoriser le saut même en pause
        {
            verticalVelocity = jumpForce;
            isJumping = true;
            animator.SetBool("Jump", true);
            animator.SetBool("Grounded", false);
        }
    }

    // 🔹 Coroutine pour ralentir le temps et activer progressivement la vignette
    IEnumerator SlowDownTimeCoroutine(float duration)
    {
        var cameraController = cameraTransform.gameObject.GetComponent<TPSCameraController>();
        cameraController.isCameraAnimating = true;

        isTransitioning = true;
        isPaused = true;

        float elapsedTime = 0f;
        float startTimeScale = 1f;
        float endTimeScale = 0f;
        float startVignette = 0f;
        float endVignette = 1f;

        Vector3 zoomOutTarget = cameraTransform.position - cameraTransform.forward * 10f; // 🔹 Ajout d'un recul de 10 unités

        // 🎥 Phase 1 : Zoom arrière (400ms)
        float zoomTime = 0f;
        while (zoomTime < cameraZoomDuration)
        {
            zoomTime += Time.unscaledDeltaTime;
            float zoomProgress = zoomTime / cameraZoomDuration;
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, zoomOutTarget, zoomProgress); // 🔹 Applique le recul
            yield return null;
        }

        // 🎥 Phase 2 : Zoom rapide vers le joueur (100ms)
        zoomTime = 0f;
        while (zoomTime < cameraZoomInDuration)
        {
            zoomTime += Time.unscaledDeltaTime;
            float zoomProgress = zoomTime / cameraZoomInDuration;
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraTarget.position + originalCameraOffset, zoomProgress);
            yield return null;
        }

        // 🔹 Appliquer le ralentissement du temps après l'effet de caméra
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float progress = elapsedTime / duration;
            Time.timeScale = Mathf.Lerp(startTimeScale, endTimeScale, progress);

            if (vignette != null)
            {
                vignette.intensity.value = Mathf.Lerp(startVignette, endVignette, progress);
            }

            yield return null;
        }

        Time.timeScale = 0f;
        isTransitioning = false;
        cameraController.isCameraAnimating = false;
    }

    // 🔹 Coroutine pour relancer le temps et désactiver progressivement la vignette
    IEnumerator ResumeTimeCoroutine(float duration)
    {
        var cameraController = cameraTransform.gameObject.GetComponent<TPSCameraController>();
        cameraController.isCameraAnimating = true;

        isTransitioning = true;
        isPaused = false;

        float elapsedTime = 0f;
        float startTimeScale = 0f;
        float endTimeScale = 1f;
        float startVignette = 1f;
        float endVignette = 0f;

        Vector3 zoomInTarget = cameraTransform.position + cameraTransform.forward * 10f; // 🔹 Remet la caméra en avant

        // 🎥 Phase 1 : Zoom rapide vers le joueur (100ms)
        float zoomTime = 0f;
        while (zoomTime < cameraZoomInDuration)
        {
            zoomTime += Time.unscaledDeltaTime;
            float zoomProgress = zoomTime / cameraZoomInDuration;
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, zoomInTarget, zoomProgress); // 🔹 Annule le recul
            yield return null;
        }

        // 🎥 Phase 2 : Zoom arrière (400ms)
        zoomTime = 0f;
        while (zoomTime < cameraZoomDuration)
        {
            zoomTime += Time.unscaledDeltaTime;
            float zoomProgress = zoomTime / cameraZoomDuration;
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraTarget.position + originalCameraOffset, zoomProgress);
            yield return null;
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float progress = elapsedTime / duration;
            Time.timeScale = Mathf.Lerp(startTimeScale, endTimeScale, progress);

            if (vignette != null)
            {
                vignette.intensity.value = Mathf.Lerp(startVignette, endVignette, progress);
            }

            yield return null;
        }

        Time.timeScale = 1f;
        isTransitioning = false;
        cameraController.isCameraAnimating = false;
    }

}
