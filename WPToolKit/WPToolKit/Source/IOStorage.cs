using System;
using System.IO;
using System.IO.IsolatedStorage;

namespace WPToolKit.Source
{
    public class IoStorage {

        private IoUrl url;
        public IoUrl Url {
            get {
                return url;
            }
            set {
                url = value;
            }
        }

        /// <summary>
        /// Gets the Application IO Storage Area.
        /// </summary>
        /// <remarks></remarks>
        static public IsolatedStorageFile GetUserFileArea {
        get {
            return IsolatedStorageFile.GetUserStoreForApplication();
            }
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="IoStorage"/> to <see cref="System.Uri"/>.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks></remarks>
        public static implicit operator Uri(IoStorage s) {
            s.HasValidIoUrl();
            return s.url;
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="IoStorage"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks></remarks>
        public static implicit operator String(IoStorage s) {
            s.HasValidIoUrl();
            return s.url;
        }
        /// <summary>
        /// Gets the path for the underlying filename.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public String GetPath() {
            HasValidIoUrl();
            return url.GetPath();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="IoStorage"/> class.
        /// </summary>
        public IoStorage() {
            url = null;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="IoStorage"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        public IoStorage(Uri url) {
            this.url = new IoUrl(url);
        }
        
        public Stream Load(Uri uri) {
            url = new IoUrl(uri);
            return Load();
        }

        /// <summary>
        /// Loads this instance of the stream.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        /// <remarks></remarks>
        public Stream Load() {
            HasValidIoUrl();

            MemoryStream stream;
            if(String.IsNullOrEmpty(this)) {
                throw new ArgumentNullException();
            }

            try {

                stream = new MemoryStream();
                using(IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication()) {
                    using(IsolatedStorageFileStream ioStream = file.OpenFile(this, FileMode.Open)) {
                        ioStream.CopyTo(stream);
                        ioStream.Close();
                    }
                }
            }
            catch(IsolatedStorageException) {
                throw new IsolatedStorageException(string.Format("Url {0} " + "not found in Isolated Storage", this));
            }

            return stream;
        }
        /// <summary>
        /// Saves the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="bufferLength">Length of the buffer.</param>
        public void Save(Uri filename, Byte[] buffer, long bufferLength) {
            url = new IoUrl(filename);
            Save(buffer, bufferLength);
        }
        /// <summary>
        /// Saves the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="bufferLength">Length of the buffer.</param>
        /// <remarks></remarks>
        public void Save(Byte[] buffer, long bufferLength) {

            HasValidIoUrl();
            if(String.IsNullOrEmpty(this)) {
                throw new ArgumentNullException();
            }

            if(!IsSpaceAvailble(bufferLength)) {
                throw new ArgumentOutOfRangeException("bufferLength");
            }

            using(var stream = new IsolatedStorageFileStream(this, FileMode.Create, IsolatedStorageFile.GetUserStoreForApplication())) {

                // Write it out to the file
                stream.Write(buffer, 0, (int)bufferLength);
                stream.Close();
            }
        }
        /// <summary>
        /// Saves the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <remarks></remarks>
        public void Save(StorageStream stream) {

            if (stream == null) {
                throw new ArgumentNullException();
            }

            // Make sure the stream is at the beginning of the file
            stream.Position = 0;

            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            Save(buffer, (int)stream.Length);
        }
        /// <summary>
        /// Removes the specified filename.
        /// </summary>
        /// <param name="uri">The filename.</param>
        /// <remarks></remarks>
        public void Remove(Uri uri) {
            if (String.IsNullOrEmpty(uri.OriginalString)) {
                throw new ArgumentNullException();
            }

            url = new IoUrl(uri);
            Remove();
        }
        /// <summary>
        /// Removes this instance of the file
        /// </summary>
        /// <remarks></remarks>
        public void Remove() {
            HasValidIoUrl();

            if (String.IsNullOrEmpty(this)) {
                throw new ArgumentNullException();
            }

            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication()) {
                store.DeleteFile(this);
            }
        }
        /// <summary>
        /// Determines whether the application has the space indicated by size.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>
        ///   <c>true</c> if space is availble, otherwise, <c>false</c>.
        /// </returns>
        public bool IsSpaceAvailble(long size) {
            using(IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication()) {
                if(size > store.AvailableFreeSpace)
                    return false;
            }
            return true;
        }
        private void HasValidIoUrl() {
            if (url == null) {
                throw new InvalidOperationException("Uri not set");
            }
        }
    }
}
