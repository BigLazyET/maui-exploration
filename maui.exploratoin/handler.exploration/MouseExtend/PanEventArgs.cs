using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace handler.exploration.Interfaces
{
    public class PanEventArgs : BaseGestureEventArgs
    {
        //
        // 摘要:
        //     The distance in X/Y that the finger was moved since this event was raised the
        //     last time.
        public virtual Point DeltaDistance { get; protected set; }

        //
        // 摘要:
        //     The distance in X/Y that the finger was moved since the pan gesture began.
        public virtual Point TotalDistance { get; protected set; }

        //
        // 摘要:
        //     The velocity of the finger in X/Y.
        public virtual Point Velocity { get; protected set; }

        protected void CalculateDistances(BaseGestureEventArgs previous)
        {
            PanEventArgs panEventArgs = previous as PanEventArgs;
            if (previous == null)
            {
                DeltaDistance = Point.Zero;
                TotalDistance = Point.Zero;
            }
            else if (Touches.Length != previous.Touches.Length)
            {
                DeltaDistance = Point.Zero;
                TotalDistance = panEventArgs?.TotalDistance ?? Point.Zero;
            }
            else
            {
                DeltaDistance = Center.Subtract(previous.Center);
                TotalDistance = panEventArgs?.TotalDistance.Add(DeltaDistance) ?? DeltaDistance;
            }
        }

        internal PanEventArgs Diff(PanEventArgs lastArgs)
        {
            return new PanEventArgs
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

            if (!(obj is PanEventArgs other))
            {
                return false;
            }

            return Equals(other);
        }

        public bool Equals(PanEventArgs other)
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
