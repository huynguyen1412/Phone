using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.ComponentModel;

namespace WPToolKit {
    /// <summary>
    /// 
    /// </summary>
    public class StorageStream : MemoryStream {

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <remarks></remarks>
        public MemoryStream Stream {
            get {
                Position = 0;
                return this;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageStream"/> class.
        /// </summary>
        /// <remarks></remarks>
        public StorageStream() {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageStream"/> class.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <remarks></remarks>
        public StorageStream(StorageStream s) {

            if(s == null) {
                throw new ArgumentNullException();
            }

            s.CopyTo(this);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageStream"/> class.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <remarks></remarks>
        public StorageStream(Stream s) {

            if (s == null) {
                throw new ArgumentNullException();
            }

            s.CopyTo(this);
        }
        /// <summary>
        /// Reads all the bytes from the current stream and writes them to the destination stream.
        /// </summary>
        /// <param name="destination">The stream that will contain the contents of the current stream.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="destination"/> is null.</exception>
        ///   
        /// <exception cref="T:System.NotSupportedException">The current stream does not support reading.-or-<paramref name="destination"/> does not support writing.</exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">Either the current stream or <paramref name="destination"/> were closed before the <see cref="M:System.IO.Stream.CopyTo(System.IO.Stream)"/> method was called.</exception>
        ///   
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        /// <remarks></remarks>
        new public void CopyTo(Stream destination) {

            if(destination == null) {
                return;
            }

            try {
                base.CopyTo(destination);
                destination.Position = 0;
            }
            catch (Exception e) {
                throw e;
            }
            return;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class IOStorage {

        private Uri iOFilenameUri;
        private String iOFilenameString;

        /// <summary>
        /// Gets or sets the IO filename URI.
        /// </summary>
        /// <value>The IO filename URI.</value>
        /// <remarks></remarks>
        public Uri IOFilenameUri {
            get {
                return iOFilenameUri;
            }
            set {
                SetUriAndFilename(value);
            }
        }
        /// <summary>
        /// Gets or sets the IO filename string.
        /// </summary>
        /// <value>The IO filename string.</value>
        /// <remarks></remarks>
        public String IOFilenameString {
            get {
                return iOFilenameString;
            }
            set {
                iOFilenameString = value;
            }
        }

        public String IOFilenamePath {
            get {
                if (iOFilenameUri == null) {
                    return null;
                }
                return (iOFilenameUri.IsAbsoluteUri == true) ? 
                    iOFilenameUri.AbsolutePath : iOFilenameUri.OriginalString;
            }
            private set { }
        }
        /// <summary>
        /// Sets the URI and filename. The filename is based on the Uri trailing '/'
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <remarks>If the Uri doesn't contain a filename, the IOFilename is set to null</remarks>
        private void SetUriAndFilename(Uri url) {

            // At this point the url is null or a valid Uri.
            iOFilenameUri = url;
            if(url == null) {
                iOFilenameString = null;
            } else {

                if (url.IsAbsoluteUri) {
                    iOFilenameString = IOFilenameUri.AbsolutePath.Substring(IOFilenameUri.AbsolutePath.LastIndexOf('/') + 1);
                }
                else {
                    iOFilenameString = url.OriginalString;
                }
 
                if (String.IsNullOrEmpty(iOFilenameString)) {
                    iOFilenameString = null;
                }
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="IOStorage"/> class.
        /// </summary>
        public IOStorage() {
            SetUriAndFilename(null);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="IOStorage"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        public IOStorage(Uri url) {
            SetUriAndFilename(url);
        }
        /// <summary>
        /// Loads the specified new URL.
        /// </summary>
        /// <param name="newUrl">The new URL.</param>
        /// <returns></returns>
        public Stream Load(Uri newUrl) {

            SetUriAndFilename(newUrl);
            return this.Load();
        }
        /// <summary>
        /// Loads the Url designated by the IOFilenameUri.
        /// </summary>
        /// <exceptions>
        /// ArgumentNullException - IOFilenameUri not set
        /// </exceptions>
        /// <returns></returns>
        public Stream Load() {

            MemoryStream stream = null;
            if(String.IsNullOrEmpty(IOFilenameString)) {
                throw new ArgumentNullException("IOFilenameString", "IO Filename parameter not set");
            }

            try {

                stream = new MemoryStream();
                using(IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication()) {
                    using(IsolatedStorageFileStream ioStream = file.OpenFile(IOFilenameString, FileMode.Open)) {
                        ioStream.CopyTo(stream);
                        ioStream.Close();
                    }
                }
            }
            catch(IsolatedStorageException) {
                throw new IsolatedStorageException("Url " + IOFilenameString + " not found in Isolated Storage");
            }

            return stream;
        }
        /// <summary>
        /// Saves the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="bufferLength">Length of the buffer.</param>
        public void Save(Uri filename, Byte[] buffer, int bufferLength) {
            this.IOFilenameUri = filename;
            this.Save(buffer, bufferLength);
        }
        /// <summary>
        /// Saves the specified buffer in filename IOFilenameUri
        /// </summary>
        /// <exceptions>
        /// ArgumentNullException - IOFilenameUri not set
        /// </exceptions>
        /// <param name="buffer">The buffer.</param>
        /// <param name="bufferLength">Length of the buffer.</param>
        public void Save(Byte[] buffer, int bufferLength) {

            if(String.IsNullOrEmpty(IOFilenameString)) {
                throw new ArgumentNullException("IOFilenameString", "IO Filename parameter not set");
            }

            try {
                if(!IsSpaceAvailble(bufferLength)) {
                    throw new ArgumentOutOfRangeException("Buffer length parameter too long for Save");
                }

                using(IsolatedStorageFileStream stream = new IsolatedStorageFileStream(IOFilenameString, System.IO.FileMode.Create, IsolatedStorageFile.GetUserStoreForApplication())) {

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

            // Make sure the stream is at the beginning of the file
            stream.Position = 0;

            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            this.Save(buffer, (int)stream.Length);
        }
        /// <summary>
        /// Determines whether the application has the space indicated by size.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>
        ///   <c>true</c> if space is availble, otherwise, <c>false</c>.
        /// </returns>
        public bool IsSpaceAvailble(int size) {
            using(IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication()) {
                if(size > store.AvailableFreeSpace)
                    return false;
            }
            return true;
        }
    }
}
