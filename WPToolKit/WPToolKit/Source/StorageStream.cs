using System;
using System.IO;

namespace WPToolKit.Source
{
    public sealed class StorageStream : MemoryStream
    {
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
        public StorageStream()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageStream"/> class.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <remarks></remarks>
        public StorageStream(StorageStream s)
        {
            if (s == null) {
                throw new ArgumentNullException();
            }

            s.CopyTo(this);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageStream"/> class.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <remarks></remarks>
        public StorageStream(Stream s)
        {
            if (s == null) {
                throw new ArgumentNullException();
            }

            long pos = s.Position;
            s.Position = 0;

            s.CopyTo(this);

            s.Position = pos;
            Position = 0;
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
        new public void CopyTo(Stream destination)
        {

            if (destination == null) {
                throw new ArgumentNullException();
            }

            base.CopyTo(destination);
            destination.Position = 0;
        }
        /// <summary>
        /// Reads all the bytes from the current stream and writes them to the destination stream.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <remarks></remarks>
        public void CopyTo(StorageStream destination)
        {

            if (destination == null) {
                throw new ArgumentNullException();
            }

            base.CopyTo(destination);
            destination.Position = 0;
        }
    }
}
