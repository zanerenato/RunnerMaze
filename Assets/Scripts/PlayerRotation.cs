using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;


public class PlayerRotation : MonoBehaviour {

    [SerializeField]
    private float _rotationSpeed = 30.0f;
    private float _playerSpeed = 0.005f;
    [SerializeField]
    private float _smoothTime = 0.003f;
    private float _xVelocity = 0;
    private  float _refRotation = 0;
//    private  float _newXRotation = 0;
    private  float _xRotation = 0;
    private  int _clockWise = 0;
    private  bool _isSlide = false;
    private  bool _isGetUp = false;
    private  Transform _playerTransform;


	// Use this for initialization
	private void Start () 
	{
        _isSlide = false;
	    _isGetUp = false;
        _rotationSpeed = 30.0f;
        _smoothTime = 0.003f;
        StartCoroutine(SlideCoroutine());
	    StartCoroutine(GetUpCoroutine());
	}
	
//	// Update is called once per frame
//	void Update () {
//
//        //@Renato: Chama a rotação da Camera até que a mesma tenha chego na rotação esperada
//        if (_isSlide)
//        {
//            if ((_clockWise > 0) && (_xRotation >= _newXRotation))
//            {
//                _isSlide = false;
//                _xVelocity = 0.0f;
//            }
//            else if ((_clockWise < 0) && (_xRotation <= _newXRotation))
//            {
//                _isSlide = false;
//                _xVelocity = 0.0f;
//            }
//            else
//            {
//                Rotate();
//            }
//        }
//    }

    #region Getters and Setters
    public bool IsSlide()
    {
       return _isSlide;
    }
    
    public bool IsGetUp()
    {
        return _isGetUp;
    }

    #endregion

    /// <summary>
    /// @Renato: seta os parametros para iniciar a rotação do player
    /// </summary>
    /// <param name="player">transform do player que será rotacionado</param>
    /// <param name="newRotation">o quanto o player irá se rotacionar</param>
    /// <param name="speed">Velocidade de rotação</param>
    public void StartSlide(Transform player, float speed)
    {
        _playerTransform = player;
        _xRotation = _playerTransform.eulerAngles.x;
        _refRotation = _xRotation;
//        _newXRotation = _playerTransform.eulerAngles.y + newRotation;
        _playerSpeed = speed;
        _clockWise = -1;
        _isSlide = true;
        
    }

    public void RotateSide(Transform player, Directions targetDirection)
    {

        try
        {
            if (player == null)
            {
                return;
            }
            if (targetDirection != Directions.None)
            {
                //player.Rotate(0,(int)targetDirection*90.0f,0);
                Quaternion rotation = Quaternion.Euler(0, (int) targetDirection * 90.0f, 0);
                if (player.rotation != rotation)
                {
                    player.rotation = rotation;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        

    }

    private IEnumerator SlideCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(IsSlide);
            //@Renato: Chama a rotação até que a mesma tenha chego na rotação esperada
            if (_xRotation <= -90.0f)
            {
                _playerTransform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y + 0.3f,
                    _playerTransform.position.z);
                _isSlide = false;
                _xVelocity = 0.0f;
                StartCoroutine(SlideTimeCoroutine());
//                _clockWise = 1;
//                _isGetUp = true;
            }
            else
            {
                Rotate();
            }
        }
    }

    private IEnumerator SlideTimeCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        _clockWise = 1;
        _isGetUp = true;
        _playerTransform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y - 0.3f,
            _playerTransform.position.z);
        
    }

    private IEnumerator GetUpCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(IsGetUp);
            //@Renato: Chama a rotação até que a mesma tenha chego na rotação esperada
            if (_xRotation >= 0)
            {
                _isGetUp = false;
                _xVelocity = 0.0f;
            }
            else
            {
                Rotate();
            }
        }
    }

    //@Renato: Faz a rotação da Camera de 90 em 90 graus
    private void Rotate()
    {
        //@Renato: Incrementa o próximo valor da rotação utilizando o clockWise para definir o sentido de rotação
        _refRotation += _clockWise * _rotationSpeed * _playerSpeed * Time.deltaTime;
        //@Renato: Calcula o quanto a Camera irá rotacionar utilizando o valor de yRotate
        _xRotation = Mathf.SmoothDamp(_xRotation, _refRotation, ref _xVelocity, _smoothTime * _playerSpeed);

        RotatePlayer();
    }

    //@Renato: Atualiza a posição e rotação da Camera
    private void RotatePlayer()
    {
        if (_playerTransform == null)
            return;
        //@Renato: Calcula um quaternion com os novos angulos
        Quaternion rotation = Quaternion.Euler(_xRotation, 0, 0);
        //@Renato: Atualiza a rotação da Camera
        _playerTransform.rotation = rotation;
    }
}
