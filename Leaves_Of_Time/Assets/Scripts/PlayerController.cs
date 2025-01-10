using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Animator anim;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        anim.SetFloat("horizontal", horizontal);
        anim.SetFloat("vertical", vertical);

        // Optionnel : Calculer la vitesse pour le mouvement.
        Vector3 direction = new Vector3(horizontal, 0, vertical);
        if (direction.magnitude > 1f)
            direction.Normalize();
    }

}
