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

    public void Awake()
    {
        if(player_collider == null)
            player_collider = GetComponent<Collider>();
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
            
            if(dicePrefab != null && diceSpawnPoint != null)
                Instantiate(dicePrefab, diceSpawnPoint.position, Quaternion.identity);

            StartCoroutine(MoveTiles(diceResult));
        }
    }

    IEnumerator MoveTiles(int steps)
    {
        isMoving = true;
        yield return new WaitForSeconds(2f);

        for(int i = 0; i < steps; i++)
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
        while(Vector3.Distance(transform.position, targetPositionFixedY) > 0.001f)
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
        }
    }
}
