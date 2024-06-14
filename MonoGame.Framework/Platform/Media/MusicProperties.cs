#if WINDOWS_UAP

using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace Microsoft.Xna.Framework.Media
{
    class MusicProperties
    {
        public StorageFile File { get; private set; }
        public string Path { get; private set; }
        public DateTimeOffset DateCreated { get; private set; }

        public string Album { get; private set; }
        public string AlbumArtist { get; private set; }
        public string Artist { get; private set; }
        public TimeSpan Duration { get; private set; }
        public string Genre { get; private set; }
        public bool IsProtected { get; private set; }
        public int Rating { get; private set; }
        public string Title { get; private set; }
        public int TrackNumber { get; private set; }

        private MusicProperties() { }

        public MusicProperties(StorageFile file)
        {
            File = file;
            Path = File.Path;
            DateCreated = File.DateCreated;

            var properties = Task.Run(async () => await file.Properties.RetrievePropertiesAsync(new String[]
                {
                    "System.Music.AlbumTitle",
                    "System.Music.AlbumArtist",
                    "System.Music.Artist",
                    "System.Media.Duration",
                    "System.Music.Genre",
                    "System.DRM.IsProtected",
                    "System.Rating",
                    "System.Title",
                    "System.Music.TrackNumber"
                })).Result;

            object property;
            Album = (properties.TryGetValue("System.Music.AlbumTitle", out property) ? (string)property : (string)null) ?? "";
            Artist = (properties.TryGetValue("System.Music.Artist", out property) ? ((string[])property)[0] : (string)null) ?? "Unknown Artist";
            AlbumArtist = (properties.TryGetValue("System.Music.AlbumArtist", out property) ? (string)property : (string)null) ?? Artist;
            Duration = properties.TryGetValue("System.Media.Duration", out property) ? new TimeSpan((long)(ulong)property) : TimeSpan.Zero;
            Genre = (properties.TryGetValue("System.Music.Genre", out property) ? ((string[])property)[0] : (string)null) ?? "";
            IsProtected = properties.TryGetValue("System.DRM.IsProtected", out property) ? (bool)property : false;
            Rating = properties.TryGetValue("System.Rating", out property) ? (int)(uint)property : 0;
            Title = (properties.TryGetValue("System.Title", out property) ? (string)property : (string)null) ?? "";
            TrackNumber = properties.TryGetValue("System.Music.TrackNumber", out property) ? (int)(uint)property : 0;
        }

        public bool TryMatch(StorageFile file)
        {
            if (file != null && file.Path == Path && file.DateCreated == DateCreated)
            {
                File = file;
                return true;
            }

            return false;
        }

        public static MusicProperties Deserialize(BinaryReader stream)
        {
            var instance = new MusicProperties();
            instance.Path = stream.ReadString();
            instance.DateCreated = new DateTimeOffset(stream.ReadInt64(), new TimeSpan(stream.ReadInt64()));

            instance.Album = stream.ReadString();
            instance.AlbumArtist = stream.ReadString();
            instance.Artist = stream.ReadString();
            instance.Duration = new TimeSpan(stream.ReadInt64());
            instance.Genre = stream.ReadString();
            instance.IsProtected = stream.ReadBoolean();
            instance.Rating = stream.ReadInt32();
            instance.Title = stream.ReadString();
            instance.TrackNumber = stream.ReadInt32();

            return instance;
        }

        public void Serialize(BinaryWriter stream)
        {
            stream.Write(Path);
            stream.Write(DateCreated.Ticks);
            stream.Write(DateCreated.Offset.Ticks);

            stream.Write(Album);
            stream.Write(AlbumArtist);
            stream.Write(Artist);
            stream.Write(Duration.Ticks);
            stream.Write(Genre);
            stream.Write(IsProtected);
            stream.Write(Rating);
            stream.Write(Title);
            stream.Write(TrackNumber);
        }
    }
}

#endif
