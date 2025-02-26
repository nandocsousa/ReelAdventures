using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public int playerCoins = 0;
    public TextMeshProUGUI coinsText;

    public FishingSystem fishingSystem;

    private SaveLoad saveLoad;

    public GameObject bronzeRod;
    public GameObject silverRod;
    public Button silverRodButton;
    public GameObject goldRod;
    public Button goldRodButton;

    private void Start()
    {
        saveLoad = FindObjectOfType<SaveLoad>(); // Find the SaveLoad script
        var savedData = saveLoad.LoadPlayerData(); // Load the player's saved data

        playerCoins = savedData.playerCoins; // Load saved coins
        UpdateCoinsUI();

        // Set the current rod based on the saved data
        SetRod(savedData.currentRod);
    }

    public void SellFish(string fishQuality) // Sell a specific type of fish
    {
        // Find a fish of the specified quality in the inventory
        Item fishToSell = InventoryManager.inventoryManager.inventory.Find(f => f.quality == fishQuality);

        if (fishToSell != null)
        {
            InventoryManager.inventoryManager.RemoveFish(fishToSell); // Remove the fish from the inventory
            fishingSystem.UpdateFishQualityCount(fishQuality, -1); // Updates counts and UI
            playerCoins += fishToSell.sellPrice; // Add the fish's price to the player's coins
            UpdateCoinsUI(); // Update the UI to the new coin count

            saveLoad.SavePlayerData(playerCoins, GetCurrentRodType()); // Save the updated coins

            Debug.Log($"Sold a {fishQuality} fish for {fishToSell.sellPrice} coins. Current coins: {playerCoins}");
        }
        else Debug.Log($"No {fishQuality} fish available to sell.");
    }

    public void SellBronzeFish()
    {
        SellFish("Bronze");
    }

    public void SellSilverFish()
    {
        SellFish("Silver");
    }

    public void SellGoldFish()
    {
        SellFish("Gold");
    }

    private void UpdateCoinsUI()
    {
        coinsText.text = $"{playerCoins} coins";
    }

    public void BuySilverRod()
    {
        bronzeRod.SetActive(false);
        silverRod.SetActive(true);
        goldRod.SetActive(false);

        playerCoins -= 50;
        UpdateCoinsUI(); // Update the UI to the new coin count

        silverRodButton.interactable = false;
        saveLoad.SavePlayerData(playerCoins, "Silver"); // Save the updated coins and rod type
    }

    public void BuyGoldRod()
    {
        bronzeRod.SetActive(false);
        silverRod.SetActive(false);
        goldRod.SetActive(true);

        playerCoins -= 150;
        UpdateCoinsUI(); // Update the UI to the new coin count

        goldRodButton.interactable = false;
        saveLoad.SavePlayerData(playerCoins, "Gold"); // Save the updated coins and rod type
    }

    private void SetRod(string rodType) // Method to set the rod based on saved data
    {
        bronzeRod.SetActive(rodType == "Bronze");
        silverRod.SetActive(rodType == "Silver");
        goldRod.SetActive(rodType == "Gold");

        silverRodButton.interactable = rodType != "Silver";
        goldRodButton.interactable = rodType != "Gold";
    }

    private string GetCurrentRodType()
    {
        if (silverRod.activeSelf) return "Silver";
        if (goldRod.activeSelf) return "Gold";
        return "Bronze"; // Default to Bronze rod
    }
}