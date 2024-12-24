using UnityEngine;
using Dreamteck.Splines;

public class CarSplineFollowerDetection : MonoBehaviour
{
    [SerializeField] private SplineFollower _follower;
    [SerializeField] private DetectionZone _detectionZone;
    [SerializeField] private float _maxFollowSpeed = 3f;
    [SerializeField] private float _accelerationRate = 1f;
    [SerializeField] private float _decelerationRate = 2f;

    private void Awake()
    {
        if (_follower == null)
        {
            _follower = GetComponent<SplineFollower>();
            if (_follower == null)
            {
                Debug.LogError("SplineFollower not found on the object.");
            }
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
        if (_follower != null && _follower.followSpeed > 0)
        {
            _follower.followSpeed = Mathf.Max(0, _follower.followSpeed - _decelerationRate * Time.deltaTime);
        }
    }

    private void MoveSmoothly()
    {
        if (_follower != null && _follower.followSpeed < _maxFollowSpeed)
        {
            _follower.followSpeed = Mathf.Min(_maxFollowSpeed, _follower.followSpeed + _accelerationRate * Time.deltaTime);
        }
    }
}