using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour 
{

	[SerializeField]
	private Transform _target;
 
	[SerializeField]
	private Vector3 _offsetPosition;
 
	[SerializeField]
	private Space _offsetPositionSpace = Space.Self;
 
	[SerializeField]
	private bool _lookAt = true;

	private void Start()
	{
		_target = GameObject.FindGameObjectWithTag("MyPlayer").transform;
		if (_target == null)
		{
			var playerMesh = Resources.Load("Caracters/StartPlayer") as GameObject;
			var player = (GameObject) Instantiate(playerMesh, new Vector3(0, 0, 1), Quaternion.identity);
			_target = player.transform;

		}
	}

	private void Update()
	{
		Refresh();
	}
 
	public void Refresh()
	{
		if(_target == null)
		{
			Debug.LogWarning("Missing target ref !", this);
 
			return;
		}
 
		// compute position
		if(_offsetPositionSpace == Space.Self)
		{
			transform.position = _target.TransformPoint(_offsetPosition);
		}
		else
		{
			transform.position = _target.position + _offsetPosition;
		}
 
		// compute rotation
		if(_lookAt)
		{
			transform.LookAt(_target);
		}
		else
		{
			transform.rotation = _target.rotation;
		}
	}
}
