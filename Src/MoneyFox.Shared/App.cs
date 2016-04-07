﻿using System.Linq;
using System.Reflection;
using MoneyFox.Shared.Authentication;
using MoneyFox.Shared.DataAccess;
using MoneyFox.Shared.Interfaces;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;

namespace MoneyFox.Shared
{
    public class App : MvxApplication
    {
        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            RegisterDependencies();

            // Start the app with the Main View Model.
            RegisterAppStart(new AppStart());

            Mvx.Resolve<IRecurringPaymentManager>().CheckRecurringPayments();
            Mvx.Resolve<IPaymentManager>().ClearPayments();
        }

        private void RegisterDependencies()
        {
            Mvx.RegisterType<ISqliteConnectionCreator, SqliteConnectionCreator>();
            Mvx.RegisterSingleton<IPasswordStorage>(new PasswordStorage(Mvx.Resolve<IProtectedData>()));
            Mvx.RegisterType(() => new Session());

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes(typeof (AccountDataAccess).GetTypeInfo().Assembly)
                .EndingWith("DataAccess")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            // Used for the settings data access who doesn't have an interface
            CreatableTypes(typeof (AccountDataAccess).GetTypeInfo().Assembly)
                .EndingWith("DataAccess")
                .AsTypes()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("Repository")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("Manager")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("ViewModel")
                .Where(x => !x.Name.StartsWith("DesignTime"))
                .AsInterfaces()
                .RegisterAsLazySingleton();


            CreatableTypes()
                .EndingWith("ViewModel")
                .AsTypes()
                .RegisterAsLazySingleton();
        }
    }
}