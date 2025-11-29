using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 5f;
    public Transform targetPosition;
    public float initialY;
    public List<Transform> boardPositions;
    public Vector3 fnisyPlayerPosition;
    public bool isMoving;
    public Collider player_collider;
    public int currentPositionIndex;
    public int diceResult;
    public GameObject dicePrefab;
    public Transform diceSpawnPoint;
    private bool isreturn;

    // ★추가: 돈/플레이어 번호/땅 색
    public int playerId = 0;
    public int money = 1000000;
    public Color ownerColor = Color.blue;

    // ★추가: 여러 플레이어 간 통행료 주고받기용
    public static List<PlayerController> allPlayers = new List<PlayerController>();

    public static PlayerController GetPlayerById(int id)
    {
        return allPlayers.Find(p => p.playerId == id);
    }

    public void Awake()
    {
        if (player_collider == null)
            player_collider = GetComponent<Collider>();

        // ★추가: 리스트에 자신 등록
        if (!allPlayers.Contains(this))
            allPlayers.Add(this);
    }

    // ★추가: 파괴될 때 리스트에서 제거
    private void OnDestroy()
    {
        if (allPlayers.Contains(this))
            allPlayers.Remove(this);
    }

    private void Start()
    {
        initialY = transform.position.y;
        Debug.Log(boardPositions.Count);
        Debug.Log("initialY");
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.N) && !isMoving)
        {
            diceResult = Random.Range(1, 7);
            Debug.Log($"주사위 수는?: {diceResult}");

            if (dicePrefab != null && diceSpawnPoint != null)
                Instantiate(dicePrefab, diceSpawnPoint.position, Quaternion.identity);

            StartCoroutine(MoveTiles(diceResult));
        }
    }

    IEnumerator MoveTiles(int steps)
    {
        isMoving = true;
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < steps; i++)
        {
            int nextIndex = (currentPositionIndex + 1) % boardPositions.Count;
            targetPosition = boardPositions[nextIndex];

            yield return StartCoroutine(MoveToNextTile());

            currentPositionIndex = nextIndex;
        }
    }

    IEnumerator MoveToNextTile()
    {
        player_collider.enabled = false;
        float fixedY = initialY;
        Vector3 targetPositionFixedY = new Vector3(targetPosition.position.x, fixedY, targetPosition.position.z);
        while (Vector3.Distance(transform.position, targetPositionFixedY) > 0.001f)
        {
            float step = playerSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPositionFixedY, step);
            yield return null;
        }
        transform.position = targetPositionFixedY;
        player_collider.enabled = true;
        isMoving = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            Debug.Log("블럭과 충돌");
            BlockData blockData = collision.gameObject.GetComponent<BlockData>();

            if (blockData == null) return;
            if (blockData.blockSO == null) return;

            string name = blockData.blockSO.Block_name;
            int price = blockData.blockSO.Block_price;

            Debug.Log($"현재 블럭 이름은 : {name}, 가격은:{price}");

            // ★★★ 여기부터 추가 로직 ★★★

            // Start 칸이면 아무 것도 안 함 (BlockType.Start 쓰고 있으면)
            if (blockData.blockSO.blockType == BlockType.Start)
                return;

            // 1) 아무도 안 산 땅 → 구매 UI
            if (blockData.ownerId == -1)
            {
                if (money >= blockData.Price && TilePurchaseUI.Instance != null)
                {
                    TilePurchaseUI.Instance.OpenBuy(blockData, this);
                }
                else
                {
                    Debug.Log($"{blockData.BlockName} 을(를) 살 돈이 부족합니다.");
                }
            }
            // 2) 내 땅
            else if (blockData.ownerId == playerId)
            {
                Debug.Log($"{blockData.BlockName} 은(는) 내 땅입니다.");
            }
            // 3) 남의 땅 → 통행료
            else
            {
                PayToll(blockData);
            }
        }
    }

    // ★추가: 남의 땅 도착 시 통행료
    private void PayToll(BlockData blockData)
    {
        int toll = blockData.Toll;

        money -= toll;
        Debug.Log($"{blockData.BlockName} 통행료 {toll} 지불. 남은 돈: {money}");

        PlayerController owner = GetPlayerById(blockData.ownerId);
        if (owner != null)
        {
            owner.money += toll;
            Debug.Log($"플레이어 {owner.playerId} 가 통행료 {toll} 획득. 현재 돈: {owner.money}");
        }

        // TODO: 돈 표시 UI 있으면 여기서 갱신
    }

    // ★추가: 빈 땅 구매
    public void BuyBlock(BlockData blockData)
    {
        if (blockData.ownerId != -1)
        {
            Debug.Log("이미 소유자가 있는 땅입니다.");
            return;
        }

        if (money < blockData.Price)
        {
            Debug.Log("돈이 부족해서 구매할 수 없습니다.");
            return;
        }

        money -= blockData.Price;
        blockData.ownerId = playerId;

        // 색 바꾸고 싶으면
        if (blockData.TryGetComponent<Renderer>(out var rend))
        {
            rend.material.color = ownerColor;
        }

        blockData.BuildHouse(ownerColor);

        Debug.Log($"{blockData.BlockName} 구매 완료! 남은 돈: {money}");
        // TODO: 돈 UI 갱신
    }
}
