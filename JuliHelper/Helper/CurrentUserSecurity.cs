﻿////Source: https://stackoverflow.com/a/22020271/5645282


//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Security.AccessControl;
//using System.Security.Principal;
//using System.Text;
//using System.Threading.Tasks;

//namespace JuliHelper.Helper
//{
//    public class CurrentUserSecurity
//    {
//        WindowsIdentity _currentUser;
//        WindowsPrincipal _currentPrincipal;

//        public CurrentUserSecurity()
//        {
//            _currentUser = WindowsIdentity.GetCurrent();
//            _currentPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
//        }

//        public bool HasAccess(DirectoryInfo directory, FileSystemRights right)
//        {
//            // Get the collection of authorization rules that apply to the directory.
//            try
//            {
//                AuthorizationRuleCollection acl = directory.GetAccessControl()
//                    .GetAccessRules(true, true, typeof(SecurityIdentifier));

//                return HasFileOrDirectoryAccess(right, acl);
//            } catch { return false; }
//        }

//        public bool HasAccess(FileInfo file, FileSystemRights right)
//        {
//            // Get the collection of authorization rules that apply to the file.
//            AuthorizationRuleCollection acl = file.GetAccessControl()
//                .GetAccessRules(true, true, typeof(SecurityIdentifier));
//            return HasFileOrDirectoryAccess(right, acl);
//        }

//        private bool HasFileOrDirectoryAccess(FileSystemRights right,
//                                              AuthorizationRuleCollection acl)
//        {
//            bool allow = false;
//            bool inheritedAllow = false;
//            bool inheritedDeny = false;

//            for (int i = 0; i < acl.Count; i++)
//            {
//                FileSystemAccessRule currentRule = (FileSystemAccessRule)acl[i];
//                // If the current rule applies to the current user.
//                if (_currentUser.User.Equals(currentRule.IdentityReference) ||
//                    _currentPrincipal.IsInRole(
//                                    (SecurityIdentifier)currentRule.IdentityReference))
//                {

//                    if (currentRule.AccessControlType.Equals(AccessControlType.Deny))
//                    {
//                        if ((currentRule.FileSystemRights & right) == right)
//                        {
//                            if (currentRule.IsInherited)
//                            {
//                                inheritedDeny = true;
//                            }
//                            else
//                            { // Non inherited "deny" takes overall precedence.
//                                return false;
//                            }
//                        }
//                    }
//                    else if (currentRule.AccessControlType
//                                                    .Equals(AccessControlType.Allow))
//                    {
//                        if ((currentRule.FileSystemRights & right) == right)
//                        {
//                            if (currentRule.IsInherited)
//                            {
//                                inheritedAllow = true;
//                            }
//                            else
//                            {
//                                allow = true;
//                            }
//                        }
//                    }
//                }
//            }

//            if (allow)
//            { // Non inherited "allow" takes precedence over inherited rules.
//                return true;
//            }
//            return inheritedAllow && !inheritedDeny;
//        }
//    }
//}
