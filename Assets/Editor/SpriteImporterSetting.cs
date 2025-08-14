using UnityEditor;
using UnityEngine;

public class SpriteImporterSetting : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        // 只处理图片文件
        if (assetPath.EndsWith(".png") || assetPath.EndsWith(".jpg") ||
            assetPath.EndsWith(".jpeg") || assetPath.EndsWith(".gif"))
        {
            TextureImporter importer = assetImporter as TextureImporter;
            if (importer != null)
            {
                // 强制设置为Sprite
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                // 设置默认像素模式
                importer.spritePixelsPerUnit = 100;
                // 关闭mipmap（2D游戏通常不需要）
                importer.mipmapEnabled = false;
            }
        }
    }
}