using UnityEngine;
using System.Collections;
using OpenCvSharp;

public class DetectaMao : MonoBehaviour {
	// Use this for initialization
	CvWindow window;
	CvCapture cap;
	CvMemStorage cm;
	private int imWidth = 160; // 640 -> 200 -> 160
	private int imHeight = 120; // 480 -> 150 -> 120

	void Start () {
		window = new CvWindow("Captura");

	}
	
	// Update is called once per frame
	void Update () {
		// Deixa a imagem menor, pra nao ficar travando
		window.Resize (imWidth, imHeight);
		cap = CvCapture.FromCamera (0);
		cap.FrameWidth = imWidth;
		cap.FrameHeight = imHeight;
		
		
		
		// Mostra a imagem na janela
		window.Image = cap.QueryFrame();
	}

	/*unsafe void detect(IplImage* img_8uc1, IplImage* img_8uc3){
		// 8uc1 is BW Image with hand as white and 8uc3 is the original image
		CvMemStorage* storage = Cv.CreateMemStorage ();
		CvSeq* first_contour = null;
		CvSeq* maxitem = null;
		double area = 0;
		double areamax = 0;
		int maxn = 0;

		// function to find the white objects in the image and return the object boundaries
		int Nc = Cv.FindContours (
			img_8uc1,
			storage,
			&first_contour, 
			sizeof(CvContour),
			ContourRetrieval.List
		);
	}

*/

















}