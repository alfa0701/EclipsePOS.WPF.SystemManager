﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows;


namespace EclipsePOS.WPF.SystemManager.Inventory.Views.Department
{
    public class DepartmentViewPresenter
    {
        private IDepartmentView _view;
        private CollectionView _colView;

        private EclipsePOS.WPF.SystemManager.Data.DepartmentDataSet deptData;
        private EclipsePOS.WPF.SystemManager.Data.organizationLookupDataSet organizationData;


        private EclipsePOS.WPF.SystemManager.Data.DepartmentDataSetTableAdapters.TableAdapterManager taManager = new EclipsePOS.WPF.SystemManager.Data.DepartmentDataSetTableAdapters.TableAdapterManager();

        public DelegateCommand<object> MoveToFirstCommand;
        public DelegateCommand<object> MoveToPreviousCommand;
        public DelegateCommand<object> MoveToNextCommand;
        public DelegateCommand<object> MoveToLastCommand;

        public DelegateCommand<object> DeleteCommand;
        public DelegateCommand<object> AddCommand;
        public DelegateCommand<object> RevertCommand;
        public DelegateCommand<object> SaveCommand;


        public DepartmentViewPresenter()
        {

            MoveToFirstCommand = new DelegateCommand<object>(OnMoveToFirstCommandExecute, OnMoveToFirstCommandCanExecute);
            MoveToPreviousCommand = new DelegateCommand<object>(OnMoveToPreviousCommandExecute, OnMoveToPreviousCommandCanExecute);
            MoveToNextCommand = new DelegateCommand<object>(OnMoveToNextCommandExecute, OnMoveToNextCommandCanExecute);
            MoveToLastCommand = new DelegateCommand<object>(OnMoveToLastCommandExecute, OnMoveToLastCommandCanExecute);

            DeleteCommand = new DelegateCommand<object>(OnDeleteCommandExecute, OnDeleteCommandCanExecute);
            AddCommand = new DelegateCommand<object>(OnAddCommandExecute, OnAddCommandCanExecute);
            RevertCommand = new DelegateCommand<object>(OnRevertCommandExecute, OnRevertCommandCanExecute);
            SaveCommand = new DelegateCommand<object>(OnSaveCommandExecute, OnSaveCommandCanExecute);
         
        }

                
        public IDepartmentView View
        {
            set
            {
                _view = value;
            }

            get
            {
                return _view;
            }
        }

        public void OnShowDepartment()
        {


            //Organization
            organizationData = new EclipsePOS.WPF.SystemManager.Data.organizationLookupDataSet();
            EclipsePOS.WPF.SystemManager.Data.organizationLookupDataSetTableAdapters.organizationTableAdapter organizationTa = new EclipsePOS.WPF.SystemManager.Data.organizationLookupDataSetTableAdapters.organizationTableAdapter();
            organizationTa.Fill(organizationData.organization);
            View.SetOrganizationDataContext(organizationData.organization);


            //Department
            deptData = new  EclipsePOS.WPF.SystemManager.Data.DepartmentDataSet();
            EclipsePOS.WPF.SystemManager.Data.DepartmentDataSetTableAdapters.departmentTableAdapter deptTa = new EclipsePOS.WPF.SystemManager.Data.DepartmentDataSetTableAdapters.departmentTableAdapter();
            deptTa.Fill(deptData.department);
            View.SetDeptDataContext(deptData.department);

            _colView = CollectionViewSource.GetDefaultView(deptData.department) as CollectionView;
            taManager.departmentTableAdapter = deptTa;
            //taManager..Connection = new con


            View.SetMoveToFirstBtnDataContext(MoveToFirstCommand);
            View.SetMoveToPreviousBtnDataContext(MoveToPreviousCommand);
            View.SetMoveToNextBtnDataContext(MoveToNextCommand);
            View.SetMoveToLastBtnDataContext(MoveToLastCommand);

            View.SetDeleteBtnDataContext(DeleteCommand);
            View.SetAddBtnDataContext(AddCommand);
            View.SetRevertBtnDataContext(RevertCommand);
            View.SetSaveBtnDataContext(SaveCommand);

            _colView.CurrentChanged += new EventHandler(_colView_CurrentChanged);
        }

        void _colView_CurrentChanged(object sender, EventArgs e)
        {
            if (_colView.IsEmpty || _colView.IsCurrentBeforeFirst || _colView.IsCurrentAfterLast)
            {
                View.SetColumnsEnabled(false);
            }
            else
            {
                View.SetColumnsEnabled(true);
            }
        }

        #region MoveToFirst Command

        public void OnMoveToFirstCommandExecute(object obj)
        {
            _colView.MoveCurrentToFirst();
            View.SetSelectedItemCursor();
        }

        public bool OnMoveToFirstCommandCanExecute(object obj)
        {
            // Implement business logic for myCommand enablement.
                return true;
        }

        #endregion 

        #region MoveToPrevioust Command

        public void OnMoveToPreviousCommandExecute(object obj)
        {
            _colView.MoveCurrentToPrevious();
            if (_colView.IsCurrentBeforeFirst) _colView.MoveCurrentToFirst();
           View.SetSelectedItemCursor();
        }

        public bool OnMoveToPreviousCommandCanExecute(object obj)
        {
            // Implement business logic for myCommand enablement.
            return true;
        }

        #endregion 

        #region MoveToNext Command

        public void OnMoveToNextCommandExecute(object obj)
        {
            _colView.MoveCurrentToNext();
            if (_colView.IsCurrentAfterLast) _colView.MoveCurrentToLast();
            View.SetSelectedItemCursor();
        }

        public bool OnMoveToNextCommandCanExecute(object obj)
        {
            // Implement business logic for myCommand enablement.
            return true;
        }

        #endregion 


        #region MoveToLast Command

        public void OnMoveToLastCommandExecute(object obj)
        {
            _colView.MoveCurrentToLast();
            View.SetSelectedItemCursor();
        }

        public bool OnMoveToLastCommandCanExecute(object obj)
        {
            // Implement business logic for myCommand enablement.
            return true;
        }

        #endregion 


        #region Delete Command

        public void OnDeleteCommandExecute(object obj)
        {
            try
            {
                System.Data.DataRow dataRow = ((System.Data.DataRowView)_colView.CurrentItem).Row;
                dataRow.Delete();
            }
            catch
            {
            }
      
        }

        public bool OnDeleteCommandCanExecute(object obj)
        {
            // Implement business logic for myCommand enablement.
            return true;
        }

        #endregion 


        #region Add Command

        public void OnAddCommandExecute(object obj)
        {
            if (!deptData.HasErrors)
            {

                EclipsePOS.WPF.SystemManager.Data.DepartmentDataSet.departmentRow dataRow = deptData.department.NewdepartmentRow();
                dataRow.organization_no  = this.View.SelectedOrganizationNo();
                dataRow.department_id = "";
                dataRow.department_name  = "";
               
                deptData.department.AdddepartmentRow(dataRow);

                _colView.MoveCurrentToLast();
                View.SetSelectedItemCursor();
                View.SetFocusToFirstInputField();
            }
       
        }

        public bool OnAddCommandCanExecute(object obj)
        {
            // Implement business logic for myCommand enablement.
            return true;
        }

        #endregion 


        #region Revert Command

        public void OnRevertCommandExecute(object obj)
        {
            if (deptData.HasChanges())
                //if (MessageBoxResult.Yes == MessageBox.Show("Are sure you want to loose all your changes", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                {
                    MessageBoxResult result = Microsoft.Windows.Controls.MessageBox.Show("Are sure you want to loose all your changes", "Revert Command", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        deptData.RejectChanges();
                    }
                }

        }

        public bool OnRevertCommandCanExecute(object obj)
        {
            // Implement business logic for myCommand enablement.
            return true;
        }

        #endregion 

        #region Save Command

        public void OnSaveCommandExecute(object obj)
        {
            if (deptData.HasErrors)
            {
                Microsoft.Windows.Controls.MessageBox.Show("Please correct the errors first", "Save Command", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                try
                {

                    if (deptData.HasChanges())
                    {
                        if (taManager.UpdateAll(deptData) > 0)
                        {
                            Microsoft.Windows.Controls.MessageBox.Show("Saved", "Save Command", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                catch
                {
                    Microsoft.Windows.Controls.MessageBox.Show("Unable to save changes", "Save Command", MessageBoxButton.OK, MessageBoxImage.Error);
                    deptData.RejectChanges();
                }
            }
        }

        public bool OnSaveCommandCanExecute(object obj)
        {
            // Implement business logic for myCommand enablement.
            return true;
        }

        #endregion 

        public void FilterDepartmentByOrganizationNo(string organizationNo)
        {

            BindingListCollectionView view1 = CollectionViewSource.GetDefaultView(this.deptData.department) as BindingListCollectionView;

            if (view1 != null)
            {
                view1.CustomFilter = "organization_no = '" + organizationNo.ToString() + "'";

            }

        }

    }

   
}
