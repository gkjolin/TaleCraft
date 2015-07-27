using UnityEngine;
using System.Collections;
using RTS;

public class MinimapController : MonoBehaviour {

	private Resolution ScreenResolution;
	
	[SerializeField] Camera playCam;
	[SerializeField] Camera miniCam;
	[SerializeField] Material lineMaterial;

//	private static Material lineMaterial;
//	private static void CreateLineMaterial() {
//		if( !lineMaterial ) {
//			lineMaterial = new Material( "Shader \"Lines/Colored Blended\" {" +
//			                            "SubShader { Pass { " +
//			                            "    Blend SrcAlpha OneMinusSrcAlpha " +
//			                            "    ZWrite Off Cull Off Fog { Mode Off } " +
//			                            "    BindChannels {" +
//			                            "      Bind \"vertex\", vertex Bind \"color\", color }" +
//			                            "} } }" );
//			lineMaterial.hideFlags = HideFlags.HideAndDontSave;
//			lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
//		}
//	}
	
	// Use this for initialization
	void Start () {
		
		Resolution currentResolution = Screen.currentResolution;
		
		if (Application.isEditor) {
			ScreenResolution = Screen.resolutions [0];
		} else {
			ScreenResolution = currentResolution;
		}

		Screen.SetResolution (ScreenResolution.width, ScreenResolution.height, true);

		// Adjust the camera to center the map (world coordinates)
		Vector3 minimapOffset = new Vector3 (0f, 350f, 0f);
		miniCam.transform.position = minimapOffset;
		
		//		miniCam.orthographicSize = ScreenResolution.height / 2;
		//		miniCam.orthographic = true;
		
		miniCam.rect = new Rect (
			ResourceManager.MinimapOffsetX,
			ResourceManager.MinimapOffsetZ,
			ResourceManager.MinimapSizeX,
			ResourceManager.MinimapSizeZ
			);
	}
	
	public void OnGUI() {
		
		Ray topLeftRay = playCam.ViewportPointToRay(new Vector3(0, 1, 0));
		Ray topRightRay = playCam.ViewportPointToRay(new Vector3(1, 1, 0));
		Ray bottomLeftRay = playCam.ViewportPointToRay(new Vector3(0, 0, 0));
		Ray bottomRightRay = playCam.ViewportPointToRay(new Vector3(1, 0, 0));
		
		RaycastHit topLeftHit, topRightHit, bottomLeftHit, bottomRightHit;
		Vector2 topLeftMinimap, topRightMinimap, bottomLeftMinimap, bottomRightMinimap;
		
		// set the current material
//		CreateLineMaterial();
		lineMaterial.SetPass( 0 );
		GL.Begin( GL.LINES );
		GL.Color( Color.white );
		
		Physics.Raycast (topLeftRay, out topLeftHit);
		Vector3 topLeftWorld = topLeftHit.point;
		topLeftMinimap = Common.CalculateMinimapPosFromWorldCoordinate (topLeftWorld);
		
		Physics.Raycast (topRightRay, out topRightHit);
		Vector3 topRightWorld = topRightHit.point;
		topRightMinimap = Common.CalculateMinimapPosFromWorldCoordinate (topRightWorld);
		
		Physics.Raycast (bottomRightRay, out bottomRightHit);
		Vector3 bottomRightWorld = bottomRightHit.point;
		bottomRightMinimap = Common.CalculateMinimapPosFromWorldCoordinate (bottomRightWorld);
		
		Physics.Raycast (bottomLeftRay, out bottomLeftHit);
		Vector3 bottomLeftWorld = bottomLeftHit.point;
		bottomLeftMinimap = Common.CalculateMinimapPosFromWorldCoordinate (bottomLeftWorld);
		
		GL.Vertex3( topLeftMinimap.x, topLeftMinimap.y, 0f );
		GL.Vertex3( topRightMinimap.x, topRightMinimap.y, 0f );
		GL.Vertex3( topRightMinimap.x, topRightMinimap.y, 0f );
		GL.Vertex3( bottomRightMinimap.x, bottomRightMinimap.y, 0f );
		GL.Vertex3( bottomRightMinimap.x, bottomRightMinimap.y, 0f );
		GL.Vertex3( bottomLeftMinimap.x, bottomLeftMinimap.y, 0f );
		GL.Vertex3( bottomLeftMinimap.x, bottomLeftMinimap.y, 0f );
		GL.Vertex3( topLeftMinimap.x, topLeftMinimap.y, 0f );
		
		GL.End();
		
	}

}
