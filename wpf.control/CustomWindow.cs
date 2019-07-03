using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
namespace wpf.control
{
    public class CustomWindow : Window
    {
        private Button WindowOutSideButton;
        private Button minimizeButton;

        private Button maximizeButton;

        private Button closeButton;

        private StackPanel buttonsPanel;
        static CustomWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomWindow), new FrameworkPropertyMetadata(typeof(CustomWindow)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            minimizeButton = (Button)Template.FindName("MinimizeButton", this);
            maximizeButton = (Button)Template.FindName("MaximizeButton", this);
            closeButton = (Button)Template.FindName("CloseButton", this);
            buttonsPanel = (StackPanel)Template.FindName("ButtonsPanel", this);
        }
        public CustomWindow()
        {

        }
    }
}
