using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace handler.exploration.Interfaces
{
    internal static class PointExtensions
    {
        //
        // 摘要:
        //     Adds the coordinates of one Point to another.
        //
        // 参数:
        //   first:
        //
        //   second:
        public static Point Add(this Point first, Point second)
        {
            return new Point(first.X + second.X, first.Y + second.Y);
        }

        //
        // 摘要:
        //     Subtracts the coordinates of one Point from another.
        //
        // 参数:
        //   first:
        //
        //   second:
        public static Point Subtract(this Point first, Point second)
        {
            return new Point(first.X - second.X, first.Y - second.Y);
        }

        //
        // 摘要:
        //     Gets the center of some touch points.
        //
        // 参数:
        //   touches:
        public static Point Center(this Point[] touches)
        {
            int num = ((touches != null) ? touches.Length : 0);
            switch (num)
            {
                case 0:
                    return Point.Zero;
                case 1:
                    return touches[0];
                default:
                    {
                        double num2 = 0.0;
                        double num3 = 0.0;
                        for (int i = 0; i < num; i++)
                        {
                            num2 += touches[i].X;
                            num3 += touches[i].Y;
                        }

                        return new Point(num2 / (double)num, num3 / (double)num);
                    }
            }
        }
    }
}
