using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using OpenCvSharp;
using OpenCvSharp.Blob;

public class dete{
	public IplImage imagem;
	CvMemStorage sto;
	IplImage imgSrc;
	IplImage imgHSV;
	IplImage imgH;
	IplImage imgS;
	IplImage imgV;
	IplImage imgBackProjection;
	IplImage imgFlesh;
	IplImage imgHull;
	IplImage imgDefect;
	IplImage imgContour;

	// Use this for initialization
	public dete(IplImage ima){
		this.imagem = ima;

		//IplImage imgSrc = new IplImage("C://hand_p.jpg", LoadMode.Color);
		Debug.Log ("Convertendo imagem no Dete.cs  ...");
		imgSrc = this.imagem;
		imgHSV = new IplImage(imgSrc.Size, BitDepth.U8, 3);
		imgH = new IplImage(imgSrc.Size, BitDepth.U8, 1);
		imgS = new IplImage(imgSrc.Size, BitDepth.U8, 1);
		imgV = new IplImage(imgSrc.Size, BitDepth.U8, 1);
		imgBackProjection = new IplImage(imgSrc.Size, BitDepth.U8, 1);
		imgFlesh = new IplImage(imgSrc.Size, BitDepth.U8, 1);
		imgHull = new IplImage(imgSrc.Size, BitDepth.U8, 1);
		imgDefect = new IplImage(imgSrc.Size, BitDepth.U8, 3);
		imgContour = new IplImage(imgSrc.Size, BitDepth.U8, 3);
		using (CvMemStorage storage = new CvMemStorage())
		{
			Cv.CvtColor(imgSrc, imgHSV, ColorConversion.BgrToHsv);
			Cv.CvtPixToPlane(imgHSV, imgH, imgS, imgV, null);
			IplImage[] hsvPlanes = { imgH, imgS, imgV };
			
			RetrieveFleshRegion(imgSrc, hsvPlanes, imgBackProjection);
			
			//FilterByMaximalBlob(imgBackProjection, imgFlesh);
			Interpolate(imgFlesh);
			
			CvSeq<CvPoint> contours = FindContours(imgFlesh, storage);
			if (contours != null)
			{
				Cv.DrawContours(imgContour, contours, CvColor.Red, CvColor.Green, 0, 3, LineType.AntiAlias);
				
				int[] hull;
				Cv.ConvexHull2(contours, out hull, ConvexHullOrientation.Clockwise);
				Cv.Copy(imgFlesh, imgHull);
				DrawConvexHull(contours, hull, imgHull);
				
				Cv.Copy(imgContour, imgDefect);
				CvSeq<CvConvexityDefect> defect = Cv.ConvexityDefects(contours, hull);
				DrawDefects(imgDefect, defect);
			}
			
			/*	using (new CvWindow("src", imgSrc))
				using (new CvWindow("back projection", imgBackProjection))
					using (new CvWindow("hull", imgHull))
					using (new CvWindow("defect", imgDefect))*/
			
			
			
			{
				Cv.WaitKey();
			}
		}

		//Start ();
	}
	private void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	private static void RetrieveFleshRegion(IplImage imgSrc, IplImage[] hsvPlanes, IplImage imgDst)
	{
		int[] histSize = new int[] { 30, 32 };
		float[] hRanges = { 0.0f, 20f };
		float[] sRanges = { 50f, 255f };
		float[][] ranges = { hRanges, sRanges };
		
		imgDst.Zero();
		using (CvHistogram hist = new CvHistogram(histSize, HistogramFormat.Array, ranges, true))
		{
			hist.Calc(hsvPlanes, false, null);
			float minValue, maxValue;
			hist.GetMinMaxValue(out minValue, out maxValue);
			hist.Normalize(imgSrc.Width * imgSrc.Height * 255 / maxValue);
			hist.CalcBackProject(hsvPlanes, imgDst);
		}
	}
	//comenta essa
	/*private static void FilterByMaximalBlob(IplImage imgSrc, IplImage imgDst)
	{
		CvBlobs blobs = new CvBlobs ();
		using (IplImage imgLabelData = new IplImage(imgSrc.Size, CvBlobLib.DepthLabel, 1))
		{
			imgDst.Zero();
			blobs.Label (imgLabelData);
			//blobs.Label(imgSrc,imgLabelData);
			//CvBlob max = blobs[blobs.GreaterBlob()];
			CvBlob max =blobs.GreaterBlob();
			if (max == null)
			{
				return;
			}
			blobs.FilterByArea(max.Area, max.Area);
			blobs.FilterLabels (imgDst);
			//blobs.FilterLabels(imgLabelData,imgDst );
		}
	}*/

	private static void Interpolate(IplImage img)
	{
		Cv.Dilate(img, img, null, 2);
		Cv.Erode(img, img, null, 2);
	}

	private static CvSeq<CvPoint> FindContours(IplImage img, CvMemStorage storage)
	{
		CvSeq<CvPoint> contours;
		using (IplImage imgClone = img.Clone())
		{
			Cv.FindContours(imgClone, storage, out contours);
			if (contours == null)
			{
				return null;
			}
			contours = Cv.ApproxPoly(contours, CvContour.SizeOf, storage, ApproxPolyMethod.DP, 3, true);
		}
		
		CvSeq<CvPoint> max = contours;
		for (CvSeq<CvPoint> c = contours; c != null; c = c.HNext)
		{
			if (max.Total < c.Total)
			{
				max = c;
			}
		}
		return max;
	}

	private static void DrawConvexHull(CvSeq<CvPoint> contours, int[] hull, IplImage img)
	{
		CvPoint pt0 = contours[hull.Last()].Value;
		foreach (int idx in hull)
		{
			CvPoint pt = contours[idx].Value;
			Cv.Line(img, pt0, pt, new CvColor(255, 255, 255));
			pt0 = pt;
		}
	}

	private static void DrawDefects(IplImage img, CvSeq<CvConvexityDefect> defect)
	{
		int count = 0;
		foreach (CvConvexityDefect item in defect)
		{
			CvPoint p1 = item.Start, p2 = item.End;
			double dist = GetDistance(p1, p2);
			CvPoint2D64f mid = GetMidpoint(p1, p2);
			img.DrawLine(p1, p2, CvColor.White, 3);
			img.DrawCircle(item.DepthPoint, 10, CvColor.Green, -1);
			img.DrawLine(mid, item.DepthPoint, CvColor.White, 1);
			Console.WriteLine("No:{0} Depth:{1} Dist:{2}", count, item.Depth, dist);
			count++;
		}
	}

	private static double GetDistance(CvPoint p1, CvPoint p2)
	{
		return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
	}
	
	private static CvPoint2D64f GetMidpoint(CvPoint p1, CvPoint p2)
	{
		return new CvPoint2D64f
		{
			X = (p1.X + p2.X) / 2.0,
			Y = (p1.Y + p2.Y) / 2.0
		};
	}

	public IplImage getHSVImage(){
		return imgHSV;
	}

	public IplImage getBackProjectionImage(){
		return imgBackProjection;
	}

	public IplImage getHullImage(){
		return imgHull;
	}

	public IplImage getDefectImage(){
		return imgDefect;
	}

}
