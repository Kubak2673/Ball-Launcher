using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class BallHandler : MonoBehaviour
{
    [SerializeField] private GameObject ballPreFab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float respawnDelay;
    [SerializeField] private float DetachDelay;
    private Rigidbody2D currentBallRigidbody;
    private SpringJoint2D currentBallSprintJoint;
    private Camera mainCamera;
    private bool isDragging;
    void Start()
    {
        mainCamera = Camera.main;
        SpawnNewBall();
    }
    void Update()
    {
        if(currentBallRigidbody == null)
        {
            return;
        }
        if(!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if(isDragging)
            {
                LaunchBall();
            }
            isDragging = false;
            currentBallRigidbody.isKinematic = false;
            return;
        }
        isDragging = true;
        currentBallRigidbody.isKinematic = true;
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
        currentBallRigidbody.position = worldPosition;
    }
    private void SpawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPreFab, pivot.position, Quaternion.identity);
        currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSprintJoint = ballInstance.GetComponent<SpringJoint2D>();
        currentBallSprintJoint.connectedBody = pivot;
    }
    private void LaunchBall()
    {
        currentBallRigidbody.isKinematic = false;
        currentBallRigidbody = null;
        Invoke(nameof(DetachBall), DetachDelay);
    }
    private void DetachBall()
    {
        currentBallSprintJoint.enabled = false;
        currentBallSprintJoint = null;
        Invoke(nameof(SpawnNewBall), respawnDelay);
    }
}
