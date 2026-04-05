using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace SudokuSolver.Ocr
{
    public class ImageLoader
    {
        private static string Img1Path = @"D:\dev\playground\SudokuSolver\SudokuSolver\bin\Debug\1.jpg";
        private static string Img2Path = @"D:\dev\playground\SudokuSolver\SudokuSolver\bin\Debug\2.jpg";
        private static string Img3Path = @"D:\dev\playground\SudokuSolver\SudokuSolver\bin\Debug\3.jpg";
        private static string Img4Path = @"D:\dev\playground\SudokuSolver\SudokuSolver\bin\Debug\4.jpg";
        private static string Img5Path = @"D:\dev\playground\SudokuSolver\SudokuSolver\bin\Debug\5.jpg";
        private static string Img6Path = @"D:\dev\playground\SudokuSolver\SudokuSolver\bin\Debug\6.tif";
        private static string Img7Path = @"D:\dev\playground\SudokuSolver\SudokuSolver\bin\Debug\7.jpg";
        private static string Img8Path = @"D:\dev\playground\SudokuSolver\SudokuSolver\bin\Debug\8.png";
        private static string Img9Path = @"D:\dev\playground\SudokuSolver\SudokuSolver\bin\Debug\9.png";

        private string ImgToUse = Img2Path;

        private System.Drawing.Size viewportSize;

        public Image LoadedImage { get; set; }

        public event Action<string> AnnotationUpdate;
        public void OnAnnotationDetailUpdate(string text)
        {
            Action<string> handler = AnnotationUpdate;
            handler?.Invoke(text);
        }

        public event Action ImageAnnotated;
        public void OnImageAnnotated()
        {
            Action handler = ImageAnnotated;
            handler?.Invoke();
        }

        public event Action ImageLoaded;
        public void OnImageLoaded()
        {
            Action handler = ImageLoaded;
            handler?.Invoke();
        }

        public event Action<int[]> ImageContentParsed;
        public void OnImageContentParsed(int[] content)
        {
            Action<int[]> handler = ImageContentParsed;
            handler?.Invoke(content);
        }

        public ImageLoader(System.Drawing.Size viewportSize)
        {
            this.viewportSize = viewportSize;

            ReplaceImage(new Bitmap(this.viewportSize.Width, this.viewportSize.Height));
        }

        private void ReplaceImage(Image newImage)
        {
            if (LoadedImage != null)
            {
                LoadedImage.Dispose();
                LoadedImage = null;
            }
    
            LoadedImage = newImage;

            OnImageLoaded();
        }

        public void LoadImage()
        {
            ReplaceImage(Image.FromFile(ImgToUse));
        }

        public void ProcessImage()
        {
            RotateIfNeeded();

            MakeBlackAndWhiteInverted();
            Denoise();

            CloseGaps();

            HighlightEdges();
            HighlightEdges();
            HighlightEdges();

            CloseGaps();
        }

        private OpenCvSharp.Point[] outerGrid = null;
        public void ProcessImage2()
        {
            //Denoise();
            //CloseGaps();
            //HighlightEdges();

            outerGrid = FindOuterGrid();
        }

        public void ProcessImage3()
        {
            RotateIfNeeded();
            FixPerspective(outerGrid);
        }

        public void ProcessImage4()
        {
            // MakeBlackAndWhite();
            ExtractCells();
        }

        private void ExtractCells()
        {
            PerformCv2Action((input, output) =>
            {

                IList<int> boardValues = new List<int>();

                var nr = new NumberRecognizer();

                // Assuming the image is already perspective-corrected and preprocessed
                int gridSize = input.Width; // Assuming square grid
                int cellSize = gridSize / 9;
                for (int row = 0; row < 9; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        var cellRect = new OpenCvSharp.Rect(col * cellSize, row * cellSize, cellSize, cellSize);

                        // scrink the borders a bit to avoid grid lines
                        cellRect.Inflate(-cellSize/10, -cellSize/10);
                        var cellImage = new Mat(input, cellRect);

                        using (var cellBitmap = cellImage.ToBitmap())
                        {
                            ReplaceImage(cellBitmap);
                            OnImageAnnotated();

                       //     System.Threading.Thread.Sleep(500); // Pause to visualize each cell

                            int threshold;
                            string details;
                            float confidence;
                            int result;

                            //result = nr.RecognizeSingleDigit(cellBitmap, out confidence, out details);

                            //if (confidence > 0.70f)
                            //{
                            //    boardValues.Add(result);
                            //    OnAnnotationDetailUpdate($"Cell ({row},{col}): {result}, high confidence: {confidence}");
                            //}
                            //else
                            //{
                                using (var bwCellBitmap = MakeBlackAndWhiteCopy(cellBitmap, out threshold))
                                {
                                    ReplaceImage(bwCellBitmap);
                                    OnImageAnnotated();

                             //       System.Threading.Thread.Sleep(500); // Pause to visualize each cell

                                    result = nr.RecognizeSingleDigit(bwCellBitmap, out confidence, out details);

                                    if (confidence > 0.70f)
                                    {
                                        boardValues.Add(result);
                                        OnAnnotationDetailUpdate($"Cell ({row},{col}): {result}, BW confidence: {confidence}");
                                    }
                                    else
                                    {
                                        boardValues.Add(0);
                                        OnAnnotationDetailUpdate($"Cell ({row},{col}): Uncertain result, confidence: {confidence}, details: {details} (threshold used: {threshold})");
                                    }
                                }
                         //   }
                        }
                    }
                }

                OnImageContentParsed(boardValues.ToArray());

                input.CopyTo(output);
            });
        }

        private void FixPerspective(OpenCvSharp.Point[] detectedGrid)
        {
            if (detectedGrid == null)
                return;

            PerformCv2Action((input, output) =>
            {
                // 1. Define the source points (detected grid corners)
                Point2f[] srcPoints = new Point2f[]
                {
                    new Point2f(detectedGrid[0].X, detectedGrid[0].Y), // Top-left
                    new Point2f(detectedGrid[3].X, detectedGrid[3].Y), // Top-right
                    new Point2f(detectedGrid[2].X, detectedGrid[2].Y), // Bottom-right
                    new Point2f(detectedGrid[1].X, detectedGrid[1].Y)  // Bottom-left
                };

                // 2. Define the destination points (perfect rectangle)
                int desiredGridSize = FindSmallestGridDimension(detectedGrid);
                Point2f[] destPoints = new Point2f[]
                {
                    new Point2f(0, 0),
                    new Point2f(desiredGridSize, 0),
                    new Point2f(desiredGridSize, desiredGridSize),
                    new Point2f(0, desiredGridSize)
                };

                // 3. Calculate the perspective transform matrix
                Mat perspectiveMatrix = Cv2.GetPerspectiveTransform(srcPoints, destPoints);

                // 4. Apply the perspective transform
                Mat warped = new Mat();
                Cv2.WarpPerspective(
                    input, // Preprocessed image (from earlier steps)
                    output,
                    perspectiveMatrix,
                    new OpenCvSharp.Size(desiredGridSize, desiredGridSize)
                );
            });
        }

        private int FindSmallestGridDimension(OpenCvSharp.Point[] detectedGrid)
        {
            var topWidth = detectedGrid[3].X - detectedGrid[0].X;
            var bottomWidth = detectedGrid[2].X - detectedGrid[1].X;
            var leftHeight = detectedGrid[1].Y - detectedGrid[0].Y;
            var rightHeight = detectedGrid[2].Y - detectedGrid[3].Y;

            return Math.Min(Math.Min(topWidth, bottomWidth), Math.Min(leftHeight, rightHeight));
        }

        private OpenCvSharp.Point[] FindOuterGrid()
        {
            return PerformCv2Action((input, output) =>
            {
                // Find all contours in the preprocessed image
                OpenCvSharp.Point[][] contours;
                HierarchyIndex[] hierarchy;
                Cv2.FindContours(input, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

                var expectedArea = (input.Width * input.Height) / 100;
                var midWidth = input.Width / 2;
                var midHeight = input.Height / 2;

                // Filter contours to find the largest quadrilateral (the Sudoku grid)
                var filteredGridContours = contours
                   .OrderByDescending(c => Cv2.ContourArea(c))
                   .Where(c => Cv2.ContourArea(c) > expectedArea);
                   //   .FirstOrDefault(c => Cv2.ContourArea(c) > 10000); // Filter small contours

                input = input.CvtColor(ColorConversionCodes.GRAY2BGR);

                OpenCvSharp.Point[] result = null;

                foreach (var gridContour in filteredGridContours)
                {

                    if (gridContour != null)
                    {
                        // Approximate the contour to a polygon
                        double peri = Cv2.ArcLength(gridContour, true);
                        var candidate = Cv2.ApproxPolyDP(gridContour, 0.02 * peri, true);

                        // Ensure the contour is a quadrilateral (4 corners)
                        if (candidate.Length == 4 &&
                            candidate[0].X < midWidth && candidate[0].Y < midHeight &&
                            candidate[1].X < midWidth && candidate[1].Y > midHeight &&
                            candidate[2].X > midWidth && candidate[2].Y > midHeight &&
                            candidate[3].X > midWidth && candidate[3].Y < midHeight)
                        {
                            Cv2.DrawContours(input, new OpenCvSharp.Point[][] { candidate }, 0, Scalar.Red, 20);

                            OnAnnotationDetailUpdate("Found a plausible polygon");

                            result = candidate;
                            break;
                        }
                    }
                }

                input.CopyTo(output);

                return result;
            });
        }

        private void CloseGaps()
        {
            PerformCv2Action((input, output) =>
            {
                // Create a kernel for morphological operations
                Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(13, 13));
                // Close small gaps
                Cv2.MorphologyEx(input, output, MorphTypes.Close, kernel);

                OnAnnotationDetailUpdate("Closed gaps (morphology)");
            });
        }

        private void Denoise()
        {
            PerformCv2Action((input, output) =>
            {
                Cv2.GaussianBlur(input, output, new OpenCvSharp.Size(15, 15), 0);

                OnAnnotationDetailUpdate("Denoised image (Gaussian blur)");
            });
        }

        private void HighlightEdges()
        {
            PerformCv2Action((input, output) =>
            {
                Cv2.Canny(input, output, 50, 200);

                OnAnnotationDetailUpdate("Edges highlighted (Canny)");
            });
        }

        private void MakeBlackAndWhiteInverted()
        {
            PerformCv2Action((input, output) =>
            {
                Cv2.AdaptiveThreshold(
                    input, output, 255,
                    AdaptiveThresholdTypes.GaussianC,
                    ThresholdTypes.BinaryInv, 11, 3
                );

                OnAnnotationDetailUpdate("Converted to black & white inverted (adaptive threshold)");
            });
        }

        public T PerformCv2Action<T>(Func<Mat, Mat, T> func) where T : class
        {
            var bmp = this.LoadedImage as Bitmap;
            if (bmp == null) return null;

            using (var input = bmp.ToMat())
            using (var inputColorFixed = new Mat())
            using (var output = new Mat())
            {
                // ensure single-channel 8-bit image
                if (input.Channels() == 1)
                {
                    input.CopyTo(inputColorFixed);
                }
                else if (input.Channels() == 3)
                {
                    Cv2.CvtColor(input, inputColorFixed, ColorConversionCodes.BGR2GRAY);
                }
                else if (input.Channels() == 4)
                {
                    Cv2.CvtColor(input, inputColorFixed, ColorConversionCodes.BGRA2GRAY);
                }
                else
                {
                    // fallback: convert to BGR then to gray
                    Cv2.CvtColor(input, inputColorFixed, ColorConversionCodes.BGR2GRAY);
                }

                var temp = func(inputColorFixed, output);

                ReplaceImage(output.ToBitmap());

                return temp;
            }
        }

        public void PerformCv2Action(Action<Mat, Mat> action)
        {
            var bmp = this.LoadedImage as Bitmap;
            if (bmp == null) return;

            using (var input = bmp.ToMat())
            using (var inputColorFixed = new Mat())
            using (var output = new Mat())
            {
                // ensure single-channel 8-bit image
                if (input.Channels() == 1)
                {
                    input.CopyTo(inputColorFixed);
                }
                else if (input.Channels() == 3)
                {
                    Cv2.CvtColor(input, inputColorFixed, ColorConversionCodes.BGR2GRAY);
                }
                else if (input.Channels() == 4)
                {
                    Cv2.CvtColor(input, inputColorFixed, ColorConversionCodes.BGRA2GRAY);
                }
                else
                {
                    // fallback: convert to BGR then to gray
                    Cv2.CvtColor(input, inputColorFixed, ColorConversionCodes.BGR2GRAY);
                }

                action(inputColorFixed, output);

                ReplaceImage(output.ToBitmap());
            }
        }

        private void ScaleToPixelHeight(int v)
        {
            if (v <= 0) return;
            if (LoadedImage == null) return;

            var src = LoadedImage as Bitmap;
            if (src == null) return;

            int srcW = src.Width;
            int srcH = src.Height;
            if (srcH == 0) return;

            float scale = (float)v / (float)srcH;
            if (Math.Abs(scale - 1.0f) < 0.001f) return; // already at target height

            int newW = Math.Max(1, (int)Math.Round(srcW * scale));
            int newH = Math.Max(1, v);

            var dest = new Bitmap(newW, newH, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(dest))
            {
                g.CompositingMode = CompositingMode.SourceCopy;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                var destRect = new Rectangle(0, 0, newW, newH);
                g.DrawImage(src, destRect, 0, 0, srcW, srcH, GraphicsUnit.Pixel);
            }

            ReplaceImage(dest);
            OnAnnotationDetailUpdate($"Scaled image to {newW}x{newH} (scale={scale:0.00})");
        }

        private void MakeBlackAndWhite()
        {
            if (LoadedImage == null) return;

            var src = LoadedImage as Bitmap;
            int threshold;

            ReplaceImage(MakeBlackAndWhiteCopy(src, out threshold));
            OnAnnotationDetailUpdate($"Converted to black & white with threshold {threshold}");
        }

        private Bitmap MakeBlackAndWhiteCopy(Bitmap src, out int threshold)
        {
            threshold = 0;

            if (src == null) return null;

            int w = src.Width;
            int h = src.Height;

            // First convert to grayscale using ColorMatrix
            var dest = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            var grayMatrix = new ColorMatrix(new float[][]
            {
                new float[] {0.299f, 0.299f, 0.299f, 0, 0},
                new float[] {0.587f, 0.587f, 0.587f, 0, 0},
                new float[] {0.114f, 0.114f, 0.114f, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {0, 0, 0, 0, 1}
            });

            using (var attrs = new ImageAttributes())
            {
                attrs.SetColorMatrix(grayMatrix);
                using (var g = Graphics.FromImage(dest))
                {
                    g.DrawImage(src, new Rectangle(0, 0, w, h), 0, 0, w, h, GraphicsUnit.Pixel, attrs);
                }
            }

            // Compute histogram and Otsu threshold on the grayscale image
            int[] hist = new int[256];
            BitmapData bd = dest.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            
            try
            {
                int stride = bd.Stride;
                int bytes = stride * h;
                var data = new byte[bytes];
                Marshal.Copy(bd.Scan0, data, 0, bytes);

                for (int y = 0; y < h; y++)
                {
                    int row = y * stride;
                    for (int x = 0; x < w; x++)
                    {
                        byte gray = data[row + x * 3];
                        hist[gray]++;
                    }
                }

                // Otsu's method
                int total = w * h;
                double sum = 0;
                for (int t = 0; t < 256; t++) sum += t * hist[t];

                double sumB = 0;
                int wB = 0;
                int wF = 0;
                double varMax = 0;

                for (int t = 0; t < 256; t++)
                {
                    wB += hist[t];
                    if (wB == 0) continue;
                    wF = total - wB;
                    if (wF == 0) break;

                    sumB += t * hist[t];
                    double mB = sumB / wB;
                    double mF = (sum - sumB) / wF;
                    double varBetween = (double)wB * (double)wF * (mB - mF) * (mB - mF);
                    if (varBetween > varMax)
                    {
                        varMax = varBetween;
                        threshold = t;
                    }
                }

                // Apply threshold
                for (int y = 0; y < h; y++)
                {
                    int row = y * stride;
                    for (int x = 0; x < w; x++)
                    {
                        int idx = row + x * 3;
                        byte v = data[idx];
                        byte bw = (byte)(v <= threshold ? 0 : 255);
                        data[idx] = bw;
                        data[idx + 1] = bw;
                        data[idx + 2] = bw;
                    }
                }

                Marshal.Copy(data, 0, bd.Scan0, bytes);
            }
            finally
            {
                dest.UnlockBits(bd);
            }

            return dest;
        }

        private void IncreaseBrightness(float brightness)
        {
            if (LoadedImage == null) return;

            var bmp = LoadedImage as Bitmap;
            if (bmp == null) return;

            // brightness: 1.0 = no change; translate added to color channels = brightness - 1
            float translate = brightness - 1f;

            var cm = new ColorMatrix(new float[][]
            {
                new float[] { 1, 0, 0, 0, 0 },
                new float[] { 0, 1, 0, 0, 0 },
                new float[] { 0, 0, 1, 0, 0 },
                new float[] { 0, 0, 0, 1, 0 },
                new float[] { translate, translate, translate, 0, 1 }
            });

            using (var attrs = new ImageAttributes())
            {
                attrs.SetColorMatrix(cm);
                var dest = new Bitmap(bmp.Width, bmp.Height);
                using (var g = Graphics.FromImage(dest))
                {
                    g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attrs);
                }

                ReplaceImage(dest);
            }

            OnAnnotationDetailUpdate($"Brightness adjusted (factor={brightness:0.00})");
        }

        private void IncreaseContrast(float contrast)
        {
            if (LoadedImage == null) return;

            var bmp = LoadedImage as Bitmap;
            if (bmp == null) return;

            // Simple contrast enhancement via ColorMatrix
            // contrast >1 increases contrast, <1 decreases
            float translate = 0.5f * (1.0f - contrast);

            var cm = new ColorMatrix(new float[][]
            {
                new float[] { contrast, 0, 0, 0, 0 },
                new float[] { 0, contrast, 0, 0, 0 },
                new float[] { 0, 0, contrast, 0, 0 },
                new float[] { 0, 0, 0, 1, 0 },
                new float[] { translate, translate, translate, 0, 1 }
            });

            using (var attrs = new ImageAttributes())
            {
                attrs.SetColorMatrix(cm);
                var dest = new Bitmap(bmp.Width, bmp.Height);
                using (var g = Graphics.FromImage(dest))
                {
                    g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attrs);
                }

                ReplaceImage(dest);
            }

            OnAnnotationDetailUpdate($"Contrast increased (factor={contrast:0.00})");
        }

        private void PerformOcr()
        {
            var nr = new NumberRecognizer();
            var result = nr.Recognize(this.LoadedImage as Bitmap);
            OnAnnotationDetailUpdate(result);
        }

        private void RotateIfNeeded()
        {
            var bmp = LoadedImage as Bitmap;
            var orient = ExifHelpers.GetOrientation(bmp);
            if (orient != RotateFlipType.RotateNoneFlipNone)
            {
                bmp.RotateFlip(orient);
            }

            OnAnnotationDetailUpdate("Image processed with orientation: " + orient.ToString());

            OnImageAnnotated();
        }
    }
}
