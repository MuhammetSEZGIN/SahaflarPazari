//using Domain.Interfaces;
//using Ninject;
//using SahaflarPazari.App_Start;
//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web.Mvc;
//using System.Web.Security;

//namespace SahaflarPazari.Security
//{
//    public class MyRoleProvider : RoleProvider
//    {
//        private  IUnitOfWork _unitOfWork;
//        private static IKernel kernel => NinjectWebCommon.bootstrapper.Kernel;

//        // Default constructor using DependencyResolver for DI
//        public MyRoleProvider()
//        {
//            _unitOfWork = kernel.Get<IUnitOfWork>();
//        }

//        // Constructor for explicit DI (testing or custom initialization)
//        public MyRoleProvider(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }

//        public override string ApplicationName { get; set;}

//        public override bool IsUserInRole(string username, string roleName)
//        {
//            _unitOfWork= kernel.Get<IUnitOfWork>();
//            var currentUser = _unitOfWork.Users.GetUserByNameAsync(username);
//            if (currentUser != null)
//            {
//                var userRoles = _unitOfWork.Roles.GetRolesByUserIdAsync(currentUser.Id);
//                if (userRoles != null)
//                {
//                    return userRoles.Result.Any(r => r.RoleName == roleName);
//                }
//            }
//            return false;

//        }


//        public override string[] GetRolesForUser(string username)
//        {
//            var deneme = _unitOfWork.Users.GetUserByNameAsync(username);

//            var result= _unitOfWork.Roles.GetRolesbyUserName(username).Select(r => r.RoleName).ToArray();
//            return result;
//        }

//        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
//        {
//            throw new NotImplementedException();
//        }

//        public override void CreateRole(string roleName)
//        {
//            throw new NotImplementedException();
//        }

//        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
//        {
//            throw new NotImplementedException();
//        }

//        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
//        {
//            throw new NotImplementedException();
//        }

//        public override string[] GetAllRoles()
//        {
//            throw new NotImplementedException();
//        }

//        public override string[] GetUsersInRole(string roleName)
//        {
//            throw new NotImplementedException();
//        }

//        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
//        {
//            throw new NotImplementedException();
//        }

//        public override bool RoleExists(string roleName)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
