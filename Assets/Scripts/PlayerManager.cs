using UnityEngine;
using UnityEngine.UI;

public enum Path
{
	Path1,
	Path2,
	Path3
}

public enum Directions
{
	None = 0,
	Left = -1,
	Right = 1
}
public class PlayerManager : MonoBehaviour
{

	[SerializeField] private Transform _playerTransform;
	[SerializeField] private Transform _caracterTransform;
//	private Rigidbody _myRigidbody;
	[SerializeField] public float MoveSpeed;
	[SerializeField] public float JumpForce = 9.8f;
	[SerializeField] public float SideSpeed;
	[SerializeField] public float Gravity = 9.8f;

	private PlayerRotation _playerRotation;
	private Vector2 _direction;
	private Vector2 _startPos;
	private bool _isGrounded;
	private bool _isJumping;
	
	private const float JumpHeight = 2.0f;

	// Use this for initialization
	void Start ()
	{
		_playerRotation = gameObject.GetComponent<PlayerRotation>();
		
		if (_caracterTransform == null)
		{
			_caracterTransform = gameObject.GetComponent<Transform>();
		}

		if (_playerTransform == null)
		{
			_playerTransform = gameObject.GetComponentInParent<Transform>();
		}
//		
//		_myRigidbody = gameObject.GetComponent<Rigidbody>();
//		if (_caracterTransform == null)
//		{
//			_myRigidbody = gameObject.AddComponent<Rigidbody>();
//		}

		_isGrounded = true;
		_isJumping = false;
	}

	private void FixedUpdate()
	{
//		var newPosition = new Vector3(_playerTransform.position.x, _playerTransform.position.y, _playerTransform.position.z);
		var newDirection = Vector3.forward * MoveSpeed * Time.deltaTime;
		newDirection = _playerTransform.TransformDirection(newDirection);
		_playerTransform.position += newDirection;
	}

	// Update is called once per frame
	private void Update ()
	{
		var newPosition = new Vector3(_playerTransform.position.x, _playerTransform.position.y, _playerTransform.position.z);
		
		if (Input.GetKeyDown("a"))
		{
//			force += Vector3.left * SideSpeed;
//			transform.position = new Vector3(transform.position.x - SideSpeed, transform.position.y, transform.position.z);
			newPosition.x -= SideSpeed;
			_playerTransform.position = newPosition;
		}
		else if (Input.GetKeyDown("d"))
		{
//			force += Vector3.right * SideSpeed;
//			transform.position = new Vector3(transform.position.x + SideSpeed, transform.position.y, transform.position.z);
			newPosition.x += SideSpeed;
			_playerTransform.position = newPosition;
		}

		else if (Input.GetKeyDown("space"))
		{
//			_myRigidbody.AddForce(transform.up * JumpForce*100, ForceMode.Impulse);
			_isGrounded = false;
			_isJumping = true;
		}
		else if (Input.GetKeyDown("s"))
		{
			_playerRotation.StartSlide(_caracterTransform, MoveSpeed);
		}
		else if (Input.GetKeyDown("q"))
		{
			_playerRotation.RotateSide(_playerTransform, Directions.Left);
		}
		else if (Input.GetKeyDown("e"))
		{
			_playerRotation.RotateSide(_playerTransform, Directions.Right);
		}

		// Track a single touch as a direction control.
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			// Handle finger movements based on TouchPhase
			switch (touch.phase)
			{
				//When a touch has first been detected, change the message and record the starting position
				case TouchPhase.Began:
					// Record initial touch position.
					_startPos = touch.position;
					break;

				//Determine if the touch is a moving touch
				case TouchPhase.Moved:
					// Determine direction by comparing the current touch position with the initial one
					_direction = touch.position - _startPos;
					break;

				case TouchPhase.Ended:
					// Report that the touch has ended when it ends
					if (Mathf.Abs(_direction.x) > Mathf.Abs(_direction.y))
					{
						var side = SideSpeed;
						if (_direction.x < 0)
						{
							side = -SideSpeed;
						}

						newPosition.x += side;
						
					}
					else
					{
						if (_direction.y > 0)
						{
//							_myRigidbody.AddForce(transform.up * JumpForce, ForceMode.Impulse);
							_isGrounded = false;
							_isJumping = true;
						}
						else
						{
							_playerRotation.StartSlide(_caracterTransform, MoveSpeed);
						}
					}
					break;
			}
		}

		if (!_isGrounded)
		{
			if (_isJumping)
			{
				newPosition.y += JumpForce * Time.deltaTime;
				if (newPosition.y >= JumpHeight)
				{
					_isJumping = false;
				}
			}
			else
			{
				newPosition.y -= Gravity * Time.deltaTime;
				if (newPosition.y <= 0)
				{
					newPosition.y = 0;
					_isGrounded = true;
				}	
			}
		}
		
		_playerTransform.position = newPosition;
		
	}


	private void OnCollisionEnter(Collision other)
	{
		Debug.Log(other.gameObject.tag);
	}
	
	
}
