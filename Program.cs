using System;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using System.Collections.Generic;

namespace Swatcher
{
	class MainClass
	{
		static List<Point> selector = new List<Point> {
			new Point(0, 0),
			new Point(32, 0),
			new Point(32, 32),
			new Point(0, 32)
		};

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
		
//			InputArray element = Cv2.GetStructuringElement (StructuringElementShape.Ellipse, new Size (16, 16));
//			Mat thresh = srcGrey.Threshold (230, 255, ThresholdType.Binary).Dilate(element);
			Mat thresh = srcGrey.Threshold (230, 255, ThresholdType.Binary);

			Cv2.FindContours (thresh, out contours, out hierarchy, ContourRetrieval.Tree, ContourChain.ApproxSimple);

			// Find center of mass in contour
			Moments center = Cv2.Moments (contours [0]);

			// Convert moment matrix to X/Y coords
			int x = (int) (center.M10 / center.M00);
			int y = (int) (center.M01 / center.M00);

			src.Circle (new Point (x, y), 5, CvColor.CornflowerBlue, 2);
			src.DrawContours (contours, 1, CvColor.Green, 2);

			bool intersect = IsIntersecting (selector[0], selector[1], contours[1][0], contours[1][1]);

			using (var orig = new Window ("src image", src)) 
			{
				orig.OnMouseCallback += new CvMouseCallback(MouseMove);

				Cv2.WaitKey();
			}
		}

		static void MouseMove(MouseEvent e, int x, int y, MouseEvent args)
		{
			Console.WriteLine ("{0}, {1}", x ,y);
		}

		static bool IsIntersecting(Point a, Point b, Point c, Point d)
		{
			float denominator = ((b.X - a.X) * (d.Y - c.Y)) - ((b.Y - a.Y) * (d.X - c.X));
			float numerator1 = ((a.Y - c.Y) * (d.X - c.X)) - ((a.X - c.X) * (d.Y - c.Y));
			float numerator2 = ((a.Y - c.Y) * (b.X - a.X)) - ((a.X - c.X) * (b.Y - a.Y));

			// Detect coincident lines (has a problem, read below)
			if (denominator == 0) return numerator1 == 0 && numerator2 == 0;

			float r = numerator1 / denominator;
			float s = numerator2 / denominator;

			return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
		}
	}
}
