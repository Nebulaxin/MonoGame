// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using UIKit;

namespace Microsoft.Xna.Framework
{
    public static class OrientationConverter
    {
        public static DisplayOrientation UIDeviceOrientationToDisplayOrientation(UIDeviceOrientation orientation)
        {
            return orientation switch
            {
                UIDeviceOrientation.FaceDown => DisplayOrientation.Unknown,
                UIDeviceOrientation.FaceUp => DisplayOrientation.Unknown,
                UIDeviceOrientation.LandscapeRight => DisplayOrientation.LandscapeLeft,
                UIDeviceOrientation.Portrait => DisplayOrientation.Portrait,
                UIDeviceOrientation.PortraitUpsideDown => DisplayOrientation.PortraitDown,
                _ => DisplayOrientation.LandscapeRight,
            };
        }

        public static DisplayOrientation ToDisplayOrientation(UIInterfaceOrientation orientation)
        {
            return orientation switch
            {
                UIInterfaceOrientation.LandscapeRight => DisplayOrientation.LandscapeLeft,
                UIInterfaceOrientation.Portrait => DisplayOrientation.Portrait,
                UIInterfaceOrientation.PortraitUpsideDown => DisplayOrientation.PortraitDown,
                _ => DisplayOrientation.LandscapeRight,
            };
        }

        public static UIInterfaceOrientationMask ToUIInterfaceOrientationMask (DisplayOrientation orientation)
        {
            return Normalize(orientation) switch
            {
                ((DisplayOrientation)0) or ((DisplayOrientation)3) => UIInterfaceOrientationMask.Landscape,
                // NOTE: in XNA, Orientation Left is a 90 degree rotation counterclockwise, while on iOS
                // it is a 90 degree rotation CLOCKWISE. They are BACKWARDS! 
                ((DisplayOrientation)2) => UIInterfaceOrientationMask.LandscapeLeft,
                ((DisplayOrientation)1) => UIInterfaceOrientationMask.LandscapeRight,
                ((DisplayOrientation)4) => UIInterfaceOrientationMask.Portrait,
                ((DisplayOrientation)8) => UIInterfaceOrientationMask.PortraitUpsideDown,
                ((DisplayOrientation)7) => UIInterfaceOrientationMask.AllButUpsideDown,
                _ => UIInterfaceOrientationMask.All,
            };
        }

        public static DisplayOrientation Normalize(DisplayOrientation orientation)
        {
            var normalized = orientation;
			
			// Xna's "default" displayorientation is Landscape Left/Right.
            if (normalized == DisplayOrientation.Default)
            {
                normalized |= DisplayOrientation.LandscapeLeft;
				normalized |= DisplayOrientation.LandscapeRight;
                normalized &= ~DisplayOrientation.Default;
            }
            return normalized;
        }
    }
}
