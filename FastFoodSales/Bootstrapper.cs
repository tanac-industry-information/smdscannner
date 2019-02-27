using System;
using Stylet;
using StyletIoC;
using System.IO.Ports;
using DAQ.Service;
using System.Threading.Tasks;
using DAQ.Pages;

namespace DAQ
{
    public class Bootstrapper : Bootstrapper<MainWindowViewModel>
    {
        protected override void ConfigureIoC(IStyletIoCBuilder builder)//2
        {
            // Configure the IoC container in here
            builder.Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();

            builder.Bind<HomeViewModel>().ToSelf().InSingletonScope();
            builder.Bind<MsgViewModel>().ToSelf().InSingletonScope();
            builder.Bind<SettingsViewModel>().ToSelf().InSingletonScope();
            builder.Bind<MsgDBSaver>().ToSelf().InSingletonScope();
            builder.Bind<MsgFileSaver<TestSpecViewModel>>().ToSelf().InSingletonScope();
            builder.Bind<PLCViewModel>().ToSelf();
            builder.Bind<IPLCService>().To<ModbusService>();
            builder.Bind<OEEViewModel>().ToSelf();
            builder.Bind<DataStore>().ToSelf().InSingletonScope();
            builder.Bind<MaterialViewModel>()
                    .ToFactory(x => new MaterialViewModel(Container.Get<IEventAggregator>(), Container.Get<ScannerService>(), 1, 12) { DisplayName="N1"})
                    .WithKey("N1").InSingletonScope();
            builder.Bind<MaterialViewModel>()
                    .ToFactory(x => new MaterialViewModel(Container.Get<IEventAggregator>(), Container.Get<ScannerService>(), 2, 12) { DisplayName = "N2" })
                    .WithKey("N2").InSingletonScope();
            builder.Bind<MaterialViewModel>()
                    .ToFactory(x => new MaterialViewModel(Container.Get<IEventAggregator>(), Container.Get<ScannerService>(), 3, 12) { DisplayName = "N3" })
                    .WithKey("N3").InSingletonScope();
            builder.Bind<MaterialViewModel>()
                    .ToFactory(x => new MaterialViewModel(Container.Get<IEventAggregator>(), Container.Get<ScannerService>(), 4, 12) { DisplayName = "N4" })
                    .WithKey("N4").InSingletonScope();
            builder.Bind<MaterialViewModel>()
                    .ToFactory(x => new MaterialViewModel(Container.Get<IEventAggregator>(), Container.Get<ScannerService>(), 5, 12) { DisplayName = "N5" })
                    .WithKey("N5").InSingletonScope();
            builder.Bind<MaterialViewModel>()
                    .ToFactory(x => new MaterialViewModel(Container.Get<IEventAggregator>(), Container.Get<ScannerService>(), 6, 12) { DisplayName = "N6" })
                    .WithKey("N6").InSingletonScope();
            builder.Bind<MaterialViewModel>()
                    .ToFactory(x => new MaterialViewModel(Container.Get<IEventAggregator>(), Container.Get<ScannerService>(), 7, 12) { DisplayName = "N7" })
                    .WithKey("N7").InSingletonScope();
            builder.Bind<MaterialViewModel>()
                    .ToFactory(x => new MaterialViewModel(Container.Get<IEventAggregator>(), Container.Get<ScannerService>(), 8, 12) { DisplayName = "N8" })
                    .WithKey("N8").InSingletonScope();
            builder.Bind<MaterialViewModel>()
                    .ToFactory(x => new MaterialViewModel(Container.Get<IEventAggregator>(), Container.Get<ScannerService>(), 9, 12) { DisplayName = "N9" })
                    .WithKey("N9").InSingletonScope();
            builder.Bind<MaterialViewModel>()
                    .ToFactory(x => new MaterialViewModel(Container.Get<IEventAggregator>(), Container.Get<ScannerService>(), 10, 12) { DisplayName = "N10" })
                    .WithKey("N10").InSingletonScope();
            builder.Bind<MaterialViewModel>()
                    .ToFactory(x => new MaterialViewModel(Container.Get<IEventAggregator>(), Container.Get<ScannerService>(), 11, 12) { DisplayName = "N11" })
                    .WithKey("N11").InSingletonScope();
            builder.Bind<MaterialViewModel>()
                    .ToFactory(x => new MaterialViewModel(Container.Get<IEventAggregator>(), Container.Get<ScannerService>(), 12, 12) { DisplayName = "N12" })
                    .WithKey("N12").InSingletonScope();
            builder.Bind<Info>().ToSelf().InSingletonScope();
            builder.Autobind();
        }

        protected override void Configure()//3
        {

            // Perform any other configuration before the application starts
        }
        protected override void OnStart()//1
        {

            base.OnStart();
        }
        protected override void OnLaunch()//4
        {

            base.OnLaunch();
        }

    }
}
