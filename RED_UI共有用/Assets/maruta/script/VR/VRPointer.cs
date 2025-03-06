using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;

public class VRPointer : MonoBehaviour
{
    public XRController controller; // コントローラーのXR Controllerを設定
    public GameObject pointer; // ポインターとして使用するGameObject
    public float maxDistance = 10f; // ポインターの最大距離

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = pointer.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(controller.transform.position, controller.transform.forward);

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            lineRenderer.SetPosition(0, controller.transform.position);
            lineRenderer.SetPosition(1, hit.point);

            // UIがヒットした場合の処理
            if (hit.collider.CompareTag("UI"))
            {
                // UI操作
                if (controller.selectInteractionState.activatedThisFrame)
                {
                    ExecuteEvents.Execute(hit.collider.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
                }
            }
        }
        else
        {
            lineRenderer.SetPosition(1, ray.GetPoint(maxDistance));
        }
    }
}

