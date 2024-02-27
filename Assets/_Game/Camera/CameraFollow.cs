using _Framework.Pool.Scripts;
using System.Collections;
using UnityEngine;


public class CameraFollow : GameUnit
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Camera mainCamera;

    public void OnInit()
    {
        offset = new Vector3(0, 12f, -10f);
    }

    private void LateUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(TF.position, desiredPosition, smoothSpeed);
        TF.position = smoothedPosition;
    }
        
    public void PowerUp()
    {
        //StartCoroutine(IncreaseFieldOfView(mainCamera.fieldOfView + 5, 1f));
        StartCoroutine(MovePosition(TF.position - TF.forward * 3f, 1f));
    }

    public IEnumerator IncreaseFieldOfView(float targetVaule, float duration)
    {
        float time = 0;

        float startVaule = mainCamera.fieldOfView;

        while (time < duration)
        {
            mainCamera.fieldOfView = Mathf.Lerp(startVaule, targetVaule, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        mainCamera.fieldOfView = targetVaule;
    }
    public IEnumerator MovePosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = TF.position;

        while (time < duration)
        {
            TF.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            Follow();
            yield return null;
        }
        offset += targetPosition - startPosition;
        //TF.position = targetPosition;
    }
}
