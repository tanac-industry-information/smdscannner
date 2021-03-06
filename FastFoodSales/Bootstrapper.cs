using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using Stylet;
using StyletIoC;
using System.IO.Ports;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using DAQ.Service;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using DAQ.Pages;
using HslCommunication.Core;
using HslCommunication.ModBus;
using DataFormat = HslCommunication.Core.DataFormat;

namespace DAQ
{
    public class CheckRun
    {
        /// </summary>  
        /// <param name="hWnd">窗口句柄</param>  
        /// <param name="cmdShow">指定窗口如何显示。查看允许值列表，请查阅ShowWlndow函数的说明部分</param>  
        /// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零</returns>  
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        /// <summary>  
        ///  该函数将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。  
        ///  系统给创建前台窗口的线程分配的权限稍高于其他线程。   
        /// </summary>  
        /// <param name="hWnd">将被激活并被调入前台的窗口句柄</param>  
        /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零</returns>  
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        /// 窗口是否已最小化        
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool OpenIcon(IntPtr hWnd);

        //相关常量        
        private const int SW_HIDE = 0;             //隐藏窗口，活动状态给另一个窗口       
        private const int SW_SHOWNORMAL = 1;       //用原来的大小和位置显示一个窗口，同时令其进入活动状态  
        private const int SW_SHOWMINIMIZED = 2;    //最小化窗口，并将其激活       
        private const int SW_SHOWMAXIMIZED = 3;    //最大化窗口，并将其激活      
        private const int SW_SHOWNOACTIVATE = 4;   //用最近的大小和位置显示一个窗口，同时不改变活动窗口      
        private const int SW_RESTORE = 9;          //用原来的大小和位置显示一个窗口，同时令其进入活动状态          
        private const int SW_SHOWDEFAULT = 10;     //根据默认 创建窗口时的样式 来显示  
        public static void HandleRunningInstance(Process instance)
        {
            IntPtr hWnd = instance.MainWindowHandle;
            bool isIcon = IsIconic(hWnd);             // 窗口是否已最小化       
            if (isIcon)
            {
                // 还原窗口           
                ShowWindowAsync(hWnd, SW_RESTORE);
            }
            else
            {
                OpenIcon(hWnd);
                //如果期望窗口显示为Normal模式，可先做如下设置      
                ShowWindowAsync(hWnd, 10);
                // 将窗口设为前台窗口             
                SetForegroundWindow(hWnd);
            }
        }


        public Process IsProcessStarted(string processName)
        {
            Process[] temp = Process.GetProcessesByName(processName);
            if (temp.Length > 0)
            {
                return temp[0];
            }
            else
                return null;
        }
    }

    public class Bootstrapper : Bootstrapper<MainWindowViewModel>
    {
        protected override void ConfigureIoC(IStyletIoCBuilder builder)//2
        {
            // Configure the IoC container in here
            builder.Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();
           // builder.Bind<MsgFileSaver<TestSpecViewModel>>().ToSelf().InSingletonScope();
      
            builder.Bind<ScannerService>().ToSelf().InSingletonScope();
            builder.Bind<LaserService>().ToSelf().InSingletonScope();
            builder.Bind<MaterialManager>().ToFactory(x => MaterialManager.Load()).InSingletonScope();
            builder.Bind<MaterialManagerViewModel>().ToSelf().InSingletonScope();
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
            builder.Bind<IIoService>().To<IoService>().InSingletonScope();
            builder.Autobind();
        }

        protected override void OnUnhandledException(DispatcherUnhandledExceptionEventArgs e)
        {
            Container.Get<IWindowManager>()
                .ShowMessageBox(e.Exception.StackTrace);
             e.Handled = true;
        }
     

        protected override void Configure()//3
        {
            // Perform any other configuration before the application starts
        }

        private Mutex hMutex;
        protected override void OnStart()//1
        {
            bool flag;
            hMutex = new Mutex(true, "TANAC.DAQ", out flag);
            if (!flag)
            {
                MessageBox.Show("当前程序已在运行，请勿重复运行。");
                Environment.Exit(1);//退出程序 
            }
            base.OnStart();
        }
        protected override void OnLaunch()//4
        {

            base.OnLaunch();
        }

    }
}
