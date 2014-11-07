using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace NativeMultiFileArchiveLib
{
    static internal class DeflateStreamExtension
    {

        public static void CopyTo(this Stream thiz, Stream destination, int bufferSize)
        {
            if (destination == null)
                throw new ArgumentNullException("destination");
            if (bufferSize <= 0)
                throw new ArgumentOutOfRangeException("bufferSize");
            if (!thiz.CanRead && !thiz.CanWrite)
                throw new ObjectDisposedException("");
            if (!destination.CanRead && !destination.CanWrite)
                throw new ObjectDisposedException("destination");
            if (!thiz.CanRead)
                throw new NotSupportedException();
            if (!destination.CanWrite)
                throw new NotSupportedException();

            thiz.InternalCopyTo(destination, bufferSize);
        }

        private static void InternalCopyTo(this Stream thiz, Stream destination, int bufferSize)
        {
            byte[] buffer = new byte[bufferSize];
            int read;
            while ((read = thiz.Read(buffer, 0, buffer.Length)) != 0)
                destination.Write(buffer, 0, read);
        }
    }
}
