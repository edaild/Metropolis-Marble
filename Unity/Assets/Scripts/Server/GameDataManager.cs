//using Microsoft.Unity.VisualStudio.Editor;
using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TextCore.Text;


[System.Serializable]
public class Weapon
{
    public int weapon_type_id;
    public string weapon_name;
    public float base_damage;
    public string ammo_type;
}

[System.Serializable]
public class Shop
{
    public int shop_id;
    public string gun_Name;
    public int transaction_price;
}

[System.Serializable]
public class NPCCaharacter
{
    public int npc_type_id;
    public string npc_name;
    public bool is_hostile;
    public int base_damage;
}

public class GameDataManager : MonoBehaviour
{
    public string serverurl = "http://localhost:3000";

    public List<Weapon> WeaponData = new List<Weapon>();
    public List<Shop> shops = new List<Shop>();
    public List<NPCCaharacter> npc_characterData = new List<NPCCaharacter>();


    private void Start()
    {
        StartCoroutine(GetWeapon());
        StartCoroutine(GetNPC_Character());
        StartCoroutine(GetShop());
    }
    private IEnumerator GetWeapon()
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"{serverurl}/weapon_types"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                WeaponData = JsonConvert.DeserializeObject<List<Weapon>>(www.downloadHandler.text);
                Debug.Log("들어온 데이터");
                Debug.Log("---------------------------");
                foreach (var weapon  in WeaponData)
                {
                    Debug.Log($"무기 이름 : {weapon.weapon_name}, 데미지 : {weapon.base_damage}");
                }
                Debug.Log("---------------------------");
            }
            else
            {
                Debug.LogError("무기 조회 실패 " + www.error);
            }
        }
    }

    private IEnumerator GetNPC_Character()
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"{serverurl}/npc_character"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                npc_characterData = JsonConvert.DeserializeObject<List<NPCCaharacter>>(www.downloadHandler.text);
                Debug.Log("들어온 데이터");
                Debug.Log("---------------------------");
                foreach (var npc in npc_characterData)
                {
                    Debug.Log($" NPC 캐릭터 id : {npc.npc_type_id}, NPC 이름 : {npc.npc_name}, 공격 여부 : {npc.is_hostile}, 공격 데미지 : {npc.base_damage}");
                }
            }
            else
            {
                Debug.LogError("NPC 캐릭터 조회 실패 " + www.error);
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
                    Debug.Log($" 상점 id : {shop.shop_id}, 무기 이름 : {shop.gun_Name}, 무기 가격 : {shop.transaction_price}");
                }
            }
            else
            {
                Debug.LogError("상점 조회 실패 " + www.error);
            }
        }
    }
}
