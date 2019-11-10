using System;
using System.Collections.Immutable;
using Stylet;
using StyletIoC;
using System.IO.Ports;
using System.Net;
using DAQ.Service;
using System.Threading.Tasks;
using System.Windows.Threading;
using DAQ.Pages;
using HslCommunication.Core;
using HslCommunication.ModBus;

namespace DAQ
{
    public class Bootstrapper : Bootstrapper<MainWindowViewModel>
    {
        protected override void ConfigureIoC(IStyletIoCBuilder builder)//2
        {
            // Configure the IoC container in here
            builder.Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();
            builder.Bind<OEEViewModel>().ToSelf();
           // builder.Bind<MsgFileSaver<TestSpecViewModel>>().ToSelf().InSingletonScope();
            builder.Bind<MsgFileSaver<Scan>>().ToFactory((x) => new  MsgFileSaver<Scan>
            {
           //     RootFolder = @"\\10.101.30.5\SumidaFile\Monitor",
           //     SubPath = "Line01_N3"
            }).InSingletonScope();
            builder.Bind<MsgFileSaver<Laser>>().ToFactory((x) => new MsgFileSaver<Laser>
            {
           //     RootFolder = @"\\10.101.30.5\SumidaFile\Monitor",
           //     SubPath = "Line01_Laser"
            }).InSingletonScope();
            builder.Bind<DataStorage>().ToSelf().InSingletonScope();
            builder.Bind<ScannerService>().ToSelf().InSingletonScope();
            builder.Bind<LaserService>().ToSelf().InSingletonScope();
            builder.Bind<IMainTabViewModel>().ToAllImplementations().InSingletonScope();
            builder.Bind<Info>().ToSelf().InSingletonScope();
            builder.Bind<IByteTransform>().ToFactory(x => new ReverseWordTransform()
            {
                DataFormat = DataFormat.CDAB,
            });
            builder.Bind<IReadWriteNet>().ToFactory(x => new ModbusTcpNet("127.0.0.1")
            {
                AddressStartWithZero = false,
                DataFormat = DataFormat.CDAB,
                IsStringReverse = false
            });
            builder.Autobind();
        }

        protected override void OnUnhandledException(DispatcherUnhandledExceptionEventArgs e)
        {
            Container.Get<IWindowManager>()
                .ShowMessageBox(e.Exception.Message + Environment.NewLine + e.Exception.StackTrace);
            e.Handled = true;
            base.OnUnhandledException(e);
        }

        protected override void Configure()//3
        {
            Container.Get<MsgFileSaver<Laser>>().ProcessError += (s, e) =>
            {
                Container.Get<IEventAggregator>().PostError(e);
            };
            Container.Get<MsgFileSaver<Scan>>().ProcessError += (s, e) =>
            {
                Container.Get<IEventAggregator>().PostError(e);
            };
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
