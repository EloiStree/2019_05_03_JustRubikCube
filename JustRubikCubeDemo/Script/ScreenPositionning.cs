using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenPositionning : MonoBehaviour {

    public Transform _toAffect;
    public Vector3 _offset;

    public Vector3 _worldPosition;

    public Ray _ray;
    public Vector3 _mousePosition;
    public Camera _usedCamera;
    public float _depth=0.5f;
	void Start () {


    }
	
	void Update () {

        if(_usedCamera ==null)
        _usedCamera = Camera.main;
        _mousePosition = Input.mousePosition;
        _mousePosition.z = _depth;
        _worldPosition = _usedCamera.ScreenToWorldPoint(_mousePosition);
        _ray =_usedCamera.ScreenPointToRay(_mousePosition);
        _toAffect.position = _worldPosition;
        _toAffect.forward = _ray.direction;


    }
}
