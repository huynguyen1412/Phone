using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.ComponentModel;

namespace WPToolKit {
    public class IOStorage {

        private Uri iOFilenameUri;
        private String iOFilenameString;

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
        /// Gets or sets the IO filename Uri.
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

        /// <summary>
        /// Gets the full file path
        /// </summary>
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
        /// Sets the Uri and filename. The filename is based on the Uri trailing '/'
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
        /// Loads the specified new Uri.
        /// </summary>
        /// <param name="newUrl">The new URL.</param>
        /// <returns></returns>
        public Stream Load(Uri newUrl) {

            SetUriAndFilename(newUrl);
            return this.Load();
        }
        /// <summary>
        /// Loads the Uri designated by the IOFilenameUri.
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
        public void Save(Uri filename, Byte[] buffer, long bufferLength) {
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
        public void Save(Byte[] buffer, long bufferLength) {

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
        public void Remove(Uri filename) {
            if (String.IsNullOrEmpty(filename.OriginalString)) {
                throw new ArgumentNullException();
            }

            SetUriAndFilename(filename);
            this.Remove();
        }
        /// <summary>
        /// Removes the file referenced by IOFilenameString.
        /// </summary>
        /// <remarks></remarks>
        public void Remove() {

            if (String.IsNullOrEmpty(IOFilenameUri.OriginalString)) {
                throw new ArgumentNullException("IOFilenameString", "IO Filename parameter not set");
            }

            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication()) {
                store.DeleteFile(IOFilenameString);
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
    }
}
