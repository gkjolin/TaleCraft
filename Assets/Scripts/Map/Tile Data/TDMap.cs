using UnityEngine;
using System.Collections.Generic;

public class TDMap{

	private int sizeX;
	private int sizeY;
	
	private int[,] mapData;
	List <DRoom> rooms;
	
	protected class DRoom {
		public int left;
		public int top;
		public int width;
		public int height;
		public bool isConnected = false;

		public int right {
			get {return left + width - 1;}
		}
		public int bottom {
			get {return top + height - 1;}
		}

		public int centerX {
			get {return left + width/2;}
		}

		public int centerY {
			get {return top + height/2;}
		}

		public bool CollidesWith(DRoom other) {
			if (left > other.right)
				return false;
			if (top > other.bottom)
				return false;
			if (right < other.left)
				return false;
			if (bottom < other.top)
				return false;
			return true;
		}

	}

	/*
	 * 0 = unknown
	 * 1 = floor
	 * 2 = wall
	 * 3 = stone
	 */

	// Constructor
	public TDMap(int width, int height) {
		this.sizeX = width;
		this.sizeY = height;

		mapData = new int[sizeX, sizeY];

		for (int x=0; x < sizeX; x++) {
			for (int y=0; y < sizeY; y++) {
				mapData[x, y] = 3;
			}
		}

		rooms = new List<DRoom> ();
		int maxFails = 3;

		while (rooms.Count < 10 ){
			int rsx = Random.Range(4,8);
			int rsy = Random.Range(4,8);

			DRoom r 	= new DRoom();
			r.left 		= Random.Range(0, sizeX - rsx);
			r.top 		= Random.Range(0, sizeY - rsy);
			r.width 	= rsx;
			r.height 	= rsy;

			if(! RoomCollides(r)){
				rooms.Add(r);
			} else {
				maxFails--;
				if(maxFails <= 0) {
					break;
				}
			}
		}
		foreach (DRoom r in rooms) {
			MakeRoom (r);
		}
		
		for (int i = 0; i < rooms.Count; i++) {
			if(!rooms[i].isConnected) {
				int j = Random.Range(1, rooms.Count);
				MakeCorridor(rooms[i], rooms[(i + j) % rooms.Count]);
			}
		}

		MakeWalls ();

	}

	public int GetTileAt(int x, int y) {
//		if (x < 0 || x >= sizeX || y < 0 || y >= sizeY)
//			return null;

		return mapData[x, y];
	}

	bool RoomCollides(DRoom r) {
		foreach (DRoom r2 in rooms) {
			if(r.CollidesWith(r2)) {
				return true;
			}
		}
		return false;
	}

	void MakeRoom(DRoom r) {
		for(int x = 0; x< r.width; x++) {
			for(int y = 0; y< r.height; y++) {
				if(x==0 || x==r.width-1 || y==0 || y ==r.height-1) {
					mapData[r.left + x, r.top+ y] = 2;
				} else{
					mapData[r.left + x, r.top+ y] = 1;
				}
			}
		}
	}

	void MakeCorridor(DRoom r1, DRoom r2) {

		int x = r1.centerX;
		int y = r1.centerY;

		while (x != r2.centerX) {
			mapData[x, y] = 1;
			
			// move x-wise
			x += x < r2.centerX ? 1 : -1;

		}
		
		while (y != r2.centerY) {
			mapData[x, y] = 1;
			
			// move y-wise
			y += y < r2.centerY ? 1 : -1;

		}
		
		r1.isConnected = true;
		r2.isConnected = true;
	}

	void MakeWalls() {
		for(int x = 0; x < sizeX; x++) {
			for(int y = 0; y < sizeY; y++) {
				if(mapData[x,y] == 3 && HasAdjacentFloor(x, y)) {
					mapData[x,y] = 2;
				}
			}
		}
	}

	bool HasAdjacentFloor(int x, int y) {
		if(x > 0 && mapData[x-1, y] == 1)
			return true;
		if(x < sizeX - 1 && mapData[x+1, y] == 1)
			return true;
		if(y > 0 && mapData[x, y-1] == 1)
			return true;
		if(y < sizeY - 1 && mapData[x, y+1] == 1)
			return true;

		// Check the top left and right tile
		if(x > 0 && y > 0 && mapData[x-1, y-1] == 1)
			return true;
		if(x < sizeX - 1 && y > 0 && mapData[x+1, y-1] == 1)
			return true;
		// Check the bottom left and right tile
		if(x > 0 && y < sizeY - 1 && mapData[x-1, y+1] == 1)
			return true;
		if(x < sizeX - 1 && y < sizeY - 1 && mapData[x+1, y+1] == 1)
			return true;

		return false;
		
	}
}
