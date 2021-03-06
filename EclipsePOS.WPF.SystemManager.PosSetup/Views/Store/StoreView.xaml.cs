﻿using System;
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
using System.Collections;
using EclipsePOS.WPF.SystemManager.PosSetup.Util;

using EclipsePOS.WPF.SystemManager.Infrastructure.Constants;


namespace EclipsePOS.WPF.SystemManager.PosSetup.Views.Store
{
    /// <summary>
    /// Interaction logic for StoreView.xaml
    /// </summary>
    public partial class StoreView : UserControl, IStoreView
    {
        private StoreViewPresenter _presenter;

        public StoreView()
        {
            InitializeComponent();
        }

        public StoreView(StoreViewPresenter presenter):this()
        {
            this._presenter = presenter;
            this._presenter.View = this;
            
            this.Loaded += new RoutedEventHandler(StoreView_Loaded);
           


            this.rootControl.SizeChanged += new SizeChangedEventHandler(rootControl_SizeChanged);
            this.cmbBoxOrganizationID.SelectionChanged += new SelectionChangedEventHandler(cmbBoxOrganizationID_SelectionChanged);
          //  this.cmbBoxOrganizationID.SelectedValue = PosSettings.Default.Organization;
           
          //  this.txtBoxStoreNo.PreviewTextInput += new TextCompositionEventHandler(txtBoxStoreNo_PreviewTextInput);
        }

        
        

       

        void cmbBoxOrganizationID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                this._presenter.FilterStoreByOrganizationNo(this.cmbBoxOrganizationID.SelectedValue.ToString());
                this._presenter.FilterCustomerByOrganizationNo(this.cmbBoxOrganizationID.SelectedValue.ToString());
            }
            catch
            {
                this._presenter.FilterStoreByOrganizationNo(PosSettings.Default.Organization);
                this._presenter.FilterCustomerByOrganizationNo(PosSettings.Default.Organization);
            }
           
        }

        void rootControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.rootControl.Height = Math.Ceiling(Application.Current.MainWindow.ActualHeight * 0.82); 
        }

        void StoreView_Loaded(object sender, RoutedEventArgs e)
        {
            this.LoadResources();

            _presenter.OnShowStore();

            this.cmbBoxOrganizationID.SelectedValue = PosSettings.Default.Organization;


        }

        private void LoadResources()
        {
            base.Resources.MergedDictionaries.Add((ResourceDictionary)Application.LoadComponent(new Uri(@"EclipsePOS.WPF.SystemManager.Infrastructure;;;component/Skins/BaseSkin.xaml", UriKind.Relative)));
        }


        #region Events

        public void OnStoreListView_SelectionChanged(object sender, RoutedEventArgs e)
        {

            object selectedItem = ((ListView)e.Source).SelectedItem;

        }


        #endregion

        #region IDepartmentView implementations

        public void SetStoreDataContext(object data)
        {
            this.storeListView.DataContext = data;
            this.DataContext = data;
        }



        public void SetSelectedItemCursor()
        {
            storeListView.ScrollIntoView(storeListView.Items.CurrentItem);
            storeListView.SelectedItem = storeListView.Items.CurrentItem;
        }

        public void SetMoveToFirstBtnDataContext(object command)
        {
            this.btnMoveFirst.DataContext = command;
        }

        public void SetMoveToPreviousBtnDataContext(object command)
        {
            this.btnMovePrevious.DataContext = command;
        }

        public void SetMoveToNextBtnDataContext(object command)
        {
            this.btnMoveNext.DataContext = command;
        }

        public void SetMoveToLastBtnDataContext(object command)
        {
            this.btnMoveLast.DataContext = command;
        }


        public void SetDeleteBtnDataContext(object command)
        {
            this.btnDelete.DataContext = command;
        }

        public void SetAddBtnDataContext(object command)
        {
            this.btnAdd.DataContext = command;
        }

        public void SetRevertBtnDataContext(object command)
        {
            this.btnRevert.DataContext = command;
        }

        public void SetSaveBtnDataContext(object command)
        {
            this.btnSave.DataContext = command;
        }

        #endregion


        public void SetOrganizationDataContext(IEnumerable data)
        {
            this.cmbBoxOrganizationID.ItemsSource = data;
        }

        public void SetCustomerDataContext(IEnumerable data)
        {
            this.cmbBoxCustomer.ItemsSource = data;
        }

        public string SelectedOrganizationNo()
        {
            string orgID = null;

            try
            {
                orgID = this.cmbBoxOrganizationID.SelectedValue.ToString();
            }
            catch
            {
            }
            return orgID;
        }



        private bool AreAllValidNumericChars(string str)
        {
            bool ret = true;

            int l = str.Length;
            for (int i = 0; i < l; i++)
            {
                char ch = str[i];
                ret &= Char.IsDigit(ch);
            }

            if (!ret) Commands.Beep(500, 50);
            return ret;
        }

        public void SetFocusToFirstElement()
        {
            this.txtBoxStoreNo.Focus();
        }

        public void SetColumnsEnabled(bool flag)
        {

            int count = VisualTreeHelper.GetChildrenCount(this.dataGrid);
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    UIElement child = (UIElement)VisualTreeHelper.GetChild(this.dataGrid, i);
                    if (child is TextBox)
                    {
                        
                        ((TextBox)child).IsEnabled = flag;
                    }
                    if (child is ComboBox)
                    {
                        ((ComboBox)child).IsEnabled = flag;
                    }
                }
            }


        }



    }
}
