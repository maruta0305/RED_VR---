using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class MultiCubeController : MonoBehaviour
{
    public GameObject[] cubes;                   // 操作対象のキューブ
    public ParticleSystem selectionParticle;     // 選択状態を示すパーティクル
    public InputActionReference rightHandGripAction;  // 右手のGripアクション
    public LineRenderer leftHandLineRenderer;    // 左手のLineRenderer

    private GameObject selectedCube = null;      // 選択待機状態のキューブ

    private void OnEnable()
    {
        // グリップアクションにMoveOrSelectCubeメソッドを登録
        rightHandGripAction.action.performed += ctx => MoveOrSelectCube();
    }

    private void OnDisable()
    {
        // イベントの解除
        rightHandGripAction.action.performed -= ctx => MoveOrSelectCube();
    }

    private void MoveOrSelectCube()
    {
        // レイキャスト処理
        Transform leftHandTransform = leftHandLineRenderer.transform;
        Vector3 rayOrigin = leftHandTransform.position;
        Vector3 rayDirection = leftHandTransform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (System.Array.Exists(cubes, cube => cube == hitObject)) // レイがキューブに当たった場合
            {
                // 選択状態のキューブを変更
                SetSelectedCube(hitObject);
            }
            else if (selectedCube != null) // キューブ以外の場所に当たった場合
            {
                // 選択状態のキューブを移動
                MoveSelectedCube(hit.point);
            }
        }
        else
        {
            Debug.Log("何もヒットしませんでした");
        }
    }

    private void SetSelectedCube(GameObject newSelectedCube)
    {
        if (selectedCube == newSelectedCube) return;

        // 以前の選択をリセット
        if (selectedCube != null)
        {
            var particle = selectedCube.GetComponentInChildren<ParticleSystem>();
            if (particle != null) particle.Stop();
        }

        // 新しい選択をセット
        selectedCube = newSelectedCube;
        var newParticle = selectedCube.GetComponentInChildren<ParticleSystem>();
        if (newParticle != null) newParticle.Play();

        Debug.Log("選択されたキューブ: " + selectedCube.name);
    }

    private void MoveSelectedCube(Vector3 targetPosition)
    {
        if (selectedCube == null) return;

        NavMeshAgent navMeshAgent = selectedCube.GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            navMeshAgent.SetDestination(targetPosition);
            Debug.Log($"キューブ {selectedCube.name} を移動: {targetPosition}");
        }
        else
        {
            Debug.LogError("NavMeshAgentが見つかりません: " + selectedCube.name);
        }
    }
}
