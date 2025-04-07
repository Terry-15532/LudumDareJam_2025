using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NoteMenu : CustomUIElement, IPointerEnterHandler, IPointerExitHandler{
	public Vector2 initPos;
	public Vector2 delta;

	public bool inOrder = true;

	public ScrollRect scrollView;
	public List<NoteItem> itemList;

	private Coroutine foldDelay;

	public override void Awake(){
		base.Awake();
		Initialize();
	}

	public void Initialize(){
		itemList = new();
		initPos = position;
	}

	public void AddFood(FoodCategory f, int required = 1){
		var item = NoteItem.Create(f, required);
		itemList.Add(item);
		item.GetComponent<RectTransform>().SetParent(scrollView.content, false);
		UpdateItems();
		if (inOrder && itemList.Count != 1){
			item.SetAlpha(0);
		}
	}

	public void OnFoodReached(Food f){
		if (inOrder){
			var item = itemList[0];
			if (f.category == item.category){
				item.got += 1;
				item.RefreshInfo();
				if (item.got >= item.required){
					Remove(item, 0);
				}
				else{
					Show();
					Tools.CallDelayed(Fold, 1f);
				}
			}
		}
		else{
			for (int i = 0; i < itemList.Count; i++){
				var item = itemList[i];
				if (f.category == item.category){
					item.got += 1;
					item.RefreshInfo();
					if (item.got >= item.required){
						Remove(item, i);
						i--;
					}

					else{
						Show();
						Tools.CallDelayed(Fold, 1f);
					}
				}
			}
		}
	}

	public void Remove(NoteItem item, int idx){
		itemList.RemoveAt(idx);
		if (inOrder){
			if (itemList.Count > 0){
				Tools.CallDelayed(() => {
						itemList[0].SetAttrAni(1, 0.3f, ColorAttr.a);
						UpdateItems();
					}, 1.8f
				);
			}
			else{
				LevelManager.ToNextLevel();
			}
		}
		else if (itemList.Count == 0){
			LevelManager.ToNextLevel();
		}

		Show();

		try{
			Tools.CallDelayed(() => {
				item.SetPositionAni((Vector2)item.position + new Vector2(20, 0), 0.3f);

				Tools.CallDelayed(() => { item.SetPositionAni((Vector2)item.position + new Vector2(-500, 0), 0.5f); }, 0.3f);

				Tools.CallDelayed(() => {
					if (item){
						Destroy(item.gameObject);
					}

					UpdateItems();
					Fold();
				}, 1f);
			}, 0.5f);
		}
		finally{ }
	}

	public void UpdateItems(){
		if (!inOrder){
			int i = 0;
			foreach (var item in itemList){
				item.SetPositionAni(new Vector2(item.position.x, -70 - 80 * i), 0.3f);
				i++;
			}
		}
	}

	public void Show(){
		SetPositionAni(initPos + delta, 0.3f);
		SetAttrAni(1, 0.3f, ColorAttr.a);
	}

	public void Fold(){
		SetPositionAni(initPos, 0.3f);
		SetAttrAni(0.5f, 0.3f, ColorAttr.a);
	}

	public void Hide(){
		SetAttrAni(0, 0.4f, ColorAttr.a, hide: true, forceChangeAll: true);
	}

	public void OnPointerEnter(PointerEventData eventData){
		if (foldDelay != null){
			StopCoroutine(foldDelay);
		}

		Show();
	}

	public void OnPointerExit(PointerEventData eventData){
		foldDelay = Tools.CallDelayed(Fold, 0.3f);
	}
}