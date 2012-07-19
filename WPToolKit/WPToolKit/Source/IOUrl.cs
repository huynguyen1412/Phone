using System;
using System.IO;

namespace WPToolKit {
    public class IOUrl {
        private Uri baseUriName;

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        /// <remarks></remarks>
        public Uri Url {
            get {
                return baseUriName;
            }
            set {
            }
        }
        public String GetPath() {
            if (!IsFile()) {
                throw new ArgumentOutOfRangeException("Url is not a filename");
            }

            int subIdx = baseUriName.AbsolutePath.LastIndexOf('/');
            if (subIdx == 0 && baseUriName.AbsolutePath.StartsWith("/") == false)
                return null;

            return baseUriName.AbsolutePath.Substring(0, subIdx+1);
        }
        public IOUrl(String url) {
            baseUriName = new Uri(url);
        }
        private IOUrl() {
            // must create this class with a Uri, hence this construction is private
        }

        /// <summary>
        /// Determines whether this instance is file.
        /// </summary>
        /// <returns><c>true</c> if this instance is file; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public bool IsFile() {
            if (baseUriName.Scheme.CompareTo("file") == 0) {
                return true;
            }

            return false;
        }
        /// <summary>
        /// Determines whether this instance is HTTP.
        /// </summary>
        /// <returns><c>true</c> if this instance is HTTP; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public bool IsHttp() {
            if (baseUriName.Scheme.CompareTo("http") == 0) {
                return true;
            }

            return false;
        }

        // conversions
        /// <summary>
        /// Performs an implicit conversion from <see cref="WPToolKit.IOUrl"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks></remarks>
        public static implicit operator String(IOUrl i) {
            if (i.IsFile()) {
                return Path.GetFileName(i.Url.OriginalString);
            }

            if (i.IsHttp()) {
                int subIdx = i.Url.AbsolutePath.LastIndexOf('/');

                // either no trailing '/' or nothing after the '/'(i.e. no file name)
                if (subIdx == -1 || i.Url.AbsolutePath.Length == (subIdx+1)) {
                    throw new ArgumentNullException("Url does not contain a filename");
                }


                return i.Url.AbsolutePath.Substring( subIdx+ 1);
            }

            // it's neither a file or http Uri
            throw new ArgumentOutOfRangeException("Url is not a filename");
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="WPToolKit.IOUrl"/> to <see cref="System.Uri"/>.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks></remarks>
        public static implicit operator Uri(IOUrl i) {
            return i.Url;
        }
    }
}
