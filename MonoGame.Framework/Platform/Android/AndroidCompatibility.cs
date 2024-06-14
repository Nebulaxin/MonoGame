using System;
using System.Linq;
using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Views;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// Properties that change from how XNA works by default
    /// </summary>
    public static class AndroidCompatibility
    {
		/// <summary>
		/// Because the Kindle Fire devices default orientation is fliped by 180 degrees from all the other android devices
		/// on the market we need to do some special processing to make sure that LandscapeLeft is the correct way round.
		/// This list contains all the Build.Model strings of the effected devices, it should be added to if and when
		/// more devices exhibit the same issues.
		/// </summary>
        private static readonly string[] Kindles = new[] { "KFTT", "KFJWI", "KFJWA", "KFSOWI", "KFTHWA", "KFTHWI", "KFAPWA", "KFAPWI" };

        public static bool FlipLandscape { get; private set; }
        public static Lazy<Orientation> NaturalOrientation { get; private set; }

        static AndroidCompatibility()
        {
			FlipLandscape = Kindles.Contains(Build.Model);
            NaturalOrientation = new Lazy<Orientation>(GetDeviceNaturalOrientation);
        }

        private static Orientation GetDeviceNaturalOrientation()
        {
            var orientation = Game.Activity.Resources.Configuration.Orientation;
            SurfaceOrientation rotation = Game.Activity.WindowManager.DefaultDisplay.Rotation;

            if (((rotation == SurfaceOrientation.Rotation0 || rotation == SurfaceOrientation.Rotation180) &&
                orientation == Orientation.Landscape)
                || ((rotation == SurfaceOrientation.Rotation90 || rotation == SurfaceOrientation.Rotation270) &&
                orientation == Orientation.Portrait))
            {
                return Orientation.Landscape;
            }
            else
            {
                return Orientation.Portrait;
            }
        }

        internal static DisplayOrientation GetAbsoluteOrientation(int orientation)
        {
            // Orientation is reported by the device in degrees compared to the natural orientation
            // Some tablets have a natural landscape orientation, which we need to account for
            if (NaturalOrientation.Value == Orientation.Landscape)
                orientation += 270;

            // Round orientation into one of 4 positions, either 0, 90, 180, 270. 
            int ort = ((orientation + 45) / 90 * 90) % 360;

            // Surprisingly 90 degree is landscape right, except on Kindle devices
            var disporientation = DisplayOrientation.Unknown;
            disporientation = ort switch
            {
                90 => FlipLandscape ? DisplayOrientation.LandscapeLeft : DisplayOrientation.LandscapeRight,
                270 => FlipLandscape ? DisplayOrientation.LandscapeRight : DisplayOrientation.LandscapeLeft,
                0 => DisplayOrientation.Portrait,
                180 => DisplayOrientation.PortraitDown,
                _ => DisplayOrientation.LandscapeLeft,
            };
            return disporientation;
        }

        /// <summary>
        /// Get the absolute orientation of the device, accounting for platform differences.
        /// </summary>
        /// <returns></returns>
        public static DisplayOrientation GetAbsoluteOrientation()
        {
            var orientation = Game.Activity.WindowManager.DefaultDisplay.Rotation;

            // Landscape degrees (provided by the OrientationListener) are swapped by default
            // Since we use the code used by OrientationListener, we have to swap manually
            System.Int32 degrees = orientation switch
            {
                SurfaceOrientation.Rotation90 => 270,
                SurfaceOrientation.Rotation180 => 180,
                SurfaceOrientation.Rotation270 => 90,
                _ => 0,
            };
            return GetAbsoluteOrientation(degrees);
        }
    }
}
