using System;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;

namespace Swatcher
{
	class MainClass
	{
		[STAThread]
		public static void Main (string[] args)
		{
			Mat src = new Mat("TestImages/07G28_NIV.jpg", LoadMode.GrayScale);
			Mat dst = new Mat ();
			Point[][] contours;

			Mat srcCopy = src.Clone ();

			HiearchyIndex[] hierarchy;

			Cv2.FindContours (srcCopy, out contours, out hierarchy, ContourRetrieval.FloodFill, ContourChain.ApproxSimple);

			Mat contoursLines = new Mat(1105, 1105, MatType.CV_8UC3);

			Cv2.DrawContours(contoursLines, contours, 0, CvColor.Green, 1, (LineType)8, hierarchy, 100, new Point(0, 0));

			using (new Window ("src image", src)) 
			using (new Window("dst image", contoursLines)) 
			{
				Cv2.WaitKey();
			}
		}
	}
}
