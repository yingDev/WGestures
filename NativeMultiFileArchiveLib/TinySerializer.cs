using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using NativeMultiFileArchiveLib;

namespace IO
{

    /// <summary>
    /// Implements compressed binary serialization: designed to fit serialized data into the smallest space possible. 
    /// 
    /// This class extends the SerializationBinder and overrides the BindToName method to prevent assembly or type information being written into the 
    /// serialization stream.
    /// 
    /// This means that the serializer must know the type of the object in order to de-serialize it, hence the use of generics in the 
    /// deserialization methods.
    /// 
    /// The data can be compressed on its way in or out using Gzip compression.
    /// 
    /// </summary>
    public class TinySerializer : SerializationBinder
    {
        /// <summary>
        /// the type being de-serialized.
        /// </summary>
        private Type _type = null;

        /// <summary>
        /// default constructor.
        /// </summary>
        private TinySerializer()
        {
        }

        /// <summary>
        /// construct with the type to be de-serialized. private to prevent construction from outside this class.
        /// </summary>
        /// <param name="toDeserialize"></param>
        private TinySerializer(Type toDeserialize)
        {
            _type = toDeserialize;
        }

        /// <summary>
        /// override the BindToName property: effectively prevents the assemblyName and typeName being written into the serialization stream.
        /// this doesn' work if the object contains a list.
        /// </summary>
        /// <param name="serializedType"></param>
        /// <param name="assemblyName"></param>
        /// <param name="typeName"></param>
//        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
//        {
//            // run the base method... not sure what it actually does.
//            base.BindToName(serializedType, out assemblyName, out typeName);
//
//            // set both assemblyName and typeName to zero-length strings
//            assemblyName = ""; typeName = "";
//        }

        /// <summary>
        /// instread of binding to the specified type (which will be null when de-serializing an object serialized using this binder)
        /// just return the type that this binder was constructed with.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public override Type BindToType(string assemblyName, string typeName)
        {
            return _type;
        }

        /// <summary>
        /// serialize the object to the stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="toSave"></param>
        public static void Serialize(Stream stream, object toSave, bool stripTypeNames = false)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            if (stripTypeNames)
                formatter.Binder = new TinySerializer();

            formatter.Serialize(stream, toSave);
        }


        public static byte[] Serialize(object graph, bool stripTypeNames = false)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Serialize(ms, graph, stripTypeNames);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// deserialize the contents of the stream to the specified type. be sure that the stream contains the type specified.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T DeSerialize<T>(Stream stream, bool useCustomBinder = false)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            if (useCustomBinder)
                formatter.Binder = new TinySerializer(typeof(T));
            object graph = formatter.Deserialize(stream);
            if (graph is T)
                return (T)graph;
            else
                throw new ArgumentException("Invalid Type!");
        }

        /// <summary>
        /// serialize the object to the destination stream and use Gzip compression to reduce it's size on the way.
        /// </summary>
        /// <param name="destinationStream">the destination stream</param>
        /// <param name="graph">the object to serialize.</param>
        public static void SerializeCompressed(Stream destinationStream, Object graph, bool stripTypeNames = false)
        {
            // create the formatter and assign the custom binder:
            BinaryFormatter formatter = new BinaryFormatter();
            if (stripTypeNames)
                formatter.Binder = new TinySerializer();

            // wrap the output stream in a deflate compression stream to reduce the size of the data.
            // deflate is chosen over GZip as there is no header information, which wastes additional space in a space critical operation.
            using (DeflateStream compressionStream = new DeflateStream(destinationStream, CompressionMode.Compress, true))
            {
                formatter.Serialize(compressionStream, graph);
            }
        }

        /// <summary>
        /// overload using byte array as output.
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static byte[] SerializeCompressed(Object graph, bool stripTypeNames = false)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                SerializeCompressed(ms, graph, stripTypeNames);

                // return the serialized data:
                return ms.ToArray();
            }
        }

        /// <summary>
        /// deserialize an object from compressed data.
        /// </summary>
        /// <typeparam name="T">the type of object to deserialize</typeparam>
        /// <param name="compressedInputStream">stream of compressed data containing an object to deserialize</param>
        /// <returns></returns>
        public static T DeSerializeCompressed<T>(Stream compressedInputStream, bool useCustomBinder = false)
        {
            // construct the binary formatter and assign the custom binder:
            BinaryFormatter formatter = new BinaryFormatter();
            if (useCustomBinder)
                formatter.Binder = new TinySerializer(typeof(T));
            
            // read the stream through a GZip decompression stream.
            using (DeflateStream decompressionStream = new DeflateStream(compressedInputStream, CompressionMode.Decompress, true))
            {
                // deserialize to an object:
                object graph = formatter.Deserialize(decompressionStream);

                // check the type is correct and return.
                if (graph is T)
                    return (T)graph;
                else
                    throw new ArgumentException("Invalid Type!");
            }
        }

        /// <summary>
        /// overload the DeSerializeCompressed method to use a byte buffer as the source data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T DeSerializeCompressed<T>(byte[] data, bool useCustomBinder = false)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                return DeSerializeCompressed<T>(ms, useCustomBinder);
            }
        }

        public static T DeSerialize<T>(byte[] data, bool useCustomBinder = false)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                return DeSerialize<T>(ms, useCustomBinder);
            }
        }

        public static byte[] Deflate(byte[] data)
        {
            using (MemoryStream inputStream = new MemoryStream(data))
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    using (DeflateStream df = new DeflateStream(outputStream, CompressionMode.Compress, true))
                    {
                        // write the input stream into the compression stream:
                        inputStream.CopyTo(df, 1024);

                        // return the underlying memory stream:
                        return outputStream.ToArray();
                    }
                }
            }
        }

        public static byte[] Inflate(byte[] data)
        {
            using (MemoryStream input = new MemoryStream(data))
            {
                using (DeflateStream decompressionStream = new DeflateStream(input, CompressionMode.Decompress, true))
                {
                    using (MemoryStream output = new MemoryStream())
                    {
                        decompressionStream.CopyTo(output, 1024);
                        return output.ToArray();
                    }
                }
            }
        }

    }
}
