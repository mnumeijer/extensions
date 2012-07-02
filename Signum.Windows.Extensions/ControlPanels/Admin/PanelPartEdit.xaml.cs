﻿using System;
using System.Collections.Generic;
using System.Linq;
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
using Signum.Windows;
using Signum.Entities;
using Signum.Entities.Exceptions;

namespace Signum.Windows.ControlPanels
{
    /// <summary>
    /// Interaction logic for ControlPanel.xaml
    /// </summary>
    public partial class PanelPartEdit : UserControl
    {
        public PanelPartEdit()
        {
            InitializeComponent();
        }

        public PanelPartEdit(PropertyRoute pr)
        {
            InitializeComponent();

            Common.SetTypeContext(this, pr);
        }
    }
}
