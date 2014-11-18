using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VirtualSales.Wpf
{
    public static class DesignModeDetector
    {
        private static readonly Lazy<bool> _isDesignMode;

        static DesignModeDetector()
        {
            _isDesignMode = new Lazy<bool>(() =>
                {
                    var prop = DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement));
                    return (bool)prop.Metadata.DefaultValue;
                });
        }

        public static bool IsDesignMode
        {
            get { return _isDesignMode.Value; }
        }

    }
}
