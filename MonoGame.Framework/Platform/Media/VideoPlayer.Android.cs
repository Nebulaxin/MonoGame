// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using Android.Widget;
using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.Media
{
    public sealed partial class VideoPlayer : IDisposable
    {
        private Game _game;

        private void PlatformInitialize()
        {
            _game = Game.Instance;
        }

        private Texture2D PlatformGetTexture()
        {
            throw new NotImplementedException();
        }

        private void PlatformGetState(ref MediaState result)
        {
        }

        private void PlatformPause()
        {
            Video.Player.Pause();
        }

        private void PlatformResume()
        {
            Video.Player.Start();
        }

        private void PlatformPlay()
        {
            Video.Player.SetDisplay(((AndroidGameWindow)_game.Window).GameView.Holder);
            Video.Player.Start();
            
            AndroidGamePlatform.IsPlayingVdeo = true;
        }

        private void PlatformStop()
        {
            Video.Player.Stop();

            AndroidGamePlatform.IsPlayingVdeo = false;
            Video.Player.SetDisplay(null);
        }

        private void PlatformSetIsLooped()
        {
            throw new NotImplementedException();
        }

        private void PlatformSetIsMuted()
        {
            throw new NotImplementedException();
        }

        private TimeSpan PlatformGetPlayPosition()
        {
            throw new NotImplementedException();
        }

        private TimeSpan PlatformSetVolume()
        {
            throw new NotImplementedException();
        }

        private void PlatformDispose(bool disposing)
        {
        }
    }
}