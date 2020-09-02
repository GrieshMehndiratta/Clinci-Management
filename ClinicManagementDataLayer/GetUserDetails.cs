using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementDataLayer
{
    public class GetUserDetails
    {                
        public UserModel GetUser(string UserName)
        {
            using (Context DBContext = new Context())
            {
                UserModel user = DBContext.LoginUsers.Single(m => m.UserName == UserName && m.AccountStatus == false);
                return user;
            }            
        }

        public List<Roles> GetRole(string UserName)
        {
            using (Context DBContext = new Context())
            {
                UserModel user = DBContext.LoginUsers.SingleOrDefault(m => m.UserName == UserName);
                //List<Roles> Role = from UserRoles in DBContext.UserRoles
                //             join AppRole in DBContext.Roles on UserRoles.RoleId equals AppRole.RoleId

                var Role = from Users in DBContext.LoginUsers
                           join UserRole in DBContext.UserRoles on Users.UserId equals UserRole.UserId
                           join AppRole in DBContext.Roles on UserRole.RoleId equals AppRole.RoleId
                           where Users.UserName.Equals(UserName, StringComparison.InvariantCultureIgnoreCase)
                           select AppRole;
                return Role.ToList();
            }            
        }

        public List<Roles> GetRoles(int UserId)
        {
            using (Context DBContext = new Context())
            {
                var Roles = from roles in DBContext.Roles
                            join userRoles in DBContext.UserRoles
                            on roles.RoleId equals userRoles.RoleId
                            where userRoles.UserId == UserId
                            select (roles);
                return Roles.ToList();
            }
        }

        public bool CheckUserName(string UserName)
        {
            using (Context DBContext = new Context())
            {
                UserModel model = DBContext.LoginUsers.SingleOrDefault(m => m.UserName == UserName);
                if (object.ReferenceEquals(model, null))
                    return false;
                return true;
            }            
        }

        public int GetUserId(string UserName)
        {
            using (Context DBContext = new Context())
            {
                try
                {
                    UserModel User = DBContext.LoginUsers.SingleOrDefault(m => m.UserName == UserName);
                    return User.UserId;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }            
        }

        public List<PatientModel> PatientList()
        {
            using (Context DBContext = new Context())
            {
                List<PatientModel> list = new List<PatientModel>();
                list = DBContext.Patient.Include(model => model.UserDetails).Where(m => m.UserDetails.AccountStatus == false).ToList();
                return list;
            }            
        }

        public PatientModel PatientDetails(int? id)
        {
            using (Context DBContext = new Context())
            {
                try
                {
                    PatientModel patientDetails = DBContext.Patient.Include(model => model.UserDetails).Where(m => m.PatientId == id).FirstOrDefault();
                    return patientDetails;
                }
                catch
                {
                    throw new Exception("Username Not Found");
                }
            }                        
        }

        public PatientModel GetPatientByUserId(int userId)
        {
            using (Context DBContext = new Context())
            {
                var patient = DBContext.Patient.Include(model => model.UserDetails).Where(m => m.UserDetails.UserId == userId).FirstOrDefault();
                return patient;
            }            
        }

        public void PatientDeleteConfrimed(int? id)
        {
            using (Context DBContext = new Context())
            {
                var patientDetails = DBContext.Patient.Single(m => m.PatientId == id);
                patientDetails.UserDetails.AccountStatus = true;
                DBContext.SaveChanges();
            }            
        }

        public List<DoctorModel> DoctorList()
        {
            using (Context DBContext = new Context())
            {
                List<DoctorModel> list = new List<DoctorModel>();
                list = DBContext.Doctors.Include(model => model.UserDetails).ToList();
                return list;
            }            
        }

        public List<DoctorModel> GetDoctorsBySpecialization(string specialization)
        {
            using (Context DBContext = new Context())
            {
                try
                {
                    List<DoctorModel> doctors = DBContext.Doctors.Include(model => model.UserDetails).Where(m => m.Specialization.ToString() == specialization && m.UserDetails.AccountStatus == false).ToList();
                    return doctors;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public DoctorModel DoctorDetails(int? id)
        {
            using (Context DBContext = new Context())
            {
                return DBContext.Doctors.Include(model => model.UserDetails).Where(m => m.DoctorId == id).FirstOrDefault();
            }            
        }

        public void DoctorDeleteConfrimed(int? id)
        {
            using (Context DBContext = new Context())
            {
                var doctorDetails = DBContext.Doctors.Single(m => m.DoctorId == id);
                doctorDetails.UserDetails.AccountStatus = true;
                DBContext.SaveChanges();
            }            
        }

        public void UpdatePatient(PatientModel patientModel)
        {
            using (Context DBContext = new Context())
            {
                PatientModel patientDetails = DBContext.Patient.SingleOrDefault(model => model.PatientId == patientModel.PatientId);
                patientDetails.UserDetails.FullName = patientModel.UserDetails.FullName;
                patientDetails.UserDetails.PhoneNo = patientModel.UserDetails.PhoneNo;
                patientDetails.UserDetails.Address = patientModel.UserDetails.Address;
                patientDetails.UserDetails.EmailId = patientModel.UserDetails.EmailId;
                patientDetails.Age = patientModel.Age;
                patientDetails.Height = patientModel.Height;
                patientDetails.Weight = patientModel.Weight;
                DBContext.Entry(patientDetails).State = EntityState.Modified;
                DBContext.SaveChanges();
            }
        }

        public void UpdateNurse(NurseModel nurseModel)
        {
            using (Context DBContext = new Context())
            {
                NurseModel nurseDetails = DBContext.Nurse.SingleOrDefault(model => model.NurseId == nurseModel.NurseId);
                nurseDetails.UserDetails.FullName = nurseModel.UserDetails.FullName;
                nurseDetails.UserDetails.PhoneNo = nurseModel.UserDetails.PhoneNo;
                nurseDetails.UserDetails.Address = nurseModel.UserDetails.Address;
                nurseDetails.UserDetails.EmailId = nurseModel.UserDetails.EmailId;
                DBContext.Entry(nurseDetails).State = EntityState.Modified;
                DBContext.SaveChanges();
            }

            
        }

        public void UpdateProfile(UserModel userModel)
        {
            using (Context DBContext = new Context())
            {
                UserModel userDetails = DBContext.LoginUsers.SingleOrDefault(model => model.UserName == userModel.UserName);
                userDetails.Address = userModel.Address;
                userDetails.City = userModel.City;
                userDetails.EmailId = userModel.EmailId;
                userDetails.Gender = userModel.Gender;
                userDetails.PhoneNo = userModel.PhoneNo;
                DBContext.Entry(userDetails).State = EntityState.Modified;
                DBContext.SaveChanges();
            }

           
        }

        public UserModel GetUserDetail(string userName)
        {
            using (Context DBContext = new Context())
            {
                UserModel userModel = DBContext.LoginUsers.SingleOrDefault(model => model.UserName == userName);
                return userModel;
            }
        }

         

        public List<NurseModel> NurseList()
        {
            using (Context DBContext = new Context())
            {
                List<NurseModel> list = new List<NurseModel>();
                list = DBContext.Nurse.Include(model => model.UserDetails).Where(m => m.UserDetails.AccountStatus == false).ToList();
                return list;
            }
        }

        public void UpdateDoctor(DoctorModel doctorModel)
        {
            using (Context DBContext = new Context())
            {
                DoctorModel doctorDetails = DBContext.Doctors.SingleOrDefault(model => model.DoctorId == doctorModel.DoctorId);
                doctorDetails.Fee = doctorModel.Fee;
                doctorDetails.UserDetails.FullName = doctorModel.UserDetails.FullName;
                doctorDetails.UserDetails.Address = doctorModel.UserDetails.Address;
                doctorDetails.UserDetails.PhoneNo = doctorModel.UserDetails.PhoneNo;
                doctorDetails.UserDetails.EmailId = doctorModel.UserDetails.EmailId;
                DBContext.Entry(doctorDetails).State = EntityState.Modified;
                DBContext.SaveChanges();
            }
        }

          

        public NurseModel NurseDetails(int? id)
        {
            using (Context DBContext = new Context())
            {
                return DBContext.Nurse.Include(model => model.UserDetails).Where(m => m.NurseId == id).FirstOrDefault();
            }            
        }

        public void NurseDeleteConfrimed(int id)
        {
            using (Context DBContext = new Context())
            {
                var nurseDetails = DBContext.Nurse.Single(m => m.NurseId == id);
                nurseDetails.UserDetails.AccountStatus = true;
                DBContext.SaveChanges();
            }            
        }
        public bool AddPatient(PatientModel model, EmergencyContactDetails emergencyContactModel)
        {
            using (Context DBContext = new Context())
            {
                try
                {
                    DBContext.Patient.Add(model);
                    DBContext.SaveChanges();
                    UserModel patientId = DBContext.LoginUsers.SingleOrDefault(m => m.UserName == model.UserDetails.UserName);
                    emergencyContactModel.PatientId = model.PatientId;
                    DBContext.EmergencyContactDetails.Add(emergencyContactModel);
                    DBContext.SaveChanges();
                    UserRoles roles = new UserRoles();
                    roles.RoleId = (int)Role.Patient;
                    roles.UserId = patientId.UserId;
                    DBContext.UserRoles.Add(roles);
                    DBContext.SaveChanges();
                    return true;
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                    return false;
                }
            }
        }



        public bool AddDoctor(DoctorModel model)
        {
            using (Context DBContext = new Context())
            {
                try
                {
                    DBContext.Doctors.Add(model);
                    DBContext.SaveChanges();
                    UserModel patientId = DBContext.LoginUsers.SingleOrDefault(m => m.UserName == model.UserDetails.UserName);
                    UserRoles roles = new UserRoles();
                    roles.RoleId = (int)Role.Doctor;
                    roles.UserId = patientId.UserId;
                    DBContext.UserRoles.Add(roles);
                    DBContext.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool AddNurse(NurseModel model)
        {
            using (Context DBContext = new Context())
            {
                try
                {
                    DBContext.Nurse.Add(model);
                    DBContext.SaveChanges();
                    UserModel patientId = DBContext.LoginUsers.SingleOrDefault(m => m.UserName == model.UserDetails.UserName);
                    UserRoles roles = new UserRoles();
                    roles.RoleId = (int)Role.Nurse;
                    roles.UserId = patientId.UserId;
                    DBContext.UserRoles.Add(roles);
                    DBContext.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }            
        }

        public bool AddEmergencyContactDetails(EmergencyContactDetails model)
        {
            using (Context DBContext = new Context())
            {
                try
                {
                    DBContext.EmergencyContactDetails.Add(model);
                    DBContext.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

           
        }

        public EmergencyContactDetails GetEmergencyDetails(int id)
        {
            using (Context DBContext = new Context())
            {
                EmergencyContactDetails emergencyContact = DBContext.EmergencyContactDetails.SingleOrDefault(m => m.PatientId == id);
                return emergencyContact;
            }

          
        }

        public List<int> GetStats(DateTime RegistrationDate)
        {
            using (Context DBContext = new Context())
            {
                List<int> result = new List<int>();
                try
                {
                    int totalPatient = DBContext.Patient.Count();
                    result.Add(totalPatient);
                    List<PatientModel> patients = DBContext.Patient.Where(m => DbFunctions.TruncateTime(m.RegistrationDate) == RegistrationDate.Date && m.UserDetails.AccountStatus == false).ToList();
                    result.Add(patients.Count);
                    int AppointmentCount = DBContext.Appointments.Count();
                    result.Add(AppointmentCount);
                    AppointmentCount = DBContext.Appointments.Count(m => DbFunctions.TruncateTime(m.AppointmentDate) == RegistrationDate.Date);
                    result.Add(AppointmentCount);
                    int doctors = DBContext.Doctors.Count();
                    result.Add(doctors);
                    int doctorAvail = DBContext.Doctors.Count(doctor => doctor.UserDetails.AccountStatus == false);
                    result.Add(doctorAvail);
                    int totalUsers = DBContext.LoginUsers.Count();
                    result.Add(totalUsers);
                    List<UserModel> users = DBContext.LoginUsers.Where(m => m.AccountStatus == false).ToList();
                    result.Add(users.Count);
                    return result;
                }
                catch (Exception ex)
                {
                    //Log the Error
                    return null;
                }
            }
            
        }

        public UserModel verifyEmail(string email)
        {
            using (Context DBContext = new Context())
            {
                try
                {
                    return DBContext.LoginUsers.SingleOrDefault(m => m.UserName == email || m.EmailId == email && m.AccountStatus == false);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public void SetResetCode(string UserName, string Code)
        {
            using (Context DBContext = new Context())
            {
                UserModel result = (from p in DBContext.LoginUsers
                                    where p.UserName == UserName
                                    select p).SingleOrDefault();

                result.ResetCode = Code;

                DBContext.SaveChanges();
            }            
        }

        public bool ChangePassword(string Password, string Code)
        {
            using (Context DBContext = new Context())
            {
                try
                {
                    UserModel result = (from user in DBContext.LoginUsers
                                        where user.ResetCode == Code
                                        select user).SingleOrDefault();
                    result.Password = Password;
                    result.ResetCode = null;
                    DBContext.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        
        public int GetPatientIdByUserName(string Name)
        {
            using (Context DbContext = new Context())
            {
                try
                {
                    var result = DbContext.Patient.SingleOrDefault(m => m.UserDetails.UserName == Name);
                    return result.PatientId;
                }
                catch(Exception ex)
                {
                    return 0;
                }
            }

        }

        public string GetOldPassword(string UserName)
        {
            using (Context DbContext = new Context())
            {
                var result = DbContext.LoginUsers.SingleOrDefault(user => user.UserName == UserName);
                return result.Password;
            }
        }

        public bool UpdatePassword(string UserName,string Password)
        {
            using (Context DbContext = new Context())
            {
                try
                {
                    UserModel user = DbContext.LoginUsers.SingleOrDefault(u => u.UserName == UserName);
                    user.Password = Password;
                    DbContext.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
                
            }
        }
    }
}
