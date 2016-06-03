using UnityEngine;
using System.Collections;
 
public class HiResScreenShots : MonoBehaviour 
{
    int resWidth = 1920; 
    int resHeight = 1080;

	[Range (1, 20)]
	public int resolutionPower;
 
    private bool takeHiResShot = false;

	private bool takeScreenShot = false;
 
    public static string ScreenShotName(int width, int height) {
        return string.Format("{0}/screen_{1}x{2}_{3}.png", 
                             Application.dataPath, 
                             width, height, 
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }
 
    public void TakeHiResShot() {
        takeHiResShot = true;
    }

	void Start ()
	{
		resWidth *= resolutionPower;
		resHeight *= resolutionPower;
	}
 
    void LateUpdate()
    {
        takeHiResShot |= Input.GetKeyDown("k");
        if (takeHiResShot) 
        {
			takeHiResShot = false;
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            GetComponent<Camera>().targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            GetComponent<Camera>().Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            GetComponent<Camera>().targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(resWidth, resHeight);

			System.IO.FileStream _FileStream = new System.IO.FileStream(filename, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write);
            // Writes a block of bytes to this stream using data from a byte array.
            _FileStream.Write(bytes, 0, bytes.Length);
            // close file stream.
            _FileStream.Close();

            Debug.Log(string.Format("Took screenshot to: {0}", filename));
        }

		takeScreenShot |= Input.GetKeyDown("l");
		if (takeScreenShot) 
        {
			takeScreenShot = false;
			Application.CaptureScreenshot (ScreenShotName (0, 0), resolutionPower);
		}
    }
}