using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private List<InventorySlotData> inventoryItems = new List<InventorySlotData>();
    public List<InventorySlotData> InventoryItems => inventoryItems;

    [SerializeField] private int maxInventorySize = 20;

    public Action OnInventoryChanged;

    public int AddItem(Item item, int quantity = 1)
    {
        if (item == null || quantity < 1) return 0;

        var slots = inventoryItems
            .Where(slot => slot.item == item)
            .OrderByDescending(slot => slot.amount)
            .ToList();

        foreach (var slot in slots)
        {
            if (quantity <= 0) break;

            int availableSpace = item.StackAmount - slot.amount;
            if (availableSpace > 0)
            {
                int amountToAdd = Math.Min(quantity, availableSpace);
                slot.amount += amountToAdd;
                quantity -= amountToAdd;
            }
        }

        while (quantity > 0 && inventoryItems.Count < maxInventorySize)
        {
            int amountToAdd = Math.Min(quantity, item.StackAmount);
            int firstFreeIndex = GetFirstFreeIndex();
            inventoryItems.Add(new InventorySlotData { item = item, amount = amountToAdd, slotIndex = firstFreeIndex });
            quantity -= amountToAdd;
        }

        OnInventoryChanged?.Invoke();

        return quantity;
    }

    private int GetFirstFreeIndex()
    {
        for (int i = 0; i < maxInventorySize; i++)
        {
            if (!inventoryItems.Any(slot => slot.slotIndex == i))
            {
                return i;
            }
        }
        return -1; // This should never happen if inventoryItems.Count < maxInventorySize
    }

    public void RemoveItem(Item item, int quantity = 1)
    {
        if (item == null || quantity < 1) return;

        var slots = inventoryItems
            .Where(slot => slot.item == item)
            .OrderBy(slot => slot.amount)
            .ToList();

        foreach (var slot in slots)
        {
            if (quantity <= 0) break;

            if (slot.amount <= quantity)
            {
                quantity -= slot.amount;
                inventoryItems.Remove(slot);
            }
            else
            {
                slot.amount -= quantity;
                quantity = 0;
            }
        }

        OnInventoryChanged?.Invoke();
    }

    public void UpdateItemQuantity(Item item, int quantity)
    {
        if (item == null) return;

        if (quantity > 0)
        {
            AddItem(item, quantity);
        }
        else if (quantity < 0)
        {
            RemoveItem(item, -quantity);
        }
    }

    public bool HasItem(Item item)
    {
        if (item == null) return false;
        var existingSlot = inventoryItems.FirstOrDefault(slot => slot.item == item);
        return existingSlot != null && existingSlot.amount > 0;
    }

    public bool HasEnoughItems(Item item, int quantity)
    {
        if (item == null || quantity < 1) return false;

        int totalQuantity = CalculateTotalQuantity(item);
        return totalQuantity >= quantity;
    }

    public int CalculateTotalQuantity(Item item)
    {
        if (item == null) return 0;

        return inventoryItems
            .Where(slot => slot.item == item)
            .Sum(slot => slot.amount);
    }

    public void SwapSlot(int fromIndex, int toIndex)
    {
        var fromSlot = inventoryItems.FirstOrDefault(slot => slot.slotIndex == fromIndex);
        var toSlot = inventoryItems.FirstOrDefault(slot => slot.slotIndex == toIndex);

        if (fromSlot == null) return;

        if (toSlot == null)
        {
            // Move the item to the empty slot
            fromSlot.slotIndex = toIndex;
        }
        else
        {
            fromSlot.slotIndex = toIndex;
            toSlot.slotIndex = fromIndex;
        }

        OnInventoryChanged?.Invoke();
    }
}
