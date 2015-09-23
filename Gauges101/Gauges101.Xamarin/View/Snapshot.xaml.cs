﻿using Gauges101.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xuni.Forms.Core;
using Xuni.Forms.Gauge;

namespace Gauges101
{
    public partial class Snapshot : ContentPage
    {
        private Random _rand = new Random();
        private uint _stepDuration = 4000;

        public Snapshot()
        {
            InitializeComponent();
            Title = AppResources.ExportImageTitle;

            BindingContext = new SampleViewModel() { Value = 25, ShowText = GaugeShowText.All };
            snapshotFrameBorder.IsVisible = false;
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                snapshotFrameBorder.IsVisible = false;
            };
            snapshotFrameBorder.GestureRecognizers.Add(tapGestureRecognizer);

            //gauge.UpdateAnimation = null;
            gauge.IsAnimated = false;
        }

        async void OnSnapshotClicked(object sender, EventArgs e)
        {
            var image = gauge.GetImage();
            snapshot.Source = ImageSource.FromStream(() => new MemoryStream(image));
            await Task.Delay(100);
            snapshotFrameBorder.BackgroundColor = ColorEx.ThemeForegroundColor;
            snapshotFrame.BackgroundColor = ColorEx.ThemeBackgroundColor;
            snapshotFrameBorder.IsVisible = true;
        }

        async void OnSaveSnapshotClicked(object sender, EventArgs e)
        {
            //uses the IPicture interface to use the appropriate saving mechanism from the picture class in each individual project
            var originalBackground = gauge.BackgroundColor;
            gauge.BackgroundColor = ColorEx.ThemeBackgroundColor;
            DependencyService.Get<IPicture>().SavePictureToDisk("Gauge", gauge.GetImage());
            gauge.BackgroundColor = originalBackground;
            //generic success message
            await DisplayAlert("Image Saved",
                "The image has been saved to your device's picture album.",
                "OK");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(TimeSpan.FromMilliseconds(gauge.LoadAnimation.Duration));
            await AnimateNextStep();
        }

        private async Task AnimateNextStep()
        {
            var viewModel = BindingContext as SampleViewModel;
            double nextValue = _rand.Next((int)viewModel.Min, (int)viewModel.Max);
            var gaugeAnimation = new Animation(d => gauge.Value = d, gauge.Value, nextValue, Easing.CubicInOut);
            this.Animate("GaugeAnimation", gaugeAnimation, rate: 60, length: _stepDuration, finished: (d2, b) => AnimateNextStep(), repeat: () => false);
        }
    }
}