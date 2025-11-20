using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public int dice_RotatesSpeed = 60;
    public AudioSource dice_soud;
    public PlayerController playerController;

    private bool isRotateDice;

    private readonly Dictionary<int, Vector3> finalRotations = new Dictionary<int, Vector3>()
    {
        { 1, new Vector3(90, 0, 0) },
        { 3, new Vector3(-90, 0, 0) },
        { 4, new Vector3(180, 0, 0) },
        { 5, new Vector3(0, 0, 90) },
        { 6, new Vector3(0, 0, -90) }
    };


    void Start()
    {
        if(playerController == null)
            playerController = GetComponent<PlayerController>();
        isRotateDice = true;
    }
    private void Update()
    {
        if (isRotateDice)
        {
            RotateDice();
        }
    }

    private void RotateDice()
    {
        transform.Rotate(dice_RotatesSpeed * Time.deltaTime, 0, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pool"))
        {
            // dice_soud.Play();

           

            if(playerController != null)
            {
                int result = playerController.diceResult;

                if (finalRotations.ContainsKey(result))
                {
                    transform.eulerAngles = finalRotations[result];
                    Debug.Log($"주사위 수는 : {result}");
                }
            }
            isRotateDice = false;
            StartCoroutine(RemoveDice());
     
        }

        IEnumerator RemoveDice()
        {
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }
    }
}
    

