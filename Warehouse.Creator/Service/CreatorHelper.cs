using Microsoft.Extensions.DependencyInjection;
using Warehouse.Core.Interface;
using Warehouse.Helper;

namespace Warehouse.Creator.Service
{
    public class CreatorHelper
    {
        /// <summary>
        /// Singleton instance fo the lacator
        /// </summary>
        public static CreatorHelper Instance { get; private set; } = new CreatorHelper();

        /// <summary>
        /// The aplication view model
        /// </summary>
        public static IServiceProvider ServiceProvider => App.AppHost?.Services;

        /// <summary>
        /// The Inner dialog service
        /// </summary>
        public static IInnerDialogService InnerDialog => App.AppHost.Services.GetRequiredService<IInnerDialogService>();

        /// <summary>
        /// Message service
        /// </summary>
        public static IMessageService MessageService => App.AppHost.Services.GetRequiredService<IMessageService>();
    }
}