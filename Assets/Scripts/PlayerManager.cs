using UnityEngine;
using UnityEngine.UI;

public enum Path
{
	Path1,
	Path2,
	Path3
}
public class PlayerManager : MonoBehaviour
{

	private Transform _myTransform;
	private Rigidbody _myRigidbody;

	[SerializeField] public float MoveSpeed;
	[SerializeField] public float JumpForce;
	[SerializeField] public float SideSpeed;
	[SerializeField] private Text m_Text;
	private Vector2 _direction;
	private Vector2 _startPos;

	// Use this for initialization
	void Start ()
	{
		_myTransform = gameObject.GetComponent<Transform>();
		if (_myTransform == null)
		{
			_myTransform = gameObject.AddComponent<Transform>();
		}
		_myRigidbody = gameObject.GetComponent<Rigidbody>();
		if (_myTransform == null)
		{
			_myRigidbody = gameObject.AddComponent<Rigidbody>();
		}
		
	}

	private void FixedUpdate()
	{
		var force = transform.forward * MoveSpeed;
		_myRigidbody.MovePosition(transform.position + force * Time.deltaTime);
	}

	// Update is called once per frame
	private void Update ()
	{
		var newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		
		if (Input.GetKeyDown("a"))
		{
//			force += Vector3.left * SideSpeed;
			transform.position = new Vector3(transform.position.x - SideSpeed, transform.position.y, transform.position.z);
		}
		else if (Input.GetKeyDown("d"))
		{
//			force += Vector3.right * SideSpeed;
			transform.position = new Vector3(transform.position.x + SideSpeed, transform.position.y, transform.position.z);
		}

		if (Input.GetKeyDown("space"))
		{
			_myRigidbody.AddForce(transform.up * JumpForce, ForceMode.Impulse);
			
		}

		
//		newPosition.z += MoveSpeed;
		
	
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
						_myTransform.position = newPosition;
					}
					else
					{
						if (_direction.y > 0)
						{
//							_myTransform.AddForce(transform.up * JumpForce, ForceMode.Impulse);
						}
					}
					break;
			}
		}
		
	}


	private void OnCollisionEnter(Collision other)
	{
		Debug.Log(other.gameObject.tag);
	}
}
