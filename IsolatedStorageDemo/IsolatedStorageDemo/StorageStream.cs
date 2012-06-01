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
    public class StorageStream : IDisposable
    {
        private MemoryStream _Stream { get; set; }
        public long Position {
            get {
                return _Stream.Position;
            }

            set {
                _Stream.Position = value;
            }
        }
        
        public Stream Stream {
            get { 
                return _Stream;
            }
        }

        public StorageStream() {
            _Stream = new MemoryStream();
        }

        public StorageStream(Stream s) : this() {
            s.CopyTo(_Stream);
            _Stream.Position = 0;
        }

        public StorageStream Copy() {

            StorageStream stream = new StorageStream(_Stream);
            return stream;
        }
        
        public void Close() {
            _Stream.Close();
        }

        public void Dispose() {
            _Stream.Dispose();
        }
    }
}
