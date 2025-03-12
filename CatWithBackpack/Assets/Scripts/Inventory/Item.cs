using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "ScriptableObjects/Inventory/Item", fileName = "New Item")]
[System.Serializable]
public class Item : ScriptableObject, IDescriptable 
{
    [SerializeField] private AssetReference selfReference;
    public AssetReference SelfReference => selfReference;

    [SerializeField] private Sprite icon;
    [SerializeField] private string itemName;
   
    [TextArea]
    [SerializeField] private string description;

    [SerializeField] private int stackAmount = 1;
    public int StackAmount => stackAmount;

    //IDescriptable fields
    public Sprite Icon => icon;
    public string Title => itemName;
    public string Description => description;
}
