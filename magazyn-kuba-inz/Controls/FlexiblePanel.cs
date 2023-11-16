using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Warehouse.Controls
{
    public class FlexiblePanel : Panel, IAddChild
    {
        private const double _xs = 576;
        private const double _sm = 720;
        private const double _md = 960;
        private const double _lg = 1140;

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public FlexiblePanel()
        {

        }

        #endregion

        protected override Size MeasureOverride(Size availableSize)
        {
            Size panelDesiredSize = new Size();

            // In our example, we just have one child.
            // Report that our panel requires just the size of its only child.
            foreach (UIElement child in InternalChildren)
            {
                child.Measure(availableSize);
                panelDesiredSize = new Size(
                    Math.Max(panelDesiredSize.Width, child.DesiredSize.Width),
                    Math.Max(panelDesiredSize.Height, child.DesiredSize.Height)
                );
            }

            return panelDesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double x = 0;
            double y = 0;
            double maxWidth = 0;

            foreach (UIElement child in InternalChildren)
            {
                Size siz = Getsize(child, finalSize);
                if (x + siz.Width > finalSize.Width)
                {
                    // If adding the current child would exceed the available width,
                    // start a new row and reset x to 0.
                    x = 0;
                    y += maxWidth; // move y down by the height of the tallest element in the current row
                    maxWidth = 0; // reset the maximum width for the new row
                }
                child.Arrange(new Rect(new Point(x, y), siz));
                x += siz.Width;
                maxWidth = Math.Max(maxWidth, siz.Height);
            }

            return finalSize;
        }

        protected Size Getsize(UIElement child, Size finalSize)
        {
            int size = 12;

            if(child is FlexibleItem item)
            {
                if (finalSize.Width <= _xs)
                    size = FlexibleItem.GetSize(item, UnitWidthType.XS);
                else if (finalSize.Width <= _sm)
                    size = FlexibleItem.GetSize(item, UnitWidthType.SM);
                else if (finalSize.Width <= _md)
                    size = FlexibleItem.GetSize(item, UnitWidthType.MD);
                else if (finalSize.Width <= _lg)
                    size = FlexibleItem.GetSize(item, UnitWidthType.LG);
            }
            finalSize = new Size((size * finalSize.Width) / 12, child.DesiredSize.Height > finalSize.Height ? child.DesiredSize.Height : finalSize.Height);

            return finalSize;

        }

    }

    public class FlexibleItem : StackPanel
    {
        #region Properties

        public FlexUnitType XSSize
        {
            get { return (FlexUnitType)GetValue(XSSizeProperty); }
            set { SetValue(XSSizeProperty, value); }
        }

        public FlexUnitType SMSize
        {
            get { return (FlexUnitType)GetValue(SMSizeProperty); }
            set { SetValue(SMSizeProperty, value); }
        }

        public FlexUnitType MDSize
        {
            get { return (FlexUnitType)GetValue(MDSizeProperty); }
            set { SetValue(MDSizeProperty, value); }
        }

        public FlexUnitType LGSize
        {
            get { return (FlexUnitType)GetValue(LGSizeProperty); }
            set { SetValue(LGSizeProperty, value); }
        }

        #endregion

        #region Dependency 

        public static readonly DependencyProperty MDSizeProperty =
            DependencyProperty.Register(nameof(MDSize), typeof(FlexUnitType), typeof(FlexibleItem), new PropertyMetadata(FlexUnitType.All, propSizeChange));

        public static readonly DependencyProperty LGSizeProperty =
            DependencyProperty.Register(nameof(LGSize), typeof(FlexUnitType), typeof(FlexibleItem), new PropertyMetadata(FlexUnitType.All, propSizeChange));

        public static readonly DependencyProperty XSSizeProperty =
            DependencyProperty.Register(nameof(XSSize), typeof(FlexUnitType), typeof(FlexibleItem), new PropertyMetadata(FlexUnitType.All, propSizeChange));

        public static readonly DependencyProperty SMSizeProperty =
            DependencyProperty.Register(nameof(SMSize), typeof(FlexUnitType), typeof(FlexibleItem), new PropertyMetadata(FlexUnitType.All, propSizeChange));


        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public FlexibleItem()
        {

        }

        #endregion

        private static void propSizeChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is FlexibleItem item)
            {
                if(item.Parent is FlexiblePanel panel)
                {
                    panel.InvalidateMeasure();
                }
            }
        }

        public static int GetSize(FlexibleItem element, UnitWidthType widthType  )
        {
            FlexUnitType type = FlexUnitType.All;
            switch (widthType)
            {
                case UnitWidthType.XS:
                    type = (FlexUnitType)element.GetValue(XSSizeProperty);
                    break;
                case UnitWidthType.SM:
                    type = (FlexUnitType)element.GetValue(SMSizeProperty);
                    break;
                case UnitWidthType.MD:
                    type = (FlexUnitType)element.GetValue(MDSizeProperty);
                    break;
                case UnitWidthType.LG:
                    type = (FlexUnitType)element.GetValue(LGSizeProperty);
                    break;
            }

            return (int)type;
        }

    }

    public enum UnitWidthType
    {
        XS,
        SM,
        MD,
        LG
    }


    public enum FlexUnitType
    {
        Unset = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Eleven = 11,
        Tweleve = 12,
        All = Tweleve,
        Half = Six,
        Quarter = Three,
        OneThird = Four,
    }
}