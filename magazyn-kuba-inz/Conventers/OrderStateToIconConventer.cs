using Warehouse.Theme.Conventers;
using System;
using System.Globalization;
using System.Windows;
using Warehouse.Models.Enums;

namespace Warehouse.Conventers;

/// <summary>
/// Base value conventer that allows direct XMAL usage
/// </summary>
/// <typeparam name="T">The type of this value conventer </typeparam>
public class OrderStateToIconConventer : BaseValueConventer<OrderStateToIconConventer>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is EOrderState state)
        {
            switch(state)
            {
                case EOrderState.Created:
                    return Application.Current.TryFindResource("OrderCreated");
                case EOrderState.Prepared:
                    return Application.Current.TryFindResource("OrderPrepared");
                case EOrderState.Reserved:
                    return Application.Current.TryFindResource("OrderReserved");
                case EOrderState.DeliveryCreated:
                    return Application.Current.TryFindResource("Delivery");
                case EOrderState.DeliveryPrepared:
                    return Application.Current.TryFindResource("DeliveryPrepared");
                case EOrderState.Received:
                    return Application.Current.TryFindResource("Forklift");
                case EOrderState.Finish:
                    return Application.Current.TryFindResource("OrderFinish"); 
                default:
                    return Application.Current.TryFindResource($"Order{state.ToString()}");
            }
        }
        return value;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }

}

public class OrderStateToColorConventer : BaseValueConventer<OrderStateToColorConventer>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is EOrderState state)
        {
            switch (state)
            {
                case EOrderState.Created:
                case EOrderState.DeliveryCreated:
                    return Application.Current.TryFindResource("OrderCreatedColorBrush");
                case EOrderState.Prepared:
                case EOrderState.DeliveryPrepared:
                    return Application.Current.TryFindResource("OrderPreparedColorBrush");
                case EOrderState.Received:
                    return Application.Current.TryFindResource("OrderReceivedColorBrush");
                case EOrderState.Reserved:
                    return Application.Current.TryFindResource("OrderReservedColorBrush");
                case EOrderState.Finish:
                    return Application.Current.TryFindResource("GoodColorBrush");
                default:
                    return Application.Current.TryFindResource($"Order{state.ToString()}ColorBrush");
            }
        }
        return value;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }

}
