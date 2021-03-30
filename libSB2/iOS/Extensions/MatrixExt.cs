using System;
using CoreGraphics;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public static class MatrixExt
    {
        public static CGAffineTransform ToCGAffine (this AffineMatrix2D affine)
        {
            return new CGAffineTransform (
                (nfloat)affine.ScaleX, (nfloat)affine.SkewY,
                (nfloat)affine.SkewX, (nfloat)affine.ScaleY,
                (nfloat)affine.OriginX, (nfloat)affine.OriginY
            );
        }
    }
}

