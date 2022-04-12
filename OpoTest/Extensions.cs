using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpoTest
{
    public static class Extensions
    {
        static IObjectSpaceAsync ToAsync(this IObjectSpace os) => os as IObjectSpaceAsync;
        static XPObjectSpace ToXPO(this IObjectSpace os) => os as XPObjectSpace;
        public static async Task<IEnumerable<ObjectType>> GetObjectsByKeyAsync<ObjectType>(this IObjectSpace os, ICollection keys, bool alwaysGetFromDataStore, CancellationToken cancellationToken = default)
            => (await os.ToXPO().Session.GetObjectsByKeyAsync(os.ToXPO().Session.GetClassInfo<ObjectType>(), keys, alwaysGetFromDataStore, cancellationToken)).Cast<ObjectType>();
        public static Task CommitChangesAsync(this IObjectSpace os, CancellationToken cancellationToken = default) => os.ToAsync().CommitChangesAsync(cancellationToken);
        public static Task<object> GetObjectAsync(this IObjectSpace os, object obj, CancellationToken cancellationToken = default) => os.ToAsync().GetObjectAsync(obj, cancellationToken);
        public static Task<ObjectType> GetObjectByKeyAsync<ObjectType>(this IObjectSpace os, object key, CancellationToken cancellationToken = default) => os.ToAsync().GetObjectByKeyAsync<ObjectType>(key, cancellationToken);
    }
}
