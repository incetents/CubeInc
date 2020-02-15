using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldEdit
{
    // [UPDATE OCCURS] Modify an existing block from world space position
    public static void SetBlock(Vector3 worldPosition, uint id, uint subId)
    {
        if(ChunkStorage.SetBlock(worldPosition, new Block(id, subId, worldPosition)))
            ChunkStorage.MakePositionDirty(worldPosition);
    }

    // [UPDATE OCCURS] Modify an existing block from world space position
    public static void SetBlockRegion(Vector3 min, Vector3 max, uint id, uint subId)
    {
        Vector3Int _min = new Vector3Int(Mathf.FloorToInt(min.x), Mathf.FloorToInt(min.y), Mathf.FloorToInt(min.z));
        Vector3Int _max = new Vector3Int(Mathf.FloorToInt(max.x), Mathf.FloorToInt(max.y), Mathf.FloorToInt(max.z));

        for (int x = _min.x; x <= _max.x; x++)
        {
            for (int y = _min.y; y <= _max.y; y++)
            {
                for (int z = _min.z; z <= _max.z; z++)
                {
                    Vector3 newWorldPosition = new Vector3(x, y, z);
                    if (ChunkStorage.SetBlock(newWorldPosition, new Block(id, subId, newWorldPosition)))
                        ChunkStorage.MakePositionDirty(newWorldPosition);
                }
            }
        }
    }

    // [UPDATE OCCURS] Modify an existing block from world space position
    public static void SetBlockSphere(Vector3 worldPosition, Vector3 radius, uint id, uint subId)
    {
        Vector3Int _center = new Vector3Int(Mathf.FloorToInt(worldPosition.x), Mathf.FloorToInt(worldPosition.y), Mathf.FloorToInt(worldPosition.z));

        Vector3Int _min = new Vector3Int(Mathf.FloorToInt(worldPosition.x - radius.x), Mathf.FloorToInt(worldPosition.y - radius.y), Mathf.FloorToInt(worldPosition.z - radius.z));
        Vector3Int _max = new Vector3Int(Mathf.FloorToInt(worldPosition.x + radius.x), Mathf.FloorToInt(worldPosition.y + radius.y), Mathf.FloorToInt(worldPosition.z + radius.z));

        for (int x = _min.x; x <= _max.x; x++)
        {
            for (int y = _min.y; y <= _max.y; y++)
            {
                for (int z = _min.z; z <= _max.z; z++)
                {
                    Vector3 newWorldPosition = new Vector3(x, y, z);
                    Vector3 offsetPosition = (_center - new Vector3Int(x, y, z));

                    // Check if new position is inside the ellipsoid (Radius)
                    float ellipsoidCheck =
                        Mathf.Pow(offsetPosition.x / radius.x, 2.0f) +
                        Mathf.Pow(offsetPosition.y / radius.y, 2.0f) +
                        Mathf.Pow(offsetPosition.z / radius.z, 2.0f);

                    if (ellipsoidCheck <= 1.0f)
                    {
                        if (ChunkStorage.SetBlock(newWorldPosition, new Block(id, subId, newWorldPosition)))
                            ChunkStorage.MakePositionDirty(newWorldPosition);
                    }
                }
            }
        }
    }
}
