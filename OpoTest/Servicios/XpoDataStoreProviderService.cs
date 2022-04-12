using DevExpress.ExpressApp.Xpo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpoTest.Services {
    public class XpoDataStoreProviderService {
        private IXpoDataStoreProvider dataStoreProvider;
        private string connectionString;

        public XpoDataStoreProviderService(IConfiguration config) {
            connectionString = config.GetConnectionString("ConnectionString");
        }

        public IXpoDataStoreProvider GetDataStoreProvider() {
            if(dataStoreProvider == null) {
                dataStoreProvider = XPObjectSpaceProvider.GetDataStoreProvider(connectionString, null, true);
            }
            return dataStoreProvider;
        }
    }
}
