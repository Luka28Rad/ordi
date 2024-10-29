using Steamworks;
using UnityEngine;

public class SkinStore : MonoBehaviour
{
    private Callback<SteamInventoryRequestPricesResult_t> m_RequestPricesCallback;
    private Callback<SteamInventoryStartPurchaseResult_t> m_PurchaseResultCallback;

    private void Start()
    {
        if (SteamManager.Initialized)
        {
            // Initialize callbacks for handling Steam Inventory events
            m_RequestPricesCallback = Callback<SteamInventoryRequestPricesResult_t>.Create(OnPricesResult);
            m_PurchaseResultCallback = Callback<SteamInventoryStartPurchaseResult_t>.Create(OnPurchaseResult);

            // Request item prices from Steam
            SteamInventory.RequestPrices();
        } 
        else 
        {
            Debug.Log("Steam not initialized.");
        }
    }

    private void OnPricesResult(SteamInventoryRequestPricesResult_t result)
    {
        if (result.m_result == EResult.k_EResultOK)
        {
            Debug.Log("Prices retrieved successfully.");
            // Use SteamInventory.GetResultItems or similar methods to get items and display their prices
        }
        else
        {
            Debug.LogError("Failed to retrieve prices from Steam.");
        }
    }

    public void PurchaseSkin(int itemDefID)
    {
        Debug.Log($"Attempting to purchase skin with itemDefID: {itemDefID}");

        // Define the item ID and quantity arrays
        SteamItemDef_t[] itemIDs = new SteamItemDef_t[] { new SteamItemDef_t(itemDefID) };
        uint[] quantities = new uint[] { 1 };  // Quantity of each item

        // Start the purchase with the specified item ID, quantity, and array length
        SteamInventory.StartPurchase(itemIDs, quantities, (uint)itemIDs.Length);
    }

    private void OnPurchaseResult(SteamInventoryStartPurchaseResult_t result)
    {
        if (result.m_result == EResult.k_EResultOK)
        {
            Debug.Log("Purchase successful!");
            // Update the player's inventory or UI to show that the item was purchased
        }
        else
        {
            Debug.LogError("Purchase failed.");
        }
    }
}
