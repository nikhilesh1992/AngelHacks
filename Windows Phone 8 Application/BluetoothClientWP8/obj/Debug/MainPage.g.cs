﻿#pragma checksum "C:\Users\Jatin\Desktop\PennApps\BluetoothClientWP8\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2756BE1677BEA486A5917A595AFE2862"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace BluetoothClientWP8 {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox DeviceName;
        
        internal System.Windows.Controls.Button ConnectAppToDeviceButton;
        
        internal System.Windows.Controls.StackPanel Tile1;
        
        internal System.Windows.Controls.StackPanel Tile2;
        
        internal System.Windows.Controls.StackPanel Tile3;
        
        internal System.Windows.Controls.TextBlock Life;
        
        internal System.Windows.Controls.StackPanel Tile4;
        
        internal System.Windows.Controls.TextBlock Coordinates;
        
        internal System.Windows.Controls.StackPanel Tile5;
        
        internal System.Windows.Controls.StackPanel Tile6;
        
        internal System.Windows.Controls.TextBlock Temp;
        
        internal System.Windows.Controls.TextBlock BodyDetectionStatus;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/BluetoothClientWP8;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.DeviceName = ((System.Windows.Controls.TextBox)(this.FindName("DeviceName")));
            this.ConnectAppToDeviceButton = ((System.Windows.Controls.Button)(this.FindName("ConnectAppToDeviceButton")));
            this.Tile1 = ((System.Windows.Controls.StackPanel)(this.FindName("Tile1")));
            this.Tile2 = ((System.Windows.Controls.StackPanel)(this.FindName("Tile2")));
            this.Tile3 = ((System.Windows.Controls.StackPanel)(this.FindName("Tile3")));
            this.Life = ((System.Windows.Controls.TextBlock)(this.FindName("Life")));
            this.Tile4 = ((System.Windows.Controls.StackPanel)(this.FindName("Tile4")));
            this.Coordinates = ((System.Windows.Controls.TextBlock)(this.FindName("Coordinates")));
            this.Tile5 = ((System.Windows.Controls.StackPanel)(this.FindName("Tile5")));
            this.Tile6 = ((System.Windows.Controls.StackPanel)(this.FindName("Tile6")));
            this.Temp = ((System.Windows.Controls.TextBlock)(this.FindName("Temp")));
            this.BodyDetectionStatus = ((System.Windows.Controls.TextBlock)(this.FindName("BodyDetectionStatus")));
        }
    }
}

