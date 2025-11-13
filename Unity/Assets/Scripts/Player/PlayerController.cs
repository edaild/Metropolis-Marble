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
    
    private int currentPositionIndex;

    public void Awake()
    {
        if(player_collider == null)
            player_collider = GetComponent<Collider>();
    }

    private void Start()
    {
        initialY = transform.position.y;
        Debug.Log(boardPositions.Count);
    }

    private void Update()
    {
        if (isMoving == true && targetPosition != null)
            HandleMove();
        

        if(transform.position == fnisyPlayerPosition)
            isMoving = false;

        if (Input.GetKeyDown(KeyCode.N) && !isMoving)
        {
            int diceResult = Random.Range(1, 7);
            Debug.Log($"주사위 결과: {diceResult}");

            int nextIndex = (currentPositionIndex + diceResult) % boardPositions.Count;

            targetPosition = boardPositions[nextIndex];
           isMoving = true;
        }

     
    }

    private void HandleMove()
    {
        player_collider.enabled = false;

        float fixedY = initialY;

        float step = playerSpeed * Time.deltaTime;

        Vector3 targetPositionFixedY = new Vector3(targetPosition.position.x, fixedY, targetPosition.position.z);

        transform.position = Vector3.MoveTowards(transform.position, targetPositionFixedY, step);

        if (transform.position == targetPositionFixedY)
        {
            isMoving = false;
            Debug.Log("목표 지점 도착 완료.");
            player_collider.enabled = true;
        }
            
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            Debug.Log("목적지와 충돌");
            BlockData blockData = collision.gameObject.GetComponent<BlockData>();

            if(blockData == null)
            {
                Debug.LogError($"'{collision.gameObject.name}' 에 컴포넌트가 없음");
                return;
            }

            if (blockData.blockSO == null)
            {
                Debug.LogError($"'{collision.gameObject.name}' SO 에셋이 연결되지 않음!");
                return;
            }
            string name = blockData.blockSO.Block_name;
            int price = blockData.blockSO.Block_price;

            Debug.Log($" 현재 위치 블럭 이름: {name}, 가격:{price}");
        }
    }
}
