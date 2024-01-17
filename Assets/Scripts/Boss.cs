using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
                     private bool _isDead;
    [SerializeField] private float _figureEightRadius;
    [SerializeField] private float _figureEightTime;
    [SerializeField] private float _speed;

    void Start()
    {
        StartCoroutine(MoveOntoScreen());
    }

    void Update()
    {
        
    }

    private IEnumerator MoveOntoScreen()
    {
        Vector3 targetPostion = new Vector3(0.75f, 5f, 0f);
        while (transform.position.y > targetPostion.y)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            yield return null;
        }
        StartCoroutine(FigureEightMovement());
    }

    private IEnumerator FigureEightMovement()
    {
        float elapsedTime = 0f;
        Vector3 centerPosition = transform.position;

        while (!_isDead)
        {
            float x = centerPosition.x + Mathf.Sin(elapsedTime / _figureEightTime) * _figureEightRadius;
            float y = centerPosition.y - Mathf.Sin(elapsedTime * 2f / _figureEightTime) * (_figureEightRadius / 2f);

            transform.position = new Vector3(x, y, 0f);

            elapsedTime += Time.deltaTime;
            
            yield return null;
        }
    }
}
