using UnityEditor;
using UnityEngine;

public class SpriteImporterSetting : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        // ֻ����ͼƬ�ļ�
        if (assetPath.EndsWith(".png") || assetPath.EndsWith(".jpg") ||
            assetPath.EndsWith(".jpeg") || assetPath.EndsWith(".gif"))
        {
            TextureImporter importer = assetImporter as TextureImporter;
            if (importer != null)
            {
                // ǿ������ΪSprite
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                // ����Ĭ������ģʽ
                importer.spritePixelsPerUnit = 100;
                // �ر�mipmap��2D��Ϸͨ������Ҫ��
                importer.mipmapEnabled = false;
            }
        }
    }
}