using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

using System.Collections.ObjectModel;

using DashboardDataAccess;

namespace VisualizationDashboard
{
    /// <summary>
    /// Interaction logic for ConfigDisplayInfoControl.xaml
    /// </summary>
    public partial class ConfigDisplayInfoControl : UserControl, INotifyPropertyChanged
    {
        static private ConfigDataModel configDataModel = null;

        public static ConfigDataModel ConfigDataModel
        {
            get { return ConfigDisplayInfoControl.configDataModel; }
            set { ConfigDisplayInfoControl.configDataModel = value; }
        }

        private ConfigDisplay configDisplayData = null;

        private ObservableEntitySetWrapper<DisplayFactor> displayFactorsWrapped = null;

        public ObservableEntitySetWrapper<DisplayFactor> DisplayFactorsWrapped
        {
            get { return displayFactorsWrapped; }
            set { displayFactorsWrapped = value; OnPropertyChanged("DisplayFactorsWrapped"); }
        }

        private ObservableEntitySetWrapper<DisplayBlockedFactor> displayBlockedFactorsWrapped = null;

        public ObservableEntitySetWrapper<DisplayBlockedFactor> DisplayBlockedFactorsWrapped
        {
            get { return displayBlockedFactorsWrapped; }
            set { displayBlockedFactorsWrapped = value; OnPropertyChanged("DisplayBlockedFactorsWrapped"); }
        }

        public ConfigDisplay ConfigDisplayData
        {
            get { return configDisplayData; }
            set
            {
                configDisplayData = value;

                displayFactorsWrapped = new ObservableEntitySetWrapper<DisplayFactor>(configDisplayData.DisplayFactors);
                factorsLB.ItemsSource = DisplayFactorsWrapped;
                factorsLB.Items.SortDescriptions.Add(new SortDescription("FactorPos", ListSortDirection.Ascending));

                displayBlockedFactorsWrapped = new ObservableEntitySetWrapper<DisplayBlockedFactor>(configDisplayData.DisplayBlockedFactors);
                blockedFactorsLB.ItemsSource = displayBlockedFactorsWrapped;
                blockedFactorsLB.Items.SortDescriptions.Add(new SortDescription("LevelName", ListSortDirection.Ascending));

                grabHandle.DataContext = this;

                DataContext = configDisplayData;

                OnPropertyChanged("ConfigDisplayData");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private Point _startPoint;
        private bool _isDragging;

        public bool IsDragging
        {
            get { return _isDragging; }
            set { _isDragging = value; }
        }

        DragAdorner _adorner = null;
        AdornerLayer _layer;

        public ConfigDisplayInfoControl()
        {
            InitializeComponent();

            grabHandle.DataContext = this;
        }

        public void RefreshDisplay()
        {
            OnPropertyChanged("");
        }

        void GrabHandle_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !IsDragging)
            {
                Point position = e.GetPosition(null);

                if (Math.Abs(position.X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(position.Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    //StartDrag(e);
                    //  StartDragCustomCursor(e);
                    // StartDragWindow(e);
                    StartDragInProcAdorner(e);

                }
            }
        }

        void GrabHandle_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(null);
        }

        private void StartDrag(MouseEventArgs e)
        {
            IsDragging = true;
            DataObject data = new DataObject(System.Windows.DataFormats.Text.ToString(), "abcd");
            DragDropEffects de = DragDrop.DoDragDrop(this.grabHandle, data, DragDropEffects.Move);
            IsDragging = false;
        }

        private void ConfigDisplayInfoControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.grabHandle.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(GrabHandle_PreviewMouseLeftButtonDown);
            this.grabHandle.PreviewMouseMove += new MouseEventHandler(GrabHandle_PreviewMouseMove);
        }

        FrameworkElement _dragScope;
        public FrameworkElement DragScope
        {
            get { return _dragScope; }
            set { _dragScope = value; }
        }

        private void StartDragInProcAdorner(MouseEventArgs e)
        {

            // Let's define our DragScope .. In this case it is every thing inside our main window .. 
            DragScope = Application.Current.MainWindow.Content as FrameworkElement;
            System.Diagnostics.Debug.Assert(DragScope != null);

            // We enable Drag & Drop in our scope ...  We are not implementing Drop, so it is OK, but this allows us to get DragOver 
            bool previousDrop = DragScope.AllowDrop;
            DragScope.AllowDrop = true;

            // Let's wire our usual events.. 
            // GiveFeedback just tells it to use no standard cursors..  

            GiveFeedbackEventHandler feedbackhandler = new GiveFeedbackEventHandler(GrabHandle_GiveFeedback);
            this.grabHandle.GiveFeedback += feedbackhandler;

            // The DragOver event ... 
            DragEventHandler draghandler = new DragEventHandler(Window1_DragOver);
            DragScope.PreviewDragOver += draghandler;

            // Drag Leave is optional, but write up explains why I like it .. 
            DragEventHandler dragleavehandler = new DragEventHandler(DragScope_DragLeave);
            DragScope.DragLeave += dragleavehandler;

            // QueryContinue Drag goes with drag leave... 
            QueryContinueDragEventHandler queryhandler = new QueryContinueDragEventHandler(DragScope_QueryContinueDrag);
            DragScope.QueryContinueDrag += queryhandler;

            //Here we create our adorner.. 
            _adorner = new DragAdorner(DragScope, (UIElement)this, true, 0.5, 0.5);
            _layer = AdornerLayer.GetAdornerLayer(DragScope as Visual);
            _layer.Add(_adorner);


            IsDragging = true;
            _dragHasLeftScope = false;

            //Finally lets drag drop 
            DragDropEffects de = DragDrop.DoDragDrop(this.grabHandle, ConfigDisplayData, DragDropEffects.Move);

            // Clean up our mess :) 
            DragScope.AllowDrop = previousDrop;
            AdornerLayer.GetAdornerLayer(DragScope).Remove(_adorner);
            _adorner = null;

            grabHandle.GiveFeedback -= feedbackhandler;
            DragScope.DragLeave -= dragleavehandler;
            DragScope.QueryContinueDrag -= queryhandler;
            DragScope.PreviewDragOver -= draghandler;

            IsDragging = false;
        }

        void GrabHandle_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("GrabHandle_GiveFeedback " + e.Effects.ToString());

            e.UseDefaultCursors = false;
            e.Handled = true;

        }

        private bool _dragHasLeftScope = false;
        void DragScope_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (this._dragHasLeftScope)
            {
                e.Action = DragAction.Cancel;
                e.Handled = true;
            }

        }


        void DragScope_DragLeave(object sender, DragEventArgs e)
        {
            if (e.OriginalSource == DragScope)
            {
                Point p = e.GetPosition(DragScope);
                Rect r = VisualTreeHelper.GetContentBounds(DragScope);
                if (!r.Contains(p))
                {
                    this._dragHasLeftScope = true;
                    e.Handled = true;
                }
            }

        }

        void Window1_DragOver(object sender, DragEventArgs args)
        {
            if (_adorner != null)
            {
                _adorner.LeftOffset = args.GetPosition(DragScope).X /* - _startPoint.X */ ;
                _adorner.TopOffset = args.GetPosition(DragScope).Y /* - _startPoint.Y */ ;
            }
        }

        private void DragThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (this.Width + e.HorizontalChange > 0)
            {
                this.Width += e.HorizontalChange;
            }

            if (this.Height + e.VerticalChange > 0)
            {
                this.Height += e.VerticalChange;
            }
        }

        private void DragThumb_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            ConfigDisplay configDisplay = (ConfigDisplay)this.DataContext;

            if ((configDataModel == null) || (configDisplay == null) || (configDisplay.ConfigID <= 0) ||
                (configDisplay.Name == null) || (configDisplay.Name.Length <= 0))
            {
                return;
            }

            configDataModel.UpdateConfigDisplay(configDisplay, (int) this.Width, (int) this.Height);
        }

    }
}
