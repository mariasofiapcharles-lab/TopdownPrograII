using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    CharacterController characterController;
    [SerializeField]
    private float speed;
    [SerializeField]
    private LayerMask EnLyr;

    [SerializeField]
    private GameObject fist;
    void Update()
    {
        Vector3 movementVector = Vector2.zero;

        movementVector.x = Input.GetAxis("Horizontal");
        movementVector.z = Input.GetAxis("Vertical");
        movementVector.y = 0;
        characterController.Move(movementVector * Time.deltaTime * speed);


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 100f))
        {
            Vector3 position = hitInfo.point;
            position.y = transform.position.y;
            transform.LookAt(position);
        }


        //disparo
        if (Input.GetMouseButtonDown(0))
        {
           
            fist.SetActive(false);
            Ray fireRay = new Ray(transform.position + Vector3.up, transform.forward);
            RaycastHit enemyInfo;


            Debug.DrawRay(fireRay.origin, fireRay.direction * 10, Color.green, 10f);

            if (Physics.Raycast(fireRay.origin, fireRay.direction * 10, out enemyInfo, 100f, EnLyr))

            {
                Debug.Log(enemyInfo.transform.gameObject.name);
            }
            else
            {
                Debug.Log("Nothing hit");
            }
        }
    }
}
