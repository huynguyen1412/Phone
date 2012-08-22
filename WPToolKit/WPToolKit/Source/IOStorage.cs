using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.ComponentModel;

namespace WPToolKit.Source
{
    public class IOStorage {

        private IOUrl url;
        public IOUrl Url {
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
            private set { }
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="WPToolKit.IOStorage"/> to <see cref="System.Uri"/>.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks></remarks>
        public static implicit operator Uri(IOStorage s) {
            s.HasValidIOUrl();
            return s.url;
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="WPToolKit.IOStorage"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks></remarks>
        public static implicit operator String(IOStorage s) {
            s.HasValidIOUrl();
            return s.url;
        }
        /// <summary>
        /// Gets the path for the underlying filename.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public String GetPath() {
            HasValidIOUrl();
            return url.GetPath();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="IOStorage"/> class.
        /// </summary>
        public IOStorage() {
            url = null;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="IOStorage"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        public IOStorage(Uri url) {
            this.url = new IOUrl(url);
        }
        /// <summary>
        /// Loads the specified new Uri.
        /// </summary>
        /// <param name="newUrl">The new URL.</param>
        /// <returns></returns>
        public Stream Load(Uri url) {
            this.url = new IOUrl(url);
            return this.Load();
        }
        /// <summary>
        /// Loads this instance of the stream.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public Stream Load() {
            HasValidIOUrl();

            MemoryStream stream = null;
            if(String.IsNullOrEmpty(this)) {
                throw new ArgumentNullException("Url filename is invalid");
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
                throw new IsolatedStorageException("Url " + (String)this + " not found in Isolated Storage");
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
            this.url = new IOUrl(filename);
            this.Save(buffer, bufferLength);
        }
        /// <summary>
        /// Saves the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="bufferLength">Length of the buffer.</param>
        /// <remarks></remarks>
        public void Save(Byte[] buffer, long bufferLength) {

            HasValidIOUrl();
            if(String.IsNullOrEmpty(this)) {
                throw new ArgumentNullException("Url filename is invalid");
            }

            try {
                if(!IsSpaceAvailble(bufferLength)) {
                    throw new ArgumentOutOfRangeException("Buffer length parameter too long for Save");
                }

                using(IsolatedStorageFileStream stream = new IsolatedStorageFileStream(this, System.IO.FileMode.Create, IsolatedStorageFile.GetUserStoreForApplication())) {

                    // Write it out to the file
                    stream.Write(buffer, 0, (int)bufferLength);
                    stream.Close();
                }
            }
            finally {
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

            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            this.Save(buffer, (int)stream.Length);
        }
        /// <summary>
        /// Removes the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <remarks></remarks>
        public void Remove(Uri uri) {
            if (String.IsNullOrEmpty(uri.OriginalString)) {
                throw new ArgumentNullException();
            }

            this.url = new IOUrl(uri);
            this.Remove();
        }
        /// <summary>
        /// Removes this instance of the file
        /// </summary>
        /// <remarks></remarks>
        public void Remove() {
            HasValidIOUrl();

            if (String.IsNullOrEmpty(this)) {
                throw new ArgumentNullException("Url filename is invalid");
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
        private void HasValidIOUrl() {
            if (url == null) {
                throw new InvalidOperationException("Uri not set");
            }
        }
    }
}
