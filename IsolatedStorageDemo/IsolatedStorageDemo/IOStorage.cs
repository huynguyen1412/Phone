using System;
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

namespace IsolatedStorageDemo {
    public class IOStorage {

        private Uri iOFilenameUri;
        public Uri IOFilenameUri {
            get { return iOFilenameUri; }
            set { iOFilenameUri = value; }
        }
        
        private String iOFilenameString;
	    public String IOFilenameString {
		    get { return iOFilenameString;}
		    set { iOFilenameString = value;}
	    }

        private IOStorage(Uri filename) {
        }
        public IOStorage(String filename) {
            IOFilenameString = filename;
        }

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

        public bool IsSpaceAvailble(int size) {
            using(IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication()) {
                if(size > store.AvailableFreeSpace)
                    return false;
            }
            return true;
        }
    }
}
