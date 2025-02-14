using UnityEngine;

public class TPSCameraController : MonoBehaviour
{
    public Transform player; // Référence au joueur
    public float mouseSensitivity = 2.0f; // Sensibilité de la souris
    public float distanceFromPlayer = 3.0f; // Distance de la caméra au joueur
    public float cameraHeight = 1.9f; // Hauteur de la caméra par rapport au joueur
    public float verticalMin = -30f; // Limite basse de la caméra
    public float verticalMax = 60f; // Limite haute de la caméra

    private float yaw = 0f; // Rotation horizontale
    private float pitch = 0f; // Rotation verticale
    public bool isCameraAnimating = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Cache et verrouille le curseur
    }

    void LateUpdate()
    {
        // Si le temps est figé, la caméra doit encore tourner
        float delta = (Time.timeScale == 0) ? Time.unscaledDeltaTime : Time.deltaTime;

        // Récupère les entrées de la souris
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, verticalMin, verticalMax); // Limite verticale

        // Applique la rotation à la caméra
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Nouvelle position de la caméra (hauteur ajustée)
        Vector3 targetPosition = player.position + Vector3.up * cameraHeight - (rotation * Vector3.forward * distanceFromPlayer);
        transform.position = targetPosition;

        // Regarde légèrement vers le haut du joueur au lieu de ses pieds
        Vector3 lookAtTarget = player.position + Vector3.up * cameraHeight;
        transform.LookAt(lookAtTarget);
    }
}
