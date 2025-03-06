using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Explorer : MonoBehaviour
{
    public NavMeshAgent agent;
    public List<Vector3> exploredLocations = new List<Vector3>();  // 自ロボットの探索済み場所
    public static List<Vector3> globalExploredLocations = new List<Vector3>();  // 全ロボットの探索済み場所
    public float moveSpeed = 3f;
    private bool isExploring = true;  // 探索中フラグ
    private Vector3 lastPosition;
    private float checkInterval = 2f;  // 分岐点をチェックする間隔
    private float timeSinceLastCheck = 0f;  // 最後にチェックした時間
    private float minimumDistance = 5f;  // 他のロボットとの最小距離

    public List<Explorer> otherExplorers;  // 他のロボットの参照を格納するリスト

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.autoBraking = false;
        lastPosition = transform.position;
        StartExploring();
    }

    void Update()
    {
        if (isExploring)
        {
            // 移動したら位置を記録
            if (Vector3.Distance(transform.position, lastPosition) > 1f)
            {
                lastPosition = transform.position;
                exploredLocations.Add(transform.position);  // 自ロボットの探索済みリスト
                globalExploredLocations.Add(transform.position);  // 全ロボットで探索済み場所を共有
            }

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                // 次の探索先に進む
                MoveToNextPoint();
            }
        }

        // 分岐点をチェックするタイミングを制御
        timeSinceLastCheck += Time.deltaTime;
        if (timeSinceLastCheck >= checkInterval)
        {
            timeSinceLastCheck = 0f;
            CheckForSplitPaths();
        }
    }

    void StartExploring()
    {
        // 初期の移動先を設定
        MoveToNextPoint();
    }

    void MoveToNextPoint()
    {
        Vector3 nextPosition = GetNextPosition();
        if (nextPosition != Vector3.zero)
        {
            agent.SetDestination(nextPosition);  // 新しい移動先
        }
        else
        {
            // 行き止まりの場合、戻る
            isExploring = false;  // 探索を停止
            StartCoroutine(ReturnToStartPosition());
        }
    }

    Vector3 GetNextPosition()
    {
        // 次の移動先が未探索であり、ナビメッシュ上にある場所を探す
        Vector3[] potentialLocations = new Vector3[]
        {
            transform.position + transform.forward * 10f,  // 前進
            transform.position + transform.right * 10f,  // 右方向
            transform.position + transform.right * -10f  // 左方向
        };

        List<Vector3> validPaths = new List<Vector3>();

        // 各方向に進むための位置を確認
        foreach (var location in potentialLocations)
        {
            if (!globalExploredLocations.Contains(location) && NavMesh.SamplePosition(location, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                // 他のロボットと距離が十分に取れるかチェック
                if (IsFarEnoughFromOtherExplorers(hit.position))
                {
                    validPaths.Add(hit.position);
                }
            }
        }

        if (validPaths.Count > 0)
        {
            // ランダムに方向を選ぶ
            Vector3 selectedPath = validPaths[Random.Range(0, validPaths.Count)];
            return selectedPath;
        }

        return Vector3.zero;  // 探索可能な場所がない場合
    }

    // 他のロボットと十分に距離が取れているか確認
    bool IsFarEnoughFromOtherExplorers(Vector3 position)
    {
        foreach (var explorer in otherExplorers)
        {
            if (explorer != this && Vector3.Distance(position, explorer.transform.position) < minimumDistance)
            {
                return false;  // 他のロボットが近すぎる場合はNG
            }
        }

        return true;  // 近くにロボットがいなければOK
    }

    // 分岐点を検出してロボットをランダムに別の方向へ進ませる
    void CheckForSplitPaths()
    {
        Vector3[] directions = new Vector3[]
        {
            transform.position + transform.forward * 10f,
            transform.position + transform.right * 10f,
            transform.position + transform.right * -10f
        };

        List<Vector3> validPaths = new List<Vector3>();
        foreach (var direction in directions)
        {
            if (!globalExploredLocations.Contains(direction) && NavMesh.SamplePosition(direction, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                if (IsFarEnoughFromOtherExplorers(hit.position))  // 他のロボットと十分に距離が取れる場合
                {
                    validPaths.Add(hit.position);
                }
            }
        }

        if (validPaths.Count > 0)
        {
            // ランダムに方向を選ぶ
            Vector3 selectedPath = validPaths[Random.Range(0, validPaths.Count)];
            agent.SetDestination(selectedPath);  // ランダムな方向に進む
            Debug.Log($"分岐で選択された場所: {selectedPath}");
        }
    }

    System.Collections.IEnumerator ReturnToStartPosition()
    {
        // 行き止まりで戻る処理
        Vector3 startPosition = exploredLocations[0];  // 初期位置に戻る
        agent.SetDestination(startPosition);

        while (Vector3.Distance(transform.position, startPosition) > 0.1f)
        {
            yield return null;  // 目的地に到達するまで待機
        }

        isExploring = true;
        exploredLocations.Clear();  // 再探索用に探索履歴をリセット
        globalExploredLocations.Clear();  // 全ロボットの探索履歴もリセット
        StartExploring();  // 再度探索を開始
    }
}
