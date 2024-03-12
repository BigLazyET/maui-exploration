using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace handler.exploration.Interfaces
{
    public class BaseGestureEventArgs : EventArgs
    {
        private Point center;

        //
        // 摘要:
        //     This flag could be set to false to run other eventhandlers of the same or surrounding
        //     elements. It is not used on every platform! Default value is true.
        public bool Handled { get; set; } = true;


        //
        // 摘要:
        //     The element which raised this event.
        public virtual IGestureAwareControl Sender { get; set; }

        //
        // 摘要:
        //     Android and iOS sometimes cancel a touch gesture. In this case we raise a *ed
        //     event with Cancelled set to true.
        public virtual bool Cancelled { get; protected set; }

        //
        // 摘要:
        //     The position of the MR.Gestures.BaseGestureEventArgs.Sender on the screen.
        public virtual Rect ViewPosition { get; protected set; }

        //
        // 摘要:
        //     Returns the position of the fingers on the screen.
        public virtual Point[] Touches { get; protected set; }

        //
        // 摘要:
        //     The number of fingers on the screen.
        public virtual int NumberOfTouches
        {
            get
            {
                Point[] touches = Touches;
                if (touches == null)
                {
                    return 0;
                }

                return touches.Length;
            }
        }

        //
        // 摘要:
        //     The sources of the coordinates in MR.Gestures.BaseGestureEventArgs.Touches. The
        //     same index in Touches and Sources refer to the same Point.
        public TouchSource[] Sources { get; protected set; }

        //
        // 摘要:
        //     The center of the fingers on the screen.
        public virtual Point Center
        {
            get
            {
                if (center.IsEmpty)
                {
                    center = Touches.Center();
                }

                return center;
            }
            protected set
            {
                center = value;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BaseGestureEventArgs other))
            {
                return false;
            }

            return Equals(other);
        }

        public bool Equals(BaseGestureEventArgs other)
        {
            if (other == null)
            {
                return false;
            }

            if (Touches == null && other.Touches == null)
            {
                return true;
            }

            if (Touches.Length != other.Touches.Length)
            {
                return false;
            }

            for (int i = 0; i < Touches.Length; i++)
            {
                if (!Touches[i].Equals(other.Touches[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return Touches.GetHashCode();
        }
    }
}
