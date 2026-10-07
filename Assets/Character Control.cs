using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private CharacterController characterController;

    [SerializeField]
    private float speed = 5f;

    [Header("Melee")]
    [SerializeField]
    private GameObject fist;

    [SerializeField]
    private float fistDuration = 0.2f;

    [SerializeField]
    private float punchRange = 2f;

    [SerializeField]
    private int punchDamage = 1;

    [SerializeField]
    private LayerMask enemyLayer;

    private bool attacking = false;

    private Vector3 aimDirection = Vector3.forward;


    void Update()
    {
        // =========================================
        // MOUSE = AIM / FACING
        // =========================================

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Plane ground = new Plane(
            Vector3.up,
            transform.position
        );

        if (ground.Raycast(ray, out float distance))
        {
            Vector3 mousePosition = ray.GetPoint(distance);

            Vector3 direction =
                mousePosition - transform.position;

            direction.y = 0f;

            if (direction.sqrMagnitude > 0.001f)
            {
                aimDirection = direction.normalized;

                transform.rotation =
                    Quaternion.LookRotation(aimDirection);
            }
        }


        // =========================================
        // WASD = PLAIN MOVEMENT
        // =========================================

        float horizontal =
            Input.GetAxisRaw("Horizontal");

        float vertical =
            Input.GetAxisRaw("Vertical");


        Vector3 movement = new Vector3(
            horizontal,
            0f,
            vertical
        );


        // Prevent faster diagonal movement
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }


        characterController.Move(
            movement *
            speed *
            Time.deltaTime
        );


        // =========================================
        // ATTACK
        // =========================================

        if (Input.GetMouseButtonDown(0) && !attacking)
        {
            StartCoroutine(MeleeAttack());
        }
    }


    IEnumerator MeleeAttack()
    {
        attacking = true;


        if (fist != null)
        {
            fist.SetActive(true);
        }


        // =========================================
        // PUNCH TOWARD MOUSE
        // =========================================

        Vector3 rayOrigin =
            transform.position +
            Vector3.up * 0.5f;


        RaycastHit hitInfo;


        if (Physics.Raycast(
            rayOrigin,
            aimDirection,
            out hitInfo,
            punchRange,
            enemyLayer))
        {
            Debug.Log(
                "Punched: " +
                hitInfo.collider.name
            );

            Enemy enemy =
                hitInfo.collider.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(punchDamage);
            }
        }


        yield return new WaitForSeconds(
            fistDuration
        );


        if (fist != null)
        {
            fist.SetActive(false);
        }

        attacking = false;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 origin =
            transform.position +
            Vector3.up * 0.5f;

        Gizmos.DrawRay(
            origin,
            aimDirection * punchRange
        );
    }
}