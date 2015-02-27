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
	}
	
	private void CreateChunk (int i, int x, int y) {
		VoxelGrid chunk = Instantiate(m_VoxelGrid) as VoxelGrid;
		chunk.Initialize(m_VoxelResolution, chunkSize);
		chunk.transform.parent = transform;
		chunk.transform.localPosition = new Vector3(x * chunkSize - halfSize, y * chunkSize - halfSize);
		chunks[i] = chunk;
	}
}
