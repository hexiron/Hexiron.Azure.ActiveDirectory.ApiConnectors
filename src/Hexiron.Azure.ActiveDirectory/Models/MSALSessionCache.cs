using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;

namespace Hexiron.Azure.ActiveDirectory.Models
{
    public class MsalSessionCache
    {
        private static readonly ReaderWriterLockSlim s_sessionLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        readonly string _cacheId;
        readonly HttpContext _httpContext;

        TokenCache cache = new TokenCache();

        public MsalSessionCache(string userId, HttpContext httpcontext)
        {
            // not object, we want the SUB
            _cacheId = userId + "_TokenCache";
            _httpContext = httpcontext;
            Load();
        }

        public TokenCache GetMsalCacheInstance()
        {
            cache.SetBeforeAccess(BeforeAccessNotification);
            cache.SetAfterAccess(AfterAccessNotification);
            Load();
            return cache;
        }

        public void SaveUserStateValue(string state)
        {
            s_sessionLock.EnterWriteLock();
            _httpContext.Session.SetString(_cacheId + "_state", state);
            s_sessionLock.ExitWriteLock();
        }
        public string ReadUserStateValue()
        {
            s_sessionLock.EnterReadLock();
            var state = _httpContext.Session.GetString(_cacheId + "_state");
            s_sessionLock.ExitReadLock();
            return state;
        }
        public void Load()
        {
            s_sessionLock.EnterReadLock();
            cache.Deserialize(_httpContext.Session.Get(_cacheId));
            s_sessionLock.ExitReadLock();
        }

        public void Persist()
        {
            s_sessionLock.EnterWriteLock();

            // Optimistically set HasStateChanged to false. We need to do it early to avoid losing changes made by a concurrent thread.
            cache.HasStateChanged = false;

            // Reflect changes in the persistent store
            _httpContext.Session.Set(_cacheId, cache.Serialize());
            s_sessionLock.ExitWriteLock();
        }

        // Triggered right before MSAL needs to access the cache.
        // Reload the cache from the persistent store in case it changed since the last access.
        void BeforeAccessNotification(TokenCacheNotificationArgs args)
        {
            Load();
        }

        // Triggered right after MSAL accessed the cache.
        void AfterAccessNotification(TokenCacheNotificationArgs args)
        {
            // if the access operation resulted in a cache update
            if (cache.HasStateChanged)
            {
                Persist();
            }
        }
    }
}
