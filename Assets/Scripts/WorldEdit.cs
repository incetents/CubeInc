using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldEdit
{
    // [UPDATE OCCURS] Modify an existing block from world space position
    public static void SetBlock(Vector3Int worldPosition, uint id, uint subId)
    {
        if(ChunkStorage.SetBlock(worldPosition, Block.CreateWorldBlock(id, subId, worldPosition)))
        {
            ChunkStorage.MakePositionDirty(worldPosition);
        }
    }

    // [UPDATE OCCURS] Modify an existing block from world space position
    public static void SetBlockRegion(Vector3Int min, Vector3Int max, uint id, uint subId)
    {
        for (int x = min.x; x <= max.x; x++)
        {
            for (int y = min.y; y <= max.y; y++)
            {
                for (int z = min.z; z <= max.z; z++)
                {
                    Vector3Int newWorldPosition = new Vector3Int(x, y, z);
                    if (ChunkStorage.SetBlock(newWorldPosition, Block.CreateWorldBlock(id, subId, newWorldPosition)))
                        ChunkStorage.MakePositionDirty(newWorldPosition);
                }
            }
        }
    }

    // [UPDATE OCCURS] Modify an existing block from world space position
    public static void SetBlockSphere(Vector3Int worldPosition, Vector3Int radius, uint id, uint subId)
    {
        Vector3Int min = new Vector3Int(Mathf.FloorToInt(worldPosition.x - radius.x), Mathf.FloorToInt(worldPosition.y - radius.y), Mathf.FloorToInt(worldPosition.z - radius.z));
        Vector3Int max = new Vector3Int(Mathf.FloorToInt(worldPosition.x + radius.x), Mathf.FloorToInt(worldPosition.y + radius.y), Mathf.FloorToInt(worldPosition.z + radius.z));

        for (int x = min.x; x <= max.x; x++)
        {
            for (int y = min.y; y <= max.y; y++)
            {
                for (int z = min.z; z <= max.z; z++)
                {
                    Vector3Int newWorldPosition = new Vector3Int(x, y, z);
                    Vector3Int offsetPosition = (worldPosition - new Vector3Int(x, y, z));

                    // Check if new position is inside the ellipsoid (Radius)
                    float e_x = (radius.x == 0) ? 0 : ((float)offsetPosition.x / radius.x);
                    float e_y = (radius.y == 0) ? 0 : ((float)offsetPosition.y / radius.y);
                    float e_z = (radius.z == 0) ? 0 : ((float)offsetPosition.z / radius.z);

                    float ellipsoidCheck = e_x * e_x + e_y * e_y + e_z * e_z;
                    if (ellipsoidCheck <= 1.0f)
                    {
                        if (ChunkStorage.SetBlock(newWorldPosition, Block.CreateWorldBlock(id, subId, newWorldPosition)))
                            ChunkStorage.MakePositionDirty(newWorldPosition);
                    }
                }
            }
        }
    }
}
