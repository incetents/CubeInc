using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SniperToolMode
{
    PAINT,
    SELECTION
}

public enum BrushType
{
    PENCIL,
    BALL,
    VOXEL,
    DISC,
    VOXEL_DISC
}
public enum BrushPaintType
{
    MATERIAL,
    INK,
    COMBO
}

public static class VoxelSniper
{
    public static SniperToolMode m_sniperToolMode = SniperToolMode.PAINT;

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
}
