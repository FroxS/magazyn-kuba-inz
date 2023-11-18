using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Warehouse.Controls
{
    public class FlexiblePanel : Panel, IAddChild
    {
        #region Const size value

        private const double _sm = 576;
        private const double _md = 720;
        private const double _lg = 960;
        private const double _xl = 1140;

        #endregion

        #region Properties

        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        #endregion

        #region Dependency 

        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register(nameof(ItemHeight), typeof(double), typeof(FlexiblePanel), new PropertyMetadata(-1d));

        #endregion


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
            double x = 0;
            double y = 0;
            double maxHeight = 0;
            double finalSizeWidth = 0;

            foreach (UIElement child in InternalChildren)
            {
                double childAvailableWidth = double.IsInfinity(availableSize.Width) ? double.PositiveInfinity : availableSize.Width;
                double childAvailableHeight = double.IsInfinity(availableSize.Height) ? double.PositiveInfinity : availableSize.Height;

                child.Measure(new Size(childAvailableWidth, childAvailableHeight));

                Size siz = Getsize(child, availableSize);

                if (x + siz.Width > availableSize.Width)
                {
                    x = 0;
                    y += maxHeight;
                    maxHeight = 0;
                }

                double childHeight = ItemHeight > 0 ? ItemHeight : child.DesiredSize.Height;

                x += siz.Width;
                maxHeight = Math.Max(maxHeight, childHeight);
                finalSizeWidth = Math.Max(finalSizeWidth, x);
            }

            double finalSizeHeight = y + maxHeight;

            finalSizeWidth = double.IsInfinity(availableSize.Width) ? finalSizeWidth : Math.Min(finalSizeWidth, availableSize.Width);
            finalSizeHeight = double.IsInfinity(availableSize.Height) ? finalSizeHeight : Math.Min(finalSizeHeight, availableSize.Height);

            return new Size(finalSizeWidth, finalSizeHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double x = 0;
            double y = 0;
            double maxHeight = 0;

            double finalSizeHeight = 0;
            double finalSizeWidth = 0;
            foreach (UIElement child in InternalChildren)
            {
                Size siz = Getsize(child, finalSize);
                if (x + siz.Width > finalSize.Width)
                {
                    finalSizeHeight += maxHeight;
                    x = 0;
                    y += maxHeight;
                    maxHeight = 0;
                }

                double childHeight = ItemHeight > 0 ? ItemHeight : child.DesiredSize.Height;

                child.Arrange(new Rect(new Point(x, y), new Size(siz.Width, childHeight)));
                x += siz.Width;
                maxHeight = Math.Max(maxHeight, childHeight);
                finalSizeWidth = Math.Max(finalSizeWidth, x);
            }

            finalSizeHeight += maxHeight;

            return finalSize;
        }

        protected Size Getsize(UIElement child, Size finalSize)
        {
            Func<int,Size> result = (size) =>
            {
                return new Size((size * finalSize.Width) / 12, child.DesiredSize.Height);
            };
            int size = 12;

            if (child is FlexibleItem item)
            {
                if(finalSize.Width > _xl)
                {
                    for (UnitWidthType i = UnitWidthType.XL; i >= UnitWidthType.SM; i--)
                    {
                        size = FlexibleItem.GetSize(item, i);
                        if(size != 0) 
                            return result.Invoke(size);
                    }
                }

                if (finalSize.Width > _lg)
                {
                    for (UnitWidthType i = UnitWidthType.LG; i >= UnitWidthType.SM; i--)
                    {
                        size = FlexibleItem.GetSize(item, i);
                        if (size != 0)
                            return result.Invoke(size);
                    }
                }

                if (finalSize.Width > _md)
                {
                    for (UnitWidthType i = UnitWidthType.MD; i >= UnitWidthType.SM; i--)
                    {
                        size = FlexibleItem.GetSize(item, i);
                        if (size != 0)
                            return result.Invoke(size);
                    }
                }

                if (finalSize.Width > _sm)
                {
                    for (UnitWidthType i = UnitWidthType.SM; i >= UnitWidthType.SM; i--)
                    {
                        size = FlexibleItem.GetSize(item, i);
                        if (size != 0)
                            return result.Invoke(size);
                    }
                }
                
            }
            return new Size((size * finalSize.Width) / 12, child.DesiredSize.Height);

        }

    }

    public class FlexibleItem : Decorator, IAddChild
    {
        #region Properties

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

        public FlexUnitType XLSize
        {
            get { return (FlexUnitType)GetValue(XLSizeProperty); }
            set { SetValue(XLSizeProperty, value); }
        }

        public UnitWidthType SizeType
        {
            get { return (UnitWidthType)GetValue(SizeTypeProperty); }
            set { SetValue(SizeTypeProperty, value); }
        }

        #endregion

        #region Dependency 

        public static readonly DependencyProperty SMSizeProperty =
            DependencyProperty.Register(nameof(SMSize), typeof(FlexUnitType), typeof(FlexibleItem), new PropertyMetadata(FlexUnitType.All, propSizeChange));

        public static readonly DependencyProperty MDSizeProperty =
            DependencyProperty.Register(nameof(MDSize), typeof(FlexUnitType), typeof(FlexibleItem), new PropertyMetadata(FlexUnitType.All, propSizeChange));

        public static readonly DependencyProperty LGSizeProperty =
           DependencyProperty.Register(nameof(LGSize), typeof(FlexUnitType), typeof(FlexibleItem), new PropertyMetadata(FlexUnitType.All, propSizeChange));

        public static readonly DependencyProperty XLSizeProperty =
            DependencyProperty.Register(nameof(XLSize), typeof(FlexUnitType), typeof(FlexibleItem), new PropertyMetadata(FlexUnitType.All, propSizeChange));

        public static readonly DependencyProperty SizeTypeProperty =
            DependencyProperty.Register(nameof(SizeType), typeof(UnitWidthType), typeof(FlexibleItem), new PropertyMetadata(UnitWidthType.XL));

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public FlexibleItem()
        {

        }

        #endregion

        protected override Size MeasureOverride(Size constraint)
        {
            // Ustaw maksymalną wysokość dziecka na wysokość kontrolki
            

            // Kontynuuj standardową procedurę pomiaru
            Size baseSize = base.MeasureOverride(constraint);

            if (Child is FrameworkElement childElement && Parent is FlexiblePanel fp)
            {
                if(fp.ItemHeight > 0)
                    childElement.MaxHeight = fp.ItemHeight;
            }

            // ... (dodatkowy kod pomiaru, jeśli jest wymagany)

            return baseSize;
        }

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
            element.SizeType = widthType;
            FlexUnitType type = FlexUnitType.Unset;
            switch (widthType)
            {
                case UnitWidthType.SM:
                    type = (FlexUnitType)element.GetValue(SMSizeProperty);
                    break;
                case UnitWidthType.MD:
                    type = (FlexUnitType)element.GetValue(MDSizeProperty);
                    break;
                case UnitWidthType.LG:
                    type = (FlexUnitType)element.GetValue(LGSizeProperty);
                    break;
                case UnitWidthType.XL:
                    type = (FlexUnitType)element.GetValue(XLSizeProperty);
                    break;
            }
            return (int)type;
        }
    }

    public enum UnitWidthType
    {
        SM = 0,
        MD = 1,
        LG = 2,
        XL = 3,
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