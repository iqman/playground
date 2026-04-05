using System;
using System.Drawing;
using System.Linq;

namespace SudokuSolver.Ocr
{
    // helper methods for EXIF orientation handling
    internal static class ExifHelpers
    {
        public static RotateFlipType GetOrientation(Bitmap bmp)
        {
            const int ExifOrientationId = 0x0112;
            try
            {
                if (bmp.PropertyIdList == null || !bmp.PropertyIdList.Contains(ExifOrientationId))
                    return RotateFlipType.RotateNoneFlipNone;

                var prop = bmp.GetPropertyItem(ExifOrientationId);
                if (prop?.Value == null || prop.Value.Length < 2) return RotateFlipType.RotateNoneFlipNone;
                ushort orientation = BitConverter.ToUInt16(prop.Value, 0);
                switch (orientation)
                {
                    case 1: return RotateFlipType.RotateNoneFlipNone;
                    case 2: return RotateFlipType.RotateNoneFlipX;
                    case 3: return RotateFlipType.Rotate180FlipNone;
                    case 4: return RotateFlipType.Rotate180FlipX;
                    case 5: return RotateFlipType.Rotate90FlipX;
                    case 6: return RotateFlipType.Rotate90FlipNone;
                    case 7: return RotateFlipType.Rotate270FlipX;
                    case 8: return RotateFlipType.Rotate270FlipNone;
                    default: return RotateFlipType.RotateNoneFlipNone;
                }
            }
            catch
            {
                return RotateFlipType.RotateNoneFlipNone;
            }
        }
    }

}
