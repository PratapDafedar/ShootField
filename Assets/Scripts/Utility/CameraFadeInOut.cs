// simple fading script
// A texture is stretched over the entire screen. The color of the pixel is set each frame until it reaches its target color.
using UnityEngine;


public class CameraFadeInOut : MonoBehaviour
{   
	public static CameraFadeInOut Instance;

	private GUIStyle m_BackgroundStyle = new GUIStyle();		// Style for background tiling
	private Texture2D m_FadeTexture;				// 1x1 pixel texture used for fading
	private Color currentScreenOverlayColor = new Color(0,0,0,0);	// default starting color: black and fully transparrent
	private Color m_DeltaColor = new Color(0,0,0,0);		// the delta-color is basically the "speed / second" at which the current color should change
	private int m_FadeGUIDepth = -1000;				// make sure this texture is drawn on top of everything

	[HideInInspector]
	public Color targetScreenOverlayColor = new Color(0,0,0,0);	// default target color: black and fully transparrent
	[HideInInspector]
	public bool isFadeInFinish = false;
	private float m_duration;

	private float fadeInOutLockTimer;

	// initialize the texture, background-style and initial color:
	private void Awake()
	{		
		m_FadeTexture = new Texture2D(1, 1);        
		m_BackgroundStyle.normal.background = m_FadeTexture;
		SetScreenOverlayColor(currentScreenOverlayColor);
	}
	
	
	// draw the texture and perform the fade:
	private void OnGUI()
	{   
		// if the current color of the screen is not equal to the desired color: keep fading!
		if (currentScreenOverlayColor != targetScreenOverlayColor && !isFadeInFinish)
		{			
			// if the difference between the current alpha and the desired alpha is smaller than delta-alpha * deltaTime, then we're pretty much done fading:
			if (Mathf.Abs(currentScreenOverlayColor.a - targetScreenOverlayColor.a) < Mathf.Abs(m_DeltaColor.a) * Time.deltaTime)
			{
				currentScreenOverlayColor = targetScreenOverlayColor;
				SetScreenOverlayColor(currentScreenOverlayColor);
//				m_DeltaColor = new Color(0,0,0,0);

				isFadeInFinish = true;
				targetScreenOverlayColor = new Color (0, 0, 0, 0);
				m_DeltaColor = (targetScreenOverlayColor - currentScreenOverlayColor) / m_duration;
			}
			else
			{
				// fade!
				SetScreenOverlayColor(currentScreenOverlayColor + m_DeltaColor * Time.deltaTime);
			}
		}
		else if (isFadeInFinish && fadeInOutLockTimer > m_duration)
		{
			// if the difference between the current alpha and the desired alpha is smaller than delta-alpha * deltaTime, then we're pretty much done fading:
			if (Mathf.Abs(currentScreenOverlayColor.a - targetScreenOverlayColor.a) < Mathf.Abs(m_DeltaColor.a) * Time.deltaTime)
			{
				currentScreenOverlayColor = new Color (0, 0, 0, 0);
				SetScreenOverlayColor(currentScreenOverlayColor);
				m_DeltaColor = new Color(0,0,0,1);
			}
			else
			{
				// fade!
				SetScreenOverlayColor(currentScreenOverlayColor + m_DeltaColor * Time.deltaTime);
				isFadeInFinish = false;
			}
		}

		if (isFadeInFinish)
		{
			fadeInOutLockTimer += Time.deltaTime;
		}
		
		// only draw the texture when the alpha value is greater than 0:
		if (currentScreenOverlayColor.a > 0)
		{			
			GUI.depth = m_FadeGUIDepth;
			GUI.Label(new Rect(-10, -10, Screen.width + 10, Screen.height + 10), m_FadeTexture, m_BackgroundStyle);
		}
	}
	
	
	// instantly set the current color of the screen-texture to "newScreenOverlayColor"
	// can be usefull if you want to start a scene fully black and then fade to opague
	public void SetScreenOverlayColor(Color newScreenOverlayColor)
	{
		currentScreenOverlayColor = newScreenOverlayColor;
		m_FadeTexture.SetPixel(0, 0, currentScreenOverlayColor);
		m_FadeTexture.Apply();
	}

	public void StartFade (Color newScreenOverlayColor, float fadeDuration)
	{
		if (fadeDuration <= 0.0f)// can't have a fade last -2455.05 seconds!
		{
			SetScreenOverlayColor (newScreenOverlayColor);
		}
		else// initiate the fade: set the target-color and the delta-color
		{
			m_duration = fadeDuration;
			targetScreenOverlayColor = newScreenOverlayColor;
			m_DeltaColor = (targetScreenOverlayColor - currentScreenOverlayColor) / fadeDuration;

			fadeInOutLockTimer = 0.0f;
		}
	}	

	public static CameraFadeInOut StartFadeInOut (float fadeDuration)
	{
		return StartFadeInOut (fadeDuration, Color.black);
	}

	public static CameraFadeInOut StartFadeIn (float fadeDuration)
	{
		return StartFadeIn (fadeDuration, Color.black);
	}

    public static CameraFadeInOut StartFadeOut(float fadeDuration)
    {
        return StartFadeOut(fadeDuration, Color.black);
    }

	public static CameraFadeInOut StartFadeIn (float fadeDuration, Color fadeOutColor)
	{
		if (Instance == null)
		{
			GameObject go = new GameObject ("FadeInOut");
			Instance = go.AddComponent <CameraFadeInOut> ();
		}
		Instance.StartFade (fadeOutColor, fadeDuration / 2.0f);
        Instance.isFadeInFinish = true;
        Instance.fadeInOutLockTimer = 9999999f;
        Instance.targetScreenOverlayColor.a = -0.1f;
		return Instance;
	}

    public static CameraFadeInOut StartFadeOut(float fadeDuration, Color fadeOutColor)
    {
        if (Instance == null)
        {
            GameObject go = new GameObject("FadeInOut");
            Instance = go.AddComponent<CameraFadeInOut>();
        }
        Instance.StartFade(fadeOutColor, fadeDuration / 3.0f);
        Instance.isFadeInFinish = true;
        Instance.targetScreenOverlayColor = fadeOutColor;
        Instance.targetScreenOverlayColor.a = 0f;
        Instance.currentScreenOverlayColor = fadeOutColor;
        Instance.currentScreenOverlayColor.a = 1f;
        Instance.SetScreenOverlayColor(fadeOutColor);
        Instance.m_DeltaColor = new Color(0, 0, 0, -1f);
        return Instance;
    }

	// initiate a fade from the current screen color (set using "SetScreenOverlayColor") towards "newScreenOverlayColor" taking "fadeDuration" seconds
	public static CameraFadeInOut StartFadeInOut (float fadeDuration, Color newScreenOverlayColor)
	{
		if (Instance == null)
		{
			GameObject go = new GameObject ("FadeInOut");
			Instance = go.AddComponent <CameraFadeInOut> ();
		}
		Instance.StartFade (newScreenOverlayColor, fadeDuration / 3.0f);
		return Instance;
	}
}