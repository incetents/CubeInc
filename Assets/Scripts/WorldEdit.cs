using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldEdit
{
    // [UPDATE OCCURS] Modify an existing block from world space position
    public static void SetBlock(Vector3 position, Block block)
    {
        if(ChunkStorage.SetBlock(position, block))
            ChunkStorage.UpdateAtPosition(position);
    }

    // [UPDATE OCCURS] Modify an existing block from world space position
    public static void SetBlockRegion(Vector3 min, Vector3 max, Block block)
    {
        Vector3Int _min = new Vector3Int(Mathf.FloorToInt(min.x), Mathf.FloorToInt(min.y), Mathf.FloorToInt(min.z));
        Vector3Int _max = new Vector3Int(Mathf.FloorToInt(max.x), Mathf.FloorToInt(max.y), Mathf.FloorToInt(max.z));

        for (int x = _min.x; x <= _max.x; x++)
        {
            for (int y = _min.y; y <= _max.y; y++)
            {
                for (int z = _min.z; z <= _max.z; z++)
                {
                    if (ChunkStorage.SetBlock(new Vector3(x, y, z), block))
                        ChunkStorage.UpdateAtPosition(new Vector3(x, y, z));
                }
            }
        }
    }
}
