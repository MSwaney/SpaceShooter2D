using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private Vector3 _startPos;
    [SerializeField]
    private GameObject _backgroundObject;



    void Start()
    {
        _startPos = transform.position;
    }

    void Update()
    {
        BackgroundMover("Nebula", -29.28f);
        BackgroundMover("BigStar", -28.01f);
        BackgroundMover("LittleStar", -26.92f);
    }

    private void BackgroundMover(string tag, float resetPosY)
    {
        CalculateMovement();
        if (_backgroundObject.tag == tag && transform.position.y < resetPosY)
        {
            ResetPosition();
        }
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void ResetPosition()
    {
        transform.position = _startPos;
    }
}
