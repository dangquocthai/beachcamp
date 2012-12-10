using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
//using Microsoft.Office.Interop.Word;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.Text;
using System.IO;
using System.Xml;

namespace SharePoint.BeachCamp.Util.Extensions
{
    public static class SPListItemExtensions
    {

        public static void SetPermissions(this SPListItem item, IEnumerable<SPPrincipal> principals, SPRoleType roleType)
        {
            if (item != null)
            {

                foreach (SPPrincipal principal in principals)
                {
                    SPRoleDefinition roleDefinition = item.Web.RoleDefinitions.GetByType(roleType);
                    SetPermissions(item, principal, roleDefinition);
                }
            }
        }


        public static void SetPermissions(this SPListItem item, SPUser user, SPRoleType roleType)
        {
            if (item != null)
            {
                SPRoleDefinition roleDefinition = item.Web.RoleDefinitions.GetByType(roleType);
                SetPermissions(item, (SPPrincipal)user, roleDefinition);
            }
        }

        public static void SetPermissions(this SPListItem item, SPPrincipal principal, SPRoleType roleType)
        {
            if (item != null)
            {
                SPRoleDefinition roleDefinition = item.Web.RoleDefinitions.GetByType(roleType);
                SetPermissions(item, principal, roleDefinition);
            }
        }

        public static void SetPermissions(this SPListItem item, SPUser user, SPRoleDefinition roleDefinition)
        {
            if (item != null)
            {
                SetPermissions(item, (SPPrincipal)user, roleDefinition);
            }
        }

        public static void SetPermissions(this SPListItem item, SPPrincipal principal, SPRoleDefinition roleDefinition)
        {
            if (item != null)
            {
                SPRoleAssignment roleAssignment = new SPRoleAssignment(principal);

                roleAssignment.RoleDefinitionBindings.Add(roleDefinition);
                item.RoleAssignments.Add(roleAssignment);
            }
        }


        public static void ChangePermissions(this SPListItem item, SPPrincipal principal, SPRoleType roleType)
        {
            if (item != null)
            {
                SPRoleDefinition roleDefinition = item.Web.RoleDefinitions.GetByType(roleType);
                ChangePermissions(item, principal, roleDefinition);
            }
        }

        public static void ChangePermissions(this SPListItem item, SPPrincipal principal, SPRoleDefinition roleDefinition)
        {
            SPRoleAssignment roleAssignment = item.RoleAssignments.GetAssignmentByPrincipal(principal);
            if (roleAssignment != null)
            {
                roleAssignment.RoleDefinitionBindings.RemoveAll();
                roleAssignment.RoleDefinitionBindings.Add(roleDefinition);
                roleAssignment.Update();
            }
        }

        public static bool VerifyFieldAccess(this SPListItem item, string fieldname)
        {
            try
            {
                bool result = item.Fields.ContainsField(fieldname);
                if (result)
                    result &= item[fieldname] != null;
                return result;
            }
            finally
            {

            }
            return false;
        }

        public static void RemoveReadPermissions(this SPListItem item)
        {
            //remove all current permissions
            if (!item.HasUniqueRoleAssignments)
            {
                return;
            }
            else
            {
                List<SPRoleAssignment> willbeRemovedAssignments = new List<SPRoleAssignment>();

                var roleAssignements = item.RoleAssignments.Cast<SPRoleAssignment>().ToList();

                foreach (var rs in roleAssignements)
                {
                    foreach (SPRoleDefinition rd in rs.RoleDefinitionBindings)
                    {
                        string permission = rd.BasePermissions.ToString();
                        if (permission.Contains("EditListItems") || permission.Contains("FullMask"))
                        {
                            continue;
                        }

                        willbeRemovedAssignments.Add(rs);
                        break;
                    }
                }

                foreach (var removeItem in willbeRemovedAssignments)
                {
                    item.RoleAssignments.Remove(removeItem.Member);
                }
                item.SystemUpdate();
            }
        }

        public static void RemoveAllPermissions(this SPListItem item)
        {
            //remove all current permissions
            if (!item.HasUniqueRoleAssignments)
            {
                item.BreakRoleInheritance(false);
            }
            else
            {
                while (item.RoleAssignments.Count > 0)
                {
                    item.RoleAssignments.Remove(0);
                }
            }
        }
    }
}

