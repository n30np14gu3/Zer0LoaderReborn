using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Newtonsoft.Json;
using xNet;
using Zer0LoaderReborn.SDK.Cryptography;
using Zer0LoaderReborn.SDK.Win32;
using Path = System.Windows.Shapes.Path;

namespace Zer0LoaderReborn
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isDrag;
        private byte[] _dll = null;
        private int _csGoPid = 0;
        public MainWindow()
        {
            new AuthWindow().ShowDialog();
            InitializeComponent();
            LName.Content = $"Игра: {ClientData.Data.game_name}";
            LEndDate.Content = $"Дата окончания подписки: {ClientData.Data.end_date}";
        }

        private void DragHeader_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDrag = true;
            DragMove();
        }

        private void DragHeader_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDrag = false;
        }

        private void DragHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrag && WindowState == WindowState.Maximized)
            {
                _isDrag = false;
                WindowState = WindowState.Normal;
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }


        void downloadLibs()
        {
            using (HttpRequest request = new HttpRequest { IgnoreProtocolErrors = true })
            {
                RequestParams data = new RequestParams();
                data["access_token"] = ClientData.Data.access_token;
                data["game_id"] = ClientData.GameId;

               _dll = request.Post($"{ClientData.AppDomain}/api/download", data).ToBytes();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Process.GetProcessesByName(ClientData.Data.process_name.Replace(".exe", "")).Length == 0)
            {
                MessageBox.Show($"Запустите {ClientData.Data.game_name}");
                return;
            }

            _csGoPid = Process.GetProcessesByName(ClientData.Data.process_name.Replace(".exe", ""))[0].Id;
            downloadLibs();

            string tmpFile = "";
            try
            {
#if DEBUG
                if (MessageBox.Show("Use CE?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    goto ce_lbl;
#endif
                tmpFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"{System.IO.Path.GetRandomFileName()}.dll");
                File.WriteAllBytes(tmpFile, _dll);
                if (!Injector.ManualMapInject(_csGoPid, tmpFile, true))
                {
                    if (File.Exists(tmpFile))
                        File.Delete(tmpFile);
                    throw new Exception("Не удалось запустить чит");
                }

#if DEBUG
            ce_lbl:
                MessageBox.Show("PRESS AFTER INJECT");
#endif
            }
            catch (Exception ex)
            {
                if (File.Exists(tmpFile))
                    File.Delete(tmpFile);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Array.Clear(_dll, 0, _dll.Length);
                _dll = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            MessageBox.Show("Чит успешно запущен!");
        }
    }
}
