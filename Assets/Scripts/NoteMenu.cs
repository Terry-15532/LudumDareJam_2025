using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NoteMenu : CustomUIElement{
	public Vector2 initPos;
	public Vector2 delta;

	[FormerlySerializedAs("keepOrder")] public bool inOrder = true;

	public ScrollRect scrollView;
	public List<NoteItem> itemList;

	public void Initialize(){
		itemList = new();
	}

	public void AddFood(FoodCategory f, int required = 1){
		var item = NoteItem.Create(f, required);
		itemList.Add(item);
		GetComponent<RectTransform>().SetParent(scrollView.content, false);
		if (!inOrder){
			LayoutRebuilder.ForceRebuildLayoutImmediate(scrollView.content);
		}
		else{
			if (itemList.Count == 1){
				LayoutRebuilder.ForceRebuildLayoutImmediate(scrollView.content);
			}
			else{
				item.SetAlpha(0);
			}
		}
	}

	public void OnFoodPicked(Food f){
		if (inOrder){
			var item = itemList[0];
			if (f.category == item.category){
				item.got += 1;
				item.RefreshInfo();
				if (item.got >= item.required){
					Remove(item, 0);
				}
			}
		}
		else{
			int i = 0;
			foreach (var item in itemList){
				if (f.category == item.category){
					item.got += 1;
					item.RefreshInfo();
					if (item.got >= item.required){
						Remove(item, i);
					}
				}

				i++;
			}
		}
	}

	public void Remove(NoteItem item, int idx){
		itemList.RemoveAt(idx);
		if (inOrder){
			if (itemList[0]){
				itemList[0].SetAttrAni(1, 0.3f, ColorAttr.a);
			}
			else{
				LevelManager.instance.ToNextLevel();
			}
		}

		item.SetPositionAni((Vector2)item.position + new Vector2(100, 0), 0.1f);
		Tools.CallDelayed(() => {
			item.SetPositionAni((Vector2)item.position + new Vector2(-1000, 0), 0.3f);
			item.SetAttrAni(0, 0.3f, ColorAttr.a, hide: true);
		}, 0.1f);
		Tools.CallDelayed(() => { Destroy(item.gameObject); }, 0.3f);
	}

	public void Show(){
		SetPositionAni(initPos + delta, 0.3f);
		SetAttrAni(1, 0.3f, ColorAttr.a);
	}

	public void Fold(){
		SetPositionAni(initPos, 0.3f);
		SetAttrAni(0.3f, 0.3f, ColorAttr.a);
	}

	public void Hide(){
		SetAttrAni(0, 0.3f, ColorAttr.a, hide: true);
	}
}