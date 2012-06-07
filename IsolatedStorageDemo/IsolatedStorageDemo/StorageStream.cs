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
using System.IO;

namespace IsolatedStorageDemo
{
    public class StorageStream : MemoryStream
    {
  
        public MemoryStream Stream {
            get {
                Position = 0;
                return this;
            }
        }

        public StorageStream() {
        }
     
        public StorageStream(Stream s) {

            if(s == null) {
                throw new ArgumentNullException();
            }

            Position = 0;
            s.CopyTo(this);
        }

        new public void CopyTo(Stream destination) {

            if (destination == null) {
              throw new ArgumentNullException();
            }

            base.CopyTo(destination);
            destination.Position = 0;
            return;
        }
    }
}
