// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace MonoGame.Framework.Utilities
{
    /// <summary>
    /// Utility class that returns information about the underlying platform
    /// </summary>
    public static partial class PlatformInfo
    {
        /// <summary>
        /// Underlying game platform type
        /// </summary>
        public static MonoGamePlatform MonoGamePlatform =>
#if ANDROID
                MonoGamePlatform.Android;
#elif DESKTOPGL
                MonoGamePlatform.DesktopGL;
#elif IOS && !TVOS
                MonoGamePlatform.iOS;
#elif TVOS
                MonoGamePlatform.tvOS;
#elif WEB
                MonoGamePlatform.WebGL;
#elif WINDOWS && DIRECTX
                MonoGamePlatform.Windows;
#elif WINDOWS_UAP
                MonoGamePlatform.WindowsUniversal;
#elif SWITCH
                MonoGamePlatform.NintendoSwitch;
#elif XB1
                MonoGamePlatform.XboxOne;
#elif PLAYSTATION4
                MonoGamePlatform.PlayStation4;
#elif PLAYSTATION5
                MonoGamePlatform.PlayStation5;
#elif STADIA
                MonoGamePlatform.Stadia;
#else
                PlatformGetMonoGamePlatform();
#endif


        /// <summary>
        /// Graphics backend
        /// </summary>
        public static GraphicsBackend GraphicsBackend =>
#if DIRECTX
                GraphicsBackend.DirectX;
#elif OPENGL
                GraphicsBackend.OpenGL;
#else
                PlatformGetGraphicsBackend();
#endif

    }
}
