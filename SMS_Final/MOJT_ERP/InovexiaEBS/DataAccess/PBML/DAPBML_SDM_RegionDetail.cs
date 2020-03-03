﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PBS.BusinessEntity.PBML;
using PBSConnLib;
using System.Collections;


namespace PBS.DataAccess.PBML
{
    public class DAPBML_SDM_RegionDetail
    {
        PBSDBUtility idbutility = new PBSDBUtility();
        public long InsertPBML_SDM_RegionDetail(BOPBML_SDM_RegionDetail boItem)
        {
            try
            {

                Hashtable ht = new Hashtable();
                ht.Add("RegionID", boItem.RegionID1);
                ht.Add("EmployeeCode", boItem.EmployeeCode1);
                ht.Add("StartDate", boItem.StartDate1);
                ht.Add("EndDate", boItem.EndDate1);
                ht.Add("IsActive", boItem.IsActive1);
                ht.Add("CreatedBy", boItem.CreatedBy1);
                ht.Add("CreatedDate", boItem.CreatedDate1);
                ht.Add("UpdatedBy", boItem.UpdatedBy1);
                ht.Add("UpdatedDate", boItem.UpdatedDate1);


                return Convert.ToInt64(idbutility.InsertData(ht, "spInsertPBML_SDM_RegionDetail"));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}