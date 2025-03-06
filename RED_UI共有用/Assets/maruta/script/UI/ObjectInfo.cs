using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic; // リストを使用するために追加

public class ObjectInfo : MonoBehaviour
{
    public GameObject UI;
    public TMP_Text infoText;
    public string targetIPAddress;
    public string GroupName;
    private NavMeshAgent agent;
    public float interval = 5f;
    public Button moveButton;
    private int n = 0;

    private float innerRadius;
    private float outerRadius;

    public GameObject targetObject;    // 移動ターゲット
    public List<GameObject> targetObjects1 = new List<GameObject>();   // 停止をトリガーするオブジェクトのリスト

    public GameObject cube1;
    public GameObject cube2;
    public GameObject cube3;
    public GameObject cube4;
    public GameObject cube5;

    public GameObject Group_IP_dropdown;
    private TMP_Dropdown dropdown;

    private int ButtonPressed = 0;
    private Coroutine moveCoroutine;
    private int stackvalue = 0;
    private bool movePaused = false;  // Moveの停止状態を管理

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgentがアタッチされていません。");
            return;
        }
        SetTargetObjectBasedOnGroupName(GroupName);
        n = 1;
        agent.enabled = false;
        moveButton.onClick.AddListener(move);
        dropdown = Group_IP_dropdown.GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        UI.SetActive(false);
        infoText.gameObject.SetActive(false);

        // targetObject1をリストに追加
        targetObjects1.Add(cube1);
        targetObjects1.Add(cube2);
        targetObjects1.Add(cube3);
        targetObjects1.Add(cube4);
        targetObjects1.Add(cube5);
    }

    private void SetTargetObjectBasedOnGroupName(string GroupName)
    {
        if (GroupName == "RED_Test1")
        {
            targetObject = cube1;
            Debug.Log("ターゲットオブジェクトに cube1 を設定しました。");
        }
        else if (GroupName == "RED_Test2")
        {
            targetObject = cube2;
            Debug.Log("ターゲットオブジェクトに cube2 を設定しました。");
        }
        else if (GroupName == "RED_Test3")
        {
            targetObject = cube3;
            Debug.Log("ターゲットオブジェクトに cube3 を設定しました。");
        }
        else if (GroupName == "RED_Test4")
        {
            targetObject = cube4;
            Debug.Log("ターゲットオブジェクトに cube4 を設定しました。");
        }
        else if (GroupName == "RED_Test5")
        {
            targetObject = cube5;
            Debug.Log("ターゲットオブジェクトに cube5 を設定しました。");
        }
        else
        {
            Debug.Log("指定されたグループ名に対応するターゲットオブジェクトがありません。");
        }
    }

    private void Update()
    {
        if (ShouldUpdateData())
        {
            ReadEntireData();
        }
        if (targetIPAddress == "192.168.1.400" || targetIPAddress == "192.168.1.401" || targetIPAddress == "192.168.1.402" || targetIPAddress == "192.168.1.403" || targetIPAddress == "192.168.1.404" || targetIPAddress == "192.168.1.405")
        {
            foreach (var target in targetObjects1)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                if (distanceToTarget <= 7f)
                {
                    //NavMeshAgent agent = cube.GetComponent<NavMeshAgent>();
                    if (agent != null&&agent.enabled)
                    {
                        agent.isStopped = true;  // 移動停止
                        if (moveCoroutine != null)
                        {
                            StopCoroutine(moveCoroutine);
                            moveCoroutine = null;
                        }
                        
                        //Debug.Log($"Cube 4 is within 5f of {target.name}. Stopping movement.");
                    }
                }
            }
        }
            // リスト内の各targetObject1との距離を取得して、範囲内に入ったら移動停止

        if (Input.GetKeyDown(KeyCode.Space) && n == 1 && stackvalue != 1) // stackvalueが1でない場合のみ移動開始
        {
            move();
        }
    }

    private bool ShouldUpdateData()
    {
        return true;
    }

    private void OnDropdownValueChanged(int index)
    {
        string selectedIPAddress = dropdown.options[index].text;
        UpdateUI($"選択されたIP: {selectedIPAddress}");
    }

    public void move()
    {
        if (n == 1 && stackvalue != 1) // stackvalueが1でない場合のみ移動
        {
            Debug.Log("111111");
            agent.enabled = true;

            if (moveCoroutine == null)
            {
                moveCoroutine = StartCoroutine(MoveToRandomPosition());
            }
        }
    }

    void ReadEntireData()
    {
        if (SaveInformation.exploreList.Count > 0)
        {
            string filteredInfo = "";
            foreach (var deviceInfo in SaveInformation.exploreList)
            {
                if (deviceInfo.name == targetIPAddress)
                {
                    n = 1;
                    outerRadius = deviceInfo.value1;
                    innerRadius = deviceInfo.value2;
                    string connectionStatus = (stackvalue == 1) ? "stack" : "normal"; // stackvalueに応じて変更

                    Debug.Log($"outerRadius: {outerRadius}, innerRadius: {innerRadius}");
                    filteredInfo += $"IP: {targetIPAddress}\n" +
                        $"connect: {connectionStatus}\n" + // connectionStatusを変更
                                    $"inner: {innerRadius}\n" +
                                    $"outer: {outerRadius}\n";
                }
            }

            if (!string.IsNullOrEmpty(filteredInfo))
            {
                UpdateUI(filteredInfo);
            }
            else
            {
                Debug.LogWarning("該当するデバイス情報が見つかりませんでした。");
            }
        }
        else
        {
            Debug.LogWarning("SaveInformation.exploreList は空です。");
        }
    }

    private void UpdateUI(string filteredInfo)
    {
        if (infoText != null)
        {
            infoText.text = filteredInfo;
            UI.SetActive(true);
        }
        else
        {
            Debug.LogWarning("infoTextが設定されていません。");
        }
    }

    private IEnumerator MoveToRandomPosition()
    {
        Debug.Log("222222222");
        while (true)
        {
            Vector3 targetPosition = GetRandomPosition();
            agent.SetDestination(targetPosition);

            while (agent.pathPending || agent.remainingDistance > 1f)
            {
                yield return null;
            }

            yield return new WaitForSeconds(interval);
        }
    }

    Vector3 GetRandomPosition()
    {
        innerRadius = 2f;
        outerRadius = 5f;
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere;
        float randomDistance = UnityEngine.Random.Range(innerRadius, outerRadius);
        Vector3 targetPosition = targetObject.transform.position + randomDirection.normalized * randomDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, outerRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return targetObject.transform.position;
    }
} 