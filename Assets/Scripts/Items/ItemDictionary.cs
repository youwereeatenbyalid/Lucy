using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    // Start is called before the first frame update
    void Awake()
    {
        Build();
    }

    // Get Item by id
    public Item GetItem(int id)
    {
        return items.Find(item => item.id == id);
    }

    // Get Item by title
    public Item GetItem(string title)
    {
        return items.Find(item => item.title == title);
    }

    // Build the list of items
    void Build()
    {
        items = new List<Item>()
        {   //item 1
            new Item(1, "Shotgun", "This helps you.", 
                    new Dictionary<string, int>
                    {
                        {"power", 10},
                        {"level", 1}
                    } ),
            new Item(2, "Rifle", "This helps you better.", 
                    new Dictionary<string, int>
                    {
                        {"power", 30},
                        {"level", 2}
                    } ),
            new Item(3, "Turret", "So you don't need to use your hand.", 
                    new Dictionary<string, int>
                    {
                        {"power", 30},
                        {"level", 3}
                    } ),
            new Item(4, "Advance Turret", "Faster and better.", 
                    new Dictionary<string, int>
                    {
                        {"power", 60},
                        {"level", 3}
                    } ),
            new Item(5, "Blade Spinning", "Becareful when use.", 
                    new Dictionary<string, int>
                    {
                        {"power", 70},
                        {"level", 3}
                    } ),
            new Item(6, "Ceiling blade", "Quick and easy.", 
                    new Dictionary<string, int>
                    {
                        {"power", 100},
                        {"level", 3}
                    } )

        };
       //items.[0].stats["power"].ToString(); /*power level in string format */
    }

    
}
