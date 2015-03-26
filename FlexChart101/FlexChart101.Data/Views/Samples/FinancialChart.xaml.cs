﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlexChartDemo.Data.Chart;
using Xamarin.Forms;
using Xuni.Xamarin.FlexChart;

namespace FlexChartDemo.Data.Views.Samples
{
    public partial class FinancialChart
    {
        public FinancialChart()
        {
            InitializeComponent();
            this.flexChart.ItemsSource = ChartSampleFactory.FinancialData();           
        }
    }
}
