using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SniperToolMode
{
    BREAK,
    PAINT,
    SELECTION
}

public enum BrushType
{
    PENCIL,
    BALL,
    VOXEL,
    DISC,
    VOXEL_DISC,
    DISC_WALL,
    VOXEL_DISC_WALL
}
public enum BrushPaintType
{
    MATERIAL,
    INK,
    COMBO
}

public static class VoxelSniper
{
    public static SniperToolMode m_sniperToolMode = SniperToolMode.BREAK;

    public static BrushType m_brushType;
    public static BrushPaintType m_brushPaintType;
    public static uint m_brushSize = 1;
    public static uint m_blockID = 1;
    public static uint m_blockSubID = 0;

    public static void Reset()
    {
        m_brushType = BrushType.PENCIL;
        m_brushPaintType = BrushPaintType.MATERIAL;
        m_brushSize = 1;
        m_blockID = 0;
        m_blockSubID = 0;
    }

    public static void Paint(Vector3Int blockPosition, Axis blockNormal)
    {
        if (m_sniperToolMode != SniperToolMode.PAINT)
        {
            Debug.LogError("Using VoxelSniper:Paint on wrong mode");
            return;
        }

        switch (VoxelSniper.m_brushType)
        {
            case BrushType.PENCIL:
                WorldEdit.SetBlock(blockPosition, VoxelSniper.m_blockID, VoxelSniper.m_blockSubID);
                break;

            case BrushType.VOXEL:
                WorldEdit.SetBlockRegion(
                    blockPosition + new Vector3Int((int)-VoxelSniper.m_brushSize, (int)-VoxelSniper.m_brushSize, (int)-VoxelSniper.m_brushSize),
                    blockPosition + new Vector3Int((int)+VoxelSniper.m_brushSize, (int)+VoxelSniper.m_brushSize, (int)+VoxelSniper.m_brushSize),
                    VoxelSniper.m_blockID, VoxelSniper.m_blockSubID
                    );
                break;

            case BrushType.BALL:
                WorldEdit.SetBlockSphere(
                    blockPosition, new Vector3Int((int)VoxelSniper.m_brushSize, (int)VoxelSniper.m_brushSize, (int)VoxelSniper.m_brushSize),
                    VoxelSniper.m_blockID, VoxelSniper.m_blockSubID
                    );
                break;

            case BrushType.DISC:
                WorldEdit.SetBlockSphere(
                    blockPosition, new Vector3Int((int)VoxelSniper.m_brushSize, 0, (int)VoxelSniper.m_brushSize),
                    VoxelSniper.m_blockID, VoxelSniper.m_blockSubID
                    );
                break;

            case BrushType.DISC_WALL:
                // Check Axis
                switch (blockNormal)
                {
                    case Axis.X:
                        WorldEdit.SetBlockSphere(
                            blockPosition, new Vector3Int(0, (int)VoxelSniper.m_brushSize, (int)VoxelSniper.m_brushSize),
                            VoxelSniper.m_blockID, VoxelSniper.m_blockSubID
                            );
                        break;

                    case Axis.Y:
                        WorldEdit.SetBlockSphere(
                            blockPosition, new Vector3Int((int)VoxelSniper.m_brushSize, 0, (int)VoxelSniper.m_brushSize),
                            VoxelSniper.m_blockID, VoxelSniper.m_blockSubID
                            );
                        break;

                    case Axis.Z:
                        WorldEdit.SetBlockSphere(
                            blockPosition, new Vector3Int((int)VoxelSniper.m_brushSize, (int)VoxelSniper.m_brushSize, 0),
                            VoxelSniper.m_blockID, VoxelSniper.m_blockSubID
                            );
                        break;
                }
                break;

            case BrushType.VOXEL_DISC:
                WorldEdit.SetBlockRegion(
                    blockPosition + new Vector3Int((int)-VoxelSniper.m_brushSize, 0, (int)-VoxelSniper.m_brushSize),
                    blockPosition + new Vector3Int((int)+VoxelSniper.m_brushSize, 0, (int)+VoxelSniper.m_brushSize),
                    VoxelSniper.m_blockID, VoxelSniper.m_blockSubID
                    );
                break;

            case BrushType.VOXEL_DISC_WALL:
                // Check Axis
                switch (blockNormal)
                {
                    case Axis.X:
                        WorldEdit.SetBlockRegion(
                            blockPosition + new Vector3Int(0, (int)-VoxelSniper.m_brushSize, (int)-VoxelSniper.m_brushSize),
                            blockPosition + new Vector3Int(0, (int)+VoxelSniper.m_brushSize, (int)+VoxelSniper.m_brushSize),
                            VoxelSniper.m_blockID, VoxelSniper.m_blockSubID
                            );
                        break;

                    case Axis.Y:
                        WorldEdit.SetBlockRegion(
                            blockPosition + new Vector3Int((int)-VoxelSniper.m_brushSize, 0, (int)-VoxelSniper.m_brushSize),
                            blockPosition + new Vector3Int((int)+VoxelSniper.m_brushSize, 0, (int)+VoxelSniper.m_brushSize),
                            VoxelSniper.m_blockID, VoxelSniper.m_blockSubID
                            );
                        break;

                    case Axis.Z:
                        WorldEdit.SetBlockRegion(
                             blockPosition + new Vector3Int((int)-VoxelSniper.m_brushSize, (int)-VoxelSniper.m_brushSize, 0),
                             blockPosition + new Vector3Int((int)+VoxelSniper.m_brushSize, (int)+VoxelSniper.m_brushSize, 0),
                             VoxelSniper.m_blockID, VoxelSniper.m_blockSubID
                             );
                        break;
                }
                break;
        }
}
}
