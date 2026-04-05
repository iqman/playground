using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace SudokuSolver.Ocr
{
    public class NumberRecognizer
    {
        // psm 4. Assume a Single Column of Text of Variable Sizes  (like receipt)
        // PSM 7. Treat the Image as a Single Text Line (like a license plate)
        // PSM 8. Treat the Image as a Single Word (like a street name)
        // PSM 10. Treat the Image as a Single Character (like a digit)
        // PSM 11. Sparse Text: Find as Much Text as Possible in No Particular Order (like a crossword puzzle)

        public string Recognize(Bitmap bitmap)
        {
            string details;
            float confidence;
            RecognizeSingleDigit(bitmap, out confidence, out details);
            return details;
        }

        public int RecognizeSingleDigit(Bitmap bitmap, out float confidence, out string details)
        {
            int foundDigit = 0;
            confidence = 0;

            details = string.Empty;
            StringBuilder sb = new StringBuilder();
            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    var converter = new BitmapToPixConverter();

                    using (var page = engine.Process(bitmap, PageSegMode.SingleWord))
                    {
                        var text = page.GetText();
                        confidence = page.GetMeanConfidence();

                        int tempDigit = 0;
                        if (int.TryParse(text, out tempDigit))
                        {
                            if (tempDigit >= 1 && tempDigit <= 9)
                            {
                                foundDigit = tempDigit;
                            }
                        }

                        sb.AppendLine(string.Format("Mean confidence: {0}", confidence));
                        sb.AppendLine(string.Format("Text (GetText): \r\n{0}", text));
                    }
                }
            }
            catch (Exception e)
            {
                sb.AppendLine("Unexpected Error: " + e.Message);
                sb.AppendLine("Details: ");
                sb.AppendLine(e.ToString());
            }

            details = sb.ToString();

            return foundDigit;
        }
    }

}
