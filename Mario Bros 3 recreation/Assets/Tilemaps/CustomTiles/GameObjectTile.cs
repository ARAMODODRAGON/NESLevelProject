using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
[CreateAssetMenu(fileName = "New GameObject Tile", menuName = "Tiles/GameObject Tile")]
public class GameObjectTile : TileBase {
    public Sprite defaultSprite;
    public GameObject gos;
    
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
        tileData.sprite = defaultSprite;
        tileData.gameObject = gos;
    }
}
