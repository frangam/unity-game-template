using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectEnsureVisible : MonoBehaviour
{
	
	#region Public Variables
	
	public float _AnimTime = 0.15f;
	public bool _Snap = false;
	public RectTransform _MaskTransform;
	
	#endregion
	
	#region Private Variables
	
	private ScrollRect mScrollRect;
	private RectTransform mScrollTransform;
	private RectTransform mContent;
	
	#endregion
	
	#region Unity Methods
	
	private void Awake ()
	{
		mScrollRect = GetComponent<ScrollRect> ();
		mScrollTransform = mScrollRect.transform  as RectTransform;
		mContent = mScrollRect.content;
	}
	
	#endregion
	
	#region Public Methods
	
	public void CenterOnItem(RectTransform target)
	{
		// Item is here
		var itemCenterPositionInScroll = GetWorldPointInWidget(mScrollTransform, GetWidgetWorldPoint(target));
		Debug.Log("Item Anchor Pos In Scroll: " + itemCenterPositionInScroll);
		// But must be here
		var targetPositionInScroll = GetWorldPointInWidget(mScrollTransform, GetWidgetWorldPoint(_MaskTransform));
		Debug.Log("Target Anchor Pos In Scroll: " + targetPositionInScroll);
		// So it has to move this distance
		var difference = targetPositionInScroll - itemCenterPositionInScroll;
		difference.z = 0f;
		
		//clear axis data that is not enabled in the scrollrect
		if (!mScrollRect.horizontal)
		{
			difference.x = 0f;
		}
		
		if (!mScrollRect.vertical)
		{
			difference.y = 0f;
		}
		
		//this is the wanted new position for the content
		var newAnchoredPosition = mContent.anchoredPosition3D + difference;
		
		if (_Snap) {
			mContent.anchoredPosition3D = newAnchoredPosition;
		} else {
			DOTween.To (() => mContent.anchoredPosition, x => mContent.anchoredPosition = x, newAnchoredPosition, _AnimTime);
		}
	}
	
	#endregion
	
	#region Private Methods
	
	Vector3 GetWidgetWorldPoint(RectTransform target)
	{
		//pivot position + item size has to be included
		var pivotOffset = new Vector3(
			(0.5f - target.pivot.x) * target.rect.size.x,
			(0.5f - target.pivot.y) * target.rect.size.y,
			0f);
		var localPosition = target.localPosition + pivotOffset;
		return target.parent.TransformPoint(localPosition);
	}
	
	Vector3 GetWorldPointInWidget(RectTransform target, Vector3 worldPoint)
	{
		return target.InverseTransformPoint(worldPoint);
	}
	#endregion
}