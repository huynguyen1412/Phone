﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.IO;
using System.ComponentModel;

namespace IsolatedStorageDemo {

    /// <summary>
    /// 
    /// </summary>
    public class IOStorage : INotifyPropertyChanged {
        private const String emptyUri = "http://null";

        private Uri iOFilenameUri;
        public Uri IOFilenameUri {
            get { 
                return iOFilenameUri; 
            }
            set { 
                iOFilenameUri = value;
                IOFilenameString = value.AbsolutePath.Substring(value.AbsolutePath.LastIndexOf('/') + 1);
                RaisePropertyChanged("IOFilenameUri");
            }
        }

        private String iOFilenameString;
	    public String IOFilenameString {
		    get { 
                return iOFilenameString;
            }
		    set { 
                iOFilenameString = value;
                RaisePropertyChanged(IOFilenameString);
            }
	    }

        /// <summary>
        /// Initializes a new instance of the <see cref="IOStorage"/> class.
        /// </summary>
        public IOStorage() {
            IOFilenameString = "";
            IOFilenameUri = new Uri(emptyUri);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IOStorage"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        public IOStorage(Uri url) {
            IOFilenameUri = url;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IOStorage"/> class.
        /// Note: Uri will be set to "emptyUri"
        /// </summary>
        /// <param name="filename">The filename.</param>
        public IOStorage(String filename) {
            IOFilenameString = filename;
            IOFilenameUri = new Uri(emptyUri);
        }

        /// <summary>
        /// Loads the specified new URL.
        /// </summary>
        /// <param name="newUrl">The new URL.</param>
        /// <returns></returns>
        public Stream Load(Uri newUrl) {
            this.IOFilenameUri = newUrl;
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

            MemoryStream result = null;
            if(String.IsNullOrEmpty(IOFilenameString)) {
                throw new ArgumentNullException("IOFilenameString", "IO Filename parameter not set");
            }

            try {
 
                using(IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication()) {
                    using(IsolatedStorageFileStream ioStream = file.OpenFile(IOFilenameString, FileMode.Open)) {

                        result = new MemoryStream();
                        ioStream.CopyTo(result);
                    }
                }
            }
            catch(Exception e) {
            }

            return result;
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
 
                using(IsolatedStorageFileStream isfs = new IsolatedStorageFileStream(IOFilenameString, System.IO.FileMode.Create, IsolatedStorageFile.GetUserStoreForApplication())) {

                    // Write it out to the file
                    isfs.Write(buffer, 0, (int)bufferLength);
                    isfs.Flush();
                }
            }
            finally {
            }
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(String property) {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
