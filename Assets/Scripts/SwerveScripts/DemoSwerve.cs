using UnityEngine;
using Listeners;
using System;
using DG.Tweening;

public class DemoSwerve : MonoBehaviour
{
    [SerializeField] private SwerveListener swerveListener;
    [SerializeField] GameObject PlayerParent;
    public Transform target;
    public float SwerveSpeed;
    public float SwerveMoveRange = 1.5f;
    public float ForwardSpeed;
    public bool IsXMove;

    bool isOnBorder;
    private void OnEnable()
    {
        swerveListener.RunFunction += Swerve;
    }

    private void OnDisable()
    {
        swerveListener.RunFunction -= Swerve;
    }

    private void Update()
    {
        if (GameManager.Instance.started && target)
        {
            Camera.main.transform.parent.position = Vector3.Lerp(Camera.main.transform.parent.position, target.transform.position, Time.deltaTime * 50f);

            PlayerParent.transform.Translate(ForwardSpeed * Time.deltaTime * PlayerParent.transform.forward, Space.World);
        }

        BorderControl();
    }

    private void Swerve(float xSwerveValue, float ySwerveValue)
    {
        if (!IsXMove)
        {
            if ((xSwerveValue < 0 && target.transform.localPosition.x >= SwerveMoveRange * -1) || (xSwerveValue > 0 && target.transform.localPosition.x <= SwerveMoveRange))
            {
                target.transform.Translate(SwerveSpeed * xSwerveValue * transform.right / 2 * Time.maximumDeltaTime);
            }
        }
    }

    private void BorderControl()
    {
        if (target)
        {
            target.transform.position = new Vector3(
                target.transform.position.x >= SwerveMoveRange ? SwerveMoveRange : (target.transform.position.x <= -SwerveMoveRange ? -SwerveMoveRange : target.transform.position.x),
                target.transform.position.y,
                target.transform.position.z);
        }
    }
}