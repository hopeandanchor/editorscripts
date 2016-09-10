using UnityEngine;
using UnityEditor;
using HAA.Fonts;
using HAA.EditorUtils;
using System;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HAA.EditorUtils
{
	public class SpriteSplitterEditor : EditorWindow
	{
		private float sliceWidth = 20;
		private float sliceHeight = 12;
		private int maxTextureSize = 256;
		private int imageWidth;
		private int imageHeight;
		private string[] names;
		private string spritePath;
		private string listPath;
		private bool showFileChooser;
		private bool selectImage;
		private bool selectNames;

		[MenuItem ("HAA/Sprite Splitter Editor")]
		public static void  ShowWindow ()
		{
			EditorWindow.GetWindow (typeof(SpriteSplitterEditor));
		}

		void OnInspectorUpdate()
		{
			if(selectImage)
			{
				spritePath = EditorUtility.OpenFilePanel("Select Image", spritePath, "png,jpg,jpeg,tif,tiff,gif,bmp");
				spritePath = spritePath.Replace(Application.dataPath, "Assets");
				Debug.Log(spritePath);
				selectImage = false;
			}
			if(selectNames)
			{
				listPath = EditorUtility.OpenFilePanel("Choose File", listPath, "");
				if(!string.IsNullOrEmpty(listPath)) names = File.ReadAllLines(listPath);
				selectNames = false;
			}
		}

		void OnGUI ()
		{
			DrawSpriteSplitterEditor();
		}

		public void DrawSpriteSplitterEditor()
		{
			EditorGUI.BeginDisabledGroup(selectImage || selectNames);

			EditorGUILayout.LabelField("Sprite Splitter", EditorStyles.boldLabel);
			if(GUILayout.Button("Select Image"))
			{
				selectImage = true;
			}

			if(GUILayout.Button("Select File Names List"))
			{
				selectNames = true;
			}

			sliceWidth = EditorGUILayout.FloatField("Slice Width", sliceWidth);
			sliceHeight = EditorGUILayout.FloatField("Slice Height", sliceHeight);
			maxTextureSize = EditorGUILayout.IntField("Texture Size", maxTextureSize);

			EditorGUI.BeginDisabledGroup (string.IsNullOrEmpty(spritePath) || !File.Exists(spritePath));
			if(GUILayout.Button("Slice"))
			{
				SplitSprite();
			}
			EditorGUI.EndDisabledGroup();

			EditorGUI.EndDisabledGroup();
		}

		public void SplitSprite()
		{
			if(File.Exists(spritePath))
			{
				TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(spritePath);
				importer.textureType = TextureImporterType.Sprite;
				importer.spriteImportMode = SpriteImportMode.Multiple;
				importer.textureFormat = TextureImporterFormat.AutomaticTruecolor;
				importer.maxTextureSize = maxTextureSize;
				importer.mipmapEnabled = false;

				SetImageSize(importer);
				Debug.Log(imageHeight);
				Debug.Log(imageWidth);

				int maxColumns = Mathf.FloorToInt(imageWidth/sliceWidth);
				int maxRows = Mathf.FloorToInt(imageHeight/sliceHeight);

				List<SpriteMetaData> spritesheet = new List<SpriteMetaData>();
				int index = 0;

				for (int row = maxRows - 1; row >= 0 ; row--)
				{
					for (int column = 0; column < maxColumns; column++)
					{
						if(index >= names.Length) break;
						SpriteMetaData metadata = new SpriteMetaData();
						metadata.alignment = 1;
						metadata.name = names[index];
						metadata.pivot = new Vector2(0, 1);
						float posX = sliceWidth * column;
						float posY = sliceHeight * row;
						metadata.rect = new Rect(posX, posY, sliceWidth, sliceHeight);
						spritesheet.Add(metadata);
						index++;
					}
				}

				importer.spritesheet = spritesheet.ToArray();
				importer.SaveAndReimport();
			}
			else
			{
				Debug.LogError(string.Format("{0} does not exist", spritePath));
			}
		}

		private void SetImageSize(TextureImporter importer)
		{
			//http://forum.unity3d.com/threads/getting-original-size-of-texture-asset-in-pixels.165295/
			object[] args = new object[2] { 0, 0 };
			MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
			mi.Invoke(importer, args);

			imageWidth = (int)args[0];
			imageHeight = (int)args[1];
		}
	}
}
