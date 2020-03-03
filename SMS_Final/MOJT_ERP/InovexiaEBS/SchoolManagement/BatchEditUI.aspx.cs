﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InovexiaEBS.BusinessEntity.SchoolManagement;
using InovexiaEBS.BusinessLogic.SchoolManagement;
using PBSConnLib;
using Telerik.Web.UI;

namespace InovexiaEBS.SchoolManagement
{
    public partial class BatchEditUI : System.Web.UI.Page
    {

        BatchEditBLL objBLL = new BatchEditBLL();
        PBSDBUtility pb = new PBSDBUtility();
           
            protected void Page_Load(object sender, EventArgs e)
            {
                

                
                //BindDropdown();
                
                if (!IsPostBack)
                {
                    int BatchID = Convert.ToInt32(Request.QueryString["BatchID"]);
                    GetBatchInfo(BatchID);


                    PopulateYearsFrom();
                    PopulateYearsTo();


                    //ddlyf();
                }
                lblyearcheck.Visible = false;
                alertbatch1.Visible = false;
                alertbatch2.Visible = false;

            }

            protected void GetBatchInfo(int BatchID)
            {
                try
                {
                    VmStudentRegistration _objVm = new VmStudentRegistration();
                    Hashtable ht = new Hashtable();
                    ht.Add("BatchID", BatchID);
                    DataTable dataTable = BatchEditBLL.GetBatchInfo(ht);
                    if (dataTable.Rows.Count > 0)
                    {
                        txtbatchname.Text = dataTable.Rows[0]["SectionName"].ToString();
                        ddlclass.Text = dataTable.Rows[0]["ClassName"].ToString();
                        ddlyearf.Text = dataTable.Rows[0]["BatchYearFrom"].ToString();
                        ddlyeart.Text = dataTable.Rows[0]["BatchYearTo"].ToString();
                        txtbatchremarks.Text = dataTable.Rows[0]["Remarks"].ToString();
                        
                    }


                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message.ToString());
                }

            }
            protected void ddlclass_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
            {
                try
                {
                    DataTable DT = pb.GetDataByProc("SM_GetClassList");
                    foreach (DataRow datarow in DT.Rows)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = (string)datarow["ClassName"];
                        item.Value = datarow["ClassID"].ToString();
                        item.Attributes.Add("ClassName", datarow["ClassName"].ToString());
                        ddlclass.Items.Add(item);
                        item.DataBind();
                    }

                }
                catch (Exception)
                {

                    throw;
                }
            }

            protected void ddlyear_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
            {
                try
                {
                    //DataTable DT = pb.GetDataByProc("SM_GetClassList");
                    //foreach (DataRow datarow in DT.Rows)
                    //{
                    //    RadComboBoxItem item = new RadComboBoxItem();
                    //    item.Text = (string)datarow["ClassName"];
                    //    item.Value = datarow["ClassID"].ToString();
                    //    item.Attributes.Add("ClassName", datarow["ClassName"].ToString());
                    //    ddlyear.Items.Add(item);
                    //    item.DataBind();
                    //}

                }
                catch (Exception)
                {

                    throw;
                }
            }

            //void BindDropdown()
            //{


            //    var currentYear = DateTime.Today.Year;
            //    for (int i = 2; i >= 0; i--)
            //    {
            //        // Now just add an entry that's the current year minus the counter
            //        ddlyear.Items.Add(new RadComboBoxItem((currentYear - i).ToString("yyyy")));
            //    }
            //}


            protected void btnsavebatch_OnClick(object sender, EventArgs e)
            {
                VmStudentRegistration objbatch = new VmStudentRegistration();
                objbatch.batchname = txtbatchname.Text.Trim();
                objbatch.batchyearName = ddlyearf.Text.Trim() + "-" + ddlyeart.Text.Trim();
                objbatch.classname = ddlclass.Text.Trim();
                objbatch.batchremarks = txtbatchremarks.Text.Trim();

                checkbatch(objbatch);
            }

            private void checkbatch(VmStudentRegistration objbatch)
            {
                try
                {

                    Hashtable ht = new Hashtable();

                    ht.Add("SectionName", objbatch.batchname);
                    ht.Add("BatchYearName", objbatch.batchyearName);
                    ht.Add("ClassName", objbatch.classname);


                    DataTable dtTable = CreateBatchBLL.checkbatchnames(ht);

                    if (dtTable.Rows.Count > 0)
                    {

                        //string result = "This record already exists!";
                        // lblbatchcheck.Visible = true;
                        // lblbatchcheck.Text = result.ToString();
                        alertbatch1.Visible = true;
                        alertbatch2.Visible = false;

                    }
                    else
                    {

                        CreateBatch(objbatch);

                    }

                }

                catch (Exception ex)
                {

                    throw new Exception(ex.Message.ToString());
                }

            }

            private void CreateBatch(VmStudentRegistration objbatch)
            {

                try
                {
                    long result = 1;
                    objbatch.batchname = txtbatchname.Text.Trim();
                    objbatch.batchyearf = Convert.ToInt32(ddlyearf.SelectedValue);
                    objbatch.batchyeart = Convert.ToInt32(ddlyeart.SelectedValue);
                    objbatch.batchyearName = ddlyearf.Text.Trim() + "-" + ddlyeart.Text.Trim();
                    objbatch.classname = ddlclass.SelectedValue.Trim();
                    objbatch.batchremarks = txtbatchremarks.Text.Trim();

                    if (objbatch.batchyearf > objbatch.batchyeart)
                    {
                        string label = "Invalid Year Selection";
                        lblyearcheck.Visible = true;
                        lblyearcheck.Text = label.ToString();

                    }
                    else
                    {
                        result = CreateBatchBLL.SaveYearBatch(objbatch);
                        alertbatch1.Visible = false;
                        alertbatch2.Visible = true;
                        clearfieldsBatch();
                    }




                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message.ToString());
                }

            }

            public void clearfieldsBatch()
            {
                txtbatchname.Text = "";
                txtbatchremarks.Text = "";
                ddlclass.ClearSelection();
                ddlyearf.ClearSelection();
                ddlyeart.ClearSelection();
                //
            }

            public void PopulateYearsFrom()
            {
                List<int> years = new List<int>();
                int currentYear = DateTime.Now.Year;
                for (int year = currentYear - 5; year <= currentYear + 5; year++)
                {
                    years.Add(year);
                }

                // Bind the dropdownlist
                ddlyearf.DataSource = years;
                ddlyearf.DataBind();
            }

            public void PopulateYearsTo()
            {
                List<int> years = new List<int>();
                int currentYear = DateTime.Now.Year;
                for (int year = currentYear - 5; year <= currentYear + 5; year++)
                {
                    years.Add(year);
                }

                // Bind the dropdownlist
                ddlyeart.DataSource = years;
                ddlyeart.DataBind();
            }

        protected void btnUpdatebatch_OnClick(object sender, EventArgs e)
        {
            ///
        }
    }
       
    }
