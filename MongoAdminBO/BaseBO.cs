using MongoAdmin.Logger;
using MongoAdmin.Models;

namespace MongoAdmin.BO
{
    public class BaseBO
    {
        public AppConfigSettings _localAppSettings = new AppConfigSettings();
        public Logging _logObj = new Logging();

        public BaseBO(AppConfigSettings _appSettings)
        {
            _localAppSettings = _appSettings;
        }
    }
}
