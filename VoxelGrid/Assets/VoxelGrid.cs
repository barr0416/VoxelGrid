using UnityEngine;
using System.Collections;

//We want to be able to select the entire grid, so this will allow it by default
[SelectionBase]
public class VoxelGrid : MonoBehaviour {

	//The size of the grid
	public int m_GridSize;
	
	//The voxel prefab
	public GameObject m_Prefab;
	
	//The materials
	private Material[] voxelMaterials;
	
	//The game objects
	private GameObject[] voxelObjects;
	
	//The voxels array
	protected bool[] voxels;
	
	//The size of the voxel
	private float voxelSize;
	
	//Space between objects
	private float voxelSpacer = 0.95f;
	
	//The transparent coloring
	private Color transparent = new Color(0.0f, 0.0f, 0.0f, 0.0f);
	
	public void Initialize(int resolution, float size) {
		this.m_GridSize = resolution;
		//Set the size of the voxel
		voxelSize = size / m_GridSize;
		
		//Set the grid size
		voxels = new bool[resolution * resolution];
		
		//Set the material 
		voxelMaterials = new Material[voxels.Length];
		
		//For the size of the grid, create a new voxel
		for (int i = 0, y = 0; y < resolution; y++) {
			for (int x = 0; x < resolution; x++, i++) {
				CreateVoxel(i, x, y);
			}
		} 
		
		InitializeVoxelColors();
	}
	
	/// <summary>
	/// Apply the specified x, y, state and stencil.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="state">If set to <c>true</c> state.</param>
	/// <param name="stencil">Stencil.</param>
	public void Apply(int x, int y, bool edit, VoxelStencil stencil) {	
		if(edit) {
			bool isOrignalColor = true;
			int index = y * m_GridSize + x;
			voxels[index] = stencil.Apply(x, y);
			
			if(voxelMaterials[index].color != Color.white) {
				isOrignalColor = false;
			}
			
			SetVoxelColors(isOrignalColor, index);
		}
	}
	
	/// <summary>
	/// Sets the voxel colors.
	/// </summary>
	/// <param name="isOriginalColor">If set to <c>true</c> is original color.</param>
	/// <param name="id">Identifier.</param>
	private void SetVoxelColors(bool isOriginalColor, int id) {
	
		//If the color has been changed before, we can change it back
		if(!isOriginalColor) {
			voxelMaterials[id].color = Color.white;
		}
		else {
			voxelMaterials[id].color = transparent;
		}
	}
	
	/// <summary>
	/// Initializes the voxel colors.
	/// </summary>
	private void InitializeVoxelColors() {
		for (int i = 0; i < voxels.Length; i++) {
			voxelMaterials[i].color = voxels[i] ? transparent : Color.white;
		}
	}
	
	/// <summary>
	/// Creates the voxel.
	/// </summary>
	/// <param name="i">The index.</param>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	private void CreateVoxel(int i, int x, int y) {
		//The new game object
		GameObject newVoxel = Instantiate(m_Prefab) as GameObject;
		
		//Set the transforms
		newVoxel.transform.parent = transform;	
		newVoxel.transform.localPosition = new Vector3((x + 0.5f) * voxelSize, (y + 0.5f) * voxelSize);
		newVoxel.transform.localScale = Vector3.one * voxelSize * voxelSpacer;
		
		voxelMaterials[i] = newVoxel.GetComponent<MeshRenderer>().material;
	}
}
