﻿using Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AnimationEditor.MountAnimationCreator
{
    /// <summary>
    /// Interaction logic for BatchProcessOptions.xaml
    /// </summary>
    public partial class BatchProcessOptionsWindow : Window
    {
        public BatchProcessOptionsWindow()
        {
            InitializeComponent();
        }

        public static BatchProcessOptions ShowDialog(string fragmentName, string savePrefix)
        {
            var options = new BatchProcessOptions() { FragmentName = fragmentName,SavePrefix = savePrefix };
            var window = new BatchProcessOptionsWindow();
            window.DataContext = options;
            if(window.ShowDialog() == true)
                return options;
            return null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }

    public class BatchProcessOptions : NotifyPropertyChangedImpl
    {
        bool _createAnimPack = true;
        public bool CreateAnimPack
        {
            get => _createAnimPack;
            set => SetAndNotify(ref _createAnimPack, value);
        }

        string _animPackName = "new_anim_tables.animpack";
        public string AnimPackName
        {
            get => _animPackName;
            set => SetAndNotify(ref _animPackName, value);
        }

        bool _createAnimBin = true;  
        public bool CreateAnimBin
        {
            get => _createAnimBin;
            set => SetAndNotify(ref _createAnimBin, value);
        }

        string _animBinName = "new_anim_tables.bin";
        public string AnimBinName
        {
            get => _animBinName;
            set => SetAndNotify(ref _animBinName, value);
        }

        bool _createFragment = true;
        public bool CreateFragment
        {
            get => _createFragment;
            set => SetAndNotify(ref _createFragment, value);
        }

        string _fragmentName;
        public string FragmentName
        {
            get => _fragmentName;
            set => SetAndNotify(ref _fragmentName, value);
        }

        bool _createAnimations = true;
        public bool CreateAnimations
        {
            get => _createAnimations;
            set => SetAndNotify(ref _createAnimations, value);
        }

        public string SavePrefix { get; set; }
    }
}
