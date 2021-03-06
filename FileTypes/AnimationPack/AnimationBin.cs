﻿using Filetypes.ByteParsing;
using FileTypes.PackFiles.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileTypes.AnimationPack
{
    public class AnimationBin
    {
        public int TableVersion { get; set; } = 2;
        public int RowCount { get; set; } = 0;
        public string FileName { get; set; }

        public List<AnimationBinEntry> AnimationTableEntries { get; set; } = new List<AnimationBinEntry>();
       

        public AnimationBin(string filename, ByteChunk data)
        {
            FileName = filename;
            TableVersion = data.ReadInt32();
            RowCount = data.ReadInt32();
            AnimationTableEntries = new List<AnimationBinEntry>(RowCount);
            for (int i = 0; i < RowCount; i++)
                AnimationTableEntries.Add(new AnimationBinEntry(data));
        }

        public AnimationBin(string fileName) 
        {
            FileName = fileName;
        }

        public byte[] ToByteArray()
        {
            using MemoryStream memStream = new MemoryStream();

            memStream.Write(ByteParsers.Int32.EncodeValue(TableVersion, out _));
            memStream.Write(ByteParsers.Int32.EncodeValue(AnimationTableEntries.Count, out _));

            foreach (var tableEntry in AnimationTableEntries)
                memStream.Write(tableEntry.ToByteArray());

            return memStream.ToArray();
        }
    }

    public class AnimationBinEntry
    {
        public class FragmentReference
        {
            public string Name { get; set; }
            public int Unknown { get; set; } = 0;

            public FragmentReference() { }

            public FragmentReference(ByteChunk data)
            {
                Name = data.ReadString();
                Unknown = data.ReadInt32();
            }

            public byte[] ToByteArray()
            {
                using MemoryStream memStream = new MemoryStream();
                memStream.Write(ByteParsers.String.WriteCaString(Name));
                memStream.Write(ByteParsers.Int32.EncodeValue(Unknown, out _));
                return memStream.ToArray();
            }

        }

        public string Name { get; set; }
        public string SkeletonName { get; set; }
        public string MountName { get; set; }

        public List<FragmentReference> FragmentReferences { get; set; } = new List<FragmentReference>();
        public short Unknown { get; set; } = 0;

        public AnimationBinEntry(ByteChunk data)
        {
            Name = data.ReadString();
            SkeletonName = data.ReadString();
            MountName = data.ReadString();
            var count = data.ReadInt32();
            for (int i = 0; i < count; i++)
                FragmentReferences.Add(new FragmentReference(data));
            Unknown = data.ReadShort();
        }

        public AnimationBinEntry(string name, string skeletonName, string mountName = "")
        {
            Name = name;
            SkeletonName = skeletonName;
            MountName = mountName;
        }

        public byte[] ToByteArray()
        {
            using MemoryStream memStream = new MemoryStream();

            memStream.Write(ByteParsers.String.WriteCaString(Name));
            memStream.Write(ByteParsers.String.WriteCaString(SkeletonName));
            memStream.Write(ByteParsers.String.WriteCaString(MountName));

            memStream.Write(ByteParsers.Int32.EncodeValue(FragmentReferences.Count, out _));
            foreach (var fragment in FragmentReferences)
                memStream.Write(fragment.ToByteArray());

            memStream.Write(ByteParsers.Short.EncodeValue(Unknown, out _));

            return memStream.ToArray();
        }
    }
}
