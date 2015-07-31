using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TGMap : MonoBehaviour {
	
	// Number of tiles
	[SerializeField] private int sizeX;
	[SerializeField] private int sizeZ;
	[SerializeField] private float tileSize;
	
	[SerializeField] private Texture2D terrainTiles;
	[SerializeField] private int tileResolution;

	// Use this for initialization
	void Start () {
		BuildMesh ();
	}

	Color[][] ChopUpTiles() {
		int numTilesPerRow = terrainTiles.width / tileResolution;
		int numRows = terrainTiles.height / tileResolution;

		Color[][] tiles = new Color[numTilesPerRow * numRows][];
		
		for (int y = 0; y < numRows; y++) {
			for (int x = 0; x < numTilesPerRow; x++) {
				tiles[y*numTilesPerRow + x] = terrainTiles.GetPixels(x*tileResolution, y*tileResolution, tileResolution, tileResolution);
			}
		}

		return tiles;
	}

	void BuildTexture() {

		TDMap map = new TDMap (sizeX, sizeZ);

		int textWidth = sizeX * tileResolution;
		int textHeight = sizeZ * tileResolution;

		// Create a new texture 10 x 10 px
		Texture2D texture = new Texture2D (textWidth, textHeight);

		Color[][] tiles = ChopUpTiles ();
		
		for (int y = 0; y < sizeZ; y++) {
			for (int x = 0; x < sizeX; x++) {
				Color[] p = tiles[map.GetTileAt(x, y)];
				texture.SetPixels(x*tileResolution, y*tileResolution, tileResolution, tileResolution, p);
			}
		}

		// For sharp edges
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;

		// Apply the textures
		texture.Apply ();

		MeshRenderer mr = GetComponent<MeshRenderer> ();
		mr.sharedMaterials [0].mainTexture = texture;

	}
	
	// Update is called once per frame
	public void BuildMesh () {
		
		int numTiles = sizeX * sizeZ;
		int numTriangles = numTiles * 2;
		
		int vSizeX = sizeX + 1;
		int vSizeZ = sizeZ + 1;
		int numVertexes = vSizeX * vSizeZ;
		
		// Generate the mesh data
		Vector3[] vertices = new Vector3[numVertexes];
		Vector3[] normals = new Vector3[numVertexes];
		Vector2[] uv = new Vector2[numVertexes];
		
		int[] triangles = new int[numTriangles * 3];
		
		// Loop the vertexes first
		int x, z;
		for (z = 0; z < vSizeZ; z++) {
			for (x = 0; x < vSizeX; x++) {
				vertices[ z * vSizeX + x ] = new Vector3( x * tileSize, 0, -z * tileSize );
				normals [ z * vSizeX + x ] = Vector3.up;
				uv      [ z * vSizeX + x ] = new Vector2( (float) x / sizeX, 1f - (float) z / sizeZ );

				// TODO: Fix upside down textures
			}
		}
		
		for (z = 0; z < sizeZ; z++) {
			for (x = 0; x < sizeX; x++) {
				int squareIndex =  z * sizeX + x;
				int triOffset = squareIndex * 6;

				triangles[ triOffset + 0 ] = z * vSizeX + x +          0;
				triangles[ triOffset + 2 ] = z * vSizeX + x + vSizeX + 0;
				triangles[ triOffset + 1 ] = z * vSizeX + x + vSizeX + 1;
				
				triangles[ triOffset + 3 ] = z * vSizeX + x +          0;
				triangles[ triOffset + 5 ] = z * vSizeX + x + vSizeX + 1;
				triangles[ triOffset + 4 ] = z * vSizeX + x +          1;
			}
		}
		
		// Create a new mesh
		Mesh mesh      = new Mesh ();
		mesh.vertices  = vertices;
		mesh.triangles = triangles;
		mesh.normals   = normals;
		mesh.uv        = uv;
		
		// Assign our mesh to our filter/renderer/collider
		MeshFilter mf = GetComponent<MeshFilter> ();
		MeshCollider mc = GetComponent<MeshCollider> ();

		// Assign the meshes to the components
		mf.mesh = mesh;
		mc.sharedMesh = mesh;

		// Move the tileMap to center position
		transform.position = new Vector3 ((float) - (sizeX * tileSize) / 2, 0, (float) (sizeZ * tileSize) / 2);

		// Add textures to the grid
		BuildTexture ();

	}
}
