using UnityEngine;

[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Camera Glitch"), ExecuteInEditMode]
public class CameraGlitch : MonoBehaviour
{
	[Header("Textures")]

	[Tooltip("The texture to use for glitch pattern.\nIf non specified the default one will be used.\n\nApplied on scene start.")]
	public Texture GlitchTexture;

	[Tooltip("The texture to show on top of the final image.\nIf non specified the default one will be used.\n\nApplied on scene start.")]
	public Texture OverlayTexture;

	[Tooltip("Whether to show the overlay texture.")]
	public bool ShowOverlay = false;


	[Header("Intensity")]

	[Range(.0f, 10f), Tooltip("General intensity of the effect.")]
	public float Intensity = 1.0f;

	[Tooltip("Whether to randomize glitch triggering.")]
	public bool RandomGlitchFrequency = true;

	[Range(.001f, 1f), Tooltip("Frequency of glitches.\n\nIgnored if RandomGlitchFrequency is enabled.")]
	public float GlitchFrequency = .5f;


	[Header("UV shifting")]

	[Tooltip("Whether to apply shifting to the UV coordinates of the final image.")]
	public bool PerformUVShifting = true;

	[Range(.001f, 10f), Tooltip("Controls amount of the UV shifting.\n\nHave no effect when PerformUVShifting is disabled.")]
	public float ShiftAmount = 1f;

	[Tooltip("Whether to apply shifting to the whole image.\n\nHave no effect when PerformUVShifting is disabled.")]
	public bool PerformScreenShifting = true;


	[Header("Colors")]

	[Tooltip("Whether to apply color shifting to the image.")]
	public bool PerformColorShifting = true;

	[Tooltip("Glitches will be tinted with the specified color.\n\nHave no effect when PerformColorShifting is disabled.")]
	public Color TintColor = new Color(.2f, .2f, 0f, 0f);

	[Tooltip("Whether to perform an advanced color blending (color burn and divide) when shifting colors.\n\nHave no effect when PerformColorShifting is disabled.")]
	public bool BurnColors = true;

	[Tooltip("Whether to perform an advanced color blending (color dodge and difference) when shifting colors.\n\nHave no effect when PerformColorShifting is disabled.")]
	public bool DodgeColors = false;


	private float glitchUp, glitchDown, flicker, glitchUpTime = .05f, glitchDownTime = .05f, flickerTime = .5f;

	private Shader shader
	{
		get
		{
			if (_shader == null) _shader = Resources.Load("CameraGlitch/CameraGlitch") as Shader;
			return _shader;
		}
	}
	private Shader _shader;

	private Material material
	{
		get
		{
			if (_material == null)
			{
				_material = new Material(shader);
				_material.SetTexture("_GlitchTex", GlitchTexture ? GlitchTexture : Resources.Load("CameraGlitch/GlitchTexture") as Texture);
				_material.SetTextureScale("_GlitchTex", new Vector2(Screen.width / (GlitchTexture ? (float)GlitchTexture.width : 512f), 
					Screen.height / (GlitchTexture ? (float)GlitchTexture.height : 512f)));
				_material.SetTexture("_OverlayTex", OverlayTexture ? OverlayTexture : Resources.Load("CameraGlitch/OverlayTexture") as Texture);
				_material.hideFlags = HideFlags.HideAndDontSave;
			}
			return _material;
		}
	}
	private Material _material;

	private void Start ()
	{
		_material = null; // force to reinit the material on scene start

		if (!SystemInfo.supportsImageEffects || !shader || !shader.isSupported)
			enabled = false;

		flickerTime = RandomGlitchFrequency ? Random.value : 1f - GlitchFrequency;
		glitchUpTime = RandomGlitchFrequency ? Random.value : .1f - GlitchFrequency / 10f;
		glitchDownTime = RandomGlitchFrequency ? Random.value : .1f - GlitchFrequency / 10f;
	}

	private void OnDisable ()
	{
		if (_material) DestroyImmediate(_material);
	}

	private void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Intensity", Intensity);
		material.SetFloat("_ShowOverlay", ShowOverlay ? 1 : 0);
		material.SetColor("_ColorTint", TintColor);
		material.SetFloat("_BurnColors", BurnColors ? 1 : 0);
		material.SetFloat("_DodgeColors", DodgeColors ? 1 : 0);
		material.SetFloat("_PerformUVShifting", PerformUVShifting ? 1 : 0);
		material.SetFloat("_PerformColorShifting", PerformColorShifting ? 1 : 0);
		material.SetFloat("_PerformScreenShifting", PerformScreenShifting ? 1 : 0);

		if (Intensity == 0) material.SetFloat("filterRadius", 0);

		glitchUp += Time.deltaTime * Intensity;
		glitchDown += Time.deltaTime * Intensity;
		flicker += Time.deltaTime * Intensity;

		if (flicker > flickerTime)
		{
			material.SetFloat("filterRadius", Random.Range(-3f, 3f) * Intensity * ShiftAmount);
			material.SetTextureOffset("_GlitchTex", new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)));
			flicker = 0;
			flickerTime = RandomGlitchFrequency ? Random.value : 1f - GlitchFrequency;
		}

		if (glitchUp > glitchUpTime)
		{
			if (Random.Range(0f, 1f) < .1f * Intensity) material.SetFloat("flipUp", Random.Range(0f, 1f) * Intensity);
			else material.SetFloat("flipUp", 0);

			glitchUp = 0;
			glitchUpTime = RandomGlitchFrequency ? Random.value / 10f : .1f - GlitchFrequency / 10f;
		}

		if (glitchDown > glitchDownTime)
		{
			if (Random.Range(0f, 1f) < .1f * Intensity) material.SetFloat("flipDown", 1f - Random.Range(0f, 1f) * Intensity);
			else material.SetFloat("flipDown", 1f);

			glitchDown = 0;
			glitchDownTime = RandomGlitchFrequency ? Random.value / 10f : .1f - GlitchFrequency / 10f;
		}

		if (Random.Range(0f, 1f) < .1f * Intensity * (RandomGlitchFrequency ? 1 : GlitchFrequency))
			material.SetFloat("displace", Random.value * Intensity);
		else material.SetFloat("displace", 0);

		Graphics.Blit(source, destination, material);
	}
}