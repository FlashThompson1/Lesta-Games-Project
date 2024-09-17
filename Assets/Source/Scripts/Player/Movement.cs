using System.Collections;
using UnityEngine;



[RequireComponent(typeof(CharacterController))]
//[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _gravity;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Transform _groundCheckObject;
    [SerializeField] private float _groundDistance;
    private Animator _characterAnimator;
    private CharacterController _characterController;
    private Vector3 _velocity;
    private bool _isGrounded;


    private float x;
    private float z;

    private void Start()
    {

        _characterAnimator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();

    }


    private void Update()
    {

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        PlayerMovement();
        Jump();
        CheckingOnGrounded();




    }

    private void PlayerMovement()
    {

        _characterAnimator.SetBool("IsMoving", false);
        _characterAnimator.SetBool("IsRunningBack", false);
        _characterAnimator.SetBool("IsRightRun", false);
        _characterAnimator.SetBool("IsLeftRun", false);
        // _runSound.Pause();



        if (z > 0)
        {
            _characterAnimator.SetBool("IsMoving", true);

        }
        if (z < 0)
        {
            _characterAnimator.SetBool("IsRunningBack", true);


        }
        else if (x < 0)
        {
            _characterAnimator.SetBool("IsLeftRun", true);


        }
        else if (x > 0)
        {
            _characterAnimator.SetBool("IsRightRun", true);


        }
        Vector3 moving = transform.right * x + transform.forward * z;
        _characterController.Move(moving * _movementSpeed * Time.deltaTime);
        _characterController.Move(_velocity * Time.deltaTime);
        _velocity.y += _gravity * Time.deltaTime;






    }

    private void Jump()
    {

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {

            _characterAnimator.SetBool("IsJumping", true);
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);



        }
        else if (_characterController.isGrounded)
        {
            _characterAnimator.SetBool("IsJumping", false);
            _characterAnimator.SetBool("IsOnAir", false);
        }

    }


    private IEnumerator IsOnAir()
    {
        yield return new WaitForSeconds(0);
        _characterAnimator.SetBool("IsOnAir", true);

    }




    private void CheckingOnGrounded()
    {
        _isGrounded = Physics.CheckSphere(_groundCheckObject.position, _groundDistance, _groundMask);


        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;

        }

        if (_characterController.isGrounded == false)
            StartCoroutine(IsOnAir());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _groundDistance);
    }
}
