using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class VRPointerController : MonoBehaviour
{
    public Camera mapCamera;           // ミニマップのカメラ
    public RawImage rawImage;          // マップ表示用のRawImage
    public GameObject agentObject;     // 移動させるオブジェクト
    private NavMeshAgent agent;

    public Transform rightController;  // 右手コントローラー
    public InputActionReference rightHandGripAction;  // VRコントローラーの入力アクション

    public LineRenderer lineRenderer;  // LineRenderer

    void Start()
    {
        agent = agentObject.GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent が見つかりません。");
        }
    }

    void Update()
    {
        // コントローラーの入力状態を取得
        if (rightHandGripAction.action.WasPressedThisFrame())
        {
            //Debug.Log("Grip ボタンが押されました");

            // LineRendererを使って右手コントローラーからレイを発射
            Vector3 rayOrigin = rightController.position;
            Vector3 rayDirection = rightController.forward;

            Ray ray = new Ray(rayOrigin, rayDirection);

            // RawImageにヒットするか判定
            RaycastHit hit;  // RaycastHitを先に宣言
            if (Physics.Raycast(ray, out hit))
            {
                // ヒットした位置を取得
                //Debug.Log($"LineRendererがRawImageにヒットしました: {hit.point}");

                // ヒットしたワールド座標をスクリーン座標に変換
                Vector3 screenPos = mapCamera.WorldToScreenPoint(hit.point);
                //Debug.Log($"スクリーン座標: {screenPos}");

                // スクリーン座標をRawImageのローカル座標に変換
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    rawImage.rectTransform,
                    screenPos,
                    mapCamera,
                    out localPoint
                );
                //Debug.Log($"RawImage上のクリック位置 (ローカル座標): {localPoint}");

                // ローカル座標を正規化されたUV座標に変換
                Vector2 normalizedPosition = localPoint / rawImage.rectTransform.sizeDelta;
                //Debug.Log($"正規化された位置 (UV座標): {normalizedPosition}");

                // UV座標をスクリーン座標に変換
                Vector3 finalScreenPos = mapCamera.ViewportToScreenPoint(new Vector3(normalizedPosition.x, normalizedPosition.y, 0f));
                //Debug.Log($"最終スクリーン座標: {finalScreenPos}");

                // スクリーン座標をワールド座標に変換
                Ray mapRay = mapCamera.ScreenPointToRay(finalScreenPos);
                RaycastHit mapHit;  // mapRay用のRaycastHitを宣言
                if (Physics.Raycast(mapRay, out mapHit, 100f))  // mapRayを使ってレイキャスト
                {
                    //Debug.Log("レイキャストでヒットしました");
                    // ヒットした位置をターゲット位置として設定
                    Vector3 targetPosition = mapHit.point;

                    // x と z 座標を調整
                    targetPosition.x += 12.58f;
                    targetPosition.z += 11.655f;

                    // NavMeshAgent を使って指定した位置に移動
                    if (agent != null)
                    {
                        //Debug.Log($"ターゲット位置に移動: {targetPosition}");
                        agent.SetDestination(targetPosition);
                    }
                    else
                    {
                        //Debug.LogError("NavMeshAgent が見つかりません");
                    }
                }
                else
                {
                    //Debug.LogWarning("mapRayでレイキャストがヒットしませんでした");
                }
            }
        }

        // LineRendererをコントローラーの位置に合わせて更新
        lineRenderer.SetPosition(0, rightController.position);
        lineRenderer.SetPosition(1, rightController.position + rightController.forward * 10f);  // 適当な距離
    }
}
