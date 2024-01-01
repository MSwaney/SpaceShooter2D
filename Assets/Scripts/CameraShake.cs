using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float _shakeDuration = 0.5f;
    [SerializeField]
    private float _shakeMagnitude = 0.1f;

    private Vector3 _originalPosition;

    private bool _shaking;

    void Start()
    {
        _originalPosition = transform.position;
    }

    public void Shake()
    {
        if (!_shaking)
        {
            StartCoroutine(ShakeRoutine());
        }
    }

    private IEnumerator ShakeRoutine()
    {
        _shaking = true;
        float elapsed = 0f;

        while (elapsed < _shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * _shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * _shakeMagnitude;
            transform.position = new Vector3(_originalPosition.x + offsetX, _originalPosition.y + offsetY, _originalPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = _originalPosition;
        _shaking = false;
    }
}
