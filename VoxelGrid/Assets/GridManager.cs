using UnityEngine;
using System.Collections;


/// <summary>
/// Grid manager.
/// </summary>
public class GridManager : MonoBehaviour {

	//Set this class as a singleton
	public static GridManager instance;
	
	//The node
	[SerializeField]private GameObject nodePrefab;
	//The tile/cube
	[SerializeField]private GameObject tilePrefab;
	//The material to use
	public Material m_Material;
	
	public int m_GridWidth;
	public int m_GridHeight;
	
	//The size of the objects
	public float m_ObjectSize = 1.0f;
	
	//The grid for the objects locations
	private int[,] grid = new int[0, 0];
	
	//Table for the grid nodes
	private Hashtable gridNodes = new Hashtable();
	
	//Table for the game objects
	private Hashtable tileObjects = new Hashtable();
	
	//The space between each object
	private float spacer = 0.95f;
	
	//Transparent coloring
	private Color transparent = new Color(0.0f, 0.0f, 0.0f, 0.0f);
	
	void Awake() {
		//Set the manager to itself as it is a singeleton
		instance = this;
		
		this.ResetGrid();
	}
	
	public void ResetGrid() {
		//Clear the hashtables and arrays
		foreach(object key in gridNodes.Keys) {
			Destroy((GameObject)gridNodes[key]);
		}
		
		foreach(object key in tileObjects.Keys) {
			Destroy((GameObject)tileObjects[key]);
		}
		
		gridNodes.Clear();
		tileObjects.Clear();
		
		//Set the grid to a new grid and with the constant values
		grid = new int[m_GridWidth, m_GridHeight];
		
		//A count for the array element
		int count = 0;
		
		//For the width and height of chosen we must create a new object for each
		for(int x = 0; x < m_GridWidth; x++) {
			for(int y = 0; y < m_GridHeight; y++) {
				//Create a new node
				GameObject newNode = Instantiate(nodePrefab) as GameObject;
				
				//Set the transforms of the node
				newNode.transform.parent = transform;
				newNode.transform.position = new Vector3(newNode.transform.position.x + (x - ((float)m_GridWidth / 2.0f)), 
											 newNode.transform.position.y - (y - ((float)m_GridHeight / 2.0f)), 
											 newNode.transform.position.z);
				
				//newNode.transform.localPosition = new Vector3((x + 0.5f) * m_ObjectSize, (y + 0.5f) * m_ObjectSize);
				//newNode.transform.localScale = Vector3.one * m_ObjectSize * spacer;
				
				//Set the name
				newNode.name = "Node " + (x + 1) + "," + (y + 1);
				
				//Set the arrray element
				grid[x, y] = count;
				
				//Add the node
				gridNodes.Add(grid[x, y], newNode);
				
				//Increment the count
				count++;
				
				//Create the game object
				GameObject newTileObject = Instantiate(tilePrefab) as GameObject;
				
				
				//Set the position anchor and grid
				newTileObject.transform.parent = newNode.transform;
				newTileObject.transform.position = newNode.transform.position;
				newTileObject.GetComponent<GridObject>().SetAnchorPoints(x, y);
				
				//Create and instance of the material
				Material newMaterial = new Material(m_Material);
				
				//Set the scale of the texture based on the size of the grid
				Vector2 textureScale = new Vector2(1.0f / (float)m_GridWidth, 1.0f / (float)m_GridHeight);
				
				//Set the texture scale to the new material
				newMaterial.mainTextureScale = new Vector2(textureScale.x, textureScale.y);
				
				//Set the offset to the correct one relative to its position in the array
				newMaterial.mainTextureOffset = new Vector2(textureScale.x * x, 1 - (textureScale.y * (y + 1)));
				
				//Set the material and add it to the table
				newTileObject.renderer.material = newMaterial;
				tileObjects.Add(grid[x, y], newTileObject);
			}
		}
	}
	
	private void Update() {

	}

	public void HandleCollision(int index, bool isOriginalColor) {
		GameObject objectToModify = tileObjects[index] as GameObject;
		
		if(!isOriginalColor) {
			objectToModify.renderer.material.color = Color.white;
		}
		else {
			objectToModify.renderer.material.color = transparent;
		}
	}
	
	/// <summary>
	/// Gets the node position.
	/// </summary>
	/// <returns>The node position.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public Vector3 GetNodePosition(int x, int y) {
		GameObject node = gridNodes[grid[x, y]] as GameObject;
		return node.transform.position;
	}
	
	/// <summary>
	/// Gets the node position.
	/// </summary>
	/// <returns>The node position.</returns>
	/// <param name="index">Index.</param>
	public Vector3 GetNodePosition(int index) {
		GameObject node = gridNodes[index] as GameObject;
		return node.transform.position;
	}
	
	/// <summary>
	/// Gets the index of the node.
	/// </summary>
	/// <returns>The node index.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public int GetNodeIndex(int x, int y) {
		return grid[x, y];
	}
}
