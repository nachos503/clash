﻿#pragma checksum "..\..\..\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5831DECA4ED41BF4A0EEF86F984F4324C3CDA859"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using ClashGame;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ClashGame {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox outputTextBox;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ChooseBlueArmy;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ChooseRedArmy;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CreateArmy;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Turn;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CancelTurn;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ToTheEnd;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button UseWizard;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button UseHealer;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button UseArcher;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ClashGame;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.outputTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.ChooseBlueArmy = ((System.Windows.Controls.Button)(target));
            
            #line 17 "..\..\..\MainWindow.xaml"
            this.ChooseBlueArmy.Click += new System.Windows.RoutedEventHandler(this.ChooseBlueArmy_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ChooseRedArmy = ((System.Windows.Controls.Button)(target));
            
            #line 23 "..\..\..\MainWindow.xaml"
            this.ChooseRedArmy.Click += new System.Windows.RoutedEventHandler(this.ChooseRedArmy_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.CreateArmy = ((System.Windows.Controls.Button)(target));
            
            #line 29 "..\..\..\MainWindow.xaml"
            this.CreateArmy.Click += new System.Windows.RoutedEventHandler(this.CreateArmy_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.Turn = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\..\MainWindow.xaml"
            this.Turn.Click += new System.Windows.RoutedEventHandler(this.Turn_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.CancelTurn = ((System.Windows.Controls.Button)(target));
            
            #line 41 "..\..\..\MainWindow.xaml"
            this.CancelTurn.Click += new System.Windows.RoutedEventHandler(this.CancelTurn_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.ToTheEnd = ((System.Windows.Controls.Button)(target));
            
            #line 47 "..\..\..\MainWindow.xaml"
            this.ToTheEnd.Click += new System.Windows.RoutedEventHandler(this.ToTheEnd_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.UseWizard = ((System.Windows.Controls.Button)(target));
            
            #line 53 "..\..\..\MainWindow.xaml"
            this.UseWizard.Click += new System.Windows.RoutedEventHandler(this.UseWizard_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.UseHealer = ((System.Windows.Controls.Button)(target));
            
            #line 60 "..\..\..\MainWindow.xaml"
            this.UseHealer.Click += new System.Windows.RoutedEventHandler(this.UseHealer_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.UseArcher = ((System.Windows.Controls.Button)(target));
            
            #line 67 "..\..\..\MainWindow.xaml"
            this.UseArcher.Click += new System.Windows.RoutedEventHandler(this.UseArcher_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

