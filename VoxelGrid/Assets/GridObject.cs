using UnityEngine;
using System.Collections;

public class GridObject : MonoBehaviour {
	
	//The anchor points for the object
	private int anchorPointX;
	public int m_AnchorPointX {
		get {
			return anchorPointX;
		}
	}
	
	private int anchorPointY;
	public int m_AnchorPointY {
		get {
			return m_AnchorPointY;
		}
	}
	
	//Has the object been selected
	private bool selected = false;
	
	void OnMouseDown() {
		if(GridManager.instance) {
			selected = true;
			bool isOriginalColor = true;
			
			int thisNode = GridManager.instance.GetNodeIndex(anchorPointX, anchorPointY);
			
			if(this.gameObject.renderer.material.color != Color.white) {
				isOriginalColor = false;
			}
			
			GridManager.instance.HandleCollision(thisNode, isOriginalColor);
		}
	}
	
	/// <summary>
	/// Sets the anchor points.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public void SetAnchorPoints(int x, int y) {
		anchorPointX = x;
		anchorPointY = y;
	}
}
