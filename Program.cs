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
			Mat src = new Mat("TestImages/07G28_NIV.jpg", LoadMode.Color);

			src = src.Resize (new Size (420, 625));

			Mat srcGrey = src.CvtColor(ColorConversion.BgrToGray);
			Mat dst = new Mat ();
			Point[][] contours;
			Mat invertColour = new Mat (src.Rows, src.Cols, src.Type(), new Scalar(255, 255, 255) );
			HiearchyIndex[] hierarchy;
			Mat contoursLines = new Mat(src.Rows, src.Cols, src.Type());
		
			Mat thresh = srcGrey.Threshold (230, 255, ThresholdType.Binary);

			Cv2.FindContours (thresh, out contours, out hierarchy, ContourRetrieval.Tree, ContourChain.ApproxSimple);

			src.DrawContours (contours, 1, CvColor.Green, 2);

			using (var orig = new Window ("src image", src)) 
			{
				Cv2.WaitKey();
			}
		}
	}
}
