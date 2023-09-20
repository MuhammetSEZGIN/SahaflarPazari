using SahaflarPazari.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SahaflarPazari.Security
{
    public class MyRoleProvider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            using (SahaflarPazariEntities entity = new SahaflarPazariEntities())
            {
              
                foreach (string username in usernames)
                {
                    foreach (string roleName in roleNames)
                    {
                        
                        Kullanici user = entity.Kullanici.SingleOrDefault(u => u.KullaniciAdi == username);
                        if (user != null)
                        {
                            // Rolü veritabanından bul
                            Roller role = entity.Roller.SingleOrDefault(r => r.RolAdi == roleName);
                            if (role != null)
                            {
                                
                                user.Roller.Add(role);
                                
                            }
                        }
                    }
                }

                
                entity.SaveChanges();
            }
        }




        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            
            using(SahaflarPazariEntities entity = new SahaflarPazariEntities())
            {
                try
                {
                    
                    Kullanici kullanici = entity.Kullanici.FirstOrDefault(x => x.KullaniciAdi == username);
                    
                    if(kullanici != null)
                    {
                        int id = kullanici.KullaniciId;
                        string[] roller = entity.Roller.Where(r => r.KullaniciId == id).Select(r => r.RolAdi).ToArray();
                        return roller;
                    }
                    else
                    return new string[0];

                    
                }
                catch (Exception ex)
                {
                    
                    throw new Exception("Roller alınamadı: " + ex.Message);
                }

               
            }
            
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}