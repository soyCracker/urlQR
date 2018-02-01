using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using ZXing;

// 空白頁項目範本已記錄在 https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x404

namespace urlQR
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public bool pipMode = false;

        public MainPage()
        {
            this.InitializeComponent();

            //取得剪貼簿的值
            Clipboard.ContentChanged += async (s, e) =>
            {
                DataPackageView dataPackageView = Clipboard.GetContent();
                if (dataPackageView.Contains(StandardDataFormats.Text))
                {
                    string urlText = await dataPackageView.GetTextAsync();
                    UrlTb.Text = urlText;
                    if(pipMode)
                    {
                        getQRcode();
                    }
                }
            };
        }

        public void getQRcode()
        {
            BarcodeWriter bw = new BarcodeWriter();
            bw.Format = BarcodeFormat.QR_CODE;
            bw.Options.Width = 200;
            bw.Options.Height = 200;
            if (!string.IsNullOrEmpty(UrlTb.Text))
            {
                WriteableBitmap WBitmap = bw.Write(UrlTb.Text);
                QRcodeImg.Source = WBitmap;
            }
        }

        private void GeneQrBtn_Click(object sender, RoutedEventArgs e)
        {
            getQRcode();
        }

        private async void PipButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationView.GetForCurrentView().IsViewModeSupported(ApplicationViewMode.CompactOverlay))
            {
                if(!pipMode)
                {
                    ViewModePreferences compactOptions = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
                    compactOptions.CustomSize = new Size(200, 200);
                    await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay, compactOptions);
                    //UrlTb.Visibility = Visibility.Collapsed;
                    //GeneQrBtn.Visibility = Visibility.Collapsed;
                    pipMode = true;
                    PipButtonFont.Glyph = Char.ConvertFromUtf32(0xE92D);
                }
                else
                {
                    await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.Default);
                    //UrlTb.Visibility = Visibility.Visible;
                    //GeneQrBtn.Visibility = Visibility.Visible;
                    pipMode = false;
                    PipButtonFont.Glyph = Char.ConvertFromUtf32(0xE8AD);
                }
            }
        }
    }
}
