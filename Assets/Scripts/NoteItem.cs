using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class NoteItem : CustomUIElement{
	public Image icon;
	public TextMeshProUGUI count;
	public FoodCategory category;
	public int required, got; //Number of items required / remaining

	public static NoteItem Create(FoodCategory category, int requiredCount = 1){
		var item = Instantiate(ResourceManager.LoadPrefab<NoteItem>());
		item.icon.sprite = ResourceManager.Load<Sprite>("Sprites/FoodIcons/" + category.ToString());
		item.category = category;
		item.required = requiredCount;
		item.got = 0;
		item.RefreshInfo();
		return item;
	}

	public void RefreshInfo(){
		count.text = got + "/" + required;
	}

	
}