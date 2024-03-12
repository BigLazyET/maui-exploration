using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace handler.exploration.Interfaces
{
    public class MouseEventArgs : BaseGestureEventArgs
    {
        //
        // 摘要:
        //     The distance in X/Y that the mouse was moved since the last mouse event was raised.
        public virtual Point DeltaDistance { get; protected set; }

        //
        // 摘要:
        //     The distance in X/Y that the mouse was moved since the mouse gesture began.
        public virtual Point TotalDistance { get; protected set; }

        //
        // 摘要:
        //     The velocity of the mouse in X/Y.
        public virtual Point Velocity { get; protected set; }

        protected void CalculateDistances(BaseGestureEventArgs previous)
        {
            MouseEventArgs mouseEventArgs = previous as MouseEventArgs;
            if (previous == null)
            {
                DeltaDistance = Point.Zero;
                TotalDistance = Point.Zero;
            }
            else if (Touches.Length != previous.Touches.Length)
            {
                DeltaDistance = Point.Zero;
                TotalDistance = mouseEventArgs?.TotalDistance ?? Point.Zero;
            }
            else
            {
                DeltaDistance = Center.Subtract(previous.Center);
                TotalDistance = mouseEventArgs?.TotalDistance.Add(DeltaDistance) ?? DeltaDistance;
            }
        }

        internal MouseEventArgs Diff(MouseEventArgs lastArgs)
        {
            return new MouseEventArgs
            {
                Cancelled = Cancelled,
                Handled = base.Handled,
                ViewPosition = ViewPosition,
                Touches = Touches,
                Sources = base.Sources,
                Sender = Sender,
                Velocity = Velocity,
                TotalDistance = TotalDistance,
                DeltaDistance = TotalDistance.Subtract(lastArgs.TotalDistance)
            };
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is MouseEventArgs other))
            {
                return false;
            }

            return Equals(other);
        }

        public bool Equals(MouseEventArgs other)
        {
            if (other == null)
            {
                return false;
            }

            if (!DeltaDistance.Equals(other.DeltaDistance))
            {
                return false;
            }

            if (!TotalDistance.Equals(other.TotalDistance))
            {
                return false;
            }

            if (!Velocity.Equals(other.Velocity))
            {
                return false;
            }

            return Equals((BaseGestureEventArgs)other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ DeltaDistance.GetHashCode() ^ TotalDistance.GetHashCode() ^ Velocity.GetHashCode();
        }
    }
}
