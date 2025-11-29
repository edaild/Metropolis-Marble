//using Microsoft.Unity.VisualStudio.Editor;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TextCore.Text;


[System.Serializable]
public class CharacterCared
{
    public int character_id;
    public string character_name;
    public string character_description;
}

[System.Serializable]
public class Shop
{
    public int shop_id;
    public string character_name;
    public int character_price;
    public int charactercard_count;
}

public class GameDataManager : MonoBehaviour
{
    public string serverurl = "http://localhost:3000";

    public List<CharacterCared> characterCareds = new List<CharacterCared>();
    public List<Shop> shops = new List<Shop>();


    private void Start()
    {
        StartCoroutine(GetCharacterCared());
        StartCoroutine(GetShop());
    }
    private IEnumerator GetCharacterCared()
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"{serverurl}/charactercard"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                characterCareds = JsonConvert.DeserializeObject<List<CharacterCared>>(www.downloadHandler.text);
                Debug.Log("들어온 데이터");
                Debug.Log("---------------------------");
                foreach (var character in characterCareds)
                {
                    Debug.Log($" 캐릭터 ID : {character.character_id} 캐릭터 이름: {character.character_name}, 캐릭터 설명: {character.character_description}");
                }
                Debug.Log("---------------------------");
            }
            else
            {
                Debug.LogError("캐릭터 조회 실패 " + www.error);
            }
        }
    }

    private IEnumerator GetShop()
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"{serverurl}/shop"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                shops = JsonConvert.DeserializeObject<List<Shop>>(www.downloadHandler.text);
                Debug.Log("들어온 데이터");
                Debug.Log("---------------------------");
                foreach (var shop in shops)
                {
                    Debug.Log($" 상점 id : {shop.shop_id}, 캐릭터 이름: {shop.character_name},  캐릭터 가격: {shop.character_price}, 재고: {shop.charactercard_count}");
                }
            }
            else
            {
                Debug.LogError("상점 조회 실패 " + www.error);
            }
        }
    }
}
