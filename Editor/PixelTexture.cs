using System;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Pixel.Texture
{
    public class PixelTexture : OdinEditorWindow
    {
        [MenuItem("Tools/Pixel Texture")]
        private static void OpenWindow()
        {
            GetWindow<PixelTexture>().Show();
        }

        [ShowInInspector, TabGroup("Create") , Range(1, 16), OnValueChanged(nameof(Resize))] private int _size;
        [ShowInInspector, TabGroup("Create"), AsPalette, PropertyOrder(1), PropertySpace] public Color[] _createPalette = new Color[1];
        [ShowInInspector, TabGroup("Edit"), HideLabel, OnValueChanged(nameof(Read)), InlineButton(nameof(Read), "Refresh")] private Texture2D _readTexture;
        [ShowInInspector, TabGroup("Edit"), AsPalette, PropertyOrder(1), PropertySpace] private Color[] _editPalette = new Color[1];

        private void Resize()
        {
            _createPalette = new Color[(int)Math.Pow(_size, 2)];
        }
        
        private void Read()
        {
            if(_readTexture == null)
                return;

            if (_readTexture.isReadable == false)
            {
                Debug.LogError("Texture is not readable");
                return;
            }
            
            _editPalette = _readTexture.GetPixels();
        }
        
        [Button("Save"), TabGroup("Create")]
        private void SaveCreate()
        {
            Save(_createPalette);
        }

        [Button("Save"), TabGroup("Edit")]
        private void SaveEdit()
        {
            Save(_editPalette);
        }
        
        private void Save(Color[] palette)
        {
            string absolutePath = EditorUtility.SaveFilePanel("Save as", "", "", "png");

            if(string.IsNullOrEmpty(absolutePath))
                return;
            
            string relativePath = "Assets" + absolutePath.Substring(Application.dataPath.Length);

            var texture = new Texture2D(_size, _size);
            texture.SetPixels(palette);
            texture.Apply();
            File.WriteAllBytes(absolutePath, texture.EncodeToPNG());
            AssetDatabase.Refresh();
            
            TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(relativePath);
            importer.isReadable = true;
            importer.mipmapEnabled = false;
            importer.filterMode = FilterMode.Point;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.npotScale = TextureImporterNPOTScale.None;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            
            AssetDatabase.ImportAsset(relativePath);
            AssetDatabase.Refresh();
        }
    }
}