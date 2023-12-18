using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows.Input;
using Warehouse.Core.Helpers;
using Warehouse.Core.Interface;
using Warehouse.Creator.Service;
using Warehouse.Models;
using Warehouse.Models.Attribute;
using Warehouse.Models.Enums;
using Warehouse.Models.Interfaces;

namespace Warehouse.Creator.ViewModel
{
    public class Step1ViewModel : BaseStepViewModel
    {
        #region Private properties

        private string _nameOfDatabase;

        private string _server;

        private bool _authWindows = true;

        private string _user;

        private string _passworld;

        private string _adminPassworld;

        private string _login;

        private string _name;

        private string _email;

        #endregion

        #region Public properties

        [Required(ErrorMessage = "Database name is required.")]
        public string NameOfDatabase
        {
            get => _nameOfDatabase;
            set => SetProperty(ref _nameOfDatabase, value);
        }

        [Required(ErrorMessage = "Server is required.")]
        public string Server
        {
            get => _server;
            set => SetProperty(ref _server, value);
        }

        public bool AuthWindows
        {
            get => _authWindows;
            set => SetProperty(ref _authWindows, value);
        }

        public string User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public string Passworld
        {
            get => _passworld;
            set => SetProperty(ref _passworld, value);
        }

        [Required(ErrorMessageResourceName = "LoginIsRequired", ErrorMessageResourceType = typeof(Core.Properties.Resources))]
        [MinLength(5, ErrorMessage = "Name is to short (5)")]
        [MaxLength(50, ErrorMessage = "No more than 50 characters")]
        public string Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        [Required(ErrorMessageResourceName = "LoginIsRequired", ErrorMessageResourceType = typeof(Core.Properties.Resources))]
        [MinLength(5, ErrorMessage = "Name is to short (5)")]
        [MaxLength(50, ErrorMessage = "No more than 50 characters")]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        [Required(ErrorMessageResourceName = "EmailIsRequired", ErrorMessageResourceType = typeof(Core.Properties.Resources))]
        [IsEmail(ErrorMessageResourceName = "WrongEmail", ErrorMessageResourceType = typeof(Core.Properties.Resources))]
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        [Required(ErrorMessageResourceName = "PasswordIsRequired", ErrorMessageResourceType = typeof(Core.Properties.Resources))]
        [MinLength(5, ErrorMessage = "Minimum length is 5")]
        [MaxLength(150, ErrorMessage = "Minimum length is 150")]
        public string AdminPassworld
        {
            get => _adminPassworld;
            set => SetProperty(ref _adminPassworld, value);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Step1ViewModel(IApp app) : base(app)
        {
            Step = EPageStep.Step1;
            NextStepCommand = new AsyncRelayCommand(NextStepAsync);
        }

        #endregion

        #region Command methods


        protected async Task NextStepAsync()
        {
            try
            {
                _CanValidate = true;

                Application.IsTaskRunning = true;
                var error = GettErrors();
                if((error?.Length ?? 0) > 0)
                {
                    Application.IsTaskRunning = false;
                    OnPropertyChanged(nameof(Server), nameof(NameOfDatabase),nameof(Passworld), nameof(User), nameof(Name), nameof(Login), nameof(AdminPassworld), nameof(Email));
                    return;
                }

                bool exist = false;
                EDialogResult result = EDialogResult.Undefind;
                using (SqlConnection conn = new SqlConnection(GetConnectionString(false)))
                {
                    exist = CheckDatabaseExists(conn, NameOfDatabase);
                }
                if (exist)
                {
                    Application.IsTaskRunning = false;
                    result = Application.GetDialogService().AskUser($"Baza danych {NameOfDatabase} juz istnieje, zmigrowac dane? ");
                    if(result != EDialogResult.Yes)
                        return;
                }

                AddOrUpdateAppSetting("ConnectionStrings:DBConnection", GetConnectionString(true));
                var db = Application.Database;
                db.Database.Migrate();
                if (exist)
                {
                    IUserService userService = Application.GetService<IUserService>();
                    var users = await Application.GetService<IUserService>().GetUsers();
                    var admin = users.FirstOrDefault(x => x.Login == Login);
                    if(admin != null)
                    {
                        userService.Delete(admin.ID);
                        userService.Save();
                    }

                }

                IUser user = await Application.Register(new Core.Resources.RegisterResource(Login, Email, AdminPassworld, Name, Models.Enums.EUserType.Admin, true));

                if(user == null)
                {
                    Application.IsTaskRunning = false;
                    Application.GetDialogService().ShowAlert($"User not created");
                    return;
                }

                await Application.LoginAsync(new Core.Resources.LoginResource(Login, AdminPassworld));

                Application.IsTaskRunning = false;
                SetNextStep();


            }
            catch(Exception ex)
            {
                Application.IsTaskRunning = false;
                Application.CatchExeption(ex);
            }
            finally
            {
                Application.IsTaskRunning = false;
            }
        }

        #endregion

        #region Helpers

        public void AddOrUpdateAppSetting<T>(string sectionPathKey, T value)
        {
            try
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
                string json = File.ReadAllText(filePath);
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                SetValueRecursively(sectionPathKey, jsonObj, value);

                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filePath, output);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing app settings | {0}", ex.Message);
            }
        }

        private void SetValueRecursively<T>(string sectionPathKey, dynamic jsonObj, T value)
        {
            // split the string at the first ':' character
            var remainingSections = sectionPathKey.Split(":", 2);

            var currentSection = remainingSections[0];
            if (remainingSections.Length > 1)
            {
                // continue with the procress, moving down the tree
                var nextSection = remainingSections[1];
                SetValueRecursively(nextSection, jsonObj[currentSection], value);
            }
            else
            {
                // we've got to the end of the tree, set the value
                jsonObj[currentSection] = value;
            }
        }

        private bool CheckDatabaseExists(SqlConnection tmpConn, string databaseName)
        {
            string sqlCreateDBQuery;
            bool result = false;

            try
            {
                
                sqlCreateDBQuery = string.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}'", databaseName);
        
                using (tmpConn)
                {
                    using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, tmpConn))
                    {
                        tmpConn.Open();

                        object resultObj = sqlCmd.ExecuteScalar();

                        int databaseID = 0;

                        if (resultObj != null)
                        {
                            int.TryParse(resultObj.ToString(), out databaseID);
                        }

                        tmpConn.Close();

                        result = (databaseID > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        private string GetConnectionString(bool addDatabase = true)
        {
            string connString = $"Server={Server};MultipleActiveResultSets=true;TrustServerCertificate=True";
            if(AuthWindows)
                connString += $";Trusted_Connection = True";
            else
                connString += $";Trusted_Connection=no;UID={User};PWD={(new System.Net.NetworkCredential(string.Empty, Passworld).Password)}";

            if(addDatabase)
                connString += $";Database={NameOfDatabase};";


            return connString;
        }

        #endregion

    }
}