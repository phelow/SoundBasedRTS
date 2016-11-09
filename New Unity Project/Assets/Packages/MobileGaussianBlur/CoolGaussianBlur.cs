using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CoolGaussianBlur : MonoBehaviour 
{
	[SerializeField] private Material screenMat;

	public Material ScreenMat { get{ return screenMat; } }

	void Start ()
	{
		screenMat.SetVector("_Resolution", new Vector4(Screen.width, Screen.height, 0, 0));
	}

	void OnRenderImage (RenderTexture src, RenderTexture dst)
	{
		Graphics.Blit(src, dst, screenMat);
	}	
}
