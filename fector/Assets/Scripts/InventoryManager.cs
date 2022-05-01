using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Item[] items;
    public Image[] images;
    public ItemSlot[] slots;
    public Item selected { get; private set; }
    public Sprite default_image;
    public Sprite selected_image;
    public Sprite transparent;

    private int index = 0;

    public static InventoryManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        slots = new ItemSlot[images.Length];
        for (int i = 0; i < images.Length; i++)
        {
            images[i].sprite = default_image;
            images[i].gameObject.GetComponentsInChildren<Image>()[1].sprite = transparent;
        }
        UpdateTexture(index, true);

        //custom
        SetItemSlot(0, items[0]);
        SetItemSlot(1, items[1]);
    }

    void Update()
    {
        int index_before = index;
        index += Input.GetAxis("Mouse ScrollWheel") > 0f ? -1 : Input.GetAxis("Mouse ScrollWheel") < 0f ? 1 : 0;
        index = Mathf.Max(0, index);
        index = Mathf.Min(images.Length - 1, index);
        if (index != index_before)
        {
            UpdateTexture(index_before, false);
            UpdateTexture(index, true);
            selected = slots[index]?.item;
        }
    }

    void UpdateTexture(int index, bool active)
    {
        images[index].sprite = active ? selected_image : default_image;
    }

    void UpdateItemTexture(int index, Item item)
    {
        images[index].gameObject.GetComponentsInChildren<Image>()[1].sprite = item == null ? transparent : item.image;
    }

    void SetItemSlot(int index, Item item)
    {
        slots[index] = new ItemSlot(item);
        UpdateItemTexture(index, item);
        if (index == this.index)
        {
            selected = item;
        }
    }
}

[System.Serializable]
public class Item
{
    public Sprite image;
    public Material material;
    public string name;
    public Type type;

    public enum Type
    {
        Block,
        Item
    }
}

public class ItemSlot
{
    public Item item;

    public ItemSlot(Item item)
    {
        this.item = item;
    }
}
