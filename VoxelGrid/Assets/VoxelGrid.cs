using UnityEngine;
using System.Collections;

public class VoxelGrid : MonoBehaviour {

	//The size of the grid
	public int m_GridSize;
	
	//The voxel prefab
	public GameObject m_Prefab;
	
	//The voxels array
	private bool[] voxels;
	
	//The size of the voxel
	private float voxelSize;
	
	private void Awake() {
		//Set the size of the voxel
		voxelSize = 1.0f / m_GridSize;
		
		//Set the grid size
		voxels = new bool[m_GridSize * m_GridSize];
	}
	
	private void Start() {
		//For the size of the grid, create a new voxel
		for (int i = 0, y = 0; y < m_GridSize; y++) {
			for (int x = 0; x < m_GridSize; x++, i++) {
				CreateVoxel(i, x, y);
			}
		}
	}
	
	/// <summary>
	/// Creates the voxel.
	/// </summary>
	/// <param name="i">The index.</param>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	private void CreateVoxel (int i, int x, int y) {
		//The new game object
		GameObject newVoxel = Instantiate(m_Prefab) as GameObject;
		
		//Set the transforms
		newVoxel.transform.parent = transform;	
		newVoxel.transform.localPosition = new Vector3((x + 0.5f) * voxelSize, (y + 0.5f) * voxelSize);
		newVoxel.transform.localScale = Vector3.one * voxelSize;
	}
}
