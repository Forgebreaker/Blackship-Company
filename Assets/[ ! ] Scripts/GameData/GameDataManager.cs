using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int coins = 0;
    public int selectedShipId = 0;
    public List<int> purchasedShips = new List<int>();
}

public static class GameDataManager
{
    static PlayerData playerData;

    static GameDataManager()
    {
        Initialize();
    }

    public static void Initialize()
    {
        if (playerData == null)
        {
            Debug.Log("<color=blue>[GameDataManager] Initializing playerData.</color>");
            LoadPlayerData();
        }

        // Validate purchasedShips
        if (playerData.purchasedShips == null)
        {
            playerData.purchasedShips = new List<int>();
            SavePlayerData();
            Debug.Log("<color=orange>[GameDataManager] Fixed purchasedShips list and saved.</color>");
        }
    }

    // Coin Management ---------------------------------------------------------------------------
    public static int GetCoins()
    {
        EnsureInitialized();
        return playerData.coins;
    }

    public static void AddCoins(int amount)
    {
        EnsureInitialized();
        playerData.coins += amount;
        SavePlayerData();
    }

    public static bool CanSpendCoins(int amount)
    {
        EnsureInitialized();
        return playerData.coins >= amount;
    }

    public static void SpendCoins(int amount)
    {
        EnsureInitialized();
        playerData.coins -= amount;
        SavePlayerData();
    }

    // Ship Management ---------------------------------------------------------------------------
    public static bool IsShipPurchased(int shipId)
    {
        EnsureInitialized();

        if (playerData.purchasedShips == null)
        {
            Debug.LogWarning("<color=orange>[GameDataManager] purchasedShips list is null. Initializing it now.</color>");
            playerData.purchasedShips = new List<int>();
        }

        return playerData.purchasedShips.Contains(shipId);
    }

    public static void PurchaseShip(int shipId, int cost)
    {
        EnsureInitialized();

        if (CanSpendCoins(cost) && !IsShipPurchased(shipId))
        {
            SpendCoins(cost); 
            playerData.purchasedShips.Add(shipId);
            SavePlayerData();
            Debug.Log($"<color=green>[GameDataManager] Ship {shipId} purchased for {cost} coins!</color>");
        }
    }

    public static void SetSelectedShip(int shipId)
    {
        EnsureInitialized();
        
        if (IsShipPurchased(shipId))
        {
            playerData.selectedShipId = shipId;
            SavePlayerData();
            Debug.Log($"<color=green>[GameDataManager] Ship {shipId} selected!</color>");
        }
        else
        {
            Debug.LogWarning($"<color=red>[GameDataManager] Ship {shipId} is not purchased and cannot be selected.</color>");
        }
    }

    public static int GetSelectedShip()
    {
        EnsureInitialized();
        return playerData.selectedShipId;
    }

    public static int GetTotalPurchasedShips()
    {
        EnsureInitialized(); 
        return playerData.purchasedShips.Count;
    }

    public static void DeactivatePurchasedShip(int shipId)
    {
        EnsureInitialized();

        if (playerData.purchasedShips.Contains(shipId))
        {
            playerData.purchasedShips.Remove(shipId);

            if (playerData.selectedShipId == shipId)
            {
                playerData.selectedShipId = -1;
            }

            SavePlayerData();
        }

    }



    // Save and Load Player Data -----------------------------------------------------------------
    static void SavePlayerData()
    {
        BinarySerializer.Save(playerData, "player-data.txt");
        Debug.Log("<color=magenta>[PlayerData] Saved.</color>");
    }

    static void LoadPlayerData()
    {
        try
        {
            if (BinarySerializer.HasSaved("player-data.txt"))
            {
                playerData = BinarySerializer.Load<PlayerData>("player-data.txt");
                Debug.Log("<color=green>[PlayerData] Loaded.</color>");
            }
            else
            {
                playerData = new PlayerData();
                playerData.purchasedShips.Add(0);
                playerData.selectedShipId = 0;
                SavePlayerData();
                Debug.Log("<color=yellow>[PlayerData] Initialized new data.</color>");
            }

            if (playerData.purchasedShips == null)
            {
                playerData.purchasedShips = new List<int>();
                Debug.Log("<color=orange>[PlayerData] purchasedShips list initialized.</color>");
            }
        }
        catch (System.Exception ex)
        {
            playerData = new PlayerData();
            Debug.LogError($"<color=red>[GameDataManager] Failed to load data: {ex.Message}</color>");
        }
    }


    // Check if things are correct -----------------------------------------------------------------

    static void EnsureInitialized()
    {
        if (playerData == null)
        {
            Debug.LogWarning("<color=orange>[GameDataManager] playerData was null. Reinitializing.</color>");
            Initialize();
        }
    }
}
