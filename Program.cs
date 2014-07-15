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
			Mat src = new Mat("TestImages/2013-12-27-09G37_TRB.jpg", LoadMode.Color);

			src = src.Resize (new Size (420, 625));

			Mat srcGrey = src.CvtColor(ColorConversion.BgrToGray);
			Mat dst = new Mat ();
			Point[][] contours;
			Mat invertColour = new Mat (src.Rows, src.Cols, src.Type(), new Scalar(255, 255, 255) );
			HiearchyIndex[] hierarchy;
			Mat contoursLines = new Mat(src.Rows, src.Cols, src.Type());
		
			InputArray element = Cv2.GetStructuringElement (StructuringElementShape.Ellipse, new Size (16, 16));
			Mat thresh = srcGrey.Threshold (230, 255, ThresholdType.Binary).Dilate(element);

			Cv2.FindContours (thresh, out contours, out hierarchy, ContourRetrieval.Tree, ContourChain.ApproxSimple);

			// Find center of mass in contour
			Moments center = Cv2.Moments (contours [0]);

			// Convert moment matrix to X/Y coords
			int x = (int) (center.M10 / center.M00);
			int y = (int) (center.M01 / center.M00);

			src.Circle (new Point (x, y), 5, CvColor.CornflowerBlue, 2);
			src.DrawContours (contours, 1, CvColor.Green, 2);

			using (var orig = new Window ("src image", src)) 
			{
				Cv2.WaitKey();
			}
		}
	}
}
