using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 1f;
    public float runSpeed = 3f;
    public float jumpPower = 0.5f;
    public float gravity = 10f;
 
 
    public float lookSpeed = 5f;
    public float lookXLimit = 100f;

    public GameObject bulletPrefab; 
    public AudioSource shootingSound;

    public float shootRange = 100f; 
    public float shootForce = 10f;
 
 
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
 
    public bool canMove = true;
    public bool canShoot = true;
 
    
    CharacterController characterController;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
 
    void Update()
    {
 
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
 
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
 

        if (Input.GetKey(KeyCode.Space) && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }
 
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
 

        characterController.Move(moveDirection * Time.deltaTime);
 
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        if (canShoot && Input.GetMouseButtonDown(0)) // Left mouse button to shoot
        {
            Shoot();
        }      
     
    }

    void Shoot()
    {
        // Play shooting sound effect (if added)
        if (shootingSound!= null)
        {
            shootingSound.Play();
        }

        GameObject bullet = Instantiate(bulletPrefab, playerCamera.transform.position, playerCamera.transform.rotation);

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb!= null)
        {
            bulletRb.AddForce(playerCamera.transform.forward * shootForce, ForceMode.Impulse);
        }


        Destroy(bullet, shootRange / shootForce);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            ShowLoserMenu();
        }
        if (other.CompareTag("Finish"))
        {
            ShowVictoryMenu();
        }

    }
    void ShowLoserMenu()
    {
        Debug.Log("You've lost! Don't Worry, You can Try Again!");
        LoserMenu.Instance.Show();
    }

    void ShowVictoryMenu()
    {
        Debug.Log("You've completed the level succesfully!");
        VictoryMenu.Instance.Show();
    }
}






