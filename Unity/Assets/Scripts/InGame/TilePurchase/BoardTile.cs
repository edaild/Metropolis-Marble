using UnityEngine;

public class BoardTile : MonoBehaviour
{
    public BlockSO data;  // ? ScriptableObject 연결

    public int ownerId = -1;   // 누구 소유인지
    public Renderer tileRenderer;

    public int Price => data.Block_price;
    public string RegionName => data.Block_name;

    public void SetOwner(int newOwner, Color ownerColor)
    {
        ownerId = newOwner;

        if (tileRenderer != null)
            tileRenderer.material.color = ownerColor;
    }
}
