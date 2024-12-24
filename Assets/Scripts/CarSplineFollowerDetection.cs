using UnityEngine;
using Dreamteck.Splines;

public class CarSplineFollowerDetection : MonoBehaviour
{
    [SerializeField] private SplineFollower _follower;
    [SerializeField] private DetectionZone _detectionZone;
    [SerializeField] private float _maxFollowSpeed = 3f;
    [SerializeField] private float _accelerationRate = 1f;
    [SerializeField] private float _decelerationRate = 2f;

    private bool _isForward = true;

    private void Awake()
    {
        if (_follower == null)
        {
            _follower = GetComponent<SplineFollower>();
            if (_follower == null)
            {
                Debug.LogError("SplineFollower not found on the object.");
                return;
            }
        }

        // Проверяем направление движения
        _isForward = _follower.direction == Spline.Direction.Forward;

        // Если направление обратное, изменяем знаки и разворачиваем объект
        if (!_isForward)
        {
            _maxFollowSpeed = -_maxFollowSpeed;
            _accelerationRate = -_accelerationRate;
            _decelerationRate = -_decelerationRate;

            // Разворачиваем объект
            Vector3 currentScale = transform.localScale;
            transform.localScale = new Vector3(-currentScale.x, currentScale.y, currentScale.z);
        }
    }

    private void Update()
    {
        if (_detectionZone.IsObjectDetected)
        {
            StopSmoothly();
        }
        else
        {
            MoveSmoothly();
        }
    }

    private void StopSmoothly()
    {
        if (_follower != null && Mathf.Abs(_follower.followSpeed) > 0)
        {
            _follower.followSpeed = Mathf.MoveTowards(_follower.followSpeed, 0, Mathf.Abs(_decelerationRate) * Time.deltaTime);
        }
    }

    private void MoveSmoothly()
    {
        if (_follower != null && Mathf.Abs(_follower.followSpeed) < Mathf.Abs(_maxFollowSpeed))
        {
            _follower.followSpeed = Mathf.MoveTowards(_follower.followSpeed, _maxFollowSpeed, Mathf.Abs(_accelerationRate) * Time.deltaTime);
        }
    }
}