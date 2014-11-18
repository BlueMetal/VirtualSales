using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VirtualSales.Core.ViewModels;
using VirtualSales.Models.Annotations;
using VirtualSales.Wpf.Behaviors;

namespace VirtualSales.Wpf.Views
{
    /// <summary>
    /// Interaction logic for AnnotationView.xaml
    /// </summary>
    public partial class AnnotationView : UserControl
    {
        public AnnotationView()
        {
            InitializeComponent();
            DataContextChanged += HandleDataContextChanged;
        }

        private void HandleDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // all we need to do is force the surface size to re-bind
            SizeObserver.UpdateObservedSizesForFrameworkElement(this);
        }
    }
}
