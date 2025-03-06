using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.UI;  // Imageコンポーネントを使うために追加
using System.Collections.Generic;

public class VRDragAndDrop : MonoBehaviour
{
    public InputActionReference rightHandGripAction;  // Gripボタンの入力アクション
    public LineRenderer rightHandLineRenderer;  // 右手のLineRenderer
    public List<GameObject> draggableUIElements;  // 移動可能なUI要素のリスト
    public List<GameObject> selectableUIElements;  // 選択可能なUI要素のリスト
    private int grabstatus = 0;
    private GameObject firstSelectedObject = null;  // 最初に選択したオブジェクト
    public List<GameObject> DestinationObjects;  // 目的地の空オブジェクト
    public List<GameObject> CubeObjects;  // 群中心キューブ
    public List<GameObject> DestinationImage; // 探索割合の色分け表示
    public List<GameObject> stackObjects1 = new List<GameObject>();
    public List<GameObject> RobotWorkStatusImage;//ロボットの動作状況

    [Header("Agent Settings")]
    [Tooltip("Speed of the agent when moving towards the destination")]
    public float agentspeed = 2f;  // NavMeshAgentの速度

    // firstSelectedObject をキーにし、(hitObject, LineRenderer) のセットを管理
    private Dictionary<GameObject, (GameObject target, LineRenderer line)> activeLines = new Dictionary<GameObject, (GameObject, LineRenderer)>();

    private void OnEnable()
    {
        rightHandGripAction.action.performed += ctx => HandleGripAction();
    }

    private void OnDisable()
    {
        rightHandGripAction.action.performed -= ctx => HandleGripAction();
    }

    private void HandleGripAction()
    {
        if (rightHandLineRenderer == null || rightHandLineRenderer.positionCount < 2)
        {
            Debug.LogWarning("LineRenderer is not set up correctly.");
            return;
        }

        Vector3 rayStart = rightHandLineRenderer.GetPosition(0);
        Vector3 rayEnd = rightHandLineRenderer.GetPosition(rightHandLineRenderer.positionCount - 1);
        Vector3 rayDirection = (rayEnd - rayStart).normalized;

        if (Physics.Raycast(rayStart, rayDirection, out RaycastHit hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            // ドラッグ可能なUI要素に当たった場合
            if (draggableUIElements.Contains(hitObject))
            {
                if (activeLines.ContainsKey(hitObject))
                {
                    // 既存のラインを削除
                    Destroy(activeLines[hitObject].line.gameObject);
                    activeLines.Remove(hitObject);
                    Debug.Log("Line removed from draggable UI Element: " + hitObject.name);
                    grabstatus = 0;
                    firstSelectedObject = null;
                }
                else
                {
                    // 新しいオブジェクトを選択
                    grabstatus = 1;
                    firstSelectedObject = hitObject;
                    Debug.Log("Draggable UI Element grabbed: " + hitObject.name);
                }
                return;
            }

            // すでに選択済みで、接続可能なUI要素に当たった場合
            if (grabstatus == 1 && selectableUIElements.Contains(hitObject) && firstSelectedObject != null)
            {
                DrawLineBetweenObjects(firstSelectedObject, hitObject);
                grabstatus = 0;
                firstSelectedObject = null;
                Debug.Log("Line drawn between objects.");
                return;
            }
        }
    }

    private void DrawLineBetweenObjects(GameObject obj1, GameObject obj2)
    {
        // LineRendererを新規作成
        LineRenderer line = new GameObject("Line").AddComponent<LineRenderer>();

        // obj1の子オブジェクトとして設定
        line.transform.SetParent(obj1.transform);

        // ラインの設定
        line.positionCount = 2;
        line.SetPosition(0, obj1.transform.position);
        line.SetPosition(1, obj2.transform.position);

        // ラインの見た目
        line.startWidth = 0.02f;
        line.endWidth = 0.02f;
        line.material = new Material(Shader.Find("Sprites/Default"));

        // activeLines に登録 (obj1 をキーにして obj2 と LineRenderer を保存)
        activeLines[obj1] = (obj2, line);

        Debug.Log($"Line drawn between objects: {obj1.name} → {obj2.name}");

        // CubeObjects を DestinationObjects に移動
        int draggableIndex = draggableUIElements.IndexOf(obj1);
        int selectableIndex = selectableUIElements.IndexOf(obj2);

        if (draggableIndex >= 0 && selectableIndex >= 0 && draggableIndex < CubeObjects.Count && selectableIndex < DestinationObjects.Count)
        {
            GameObject cube = CubeObjects[draggableIndex];
            GameObject destination = DestinationObjects[selectableIndex];

            NavMeshAgent agent = cube.GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                agent = cube.AddComponent<NavMeshAgent>();
            }

            // エージェントの速度をインスペクターで設定できるようにする
            agent.speed = agentspeed;

            agent.destination = destination.transform.position;

            Debug.Log($"Moving CubeObject {cube.name} to DestinationObject {destination.name}");
        }
    }

    void Update()
    {
        // activeLines 内の各 LineRenderer の位置を更新
        foreach (var linePair in activeLines)
        {
            GameObject obj1 = linePair.Key;
            GameObject obj2 = linePair.Value.target;
            LineRenderer line = linePair.Value.line;

            if (obj1 != null && obj2 != null && line != null)
            {
                line.SetPosition(0, obj1.transform.position);
                line.SetPosition(1, obj2.transform.position);
            }
        }

        // CubeとDestination間の距離を測定し、DestinationImageの色を変更
        for (int j = 0; j < DestinationObjects.Count && j < DestinationImage.Count; j++)
        {
            GameObject destination = DestinationObjects[j];
            bool isCloseToAnyCube = false; // 近くにCubeがあるかどうかのフラグ

            // すべてのCubeObjectsとの距離をチェック
            for (int i = 0; i < CubeObjects.Count; i++)
            {
                GameObject cube = CubeObjects[i];
                float distance = Vector3.Distance(cube.transform.position, destination.transform.position);

                // もし3f以内のCubeがあればフラグを立てる
                if (distance <= 3f)
                {
                    isCloseToAnyCube = true;
                    break; // 1つでも3f以内のCubeがあればそれ以上チェックしない
                }
            }

            // DestinationImageがImageコンポーネントを持つUI要素である場合
            Image destinationImageComponent = DestinationImage[j].GetComponent<Image>();
            if (destinationImageComponent != null)
            {
                // すでに緑なら何もしない
                if (destinationImageComponent.color != Color.green)
                {
                    Color newColor = isCloseToAnyCube ? Color.green : Color.yellow;
                    destinationImageComponent.color = newColor;  // 色を変更
                }
            }
        }


        //ロボットの動作状況の色別表示
        for (int i = 0; i < CubeObjects.Count && i < RobotWorkStatusImage.Count; i++)
        {
            GameObject cube = CubeObjects[i];
            NavMeshAgent agent = cube.GetComponent<NavMeshAgent>();

            if (agent != null)
            {
                // NavMeshが動作中か停止中かを判定
                bool isMoving = !agent.isStopped;

                // 色の設定 (動作中なら青、停止中なら赤)
                Color workStatusColor = isMoving ? Color.blue : Color.red;

                // RobotWorkStatusImageがImageコンポーネントを持つUI要素である場合
                Image workStatusImageComponent = RobotWorkStatusImage[i].GetComponent<Image>();
                if (workStatusImageComponent != null)
                {
                    workStatusImageComponent.color = workStatusColor;  // 色を変更
                }
            }
        }

        // CubeObjectのNavMesh移動停止をチェック
        foreach (var cube in CubeObjects)
        {
            // "Cube 4"の名前のオブジェクトを見つけた場合
            if (cube.name == "Cube 4")
            {
                foreach (var target in stackObjects1)
                {
                    // CubeとstackObjects1間の距離を計算
                    float distanceToTarget = Vector3.Distance(cube.transform.position, target.transform.position);

                    // もしCube 4がstackObjects1の周囲5f以内にいる場合、NavMeshAgentを停止
                    if (distanceToTarget <= 5f)
                    {
                        NavMeshAgent agent = cube.GetComponent<NavMeshAgent>();
                        if (agent != null)
                        {
                            agent.isStopped = true;  // 移動停止
                            Debug.Log($"Cube 4 is within 5f of {target.name}. Stopping movement.");
                        }
                    }
                }
            }
        }
    }
}