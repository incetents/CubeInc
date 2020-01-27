using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAtlasGenerator
{
    // Data
    private Texture2D m_textureAtlas;
    private int m_atlasSize;
    private int m_atlasLengthCount;
    private int m_blockPixelSize;
    private int m_padding;
    private float m_texelSize;

    public Texture2D GetAtlasTexture()
    {
        return m_textureAtlas;
    }

    public BlockAtlasGenerator(BlockInfoImport[] blocks, int blockPixelSize, int padding)
    {
        // Setup Values
        m_blockPixelSize = blockPixelSize;
        m_padding = padding;
        m_atlasLengthCount = Mathf.CeilToInt(Mathf.Sqrt(blocks.Length));
        m_atlasSize = (blockPixelSize + 2 * padding) * m_atlasLengthCount;
        m_texelSize = 1.0f / m_atlasSize;

        // Create Texture
        m_textureAtlas = new Texture2D(
            m_atlasSize, m_atlasSize,
            TextureFormat.RGB24,
            true
            );
        // Parameters
        m_textureAtlas.filterMode = FilterMode.Point;
        m_textureAtlas.wrapMode = TextureWrapMode.Clamp;

        // Iterate through blocks
        int blockCounter = 0;
        foreach (BlockInfoImport info in blocks)
        {
            if (info.m_texture == null)
            {
                Debug.LogError("BlockInfoImport missing Texture: " + info.name);
                continue;
            }

            Debug.Assert(info.m_texture.width == m_blockPixelSize,
                "Block Texture Size does not match specified size (Block Size= " + info.m_texture.width.ToString() + ")(Target Size: " + m_blockPixelSize.ToString()
                );

            // Location on atlas
            int x = (blockCounter % m_atlasLengthCount);
            int y = Mathf.FloorToInt(blockCounter / m_atlasLengthCount);

            // Set Main Block in center
            m_textureAtlas.SetPixels(
                x * (blockPixelSize + padding * 2) + padding,
                y * (blockPixelSize + padding * 2) + padding,
                blockPixelSize, blockPixelSize,
                info.m_texture.GetPixels()
                );

            // Set Padding around block
            if (padding > 0)
            {
                // Horizontal Lines
                Color[] TopHLine = info.m_texture.GetPixels(0, 0, m_blockPixelSize, 1);
                Color[] BotHLine = info.m_texture.GetPixels(0, m_blockPixelSize - 1, m_blockPixelSize, 1);
                // Vertical Lines
                Color[] LeftVLine = info.m_texture.GetPixels(0, 0, 1, m_blockPixelSize);
                Color[] RightVLine = info.m_texture.GetPixels(m_blockPixelSize - 1, 0, 1, m_blockPixelSize);
                
                // Add Padding Lines
                for (int p = 0; p < padding; p++)
                {
                    // Top Section
                    m_textureAtlas.SetPixels(
                        x * (blockPixelSize + padding * 2) + padding,
                        y * (blockPixelSize + padding * 2) + p,
                        blockPixelSize, 1,
                        TopHLine
                        );
                    
                    // Bottom Section
                    m_textureAtlas.SetPixels(
                        x * (blockPixelSize + padding * 2) + padding,
                        y * (blockPixelSize + padding * 2) + padding + blockPixelSize + p,
                        blockPixelSize, 1,
                        LeftVLine
                        );
                    
                    // Left Section
                    m_textureAtlas.SetPixels(
                        x * (blockPixelSize + padding * 2) + p,
                        y * (blockPixelSize + padding * 2) + padding,
                        1, blockPixelSize,
                        TopHLine
                        );
                    
                    // Right Section
                    m_textureAtlas.SetPixels(
                        x * (blockPixelSize + padding * 2) + padding + blockPixelSize + p,
                        y * (blockPixelSize + padding * 2) + padding,
                        1, blockPixelSize,
                        RightVLine
                        );
                }

                // Corner Pixels
                Color TopLeft = info.m_texture.GetPixel(0, 0);
                Color TopRight = info.m_texture.GetPixel(blockPixelSize - 1, 0);
                Color BottomLeft = info.m_texture.GetPixel(0, blockPixelSize - 1);
                Color BottomRight = info.m_texture.GetPixel(blockPixelSize - 1, blockPixelSize - 1);
                // Convert Corner Pixels to corner rectangles for faster blitting
                Color[] TopLeftRect = new Color[padding * padding];
                Color[] TopRightRect = new Color[padding * padding];
                Color[] BotLeftRect = new Color[padding * padding];
                Color[] BotRightRect = new Color[padding * padding];
                // Fill
                int pSquared = padding * padding;
                for (int p = 0; p < pSquared; p++)
                {
                    TopLeftRect[p] = TopLeft;
                    TopRightRect[p] = TopRight;
                    BotLeftRect[p] = BottomLeft;
                    BotRightRect[p] = BottomRight;
                }

                // Add Padding Corners

                // TopLeft
                m_textureAtlas.SetPixels(
                        x * (blockPixelSize + padding * 2) + 0,
                        y * (blockPixelSize + padding * 2) + 0,
                        padding, padding,
                        TopLeftRect
                        );

                // TopRight
                m_textureAtlas.SetPixels(
                        x * (blockPixelSize + padding * 2) + padding + blockPixelSize,
                        y * (blockPixelSize + padding * 2) + 0,
                        padding, padding,
                        TopRightRect
                        );

                // BotLeft
                m_textureAtlas.SetPixels(
                        x * (blockPixelSize + padding * 2) + 0,
                        y * (blockPixelSize + padding * 2) + padding + blockPixelSize,
                        padding, padding,
                        BotLeftRect
                        );

                // BotRight
                m_textureAtlas.SetPixels(
                        x * (blockPixelSize + padding * 2) + padding + blockPixelSize,
                        y * (blockPixelSize + padding * 2) + padding + blockPixelSize,
                        padding, padding,
                        BotRightRect
                        );
            }

            // Add block to dictionary
            BlockInfo block = new BlockInfo(info.m_id);
            block.m_name = info.name;
            block.m_texture = info.m_texture;
            block.m_atlasLocation = new Vector4(
                ((float)x / (float)m_atlasLengthCount) + m_texelSize * padding,
                ((float)(x + 1) / (float)m_atlasLengthCount) - m_texelSize * padding,
                ((float)y / (float)m_atlasLengthCount) + m_texelSize * padding,
                ((float)(y + 1) / (float)m_atlasLengthCount) - m_texelSize * padding
                );

            Debug.Assert(
                blockPixelSize == info.m_texture.width && blockPixelSize == info.m_texture.height,
                "Imported Block Texture does not match blockPixelSize setting"
                );

            BlockDictionary.Set(block);
            //

            // Flag
            blockCounter++;
        }

        m_textureAtlas.Apply(true);
    }
}
