using UnityEngine;
using System.Collections;

public class VoxelMap : MonoBehaviour {
	
	public float m_Size = 2.0f;
	
	public int m_VoxelResolution = 8;
	public int m_ChunkResolution = 2;
	
	public VoxelGrid m_VoxelGrid;
	private VoxelGrid[] chunks;
	
	private float chunkSize;
	private float voxelSize;
	private float halfSize;
	
	private void Awake() {
		//Set the half size
		halfSize = m_Size * 0.5f;
		//Set the size of the chunk
		chunkSize = m_Size / m_ChunkResolution;
		//Set the size of the voxels
		voxelSize = chunkSize / m_VoxelResolution;
		
		//For making the chunks
		chunks= new VoxelGrid[m_ChunkResolution * m_ChunkResolution];
		
		//Create a new chunk
		for(int i = 0, y = 0; y < m_ChunkResolution; y++) {
			for(int x = 0; x < m_ChunkResolution; x++, i++) {
				CreateChunk(i, x, y);
			}
		}
		
		BoxCollider box = gameObject.AddComponent<BoxCollider>();
		box.size = new Vector3(m_Size, m_Size);
	}
	
	private void Update() {
		//Get the input from the user
		if (Input.GetMouseButtonDown(0)) {
			//The info from the raycast
			RaycastHit hitInfo;
			//Check to see if anything was hit
			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo)) {
				//If the hit object is a game object
				if (hitInfo.collider.gameObject == gameObject) {
					//Edit the hit object
					EditVoxels(transform.InverseTransformPoint(hitInfo.point));
				}
			}
		}
	}
	
	/// <summary>
	/// Edits the voxels.
	/// </summary>
	/// <param name="point">Point.</param>
	private void EditVoxels(Vector3 point) {
		int voxelX = (int)((point.x + halfSize) / voxelSize);
		int voxelY = (int)((point.y + halfSize) / voxelSize);
		int chunkX = voxelX / m_VoxelResolution;
		int chunkY = voxelY / m_VoxelResolution;
		
		VoxelStencil stencil = new VoxelStencil();
		Debug.Log(voxelX + ", " + voxelY + " in chunk " + chunkX + ", " + chunkY);
		
		voxelX -= chunkX * m_VoxelResolution;
		voxelY -= chunkY * m_VoxelResolution;
		chunks[chunkY * m_ChunkResolution + chunkX].Apply(voxelX, voxelY, true, stencil);
	}
	
	/// <summary>
	/// Creates the chunk.
	/// </summary>
	/// <param name="i">The index.</param>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	private void CreateChunk(int i, int x, int y) {
		VoxelGrid chunk = Instantiate(m_VoxelGrid) as VoxelGrid;
		chunk.Initialize(m_VoxelResolution, chunkSize);
		chunk.transform.parent = transform;
		chunk.transform.localPosition = new Vector3(x * chunkSize - halfSize, y * chunkSize - halfSize);
		chunks[i] = chunk;
	}
}