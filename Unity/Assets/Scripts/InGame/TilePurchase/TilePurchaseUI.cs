using UnityEngine.UI;
using UnityEngine;

public class TilePurchaseUI : MonoBehaviour
{
    public static TilePurchaseUI Instance;

    [Header("구매 패널")]
    public GameObject panel;
    public Text titleText;
    public Text descriptionText;
    public Text priceText;

    private BlockData currentBlock;
    private PlayerController currentPlayer;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        panel.SetActive(false);
    }

    public void OpenBuy(BlockData block, PlayerController player)
    {
        currentBlock = block;
        currentPlayer = player;

        titleText.text = $"{block.BlockName}";
        descriptionText.text = "이 지역을 구매하시겠습니까?";
        priceText.text = $"가격 : {block.Price} G";

        panel.SetActive(true);
    }

    public void OnClickConfirm()
    {
        if (currentBlock != null && currentPlayer != null)
        {
            currentPlayer.BuyBlock(currentBlock);
        }
        Close();
    }

    public void OnClickCancel()
    {
        Close();
    }

    private void Close()
    {
        panel.SetActive(false);
        currentBlock = null;
        currentPlayer = null;
    }
}
