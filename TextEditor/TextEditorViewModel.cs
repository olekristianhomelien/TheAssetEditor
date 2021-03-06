﻿using Common;
using CommonControls.Common;
using CommonControls.Services;
using FileTypes.PackFiles.Models;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;

namespace TextEditor
{
    public class TextEditorViewModel : NotifyPropertyChangedImpl, IEditorViewModel
    {
        public ICommand SaveCommand { get; set; }


        string _displayName;
        public string DisplayName { get => _displayName; set => SetAndNotify(ref _displayName, value); }


        string _text;
        public string Text { get => _text; set => SetAndNotify(ref _text, value); }

        IPackFile _packFile;
        PackFileService _pf;

        public TextEditorViewModel(PackFileService pf)
        {
            _pf = pf;
            SaveCommand = new RelayCommand(() => Save());
        }

        public IPackFile MainFile
        {
            get => _packFile;
            set
            {
                _packFile = value;
                SetCurrentPackFile(_packFile);
            }
        }



        void SetCurrentPackFile(IPackFile packedFile)
        {
            PackFile file = packedFile as PackFile;
            DisplayName = file.Name;

            byte[] data = file.DataSource.ReadData();
            using (MemoryStream stream = new MemoryStream(data, 0, data.Length))
            {
                using (var reader = new StreamReader(stream, Encoding.ASCII))
                    Text = reader.ReadToEnd();
            }
        }

        public bool Save()
        {
            var path = _pf.GetFullPath(MainFile as PackFile);
            var res = SaveHelper.Save(_pf, path, MainFile as PackFile, Encoding.ASCII.GetBytes(Text));
            if (res != null)
                MainFile = res;
            return false;
        }

        public void Close()
        {
        }

        public bool HasUnsavedChanges()
        {
            return false;
        }
    }
}
