using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler {
	
	public Text uiText;
	public UiPlayList uiPlayList;
	static GameObject draggedItem;
	public Image panel;
	public Color panelColour;
	public Color panelHighlightColour;

	Vector3 startPositon;
	CanvasGroup canvasGroup;
	bool isDragging;

	#region IPointerClickHandler implementation
	public void OnPointerClick (PointerEventData eventData)
	{
		uiPlayList.selectedSong = gameObject;
		uiPlayList.selectedSongName = uiText.text;
		panel.color = panelHighlightColour;
	}
	#endregion


	#region IBeginDragHandler implementation
	public void OnBeginDrag (PointerEventData eventData)
	{
		//Store dragged item
		draggedItem = gameObject;
		//Store position the object started at
		startPositon = transform.position;
		//Disabled so when it is dropped, "hovered" can see the pannel underneath
		canvasGroup.blocksRaycasts = false;
		isDragging = true;
		uiPlayList.selectedSong = null;
		uiPlayList.selectedSongName = string.Empty;
	}
	#endregion

	#region IDragHandler implementation
	public void OnDrag (PointerEventData eventData)
	{
		if(isDragging)
		{
			//Makes panel follow the mouse position + offset so it's more centered
			transform.position = eventData.position + new Vector2 (88f, 0);
		}
	}
	#endregion

	#region IEndDragHandler implementation
	public void OnEndDrag (PointerEventData eventData)
	{
		if(isDragging)
		{
			//Find the pannel that this pannal has been dropped on top of
			for (int i = 0; i < eventData.hovered.Count; i++)
			{
				if(eventData.hovered[i].name == draggedItem.name)
				{
					//Get the other pannels text so we know which "song" it is
					Text otherText = eventData.hovered[i].GetComponentInChildren<Text> ();
					//Places the dropped "song" above/below song it was dropped onto
					uiPlayList.MoveSong(uiText.text, otherText.text);
				}
			}
			canvasGroup.blocksRaycasts = true;
			draggedItem = null;
			transform.position = startPositon;
		}
		isDragging = false;
	}
	#endregion

	// Use this for initialization
	void Awake () 
	{
		uiText = GetComponentInChildren<Text> ();
		canvasGroup = gameObject.GetComponent<CanvasGroup> ();
		uiPlayList = GameObject.Find ("Playlist").GetComponent<UiPlayList> ();
		panel = GetComponent<Image>();
		panelColour = panel.color;
		panelHighlightColour = Color.green;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Stops empty "songs" being dragged
		if(uiText.text == string.Empty)
		{
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;
		}
		else
		{
			if(!isDragging)
			{
				canvasGroup.interactable = true;
				canvasGroup.blocksRaycasts = true;
			}
		}
		//Highlighting
		if(panel.color == panelHighlightColour)
		{
			if(uiPlayList.selectedSong != this.gameObject)
			{
				panel.color = panelColour;
			}
		}
	}

}
