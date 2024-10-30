using Steamworks;
using UnityEngine;
using System.Collections;

public class SkinStore : MonoBehaviour
{
    private Callback<SteamInventoryRequestPricesResult_t> m_RequestPricesCallback;
    private Callback<SteamInventoryStartPurchaseResult_t> m_PurchaseResultCallback;

private void Start()
{
    if (SteamManager.Initialized)
    {
        Debug.Log("SteamManager initialized. Setting up callbacks and requesting prices.");
        m_RequestPricesCallback = Callback<SteamInventoryRequestPricesResult_t>.Create(OnPricesResult);
        m_PurchaseResultCallback = Callback<SteamInventoryStartPurchaseResult_t>.Create(OnPurchaseResult);

        SteamInventory.RequestPrices();
        Debug.Log("Price request sent.");

        // Start a coroutine to check for a response timeout
        StartCoroutine(CheckForPricesTimeout());
    } 
    else 
    {
        Debug.Log("Steam not initialized.");
    }
}

private IEnumerator CheckForPricesTimeout()
{
    yield return new WaitForSeconds(5);  // Wait 5 seconds to see if the callback fires
    Debug.Log("Timeout reached, no response received.");
}

private void OnPricesResult(SteamInventoryRequestPricesResult_t result)
{
    Debug.Log("Entered OnPricesResult callback.");
    if (result.m_result == EResult.k_EResultOK)
    {
        Debug.Log("Prices retrieved successfully.");
        DisplayAvailableItems();
    }
    else
    {
        Debug.LogError("Failed to retrieve prices from Steam.");
    }
}


  private void DisplayAvailableItems()
{
    // Define arrays for item definitions, prices, and base prices
    SteamItemDef_t[] itemDefs = new SteamItemDef_t[128];  // Adjust size if you expect more or fewer items
    ulong[] prices = new ulong[itemDefs.Length];
    ulong[] basePrices = new ulong[itemDefs.Length];
    uint arrayLength = (uint)itemDefs.Length;

    // Retrieve items with prices from SteamInventory
    if (SteamInventory.GetItemsWithPrices(itemDefs, prices, basePrices, arrayLength))
    {
        for (int i = 0; i < itemDefs.Length; i++)
        {
            Debug.Log($"Item ID: {itemDefs[i].m_SteamItemDef}, Price: {prices[i] / 100.0f} USD, Base Price: {basePrices[i] / 100.0f} USD");
        }
    }
    else
    {
        Debug.LogError("Failed to retrieve items with prices.");
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
