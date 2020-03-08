using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PokemonAutomation
{
    /// <summary>
    /// CustomButton.xaml の相互作用ロジック
    /// </summary>
    public partial class CustomButton : Button
    {
        public event PropertyChangedCallback IsPressedChaged;

        public CustomButton()
        {
            InitializeComponent();
        }

        protected override void OnIsPressedChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsPressedChanged(e);
            IsPressedChaged.Invoke(this, e);
        }
    }
}
