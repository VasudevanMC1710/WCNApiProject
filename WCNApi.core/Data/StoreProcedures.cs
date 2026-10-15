namespace WCNApi.core.Data
{
    public static class StoreProcedures
    {
        #region login 
        
        public static string sp_CheckLoginCredinitals_Dig = "sp_CheckLoginCredinitals_Dig";
        public static string sp_Add_LoginHistory_Dig = "sp_Add_LoginHistory_Dig";
        public static string sp_Get_LoginHistory_ByUid_Dig = "sp_Get_LoginHistory_ByUid_Dig";
        public static string sp_Get_UserDashboard_Dig = "sp_Get_UserDashboard_Dig";
        public static string sp_SearchProspects_Dig = "sp_SearchProspects_Dig";

        #endregion

        #region ProspectDetails
        public static string sp_Get_Prospects_Dig = "sp_Get_Prospects_Dig";
        #endregion
    }
}
